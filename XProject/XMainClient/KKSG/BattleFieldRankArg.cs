﻿using System;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "BattleFieldRankArg")]
	[Serializable]
	public class BattleFieldRankArg : IExtensible
	{

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private IExtension extensionObject;
	}
}
