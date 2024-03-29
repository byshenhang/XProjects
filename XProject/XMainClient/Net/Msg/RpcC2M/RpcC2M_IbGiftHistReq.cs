﻿using System;
using System.IO;
using KKSG;
using ProtoBuf;

namespace XMainClient
{

	internal class RpcC2M_IbGiftHistReq : Rpc
	{

		public override uint GetRpcType()
		{
			return 27050U;
		}

		public override void Serialize(MemoryStream stream)
		{
			Serializer.Serialize<IBGiftHistAllItemArg>(stream, this.oArg);
		}

		public override void DeSerialize(MemoryStream stream)
		{
			this.oRes = Serializer.Deserialize<IBGiftHistAllItemRes>(stream);
		}

		public override void Process()
		{
			base.Process();
			Process_RpcC2M_IbGiftHistReq.OnReply(this.oArg, this.oRes);
		}

		public override void OnTimeout(object args)
		{
			Process_RpcC2M_IbGiftHistReq.OnTimeout(this.oArg);
		}

		public IBGiftHistAllItemArg oArg = new IBGiftHistAllItemArg();

		public IBGiftHistAllItemRes oRes = null;
	}
}
