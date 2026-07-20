using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using Microsoft.AspNetCore.Http;
using System;

namespace AppLogistics.Tests;

public class AllTypesView : BaseView
{
    public TestEnum EnumField { get; set; }
    public sbyte SByteField { get; set; }
    public byte ByteField { get; set; }
    public short Int16Field { get; set; }
    public ushort UInt16Field { get; set; }
    public int Int32Field { get; set; }
    public uint UInt32Field { get; set; }
    public long Int64Field { get; set; }
    public ulong UInt64Field { get; set; }
    public float SingleField { get; set; }
    public double DoubleField { get; set; }
    public decimal DecimalField { get; set; }
    public bool BooleanField { get; set; }
    public DateTime DateTimeField { get; set; }

    public TestEnum? NullableEnumField { get; set; }
    public sbyte? NullableSByteField { get; set; }
    public byte? NullableByteField { get; set; }
    public short? NullableInt16Field { get; set; }
    public ushort? NullableUInt16Field { get; set; }
    public int? NullableInt32Field { get; set; }
    public uint? NullableUInt32Field { get; set; }
    public long? NullableInt64Field { get; set; }
    public ulong? NullableUInt64Field { get; set; }
    public float? NullableSingleField { get; set; }
    public double? NullableDoubleField { get; set; }
    public decimal? NullableDecimalField { get; set; }
    public bool? NullableBooleanField { get; set; }
    public DateTime? NullableDateTimeField { get; set; }

    public string StringField { get; set; }

    public IFormFile FileField { get; set; }

    [Truncated]
    public DateTime TruncatedDateTimeField { get; set; }

    public AllTypesView Child { get; set; }
}

public enum TestEnum
{
    First,
    Second
}
