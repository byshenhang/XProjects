﻿using System;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "LeagueBattleStartMatchNtf")]
	[Serializable]
	public class LeagueBattleStartMatchNtf : IExtensible
	{

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private IExtension extensionObject;
	}
}
