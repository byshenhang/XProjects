﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "StartGuildCardRes")]
	[Serializable]
	public class StartGuildCardRes : IExtensible
	{

		[ProtoMember(1, IsRequired = false, Name = "errorcode", DataFormat = DataFormat.TwosComplement)]
		public ErrorCode errorcode
		{
			get
			{
				return this._errorcode ?? ErrorCode.ERR_SUCCESS;
			}
			set
			{
				this._errorcode = new ErrorCode?(value);
			}
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool errorcodeSpecified
		{
			get
			{
				return this._errorcode != null;
			}
			set
			{
				bool flag = value == (this._errorcode == null);
				if (flag)
				{
					this._errorcode = (value ? new ErrorCode?(this.errorcode) : null);
				}
			}
		}

		private bool ShouldSerializeerrorcode()
		{
			return this.errorcodeSpecified;
		}

		private void Reseterrorcode()
		{
			this.errorcodeSpecified = false;
		}

		[ProtoMember(2, Name = "card", DataFormat = DataFormat.TwosComplement)]
		public List<uint> card
		{
			get
			{
				return this._card;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "result", DataFormat = DataFormat.TwosComplement)]
		public uint result
		{
			get
			{
				return this._result ?? 0U;
			}
			set
			{
				this._result = new uint?(value);
			}
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool resultSpecified
		{
			get
			{
				return this._result != null;
			}
			set
			{
				bool flag = value == (this._result == null);
				if (flag)
				{
					this._result = (value ? new uint?(this.result) : null);
				}
			}
		}

		private bool ShouldSerializeresult()
		{
			return this.resultSpecified;
		}

		private void Resetresult()
		{
			this.resultSpecified = false;
		}

		[ProtoMember(4, IsRequired = false, Name = "store", DataFormat = DataFormat.TwosComplement)]
		public uint store
		{
			get
			{
				return this._store ?? 0U;
			}
			set
			{
				this._store = new uint?(value);
			}
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool storeSpecified
		{
			get
			{
				return this._store != null;
			}
			set
			{
				bool flag = value == (this._store == null);
				if (flag)
				{
					this._store = (value ? new uint?(this.store) : null);
				}
			}
		}

		private bool ShouldSerializestore()
		{
			return this.storeSpecified;
		}

		private void Resetstore()
		{
			this.storeSpecified = false;
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private ErrorCode? _errorcode;

		private readonly List<uint> _card = new List<uint>();

		private uint? _result;

		private uint? _store;

		private IExtension extensionObject;
	}
}
