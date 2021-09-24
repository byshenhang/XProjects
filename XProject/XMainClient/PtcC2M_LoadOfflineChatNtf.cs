﻿using System;
using System.IO;
using KKSG;
using ProtoBuf;

namespace XMainClient
{

	internal class PtcC2M_LoadOfflineChatNtf : Protocol
	{

		public override uint GetProtoType()
		{
			return 26622U;
		}

		public override void Serialize(MemoryStream stream)
		{
			Serializer.Serialize<LoadOffLineChatNtf>(stream, this.Data);
		}

		public override void DeSerialize(MemoryStream stream)
		{
			this.Data = Serializer.Deserialize<LoadOffLineChatNtf>(stream);
		}

		public override void Process()
		{
			throw new Exception("Send only protocol can not call process");
		}

		public LoadOffLineChatNtf Data = new LoadOffLineChatNtf();
	}
}
