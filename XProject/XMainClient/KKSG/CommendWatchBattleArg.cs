﻿using System;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "CommendWatchBattleArg")]
	[Serializable]
	public class CommendWatchBattleArg : IExtensible
	{

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private IExtension extensionObject;
	}
}
