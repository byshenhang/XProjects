﻿using System;
using System.IO;
using KKSG;
using ProtoBuf;

namespace XMainClient
{

	internal class PtcC2M_PayParameterInfoNtf : Protocol
	{

		public override uint GetProtoType()
		{
			return 1181U;
		}

		public override void Serialize(MemoryStream stream)
		{
			Serializer.Serialize<PayParameterInfo>(stream, this.Data);
		}

		public override void DeSerialize(MemoryStream stream)
		{
			this.Data = Serializer.Deserialize<PayParameterInfo>(stream);
		}

		public override void Process()
		{
			throw new Exception("Send only protocol can not call process");
		}

		public PayParameterInfo Data = new PayParameterInfo();
	}
}
