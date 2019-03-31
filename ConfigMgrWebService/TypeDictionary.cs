using Microsoft.ConfigurationManagement.AdminConsole.Schema;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;

namespace ConfigMgrWebService
{
    public static class TypeDictionary
    {
        public static object GetWqlValue(IQueryPropertyItem si)
        {
            object retObj = null;
            for (int i = 0; i < TypeKeys.Count; i++)
            {
                TypeKey tk = TypeKeys[i];
                if (si.ObjectValue != null && !(si.ObjectValue is ManagementBaseObject[]) && tk.IsArray == si.IsArray && tk.TypeOfData == si.DataType)
                {
                    PropertyInfo pi = si.GetType().GetProperty(tk.McProperty);
                    retObj = pi.GetValue(si);
                    break;
                }
                else if (si.ObjectValue != null && si.ObjectValue is ManagementBaseObject[] mbos)
                {
                    retObj = mbos;
                    break;
                }
            }
            return retObj;
        }

        public static object GetMboValue(PropertyData pd, PropertyInfo pi)
        {
            object retObj = null;
            MethodInfo mi = typeof(TypeDictionary).GetMethod("Cast", BindingFlags.NonPublic | BindingFlags.Static);
            for (int i = 0; i < TypeKeys.Count; i++)
            {
                TypeKey tk = TypeKeys[i];
                if (pd.Value != null && pd.IsArray == tk.IsArray && pd.Type == tk.CimType)
                {
                    var genMeth = mi.MakeGenericMethod(pi.PropertyType);
                    retObj = genMeth.Invoke(null, new object[1] { pd.Value });
                    break;
                }

            }
            return retObj;
        }

        private static T Cast<T>(dynamic o) => (T)o;

        private static readonly List<TypeKey> TypeKeys = new List<TypeKey>(17)
        {
            new TypeKey(false, ManagementClassPropertyDescription.TypeOfData.String, CimType.String, "StringValue"),
            new TypeKey(true, ManagementClassPropertyDescription.TypeOfData.String, CimType.String, "StringArrayValue"),
            new TypeKey(false, ManagementClassPropertyDescription.TypeOfData.Boolean, CimType.Boolean, "BooleanValue"),
            new TypeKey(false, ManagementClassPropertyDescription.TypeOfData.Integer, CimType.SInt32, "IntegerValue"),
            new TypeKey(false, ManagementClassPropertyDescription.TypeOfData.InvariantInteger, CimType.UInt32, "IntegerValue"),
            new TypeKey(true, ManagementClassPropertyDescription.TypeOfData.Integer, CimType.SInt32, "IntegerArrayValue"),
            new TypeKey(true, ManagementClassPropertyDescription.TypeOfData.InvariantInteger, CimType.UInt32, "IntegerArrayValue"),
            new TypeKey(false, ManagementClassPropertyDescription.TypeOfData.Long, CimType.SInt64, "LongValue"),
            new TypeKey(false, ManagementClassPropertyDescription.TypeOfData.InvariantLong, CimType.UInt64, "LongValue"),
            new TypeKey(true, ManagementClassPropertyDescription.TypeOfData.InvariantLong, CimType.UInt64, "IntegerArrayValue"),
            new TypeKey(false, ManagementClassPropertyDescription.TypeOfData.DateTime, CimType.DateTime, "DateTimeValue"),
            new TypeKey(false, ManagementClassPropertyDescription.TypeOfData.DateOnly, CimType.DateTime, "DateTimeValue"),
            new TypeKey(true, ManagementClassPropertyDescription.TypeOfData.DateTime, CimType.DateTime, "DateTimeArrayValue"),
            new TypeKey(false, ManagementClassPropertyDescription.TypeOfData.TimeOnly, CimType.DateTime, "TimeValue"),
            new TypeKey(false, ManagementClassPropertyDescription.TypeOfData.TimeSpan, CimType.Object, "TimeSpanValue"),
            new TypeKey(false, ManagementClassPropertyDescription.TypeOfData.TimeOnlyLocale, CimType.Object, "TimeValue"),
            new TypeKey(false, ManagementClassPropertyDescription.TypeOfData.DateTimeLocale, CimType.DateTime, "DateTimeValue"),
            new TypeKey(false, ManagementClassPropertyDescription.TypeOfData.None, CimType.Object, "ObjectValue"),
            new TypeKey(true, ManagementClassPropertyDescription.TypeOfData.None, CimType.Object, "ObjectArrayValue"),
        };

        private class TypeKey
        {
            public CimType CimType { get; }
            public ManagementClassPropertyDescription.TypeOfData TypeOfData { get; }

            public bool IsArray { get; }

            public string McProperty { get; }

            public TypeKey(bool isArray, ManagementClassPropertyDescription.TypeOfData typeofData, CimType cimType, string mcProp)
            {
                this.CimType = cimType;
                this.TypeOfData = typeofData;
                this.McProperty = mcProp;
                this.IsArray = isArray;
            }
        }
    }

    
}
