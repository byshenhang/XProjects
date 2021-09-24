﻿using System;
using System.IO;
using KKSG;
using ProtoBuf;

namespace XMainClient
{

	internal class PtcG2C_StartBattleFailedNtf : Protocol
	{

		public override uint GetProtoType()
		{
			return 54098U;
		}

		public override void Serialize(MemoryStream stream)
		{
			Serializer.Serialize<StartBattleFailedRes>(stream, this.Data);
		}

		public override void DeSerialize(MemoryStream stream)
		{
			this.Data = Serializer.Deserialize<StartBattleFailedRes>(stream);
		}

		public override void Process()
		{
			Process_PtcG2C_StartBattleFailedNtf.Process(this);
		}

		public StartBattleFailedRes Data = new StartBattleFailedRes();
	}
}
