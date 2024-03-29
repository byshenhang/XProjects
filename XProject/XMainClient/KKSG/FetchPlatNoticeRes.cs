﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "FetchPlatNoticeRes")]
	[Serializable]
	public class FetchPlatNoticeRes : IExtensible
	{

		[ProtoMember(1, IsRequired = false, Name = "notice", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public PlatNotice notice
		{
			get
			{
				return this._notice;
			}
			set
			{
				this._notice = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "result", DataFormat = DataFormat.TwosComplement)]
		public ErrorCode result
		{
			get
			{
				return this._result ?? ErrorCode.ERR_SUCCESS;
			}
			set
			{
				this._result = new ErrorCode?(value);
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
					this._result = (value ? new ErrorCode?(this.result) : null);
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

		[ProtoMember(3, Name = "data", DataFormat = DataFormat.Default)]
		public List<PlatNotice> data
		{
			get
			{
				return this._data;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private PlatNotice _notice = null;

		private ErrorCode? _result;

		private readonly List<PlatNotice> _data = new List<PlatNotice>();

		private IExtension extensionObject;
	}
}
