﻿using System;
using System.IO;
using KKSG;
using ProtoBuf;

namespace XMainClient
{

	internal class PtcC2G_UpdateFreeflowHintInfo : Protocol
	{

		public override uint GetProtoType()
		{
			return 27628U;
		}

		public override void Serialize(MemoryStream stream)
		{
			Serializer.Serialize<UpdateFreeflowHintInfo>(stream, this.Data);
		}

		public override void DeSerialize(MemoryStream stream)
		{
			this.Data = Serializer.Deserialize<UpdateFreeflowHintInfo>(stream);
		}

		public override void Process()
		{
			throw new Exception("Send only protocol can not call process");
		}

		public UpdateFreeflowHintInfo Data = new UpdateFreeflowHintInfo();
	}
}
