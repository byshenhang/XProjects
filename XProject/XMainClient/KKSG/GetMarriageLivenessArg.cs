﻿using System;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "GetMarriageLivenessArg")]
	[Serializable]
	public class GetMarriageLivenessArg : IExtensible
	{

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private IExtension extensionObject;
	}
}
