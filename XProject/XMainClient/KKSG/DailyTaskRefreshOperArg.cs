﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "DailyTaskRefreshOperArg")]
	[Serializable]
	public class DailyTaskRefreshOperArg : IExtensible
	{

		[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
		public DailyRefreshOperType type
		{
			get
			{
				return this._type ?? DailyRefreshOperType.DROT_Refresh;
			}
			set
			{
				this._type = new DailyRefreshOperType?(value);
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
					this._type = (value ? new DailyRefreshOperType?(this.type) : null);
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

		[ProtoMember(2, IsRequired = false, Name = "roleid", DataFormat = DataFormat.TwosComplement)]
		public ulong roleid
		{
			get
			{
				return this._roleid ?? 0UL;
			}
			set
			{
				this._roleid = new ulong?(value);
			}
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool roleidSpecified
		{
			get
			{
				return this._roleid != null;
			}
			set
			{
				bool flag = value == (this._roleid == null);
				if (flag)
				{
					this._roleid = (value ? new ulong?(this.roleid) : null);
				}
			}
		}

		private bool ShouldSerializeroleid()
		{
			return this.roleidSpecified;
		}

		private void Resetroleid()
		{
			this.roleidSpecified = false;
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private DailyRefreshOperType? _type;

		private ulong? _roleid;

		private IExtension extensionObject;
	}
}
