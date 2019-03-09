using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace ConfigMgr.WebApi
{
    public class WindowsAuth : ActionFilterAttribute, IAuthenticationFilter
    {
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (context.Principal != null && context.Principal is WindowsPrincipal winPrin)
            {
                var winId = winPrin.Identity as WindowsIdentity;

                if (winId.AuthenticationType != "Negotiate")
                    return;

                if (!context.Request.Headers.Authorization.Parameter.StartsWith("Y", StringComparison.CurrentCulture))
                    return;

                if (!winId.IsAuthenticated)
                    return;

                IEnumerable<string> userSIDs = await this.GetUserSIDs(winId);
                List<string> roles = await this.CheckSCCMRights(userSIDs);
                var newPrin = new ApiPrincipal(winId, roles);
                context.Principal = newPrin;
            }
        }

        private async Task<IEnumerable<string>> GetUserSIDs(WindowsIdentity winId)
        {
            var result = await Task.Run(() =>
            {
                var list = new List<string>
                {
                    winId.User.Value
                };
                list.AddRange(winId.Groups.Select(x => x.Value));
                return list;
            });
            return result;
        } 

        private async Task<List<string>> CheckSCCMRights(IEnumerable<string> sids)
        {
            string sqlConStr = string.Format(
                Constants.SQL_CONNECTION_STRING, 
                ConfigurationManager.AppSettings[Constants.PRIMARY_SITE_SERVER], 
                ConfigurationManager.AppSettings[Constants.SITE_CODE]
            );
            string admObj = null;
            using (var sqlConn = new SqlConnection(sqlConStr))
            {
                await sqlConn.OpenAsync();
                string sidsStr = string.Join(",", sids);
                string query = string.Format(Constants.RBAC_ADMIN_FUNCTION, sidsStr);
                using (var sqlCommand = new SqlCommand(query, sqlConn))
                {
                    SqlDataReader dataReader = await sqlCommand.ExecuteReaderAsync();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (!await dataReader.IsDBNullAsync(0))
                            {
                                admObj = dataReader.GetString(0);
                            }
                        }
                    }
                }
            }
            var prinRoles = await this.GetUserRoles(admObj);
            return prinRoles.Count > 0 ? prinRoles.Distinct().ToList() : prinRoles;
        }

        private async Task<List<string>> GetUserRoles(string adminString)
        {
            var adminRoles = await Task.Run(() => this.QueryForAdminRoles(adminString));
            return adminRoles;
        }

        private List<string> QueryForAdminRoles(string adminIds)
        {
            var roles = new List<string>();
            if (!string.IsNullOrWhiteSpace(adminIds))
            {
                foreach (string s in adminIds.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    WqlConnectionManager wql = Methods.NewSMSProvider();
                    string adminQuery = string.Format(Constants.SMS_ADMIN_QUERY, s);
                    IResultObject admRes = wql.QueryProcessor.ExecuteQuery(adminQuery);
                    foreach (IResultObject o in admRes)
                    {
                        roles.AddRange(o[Constants.ROLE_NAMES].StringArrayValue);
                    }
                }
            }
            return roles;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Negotiate");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }
    }
}