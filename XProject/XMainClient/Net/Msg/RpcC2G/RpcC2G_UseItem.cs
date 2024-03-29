﻿using System;
using System.IO;
using KKSG;
using ProtoBuf;

namespace XMainClient
{

	internal class RpcC2G_UseItem : Rpc
	{

		public override uint GetRpcType()
		{
			return 64132U;
		}

		public override void Serialize(MemoryStream stream)
		{
			Serializer.Serialize<UseItemArg>(stream, this.oArg);
		}

		public override void DeSerialize(MemoryStream stream)
		{
			this.oRes = Serializer.Deserialize<UseItemRes>(stream);
		}

		public override void Process()
		{
			base.Process();
			Process_RpcC2G_UseItem.OnReply(this.oArg, this.oRes);
		}

		public override void OnTimeout(object args)
		{
			Process_RpcC2G_UseItem.OnTimeout(this.oArg);
		}

		public UseItemArg oArg = new UseItemArg();

		public UseItemRes oRes = new UseItemRes();
	}
}
