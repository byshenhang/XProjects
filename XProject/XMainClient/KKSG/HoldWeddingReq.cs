﻿using System;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "HoldWeddingReq")]
	[Serializable]
	public class HoldWeddingReq : IExtensible
	{

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private IExtension extensionObject;
	}
}
