using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ConfigMgr.WebApi
{
    public class SimpleAuth : ActionFilterAttribute, IAuthenticationFilter
    {
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Basic");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // 1. Look for key in the request.
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue auth = request.Headers.Authorization;

            // 2. If there is no key, do nothing.
            if (auth == null)
                return;

            // 3. If there is a key, but the filter doesn't recognize the scheme, do nothing.
            //if (auth.Scheme != "Basic")
            //    return;

            if (auth.Scheme != "Basic")
                return;

            // 4. If there is a key that the filter understands, try to validate it.
            // 5. If the key is bad, set the error result.
            if (string.IsNullOrEmpty(auth.Parameter))
            {
                return;
            }

            string apiKey = auth.Parameter;
            IPrincipal prin = await ValidateApiKey(apiKey);
            if (prin != null)
            {
                context.Principal = prin;
            }
        }
        private Task<IPrincipal> ValidateApiKey(string key)
        {
            string configKey = ConfigurationManager.AppSettings["ApiKey"];
            byte[] keyBytes = Convert.FromBase64String(key);
            string realKey = Encoding.UTF8.GetString(keyBytes);
            return Task.FromResult(KeyToAuth(configKey, realKey));
        }

        private IPrincipal KeyToAuth(string configKey, string passedKey)
        {
            IPrincipal prin = null;
            if (configKey.Equals(passedKey))
            {
                var id = new GenericIdentity("TaskSequenceApi", "Basic");
                prin = new ApiPrincipal(id, new string[1] { "Full Administrator" });
            }
            return prin;
        }
    }

    public class AddChallengeOnUnauthorizedResult : IHttpActionResult
    {
        public AuthenticationHeaderValue Challenge { get; private set; }
        public IHttpActionResult InnerResult { get; private set; }

        public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            Challenge = challenge;
            InnerResult = innerResult;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancelToken)
        {
            HttpResponseMessage response = await InnerResult.ExecuteAsync(cancelToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (!response.Headers.WwwAuthenticate.Any((h) => h.Scheme == Challenge.Scheme))
                {
                    response.Headers.WwwAuthenticate.Add(Challenge);
                }
            }
            return response;
        }
    }
}