﻿using System;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "ReqGuildArenaHistoryRes")]
	[Serializable]
	public class ReqGuildArenaHistoryRes : IExtensible
	{

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private IExtension extensionObject;
	}
}
