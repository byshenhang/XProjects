﻿using System;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "GetGuildCheckinRecordsArg")]
	[Serializable]
	public class GetGuildCheckinRecordsArg : IExtensible
	{

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private IExtension extensionObject;
	}
}
