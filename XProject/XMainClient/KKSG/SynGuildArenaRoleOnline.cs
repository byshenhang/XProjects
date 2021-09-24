﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "SynGuildArenaRoleOnline")]
	[Serializable]
	public class SynGuildArenaRoleOnline : IExtensible
	{

		[ProtoMember(1, IsRequired = false, Name = "roleid", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(2, IsRequired = false, Name = "online", DataFormat = DataFormat.Default)]
		public bool online
		{
			get
			{
				return this._online ?? false;
			}
			set
			{
				this._online = new bool?(value);
			}
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool onlineSpecified
		{
			get
			{
				return this._online != null;
			}
			set
			{
				bool flag = value == (this._online == null);
				if (flag)
				{
					this._online = (value ? new bool?(this.online) : null);
				}
			}
		}

		private bool ShouldSerializeonline()
		{
			return this.onlineSpecified;
		}

		private void Resetonline()
		{
			this.onlineSpecified = false;
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private ulong? _roleid;

		private bool? _online;

		private IExtension extensionObject;
	}
}
