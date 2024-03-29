﻿using System;
using KKSG;
using XMainClient.UI;
using XUtliPoolLib;

namespace XMainClient
{

	internal class Process_PtcM2C_GuildCardMatchNtf
	{

		public static void Process(PtcM2C_GuildCardMatchNtf roPtc)
		{
			bool flag = roPtc.Data.errorcode > ErrorCode.ERR_SUCCESS;
			if (flag)
			{
				XSingleton<UiUtility>.singleton.ShowErrorCode(roPtc.Data.errorcode);
			}
			else
			{
				bool flag2 = roPtc.Data.match_type == 1U;
				if (flag2)
				{
					XJokerKingDocument specificDocument = XDocuments.GetSpecificDocument<XJokerKingDocument>(XJokerKingDocument.uuID);
					specificDocument.ReceiveJokerKingMatchInfo(roPtc.Data);
				}
				else
				{
					XGuildJockerMatchDocument specificDocument2 = XDocuments.GetSpecificDocument<XGuildJockerMatchDocument>(XGuildJockerMatchDocument.uuID);
					specificDocument2.ReceiveGuildJokerMatchInfo(roPtc.Data);
					XSingleton<XDebug>.singleton.AddGreenLog(string.Format("op: {0}, state: {1}, round: {2}, result: {3}, timeleft: {4}, errorcode: {5}", new object[]
					{
						roPtc.Data.op.ToString(),
						roPtc.Data.state.ToString(),
						roPtc.Data.round.ToString(),
						roPtc.Data.result.ToString(),
						roPtc.Data.timeleft.ToString(),
						roPtc.Data.errorcode.ToString()
					}), null, null, null, null, null);
				}
			}
		}
	}
}
