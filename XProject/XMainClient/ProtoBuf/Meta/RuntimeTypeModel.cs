﻿

using ProtoBuf.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ProtoBuf.Meta
{
    public sealed class RuntimeTypeModel : TypeModel
    {
        private byte options;
        private const byte OPTIONS_InferTagFromNameDefault = 1;
        private const byte OPTIONS_IsDefaultModel = 2;
        private const byte OPTIONS_Frozen = 4;
        private const byte OPTIONS_AutoAddMissingTypes = 8;
        private const byte OPTIONS_UseImplicitZeroDefaults = 32;
        private const byte OPTIONS_AllowParseableTypes = 64;
        private const byte OPTIONS_AutoAddProtoContractTypesOnly = 128;
        private static readonly BasicList.MatchPredicate MetaTypeFinder = new BasicList.MatchPredicate(RuntimeTypeModel.MetaTypeFinderImpl);
        private static readonly BasicList.MatchPredicate BasicTypeFinder = new BasicList.MatchPredicate(RuntimeTypeModel.BasicTypeFinderImpl);
        private BasicList basicTypes = new BasicList();
        private readonly BasicList types = new BasicList();
        private int metadataTimeoutMilliseconds = 5000;
        private int lockCount;
        private int contentionCounter = 1;
        private MethodInfo defaultFactory;

        private bool GetOption(byte option) => ((int)this.options & (int)option) == (int)option;

        private void SetOption(byte option, bool value)
        {
            if (value)
                this.options |= option;
            else
                this.options &= (byte)~option;
        }

        public bool InferTagFromNameDefault
        {
            get => this.GetOption((byte)1);
            set => this.SetOption((byte)1, value);
        }

        public bool AutoAddProtoContractTypesOnly
        {
            get => this.GetOption((byte)128);
            set => this.SetOption((byte)128, value);
        }

        public bool UseImplicitZeroDefaults
        {
            get => this.GetOption((byte)32);
            set
            {
                if (!value && this.GetOption((byte)2))
                    throw new InvalidOperationException("UseImplicitZeroDefaults cannot be disabled on the default model");
                this.SetOption((byte)32, value);
            }
        }

        public bool AllowParseableTypes
        {
            get => this.GetOption((byte)64);
            set => this.SetOption((byte)64, value);
        }

        public static void SetMultiThread(bool multiThread) => RuntimeTypeModel.Singleton.SetMultiThread(multiThread);

        public static RuntimeTypeModel Default => RuntimeTypeModel.Singleton.Value;

        public static RuntimeTypeModel ThreadDefault => RuntimeTypeModel.Singleton.DefaultValue;

        public IEnumerable GetTypes() => (IEnumerable)this.types;

        public override string GetSchema(Type type)
        {
            BasicList list = new BasicList();
            MetaType metaType1 = (MetaType)null;
            bool flag = false;
            if (type == null)
            {
                foreach (MetaType type1 in this.types)
                {
                    MetaType surrogateOrBaseOrSelf = type1.GetSurrogateOrBaseOrSelf(false);
                    if (!list.Contains((object)surrogateOrBaseOrSelf))
                    {
                        list.Add((object)surrogateOrBaseOrSelf);
                        this.CascadeDependents(list, surrogateOrBaseOrSelf);
                    }
                }
            }
            else
            {
                Type underlyingType = Helpers.GetUnderlyingType(type);
                if (underlyingType != null)
                    type = underlyingType;
                flag = ValueMember.TryGetCoreSerializer(this, DataFormat.Default, type, out WireType _, false, false, false, false) != null;
                if (!flag)
                {
                    int orAddAuto = this.FindOrAddAuto(type, false, false, false);
                    metaType1 = orAddAuto >= 0 ? ((MetaType)this.types[orAddAuto]).GetSurrogateOrBaseOrSelf(false) : throw new ArgumentException("The type specified is not a contract-type", nameof(type));
                    list.Add((object)metaType1);
                    this.CascadeDependents(list, metaType1);
                }
            }
            StringBuilder builder1 = new StringBuilder();
            string str1 = (string)null;
            if (!flag)
            {
                foreach (MetaType metaType2 in metaType1 == null ? (IEnumerable)this.types : (IEnumerable)list)
                {
                    if (!metaType2.IsList)
                    {
                        string str2 = metaType2.Type.Namespace;
                        if (!Helpers.IsNullOrEmpty(str2) && !str2.StartsWith("System."))
                        {
                            if (str1 == null)
                                str1 = str2;
                            else if (!(str1 == str2))
                            {
                                str1 = (string)null;
                                break;
                            }
                        }
                    }
                }
            }
            if (!Helpers.IsNullOrEmpty(str1))
            {
                builder1.Append("package ").Append(str1).Append(';');
                Helpers.AppendLine(builder1);
            }
            bool requiresBclImport = false;
            StringBuilder builder2 = new StringBuilder();
            MetaType[] array = new MetaType[list.Count];
            list.CopyTo((Array)array, 0);
            Array.Sort<MetaType>(array, (IComparer<MetaType>)MetaType.Comparer.Default);
            if (flag)
            {
                Helpers.AppendLine(builder2).Append("message ").Append(type.Name).Append(" {");
                MetaType.NewLine(builder2, 1).Append("optional ").Append(this.GetSchemaTypeName(type, DataFormat.Default, false, false, ref requiresBclImport)).Append(" value = 1;");
                Helpers.AppendLine(builder2).Append('}');
            }
            else
            {
                for (int index = 0; index < array.Length; ++index)
                {
                    MetaType metaType3 = array[index];
                    if (!metaType3.IsList || metaType3 == metaType1)
                        metaType3.WriteSchema(builder2, 0, ref requiresBclImport);
                }
            }
            if (requiresBclImport)
            {
                builder1.Append("import \"bcl.proto\"; // schema for protobuf-net's handling of core .NET types");
                Helpers.AppendLine(builder1);
            }
            return Helpers.AppendLine(builder1.Append((object)builder2)).ToString();
        }

        private void CascadeDependents(BasicList list, MetaType metaType)
        {
            if (metaType.IsList)
            {
                Type listItemType = TypeModel.GetListItemType((TypeModel)this, metaType.Type);
                if (ValueMember.TryGetCoreSerializer(this, DataFormat.Default, listItemType, out WireType _, false, false, false, false) != null)
                    return;
                int orAddAuto = this.FindOrAddAuto(listItemType, false, false, false);
                if (orAddAuto >= 0)
                {
                    MetaType surrogateOrBaseOrSelf = ((MetaType)this.types[orAddAuto]).GetSurrogateOrBaseOrSelf(false);
                    if (!list.Contains((object)surrogateOrBaseOrSelf))
                    {
                        list.Add((object)surrogateOrBaseOrSelf);
                        this.CascadeDependents(list, surrogateOrBaseOrSelf);
                    }
                }
            }
            else
            {
                if (metaType.IsAutoTuple)
                {
                    MemberInfo[] mappedMembers;
                    if (MetaType.ResolveTupleConstructor(metaType.Type, out mappedMembers) != null)
                    {
                        for (int index = 0; index < mappedMembers.Length; ++index)
                        {
                            Type type = (Type)null;
                            if (mappedMembers[index] is PropertyInfo)
                                type = ((PropertyInfo)mappedMembers[index]).PropertyType;
                            else if (mappedMembers[index] is FieldInfo)
                                type = ((FieldInfo)mappedMembers[index]).FieldType;
                            if (ValueMember.TryGetCoreSerializer(this, DataFormat.Default, type, out WireType _, false, false, false, false) == null)
                            {
                                int orAddAuto = this.FindOrAddAuto(type, false, false, false);
                                if (orAddAuto >= 0)
                                {
                                    MetaType surrogateOrBaseOrSelf = ((MetaType)this.types[orAddAuto]).GetSurrogateOrBaseOrSelf(false);
                                    if (!list.Contains((object)surrogateOrBaseOrSelf))
                                    {
                                        list.Add((object)surrogateOrBaseOrSelf);
                                        this.CascadeDependents(list, surrogateOrBaseOrSelf);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (ValueMember field in metaType.Fields)
                    {
                        Type type = field.ItemType ?? field.MemberType;
                        if (ValueMember.TryGetCoreSerializer(this, DataFormat.Default, type, out WireType _, false, false, false, false) == null)
                        {
                            int orAddAuto = this.FindOrAddAuto(type, false, false, false);
                            if (orAddAuto >= 0)
                            {
                                MetaType surrogateOrBaseOrSelf = ((MetaType)this.types[orAddAuto]).GetSurrogateOrBaseOrSelf(false);
                                if (!list.Contains((object)surrogateOrBaseOrSelf))
                                {
                                    list.Add((object)surrogateOrBaseOrSelf);
                                    this.CascadeDependents(list, surrogateOrBaseOrSelf);
                                }
                            }
                        }
                    }
                }
                if (metaType.HasSubtypes)
                {
                    foreach (SubType subtype in metaType.GetSubtypes())
                    {
                        MetaType surrogateOrSelf = subtype.DerivedType.GetSurrogateOrSelf();
                        if (!list.Contains((object)surrogateOrSelf))
                        {
                            list.Add((object)surrogateOrSelf);
                            this.CascadeDependents(list, surrogateOrSelf);
                        }
                    }
                }
                MetaType metaType1 = metaType.BaseType;
                if (metaType1 != null)
                    metaType1 = metaType1.GetSurrogateOrSelf();
                if (metaType1 != null && !list.Contains((object)metaType1))
                {
                    list.Add((object)metaType1);
                    this.CascadeDependents(list, metaType1);
                }
            }
        }

        internal RuntimeTypeModel(bool isDefault)
        {
            this.AutoAddMissingTypes = true;
            this.UseImplicitZeroDefaults = true;
            this.SetOption((byte)2, isDefault);
        }

        public MetaType this[Type type] => (MetaType)this.types[this.FindOrAddAuto(type, true, false, false)];

        internal MetaType FindWithoutAdd(Type type)
        {
            foreach (MetaType type1 in this.types)
            {
                if (type1.Type == type)
                {
                    if (type1.Pending)
                        this.WaitOnLock(type1);
                    return type1;
                }
            }
            Type type2 = TypeModel.ResolveProxies(type);
            return type2 == null ? (MetaType)null : this.FindWithoutAdd(type2);
        }

        private static bool MetaTypeFinderImpl(object value, object ctx) => ((MetaType)value).Type == (Type)ctx;

        private static bool BasicTypeFinderImpl(object value, object ctx) => ((RuntimeTypeModel.BasicType)value).Type == (Type)ctx;

        private void WaitOnLock(MetaType type)
        {
            int opaqueToken = 0;
            try
            {
                this.TakeLock(ref opaqueToken);
            }
            finally
            {
                this.ReleaseLock(opaqueToken);
            }
        }

        internal IProtoSerializer TryGetBasicTypeSerializer(Type type)
        {
            int index1 = this.basicTypes.IndexOf(RuntimeTypeModel.BasicTypeFinder, (object)type);
            if (index1 >= 0)
                return ((RuntimeTypeModel.BasicType)this.basicTypes[index1]).Serializer;
            lock (this.basicTypes)
            {
                int index2 = this.basicTypes.IndexOf(RuntimeTypeModel.BasicTypeFinder, (object)type);
                if (index2 >= 0)
                    return ((RuntimeTypeModel.BasicType)this.basicTypes[index2]).Serializer;
                IProtoSerializer serializer = MetaType.GetContractFamily(this, type, (AttributeMap[])null) == MetaType.AttributeFamily.None ? ValueMember.TryGetCoreSerializer(this, DataFormat.Default, type, out WireType _, false, false, false, false) : (IProtoSerializer)null;
                if (serializer != null)
                    this.basicTypes.Add((object)new RuntimeTypeModel.BasicType(type, serializer));
                return serializer;
            }
        }

        public void Clear() => this.types.Clear();

        internal int FindOrAddAuto(
          Type type,
          bool demand,
          bool addWithContractOnly,
          bool addEvenIfAutoDisabled)
        {
            int index = this.types.IndexOf(RuntimeTypeModel.MetaTypeFinder, (object)type);
            if (index >= 0)
            {
                MetaType type1 = (MetaType)this.types[index];
                if (type1.Pending)
                    this.WaitOnLock(type1);
                return index;
            }
            bool flag1 = this.AutoAddMissingTypes | addEvenIfAutoDisabled;
            if (!Helpers.IsEnum(type) && this.TryGetBasicTypeSerializer(type) != null)
            {
                if (flag1 && !addWithContractOnly)
                    throw MetaType.InbuiltType(type);
                return -1;
            }
            Type type2 = TypeModel.ResolveProxies(type);
            if (type2 != null)
            {
                index = this.types.IndexOf(RuntimeTypeModel.MetaTypeFinder, (object)type2);
                type = type2;
            }
            if (index < 0)
            {
                int opaqueToken = 0;
                try
                {
                    this.TakeLock(ref opaqueToken);
                    MetaType metaType;
                    if ((metaType = this.RecogniseCommonTypes(type)) == null)
                    {
                        MetaType.AttributeFamily contractFamily = MetaType.GetContractFamily(this, type, (AttributeMap[])null);
                        if (contractFamily == MetaType.AttributeFamily.AutoTuple)
                            flag1 = addEvenIfAutoDisabled = true;
                        if (!flag1 || !Helpers.IsEnum(type) & addWithContractOnly && contractFamily == MetaType.AttributeFamily.None)
                        {
                            if (demand)
                                TypeModel.ThrowUnexpectedType(type);
                            return index;
                        }
                        metaType = this.Create(type);
                    }
                    metaType.Pending = true;
                    bool flag2 = false;
                    int num = this.types.IndexOf(RuntimeTypeModel.MetaTypeFinder, (object)type);
                    if (num < 0)
                    {
                        this.ThrowIfFrozen();
                        index = this.types.Add((object)metaType);
                        flag2 = true;
                    }
                    else
                        index = num;
                    if (flag2)
                    {
                        metaType.ApplyDefaultBehaviour();
                        metaType.Pending = false;
                    }
                }
                finally
                {
                    this.ReleaseLock(opaqueToken);
                }
            }
            return index;
        }

        private MetaType RecogniseCommonTypes(Type type) => (MetaType)null;

        private MetaType Create(Type type)
        {
            this.ThrowIfFrozen();
            return new MetaType(this, type, this.defaultFactory);
        }

        public MetaType Add(Type type, bool applyDefaultBehaviour)
        {
            MetaType metaType = type != null ? this.FindWithoutAdd(type) : throw new ArgumentNullException(nameof(type));
            if (metaType != null)
                return metaType;
            int opaqueToken = 0;
            if (type.IsInterface && this.MapType(MetaType.ienumerable).IsAssignableFrom(type) && TypeModel.GetListItemType((TypeModel)this, type) == null)
                throw new ArgumentException("IEnumerable[<T>] data cannot be used as a meta-type unless an Add method can be resolved");
            try
            {
                metaType = this.RecogniseCommonTypes(type);
                if (metaType != null)
                    applyDefaultBehaviour = applyDefaultBehaviour ? false : throw new ArgumentException("Default behaviour must be observed for certain types with special handling; " + type.FullName, nameof(applyDefaultBehaviour));
                if (metaType == null)
                    metaType = this.Create(type);
                metaType.Pending = true;
                this.TakeLock(ref opaqueToken);
                if (this.FindWithoutAdd(type) != null)
                    throw new ArgumentException("Duplicate type", nameof(type));
                this.ThrowIfFrozen();
                this.types.Add((object)metaType);
                if (applyDefaultBehaviour)
                    metaType.ApplyDefaultBehaviour();
                metaType.Pending = false;
            }
            finally
            {
                this.ReleaseLock(opaqueToken);
            }
            return metaType;
        }

        public bool AutoAddMissingTypes
        {
            get => this.GetOption((byte)8);
            set
            {
                if (!value && this.GetOption((byte)2))
                    throw new InvalidOperationException("The default model must allow missing types");
                this.ThrowIfFrozen();
                this.SetOption((byte)8, value);
            }
        }

        private void ThrowIfFrozen()
        {
            if (this.GetOption((byte)4))
                throw new InvalidOperationException("The model cannot be changed once frozen");
        }

        public void Freeze()
        {
            if (this.GetOption((byte)2))
                throw new InvalidOperationException("The default model cannot be frozen");
            this.SetOption((byte)4, true);
        }

        protected override int GetKeyImpl(Type type) => this.GetKey(type, false, true);

        internal int GetKey(Type type, bool demand, bool getBaseKey)
        {
            Helpers.DebugAssert(type != null);
            try
            {
                int orAddAuto = this.FindOrAddAuto(type, demand, true, false);
                if (orAddAuto >= 0)
                {
                    MetaType type1 = (MetaType)this.types[orAddAuto];
                    if (getBaseKey)
                        orAddAuto = this.FindOrAddAuto(MetaType.GetRootType(type1).Type, true, true, false);
                }
                return orAddAuto;
            }
            catch (NotSupportedException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(type.FullName) < 0)
                    throw new ProtoException(ex.Message + " (" + type.FullName + ")", ex);
                throw;
            }
        }

        protected internal override void Serialize(int key, object value, ProtoWriter dest) => ((MetaType)this.types[key]).Serializer.Write(value, dest);

        protected internal override object Deserialize(int key, object value, ProtoReader source)
        {
            IProtoSerializer serializer = (IProtoSerializer)((MetaType)this.types[key]).Serializer;
            if (value != null || !Helpers.IsValueType(serializer.ExpectedType) || !serializer.RequiresOldValue)
                return serializer.Read(value, source);
            value = Activator.CreateInstance(serializer.ExpectedType);
            return serializer.Read(value, source);
        }

        internal bool IsPrepared(Type type)
        {
            MetaType withoutAdd = this.FindWithoutAdd(type);
            return withoutAdd != null && withoutAdd.IsPrepared();
        }

        internal EnumSerializer.EnumPair[] GetEnumMap(Type type)
        {
            int orAddAuto = this.FindOrAddAuto(type, false, false, false);
            return orAddAuto < 0 ? (EnumSerializer.EnumPair[])null : ((MetaType)this.types[orAddAuto]).GetEnumMap();
        }

        public int MetadataTimeoutMilliseconds
        {
            get => this.metadataTimeoutMilliseconds;
            set => this.metadataTimeoutMilliseconds = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(MetadataTimeoutMilliseconds));
        }

        public int LockCount => this.lockCount;

        internal void TakeLock(ref int opaqueToken)
        {
            opaqueToken = 0;
            if (Monitor.TryEnter((object)this.types, this.metadataTimeoutMilliseconds))
            {
                opaqueToken = this.GetContention();
                ++this.lockCount;
            }
            else
            {
                this.AddContention();
                throw new TimeoutException("Timeout while inspecting metadata; this may indicate a deadlock. This can often be avoided by preparing necessary serializers during application initialization, rather than allowing multiple threads to perform the initial metadata inspection; please also see the LockContended event");
            }
        }

        private int GetContention() => Interlocked.CompareExchange(ref this.contentionCounter, 0, 0);

        private void AddContention() => Interlocked.Increment(ref this.contentionCounter);

        internal void ReleaseLock(int opaqueToken)
        {
            if ((uint)opaqueToken <= 0U)
                return;
            Monitor.Exit((object)this.types);
            if (opaqueToken != this.GetContention())
            {
                LockContentedEventHandler lockContended = this.LockContended;
                if (lockContended != null)
                {
                    string stackTrace;
                    try
                    {
                        throw new ProtoException();
                    }
                    catch (Exception ex)
                    {
                        stackTrace = ex.StackTrace;
                    }
                    lockContended((object)this, new LockContentedEventArgs(stackTrace));
                }
            }
        }

        public event LockContentedEventHandler LockContended;

        internal void ResolveListTypes(Type type, ref Type itemType, ref Type defaultType)
        {
            if (type == null || Helpers.GetTypeCode(type) != ProtoTypeCode.Unknown || this[type].IgnoreListHandling)
                return;
            if (type.IsArray)
            {
                itemType = type.GetArrayRank() == 1 ? type.GetElementType() : throw new NotSupportedException("Multi-dimension arrays are supported");
                defaultType = itemType != this.MapType(typeof(byte)) ? type : (itemType = (Type)null);
            }
            if (itemType == null)
                itemType = TypeModel.GetListItemType((TypeModel)this, type);
            if (itemType != null)
            {
                Type itemType1 = (Type)null;
                Type defaultType1 = (Type)null;
                this.ResolveListTypes(itemType, ref itemType1, ref defaultType1);
                if (itemType1 != null)
                    throw TypeModel.CreateNestedListsNotSupported();
            }
            if (itemType == null || defaultType != null)
                return;
            if (type.IsClass && !type.IsAbstract && Helpers.GetConstructor(type, Helpers.EmptyTypes, true) != null)
                defaultType = type;
            if (defaultType == null && type.IsInterface)
            {
                Type[] genericArguments;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == this.MapType(typeof(IDictionary<,>)) && itemType == this.MapType(typeof(KeyValuePair<,>)).MakeGenericType(genericArguments = type.GetGenericArguments()))
                    defaultType = this.MapType(typeof(Dictionary<,>)).MakeGenericType(genericArguments);
                else
                    defaultType = this.MapType(typeof(List<>)).MakeGenericType(itemType);
            }
            if (defaultType != null && !Helpers.IsAssignableFrom(type, defaultType))
                defaultType = (Type)null;
        }

        internal string GetSchemaTypeName(
          Type effectiveType,
          DataFormat dataFormat,
          bool asReference,
          bool dynamicType,
          ref bool requiresBclImport)
        {
            Type underlyingType = Helpers.GetUnderlyingType(effectiveType);
            if (underlyingType != null)
                effectiveType = underlyingType;
            if (effectiveType == this.MapType(typeof(byte[])))
                return "bytes";
            IProtoSerializer coreSerializer = ValueMember.TryGetCoreSerializer(this, dataFormat, effectiveType, out WireType _, false, false, false, false);
            if (coreSerializer == null)
            {
                if (!(asReference | dynamicType))
                    return this[effectiveType].GetSurrogateOrBaseOrSelf(true).GetSchemaTypeName();
                requiresBclImport = true;
                return "bcl.NetObjectProxy";
            }
            if (coreSerializer is ParseableSerializer)
            {
                if (asReference)
                    requiresBclImport = true;
                return asReference ? "bcl.NetObjectProxy" : "string";
            }
            switch (Helpers.GetTypeCode(effectiveType))
            {
                case ProtoTypeCode.Boolean:
                    return "bool";
                case ProtoTypeCode.Char:
                case ProtoTypeCode.Byte:
                case ProtoTypeCode.UInt16:
                case ProtoTypeCode.UInt32:
                    return dataFormat == DataFormat.FixedSize ? "fixed32" : "uint32";
                case ProtoTypeCode.SByte:
                case ProtoTypeCode.Int16:
                case ProtoTypeCode.Int32:
                    switch (dataFormat)
                    {
                        case DataFormat.ZigZag:
                            return "sint32";
                        case DataFormat.FixedSize:
                            return "sfixed32";
                        default:
                            return "int32";
                    }
                case ProtoTypeCode.Int64:
                    switch (dataFormat)
                    {
                        case DataFormat.ZigZag:
                            return "sint64";
                        case DataFormat.FixedSize:
                            return "sfixed64";
                        default:
                            return "int64";
                    }
                case ProtoTypeCode.UInt64:
                    return dataFormat == DataFormat.FixedSize ? "fixed64" : "uint64";
                case ProtoTypeCode.Single:
                    return "float";
                case ProtoTypeCode.Double:
                    return "double";
                case ProtoTypeCode.Decimal:
                    requiresBclImport = true;
                    return "bcl.Decimal";
                case ProtoTypeCode.DateTime:
                    requiresBclImport = true;
                    return "bcl.DateTime";
                case ProtoTypeCode.String:
                    if (asReference)
                        requiresBclImport = true;
                    return asReference ? "bcl.NetObjectProxy" : "string";
                case ProtoTypeCode.TimeSpan:
                    requiresBclImport = true;
                    return "bcl.TimeSpan";
                case ProtoTypeCode.Guid:
                    requiresBclImport = true;
                    return "bcl.Guid";
                default:
                    throw new NotSupportedException("No .proto map found for: " + effectiveType.FullName);
            }
        }

        public void SetDefaultFactory(MethodInfo methodInfo)
        {
            this.VerifyFactory(methodInfo, (Type)null);
            this.defaultFactory = methodInfo;
        }

        internal void VerifyFactory(MethodInfo factory, Type type)
        {
            if (factory == null)
                return;
            if (type != null && Helpers.IsValueType(type))
                throw new InvalidOperationException();
            if (!factory.IsStatic)
                throw new ArgumentException("A factory-method must be static", nameof(factory));
            if (type != null && factory.ReturnType != type && factory.ReturnType != this.MapType(typeof(object)))
                throw new ArgumentException("The factory-method must return object" + (type == null ? "" : " or " + type.FullName), nameof(factory));
            if (!CallbackSet.CheckCallbackParameters((TypeModel)this, factory))
                throw new ArgumentException("Invalid factory signature in " + factory.DeclaringType.FullName + "." + factory.Name, nameof(factory));
        }

        private sealed class Singleton
        {
            internal static readonly RuntimeTypeModel Value = new RuntimeTypeModel(true);
            private static RuntimeTypeModel ThreadValue = (RuntimeTypeModel)null;
            internal static RuntimeTypeModel DefaultValue = (RuntimeTypeModel)null;

            private Singleton()
            {
            }

            public static void SetMultiThread(bool multiThread)
            {
                if (multiThread)
                {
                    RuntimeTypeModel.Singleton.ThreadValue = new RuntimeTypeModel(true);
                    RuntimeTypeModel.Singleton.DefaultValue = RuntimeTypeModel.Singleton.ThreadValue;
                }
                else
                    RuntimeTypeModel.Singleton.DefaultValue = RuntimeTypeModel.Singleton.Value;
            }
        }

        private sealed class BasicType
        {
            private readonly Type type;
            private readonly IProtoSerializer serializer;

            public Type Type => this.type;

            public IProtoSerializer Serializer => this.serializer;

            public BasicType(Type type, IProtoSerializer serializer)
            {
                this.type = type;
                this.serializer = serializer;
            }
        }
    }
}
