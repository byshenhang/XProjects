﻿using System;
using System.IO;
using KKSG;
using ProtoBuf;

namespace XMainClient
{

	internal class PtcM2C_TeamFullDataM2CNtf : Protocol
	{

		public override uint GetProtoType()
		{
			return 39119U;
		}

		public override void Serialize(MemoryStream stream)
		{
			Serializer.Serialize<TeamFullDataNtf>(stream, this.Data);
		}

		public override void DeSerialize(MemoryStream stream)
		{
			this.Data = Serializer.Deserialize<TeamFullDataNtf>(stream);
		}

		public override void Process()
		{
			Process_PtcM2C_TeamFullDataM2CNtf.Process(this);
		}

		public TeamFullDataNtf Data = new TeamFullDataNtf();
	}
}
