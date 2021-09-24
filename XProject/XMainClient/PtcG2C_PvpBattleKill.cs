﻿using System;
using System.IO;
using KKSG;
using ProtoBuf;

namespace XMainClient
{

	internal class PtcG2C_PvpBattleKill : Protocol
	{

		public override uint GetProtoType()
		{
			return 61000U;
		}

		public override void Serialize(MemoryStream stream)
		{
			Serializer.Serialize<PvpBattleKill>(stream, this.Data);
		}

		public override void DeSerialize(MemoryStream stream)
		{
			this.Data = Serializer.Deserialize<PvpBattleKill>(stream);
		}

		public override void Process()
		{
			Process_PtcG2C_PvpBattleKill.Process(this);
		}

		public PvpBattleKill Data = new PvpBattleKill();
	}
}
