﻿using System;
using System.Reflection;
using KKSG;
using XMainClient.UI;
using XUtliPoolLib;

namespace XMainClient
{
	// Token: 0x02001681 RID: 5761
	internal class Process_RpcC2G_CampDuelActivityOperation
	{
		// Token: 0x0600EF54 RID: 61268 RVA: 0x0034B1FC File Offset: 0x003493FC
		public static void OnReply(CampDuelActivityOperationArg oArg, CampDuelActivityOperationRes oRes)
		{
			bool flag = oRes.errorcode == ErrorCode.ERR_INVALID_REQUEST;
			if (flag)
			{
				string fullName = MethodBase.GetCurrentMethod().ReflectedType.FullName;
				XSingleton<UiUtility>.singleton.OnGetInvalidRequest(fullName);
			}
			else
			{
				bool flag2 = oRes.errorcode > ErrorCode.ERR_SUCCESS;
				if (flag2)
				{
					XSingleton<UiUtility>.singleton.ShowSystemTip(oRes.errorcode, "fece00");
				}
				else
				{
					XCampDuelDocument.Doc.SetCampDuelData(oArg, oRes);
					bool flag3 = oArg.type == 2U && XCampDuelDocument.Doc.handler != null;
					if (flag3)
					{
						XSingleton<UiUtility>.singleton.ShowSystemTip(string.Format(XStringDefineProxy.GetString("CAMPDUEL_JOIN_OK"), XCampDuelDocument.Doc.handler.CampName), "fece00");
					}
					bool flag4 = oArg.type == 3U;
					if (flag4)
					{
						XSingleton<UiUtility>.singleton.ShowSystemTip(string.Format(XStringDefineProxy.GetString("CAMPDUEL_CONFIRM_OK"), oArg.arg), "fece00");
					}
					bool flag5 = oArg.type == 4U || oArg.type == 5U;
					if (flag5)
					{
						XSingleton<UiUtility>.singleton.ShowSystemTip(string.Format(XStringDefineProxy.GetString("CAMPDUEL_COURAGE_OK"), oArg.arg), "fece00");
					}
				}
			}
		}

		// Token: 0x0600EF55 RID: 61269 RVA: 0x000FEEFC File Offset: 0x000FD0FC
		public static void OnTimeout(CampDuelActivityOperationArg oArg)
		{
		}
	}
}
