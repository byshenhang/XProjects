﻿using System;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "AskGuildArenaInfoArg")]
	[Serializable]
	public class AskGuildArenaInfoArg : IExtensible
	{

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private IExtension extensionObject;
	}
}
