﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "PvpArg")]
	[Serializable]
	public class PvpArg : IExtensible
	{

		[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
		public PvpReqType type
		{
			get
			{
				return this._type ?? PvpReqType.PVP_REQ_IN_MATCH;
			}
			set
			{
				this._type = new PvpReqType?(value);
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
					this._type = (value ? new PvpReqType?(this.type) : null);
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private PvpReqType? _type;

		private IExtension extensionObject;
	}
}
