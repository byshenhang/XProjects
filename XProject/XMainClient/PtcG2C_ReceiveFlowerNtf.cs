﻿using System;
using System.IO;
using KKSG;
using ProtoBuf;

namespace XMainClient
{

	internal class PtcG2C_ReceiveFlowerNtf : Protocol
	{

		public override uint GetProtoType()
		{
			return 43606U;
		}

		public override void Serialize(MemoryStream stream)
		{
			Serializer.Serialize<ReceiveFlowerData>(stream, this.Data);
		}

		public override void DeSerialize(MemoryStream stream)
		{
			this.Data = Serializer.Deserialize<ReceiveFlowerData>(stream);
		}

		public override void Process()
		{
			Process_PtcG2C_ReceiveFlowerNtf.Process(this);
		}

		public ReceiveFlowerData Data = new ReceiveFlowerData();
	}
}
