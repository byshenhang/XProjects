﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "PushInfo")]
	[Serializable]
	public class PushInfo : IExtensible
	{

		[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
		public uint type
		{
			get
			{
				return this._type ?? 0U;
			}
			set
			{
				this._type = new uint?(value);
			}
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool typeSpecified
		{
			get
			{
				return this._type != null;
			}
			set
			{
				bool flag = value == (this._type == null);
				if (flag)
				{
					this._type = (value ? new uint?(this.type) : null);
				}
			}
		}

		private bool ShouldSerializetype()
		{
			return this.typeSpecified;
		}

		private void Resettype()
		{
			this.typeSpecified = false;
		}

		[ProtoMember(2, IsRequired = false, Name = "sub_type", DataFormat = DataFormat.TwosComplement)]
		public uint sub_type
		{
			get
			{
				return this._sub_type ?? 0U;
			}
			set
			{
				this._sub_type = new uint?(value);
			}
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool sub_typeSpecified
		{
			get
			{
				return this._sub_type != null;
			}
			set
			{
				bool flag = value == (this._sub_type == null);
				if (flag)
				{
					this._sub_type = (value ? new uint?(this.sub_type) : null);
				}
			}
		}

		private bool ShouldSerializesub_type()
		{
			return this.sub_typeSpecified;
		}

		private void Resetsub_type()
		{
			this.sub_typeSpecified = false;
		}

		[ProtoMember(3, IsRequired = false, Name = "time", DataFormat = DataFormat.TwosComplement)]
		public uint time
		{
			get
			{
				return this._time ?? 0U;
			}
			set
			{
				this._time = new uint?(value);
			}
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool timeSpecified
		{
			get
			{
				return this._time != null;
			}
			set
			{
				bool flag = value == (this._time == null);
				if (flag)
				{
					this._time = (value ? new uint?(this.time) : null);
				}
			}
		}

		private bool ShouldSerializetime()
		{
			return this.timeSpecified;
		}

		private void Resettime()
		{
			this.timeSpecified = false;
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private uint? _type;

		private uint? _sub_type;

		private uint? _time;

		private IExtension extensionObject;
	}
}
