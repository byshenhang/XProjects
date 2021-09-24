﻿using System;
using System.IO;
using KKSG;
using ProtoBuf;

namespace XMainClient
{

	internal class PtcC2M_FMBRefuseC2M : Protocol
	{

		public override uint GetProtoType()
		{
			return 44407U;
		}

		public override void Serialize(MemoryStream stream)
		{
			Serializer.Serialize<FMBRes>(stream, this.Data);
		}

		public override void DeSerialize(MemoryStream stream)
		{
			this.Data = Serializer.Deserialize<FMBRes>(stream);
		}

		public override void Process()
		{
			throw new Exception("Send only protocol can not call process");
		}

		public FMBRes Data = new FMBRes();
	}
}
