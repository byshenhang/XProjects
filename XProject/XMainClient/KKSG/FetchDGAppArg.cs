﻿using System;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "FetchDGAppArg")]
	[Serializable]
	public class FetchDGAppArg : IExtensible
	{

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private IExtension extensionObject;
	}
}
