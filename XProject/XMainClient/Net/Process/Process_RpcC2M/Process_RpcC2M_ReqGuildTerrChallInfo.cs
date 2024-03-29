﻿using System;
using KKSG;
using XMainClient.UI;
using XUtliPoolLib;

namespace XMainClient
{

	internal class Process_RpcC2M_ReqGuildTerrChallInfo
	{

		public static void OnReply(ReqGuildTerrChallInfoArg oArg, ReqGuildTerrChallInfoRes oRes)
		{
			bool flag = oRes == null;
			if (flag)
			{
				XSingleton<UiUtility>.singleton.ShowErrorCode(ErrorCode.ERR_FAILED);
			}
			else
			{
				XGuildTerritoryDocument specificDocument = XDocuments.GetSpecificDocument<XGuildTerritoryDocument>(XGuildTerritoryDocument.uuID);
				specificDocument.ReceiveGuildTerritoryChallInfo(oArg, oRes);
			}
		}

		public static void OnTimeout(ReqGuildTerrChallInfoArg oArg)
		{
		}
	}
}
