﻿using System;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "GetGuildCampPartyExchangeInfoArg")]
	[Serializable]
	public class GetGuildCampPartyExchangeInfoArg : IExtensible
	{

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private IExtension extensionObject;
	}
}
