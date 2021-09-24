﻿using System;
using System.IO;
using KKSG;
using ProtoBuf;

namespace XMainClient
{

	internal class RpcC2G_UpgradeEquip : Rpc
	{

		public override uint GetRpcType()
		{
			return 32424U;
		}

		public override void Serialize(MemoryStream stream)
		{
			Serializer.Serialize<UpgradeEquipArg>(stream, this.oArg);
		}

		public override void DeSerialize(MemoryStream stream)
		{
			this.oRes = Serializer.Deserialize<UpgradeEquipRes>(stream);
		}

		public override void Process()
		{
			base.Process();
			Process_RpcC2G_UpgradeEquip.OnReply(this.oArg, this.oRes);
		}

		public override void OnTimeout(object args)
		{
			Process_RpcC2G_UpgradeEquip.OnTimeout(this.oArg);
		}

		public UpgradeEquipArg oArg = new UpgradeEquipArg();

		public UpgradeEquipRes oRes = null;
	}
}
