using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MG.Sccm.Api
{
    #region ATTRIBUTES

    /// <summary>
    /// The base <see cref="Attribute"/> class for the mapping of <see cref="IQueryPropertyItem"/> properties.
    /// </summary>
    public abstract class SccmAttribute : Attribute
    {
        /// <summary>
        /// *NOT USED* - The <see cref="Type"/> the <see cref="IQueryPropertyItem"/>'s value should cast into.
        /// </summary>
        public abstract Type CastType { get; }
        /// <summary>
        /// The property name of a <see cref="IQueryPropertyItem"/> that will be called through Reflection to retrieve its value.
        /// </summary>
        public abstract string Property { get; }

        public string PropertyName { get; }

        public SccmAttribute() { }
        public SccmAttribute(string propertyName) => this.PropertyName = propertyName;
    }

    /// <summary>
    /// Designates the given property will gets its value from the "BooleanArrayValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmBoolArrayAttribute : SccmAttribute
    {
        public override Type CastType => typeof(bool[]);
        public override string Property => "BooleanArrayValue";
        public SccmBoolArrayAttribute() : base() { }
        public SccmBoolArrayAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Designates the given property will gets its value from the "BooleanValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmBoolAttribute : SccmAttribute
    {
        public override Type CastType => typeof(bool);
        public override string Property => "BooleanValue";
        public SccmBoolAttribute() : base() { }
        public SccmBoolAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Designates the given property will gets its value from the "ByteArrayValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmByteArrayAttribute : SccmAttribute
    {
        public override Type CastType => typeof(byte[]);
        public override string Property => "ByteArrayValue";
        public SccmByteArrayAttribute() : base() { }
        public SccmByteArrayAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Designates the given property will gets its value from the "CombinedStringValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmCombinedStringAttribute : SccmAttribute
    {
        public override Type CastType => typeof(string);
        public override string Property => "CombinedStringValue";
        public SccmCombinedStringAttribute() : base() { }
        public SccmCombinedStringAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Designates the given property will gets its value from the "DateTimeArrayValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmDateTimeArrayAttribute : SccmAttribute
    {
        public override Type CastType => typeof(DateTime[]);
        public override string Property => "DateTimeArrayValue";
        public SccmDateTimeArrayAttribute() : base() { }
        public SccmDateTimeArrayAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Designates the given property will gets its value from the "DateTimeValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmDateTimeAttribute : SccmAttribute
    {
        public override Type CastType => typeof(DateTime);
        public override string Property => "DateTimeValue";
        public SccmDateTimeAttribute() : base() { }
        public SccmDateTimeAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Designates the given property will gets its value from the "IntegerArrayValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmIntegerArrayAttribute : SccmAttribute
    {
        public override Type CastType => typeof(int[]);
        public override string Property => "IntegerArrayValue";
        public SccmIntegerArrayAttribute() : base() { }
        public SccmIntegerArrayAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Designates the given property will gets its value from the "IntegerValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmIntegerAttribute : SccmAttribute
    {
        public override Type CastType => typeof(int);
        public override string Property => "IntegerValue";
        public SccmIntegerAttribute() : base() { }
        public SccmIntegerAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Designates the given property will gets its value from the "LongValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmLongAttribute : SccmAttribute
    {
        public override Type CastType => typeof(long);
        public override string Property => "LongValue";
        public SccmLongAttribute() : base() { }
        public SccmLongAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Indicates that the <see cref="IQueryPropertyItem"/> name and the mapped property name do not match.  Therefore,
    /// the given RealName should be searched for instead.
    /// </summary>
    //public class SccmRealNameAttribute : Attribute
    //{
    //    /// <summary>
    //    /// The real name of the <see cref="IQueryPropertyItem"/> to match to.
    //    /// </summary>
    //    public string RealName { get; }
    //    /// <summary>
    //    /// The default constructor to initialize this <see cref="Attribute"/>.
    //    /// </summary>
    //    /// <param name="realName">The real name of the <see cref="IQueryPropertyItem"/> to match to.</param>
    //    public SccmRealNameAttribute(string realName) => this.RealName = realName;
    //}

    /// <summary>
    /// Designates the given property will gets its value from the "ObjectArrayValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmObjectArrayAttribute : SccmAttribute
    {
        public override Type CastType => typeof(object[]);
        public override string Property => "ObjectArrayValue";
        public SccmObjectArrayAttribute() : base() { }
        public SccmObjectArrayAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Designates the given property will gets its value from the "ObjectValue" property of <see cref="IQueryPropertyItem"/>.
    /// This should be used when the returned value may be null.
    /// </summary>
    public class SccmObjectAttribute : SccmAttribute
    {
        public override Type CastType => typeof(object);
        public override string Property => "ObjectValue";
        public SccmObjectAttribute() : base() { }
        public SccmObjectAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Designates the given property will gets its value from the "StringArrayValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmStringArrayAttribute : SccmAttribute
    {
        public override Type CastType => typeof(string[]);
        public override string Property => "StringArrayValue";
        public SccmStringArrayAttribute() : base() { }
        public SccmStringArrayAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Designates the given property will gets its value from the "StringValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmStringAttribute : SccmAttribute
    {
        public override Type CastType => typeof(string);
        public override string Property => "StringValue";
        public SccmStringAttribute() : base() { }
        public SccmStringAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Designates the given property will gets its value from the "TimeSpanValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmTimeSpanAttribute : SccmAttribute
    {
        public override Type CastType => typeof(TimeSpan);
        public override string Property => "TimeSpanValue";
        public SccmTimeSpanAttribute() : base() { }
        public SccmTimeSpanAttribute(string propertyName) : base(propertyName) { }
    }

    /// <summary>
    /// Designates the given property will gets its value from the "TimeValue" property of <see cref="IQueryPropertyItem"/>.
    /// </summary>
    public class SccmTimeAttribute : SccmAttribute
    {
        public override Type CastType => typeof(DateTime);
        public override string Property => "TimeValue";
        public SccmTimeAttribute() : base() { }
        public SccmTimeAttribute(string propertyName) : base(propertyName) { }
    }

    #endregion
}