using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MG.Sccm.Api
{
    public static class SccmSerialization
    {
        private static readonly Type[] GuidTypes = new Type[2]
        {
            typeof(Guid), typeof(Guid?)
        };

        /// <summary>
        /// Deserializes a given <see cref="IResultObject"/> into an <see cref="IJsonObject"/>-inherited class.
        /// Properties from the <see cref="IResultObject"/> should be matched with <see cref="SccmAttribute"/>s on the generic class's properties.
        /// </summary>
        /// <typeparam name="T">The type of class to deserialize the <see cref="IResultObject"/> into.</typeparam>
        /// <param name="iResultObject">The SCCM WMI query result object to deserialize.</param>
        public static T DeserializeIResult<T>(IResultObject iResultObject, SmsConnection connection = null) where T : BaseWmiObject
        {
            iResultObject.Get();
            var newTee = (T)Activator.CreateInstance(typeof(T), new object[1] { iResultObject.ConnectionManager });

            //FieldInfo ctxProp = typeof(T).GetField("_cmb", BindingFlags.NonPublic | BindingFlags.Instance);
            //ctxProp.SetValue(newTee, iResultObject.ConnectionManager);

            // Get all properties of T that have attributes (excluding JsonIgnoreAttribute).
            var writableProps = typeof(T).GetProperties().Where(
                x => x.GetCustomAttributes().Count() != 0).ToList();

            // Build a dictionary of PropertyInfo items and possible IResultObject property names.
            Dictionary<PropertyInfo, KeyValuePair<string, string>> propsAndAtts = GetPropsAndAtts(writableProps);

            for (int i = 0; i < iResultObject.PropertyNames.Length; i++)
            {
                string propName = iResultObject.PropertyNames[i];

                // If any of the properties from T match the name directly or a name defined in a SccmRealNameAttribute, then proceed to get its value.
                if (propsAndAtts.Any(
                    x => x.Key.Name.Equals(propName) || (!string.IsNullOrEmpty(x.Value.Value) && x.Value.Value.Equals(propName))))
                {
                    IQueryPropertyItem value = iResultObject[propName];

                    var maybe = propsAndAtts.First(
                        x => x.Key.Name.Equals(propName) ||
                            (!string.IsNullOrEmpty(x.Value.Value) && x.Value.Value.Equals(propName)));

                    object realValue = null;

                    // If the T property is a Guid, then parse the string value into one.
                    if (GuidTypes.Contains(maybe.Key.PropertyType) &&
                        Guid.TryParse(value.StringValue, out Guid guid))
                    {
                        realValue = guid;
                    }
                    // If the T property is a Uri, then parse the string value into one.
                    else if (maybe.Key.PropertyType.Equals(typeof(Uri)) &&
                        Uri.TryCreate(value.StringValue, UriKind.RelativeOrAbsolute, out Uri uri))
                    {
                        realValue = uri;
                    }
                    // ...Else, find the matching IQueryPropertyItem property that matches the PropertyName of the SccmAttribute and gets its value.
                    else
                    {
                        PropertyInfo iqProp = typeof(IQueryPropertyItem).GetProperty(maybe.Value.Key);
                        realValue = iqProp.GetValue(value);
                    }

                    maybe.Key.SetValue(newTee, realValue); // Set the new T object's property's value with the retrieved value from the IQueryPropertyItem.
                }
                else
                {
                    object obj = iResultObject[propName].ObjectValue;
                    if (obj != null)
                    {
                        var jtok = JToken.FromObject(obj);
                        newTee.ExtraData.Add(propName, jtok);
                    }
                }
            }

            if (typeof(T).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Any(x => x.Name.Equals("OnSccmDeserialized")))
            {
                MethodInfo mi = typeof(T).GetMethod("OnSccmDeserialized", BindingFlags.NonPublic | BindingFlags.Instance);
                if (mi != null)
                {
                    if (connection != null)
                        mi.Invoke(newTee, new object[1] { connection });

                    else
                        mi.Invoke(newTee, null);
                }
            }

            return newTee;  // All properties in T should be populated, and anything else not mapped is in 'ExtraData'.
        }

        private static Dictionary<PropertyInfo, KeyValuePair<string, string>> GetPropsAndAtts(List<PropertyInfo> allProps)
        {
            var dict = new Dictionary<PropertyInfo, KeyValuePair<string, string>>(allProps.Count);
            for (int i = allProps.Count - 1; i >= 0; i--)
            {
                PropertyInfo one = allProps[i];

                // If the given PropertyInfo does NOT have a SccmAttribute-derived Attribute defined, then skip it.
                if (!one.GetCustomAttributes().Any(x => x.GetType().BaseType.Equals(typeof(SccmAttribute))))
                    continue;

                // Retrieve the SccmAttribute from the property.
                var kAtt = one.GetCustomAttribute<SccmAttribute>();

                // Populate the dictionary with the PropertyInfo, and another KVP of the IQueryPropertyItem property to query and an alternative 
                // property name if one was defined.
                dict.Add(one, new KeyValuePair<string, string>(kAtt.Property, kAtt.PropertyName));
                //allProps.Remove(one);
            }
            return dict;
        }
    }
}