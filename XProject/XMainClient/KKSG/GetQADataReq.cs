﻿using System;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "GetQADataReq")]
	[Serializable]
	public class GetQADataReq : IExtensible
	{

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private IExtension extensionObject;
	}
}
