﻿using System;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "PhotographEffectArg")]
	[Serializable]
	public class PhotographEffectArg : IExtensible
	{

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private IExtension extensionObject;
	}
}
