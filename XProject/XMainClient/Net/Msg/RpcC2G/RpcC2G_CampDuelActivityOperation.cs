﻿using System;
using System.IO;
using KKSG;
using ProtoBuf;

namespace XMainClient
{

	internal class RpcC2G_CampDuelActivityOperation : Rpc
	{

		public override uint GetRpcType()
		{
			return 1361U;
		}

		public override void Serialize(MemoryStream stream)
		{
			Serializer.Serialize<CampDuelActivityOperationArg>(stream, this.oArg);
		}

		public override void DeSerialize(MemoryStream stream)
		{
			this.oRes = Serializer.Deserialize<CampDuelActivityOperationRes>(stream);
		}

		public override void Process()
		{
			base.Process();
			Process_RpcC2G_CampDuelActivityOperation.OnReply(this.oArg, this.oRes);
		}

		public override void OnTimeout(object args)
		{
			Process_RpcC2G_CampDuelActivityOperation.OnTimeout(this.oArg);
		}

		public CampDuelActivityOperationArg oArg = new CampDuelActivityOperationArg();

		public CampDuelActivityOperationRes oRes = null;
	}
}
