﻿

using System;

namespace ProtoBuf
{
    public static class BclHelpers
    {
        private const int FieldTimeSpanValue = 1;
        private const int FieldTimeSpanScale = 2;
        internal static readonly DateTime EpochOrigin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private const int FieldDecimalLow = 1;
        private const int FieldDecimalHigh = 2;
        private const int FieldDecimalSignScale = 3;
        private const int FieldGuidLow = 1;
        private const int FieldGuidHigh = 2;
        private const int FieldExistingObjectKey = 1;
        private const int FieldNewObjectKey = 2;
        private const int FieldExistingTypeKey = 3;
        private const int FieldNewTypeKey = 4;
        private const int FieldTypeName = 8;
        private const int FieldObject = 10;

        public static object GetUninitializedObject(Type type) => throw new NotSupportedException("Constructor-skipping is not supported on this platform");

        public static void WriteTimeSpan(TimeSpan timeSpan, ProtoWriter dest)
        {
            if (dest == null)
                throw new ArgumentNullException(nameof(dest));
            switch (dest.WireType)
            {
                case WireType.Fixed64:
                    ProtoWriter.WriteInt64(timeSpan.Ticks, dest);
                    break;
                case WireType.String:
                case WireType.StartGroup:
                    long num = timeSpan.Ticks;
                    TimeSpanScale timeSpanScale;
                    if (timeSpan == TimeSpan.MaxValue)
                    {
                        num = 1L;
                        timeSpanScale = TimeSpanScale.MinMax;
                    }
                    else if (timeSpan == TimeSpan.MinValue)
                    {
                        num = -1L;
                        timeSpanScale = TimeSpanScale.MinMax;
                    }
                    else if (num % 864000000000L == 0L)
                    {
                        timeSpanScale = TimeSpanScale.Days;
                        num /= 864000000000L;
                    }
                    else if (num % 36000000000L == 0L)
                    {
                        timeSpanScale = TimeSpanScale.Hours;
                        num /= 36000000000L;
                    }
                    else if (num % 600000000L == 0L)
                    {
                        timeSpanScale = TimeSpanScale.Minutes;
                        num /= 600000000L;
                    }
                    else if (num % 10000000L == 0L)
                    {
                        timeSpanScale = TimeSpanScale.Seconds;
                        num /= 10000000L;
                    }
                    else if (num % 10000L == 0L)
                    {
                        timeSpanScale = TimeSpanScale.Milliseconds;
                        num /= 10000L;
                    }
                    else
                        timeSpanScale = TimeSpanScale.Ticks;
                    SubItemToken token = ProtoWriter.StartSubItem((object)null, dest);
                    if ((ulong)num > 0UL)
                    {
                        ProtoWriter.WriteFieldHeader(1, WireType.SignedVariant, dest);
                        ProtoWriter.WriteInt64(num, dest);
                    }
                    if ((uint)timeSpanScale > 0U)
                    {
                        ProtoWriter.WriteFieldHeader(2, WireType.Variant, dest);
                        ProtoWriter.WriteInt32((int)timeSpanScale, dest);
                    }
                    ProtoWriter.EndSubItem(token, dest);
                    break;
                default:
                    throw new ProtoException("Unexpected wire-type: " + dest.WireType.ToString());
            }
        }

        public static TimeSpan ReadTimeSpan(ProtoReader source)
        {
            long num = BclHelpers.ReadTimeSpanTicks(source);
            switch (num)
            {
                case long.MinValue:
                    return TimeSpan.MinValue;
                case long.MaxValue:
                    return TimeSpan.MaxValue;
                default:
                    return TimeSpan.FromTicks(num);
            }
        }

        public static DateTime ReadDateTime(ProtoReader source)
        {
            long num = BclHelpers.ReadTimeSpanTicks(source);
            switch (num)
            {
                case long.MinValue:
                    return DateTime.MinValue;
                case long.MaxValue:
                    return DateTime.MaxValue;
                default:
                    return BclHelpers.EpochOrigin.AddTicks(num);
            }
        }

        public static void WriteDateTime(DateTime value, ProtoWriter dest)
        {
            if (dest == null)
                throw new ArgumentNullException(nameof(dest));
            TimeSpan timeSpan;
            switch (dest.WireType)
            {
                case WireType.String:
                case WireType.StartGroup:
                    timeSpan = !(value == DateTime.MaxValue) ? (!(value == DateTime.MinValue) ? value - BclHelpers.EpochOrigin : TimeSpan.MinValue) : TimeSpan.MaxValue;
                    break;
                default:
                    timeSpan = value - BclHelpers.EpochOrigin;
                    break;
            }
            BclHelpers.WriteTimeSpan(timeSpan, dest);
        }

        private static long ReadTimeSpanTicks(ProtoReader source)
        {
            switch (source.WireType)
            {
                case WireType.Fixed64:
                    return source.ReadInt64();
                case WireType.String:
                case WireType.StartGroup:
                    SubItemToken token = ProtoReader.StartSubItem(source);
                    TimeSpanScale timeSpanScale = TimeSpanScale.Days;
                    long num1 = 0;
                    int num2;
                    while ((num2 = source.ReadFieldHeader()) > 0)
                    {
                        switch (num2)
                        {
                            case 1:
                                source.Assert(WireType.SignedVariant);
                                num1 = source.ReadInt64();
                                break;
                            case 2:
                                timeSpanScale = (TimeSpanScale)source.ReadInt32();
                                break;
                            default:
                                source.SkipField();
                                break;
                        }
                    }
                    ProtoReader.EndSubItem(token, source);
                    switch (timeSpanScale)
                    {
                        case TimeSpanScale.Days:
                            return num1 * 864000000000L;
                        case TimeSpanScale.Hours:
                            return num1 * 36000000000L;
                        case TimeSpanScale.Minutes:
                            return num1 * 600000000L;
                        case TimeSpanScale.Seconds:
                            return num1 * 10000000L;
                        case TimeSpanScale.Milliseconds:
                            return num1 * 10000L;
                        case TimeSpanScale.Ticks:
                            return num1;
                        case TimeSpanScale.MinMax:
                            switch (num1)
                            {
                                case -1:
                                    return long.MinValue;
                                case 1:
                                    return long.MaxValue;
                                default:
                                    throw new ProtoException("Unknown min/max value: " + num1.ToString());
                            }
                        default:
                            throw new ProtoException("Unknown timescale: " + timeSpanScale.ToString());
                    }
                default:
                    throw new ProtoException("Unexpected wire-type: " + source.WireType.ToString());
            }
        }

        public static Decimal ReadDecimal(ProtoReader reader)
        {
            ulong num1 = 0;
            uint num2 = 0;
            uint num3 = 0;
            SubItemToken token = ProtoReader.StartSubItem(reader);
            int num4;
            while ((num4 = reader.ReadFieldHeader()) > 0)
            {
                switch (num4)
                {
                    case 1:
                        num1 = reader.ReadUInt64();
                        break;
                    case 2:
                        num2 = reader.ReadUInt32();
                        break;
                    case 3:
                        num3 = reader.ReadUInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
            ProtoReader.EndSubItem(token, reader);
            return num1 == 0UL && num2 == 0U ? 0M : new Decimal((int)((long)num1 & (long)uint.MaxValue), (int)((long)(num1 >> 32) & (long)uint.MaxValue), (int)num2, ((int)num3 & 1) == 1, (byte)((num3 & 510U) >> 1));
        }

        public static void WriteDecimal(Decimal value, ProtoWriter writer)
        {
            int[] bits = Decimal.GetBits(value);
            ulong num1 = (ulong)bits[1] << 32 | (ulong)bits[0] & (ulong)uint.MaxValue;
            uint num2 = (uint)bits[2];
            uint num3 = (uint)(bits[3] >> 15 & 510 | bits[3] >> 31 & 1);
            SubItemToken token = ProtoWriter.StartSubItem((object)null, writer);
            if (num1 > 0UL)
            {
                ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
                ProtoWriter.WriteUInt64(num1, writer);
            }
            if (num2 > 0U)
            {
                ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
                ProtoWriter.WriteUInt32(num2, writer);
            }
            if (num3 > 0U)
            {
                ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
                ProtoWriter.WriteUInt32(num3, writer);
            }
            ProtoWriter.EndSubItem(token, writer);
        }

        public static void WriteGuid(Guid value, ProtoWriter dest)
        {
            byte[] byteArray = value.ToByteArray();
            SubItemToken token = ProtoWriter.StartSubItem((object)null, dest);
            if (value != Guid.Empty)
            {
                ProtoWriter.WriteFieldHeader(1, WireType.Fixed64, dest);
                ProtoWriter.WriteBytes(byteArray, 0, 8, dest);
                ProtoWriter.WriteFieldHeader(2, WireType.Fixed64, dest);
                ProtoWriter.WriteBytes(byteArray, 8, 8, dest);
            }
            ProtoWriter.EndSubItem(token, dest);
        }

        public static Guid ReadGuid(ProtoReader source)
        {
            ulong num1 = 0;
            ulong num2 = 0;
            SubItemToken token = ProtoReader.StartSubItem(source);
            int num3;
            while ((num3 = source.ReadFieldHeader()) > 0)
            {
                switch (num3)
                {
                    case 1:
                        num1 = source.ReadUInt64();
                        break;
                    case 2:
                        num2 = source.ReadUInt64();
                        break;
                    default:
                        source.SkipField();
                        break;
                }
            }
            ProtoReader.EndSubItem(token, source);
            if (num1 == 0UL && num2 == 0UL)
                return Guid.Empty;
            uint num4 = (uint)(num1 >> 32);
            uint num5 = (uint)num1;
            uint num6 = (uint)(num2 >> 32);
            uint num7 = (uint)num2;
            return new Guid((int)num5, (short)num4, (short)(num4 >> 16), (byte)num7, (byte)(num7 >> 8), (byte)(num7 >> 16), (byte)(num7 >> 24), (byte)num6, (byte)(num6 >> 8), (byte)(num6 >> 16), (byte)(num6 >> 24));
        }

        public static object ReadNetObject(
          object value,
          ProtoReader source,
          int key,
          Type type,
          BclHelpers.NetObjectOptions options)
        {
            SubItemToken token = ProtoReader.StartSubItem(source);
            int num1 = -1;
            int key1 = -1;
            int num2;
            while ((num2 = source.ReadFieldHeader()) > 0)
            {
                switch (num2)
                {
                    case 1:
                        int key2 = source.ReadInt32();
                        value = source.NetCache.GetKeyedObject(key2);
                        break;
                    case 2:
                        num1 = source.ReadInt32();
                        break;
                    case 3:
                        int key3 = source.ReadInt32();
                        type = (Type)source.NetCache.GetKeyedObject(key3);
                        key = source.GetTypeKey(ref type);
                        break;
                    case 4:
                        key1 = source.ReadInt32();
                        break;
                    case 8:
                        string str = source.ReadString();
                        type = source.DeserializeType(str);
                        if (type == null)
                            throw new ProtoException("Unable to resolve type: " + str + " (you can use the TypeModel.DynamicTypeFormatting event to provide a custom mapping)");
                        if (type == typeof(string))
                        {
                            key = -1;
                            break;
                        }
                        key = source.GetTypeKey(ref type);
                        if (key < 0)
                            throw new InvalidOperationException("Dynamic type is not a contract-type: " + type.Name);
                        break;
                    case 10:
                        bool flag1 = type == typeof(string);
                        bool flag2 = value == null;
                        bool flag3 = flag2 && (flag1 || (uint)(options & BclHelpers.NetObjectOptions.LateSet) > 0U);
                        if (num1 >= 0 && !flag3)
                        {
                            if (value == null)
                                source.TrapNextObject(num1);
                            else
                                source.NetCache.SetKeyedObject(num1, value);
                            if (key1 >= 0)
                                source.NetCache.SetKeyedObject(key1, (object)type);
                        }
                        object obj = value;
                        value = !flag1 ? ProtoReader.ReadTypedObject(obj, key, source, type) : (object)source.ReadString();
                        if (num1 >= 0)
                        {
                            if (flag2 && !flag3)
                                obj = source.NetCache.GetKeyedObject(num1);
                            if (flag3)
                            {
                                source.NetCache.SetKeyedObject(num1, value);
                                if (key1 >= 0)
                                    source.NetCache.SetKeyedObject(key1, (object)type);
                            }
                        }
                        if (num1 >= 0 && !flag3 && obj != value)
                            throw new ProtoException("A reference-tracked object changed reference during deserialization");
                        if (num1 < 0 && key1 >= 0)
                        {
                            source.NetCache.SetKeyedObject(key1, (object)type);
                            break;
                        }
                        break;
                    default:
                        source.SkipField();
                        break;
                }
            }
            if (num1 >= 0 && (options & BclHelpers.NetObjectOptions.AsReference) == BclHelpers.NetObjectOptions.None)
                throw new ProtoException("Object key in input stream, but reference-tracking was not expected");
            ProtoReader.EndSubItem(token, source);
            return value;
        }

        public static void WriteNetObject(
          object value,
          ProtoWriter dest,
          int key,
          BclHelpers.NetObjectOptions options)
        {
            if (dest == null)
                throw new ArgumentNullException(nameof(dest));
            bool flag1 = (uint)(options & BclHelpers.NetObjectOptions.DynamicType) > 0U;
            bool flag2 = (uint)(options & BclHelpers.NetObjectOptions.AsReference) > 0U;
            WireType wireType = dest.WireType;
            SubItemToken token = ProtoWriter.StartSubItem((object)null, dest);
            bool flag3 = true;
            if (flag2)
            {
                bool existing;
                int num = dest.NetCache.AddObjectKey(value, out existing);
                ProtoWriter.WriteFieldHeader(existing ? 1 : 2, WireType.Variant, dest);
                ProtoWriter.WriteInt32(num, dest);
                if (existing)
                    flag3 = false;
            }
            if (flag3)
            {
                if (flag1)
                {
                    Type type = value.GetType();
                    if (!(value is string))
                    {
                        key = dest.GetTypeKey(ref type);
                        if (key < 0)
                            throw new InvalidOperationException("Dynamic type is not a contract-type: " + type.Name);
                    }
                    bool existing;
                    int num = dest.NetCache.AddObjectKey((object)type, out existing);
                    ProtoWriter.WriteFieldHeader(existing ? 3 : 4, WireType.Variant, dest);
                    ProtoWriter.WriteInt32(num, dest);
                    if (!existing)
                    {
                        ProtoWriter.WriteFieldHeader(8, WireType.String, dest);
                        ProtoWriter.WriteString(dest.SerializeType(type), dest);
                    }
                }
                ProtoWriter.WriteFieldHeader(10, wireType, dest);
                if (value is string)
                    ProtoWriter.WriteString((string)value, dest);
                else
                    ProtoWriter.WriteObject(value, key, dest);
            }
            ProtoWriter.EndSubItem(token, dest);
        }

        [Flags]
        public enum NetObjectOptions : byte
        {
            None = 0,
            AsReference = 1,
            DynamicType = 2,
            UseConstructor = 4,
            LateSet = 8,
        }
    }
}
