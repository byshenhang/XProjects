﻿using System;
using System.IO;
using KKSG;
using ProtoBuf;

namespace XMainClient
{

	internal class PtcG2C_TeleportNotice : Protocol
	{

		public override uint GetProtoType()
		{
			return 27305U;
		}

		public override void Serialize(MemoryStream stream)
		{
			Serializer.Serialize<TeleportNoticeState>(stream, this.Data);
		}

		public override void DeSerialize(MemoryStream stream)
		{
			this.Data = Serializer.Deserialize<TeleportNoticeState>(stream);
		}

		public override void Process()
		{
			Process_PtcG2C_TeleportNotice.Process(this);
		}

		public TeleportNoticeState Data = new TeleportNoticeState();
	}
}
