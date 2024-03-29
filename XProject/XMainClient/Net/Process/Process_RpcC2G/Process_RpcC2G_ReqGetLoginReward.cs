﻿using System;
using System.Reflection;
using KKSG;
using XMainClient.UI;
using XUtliPoolLib;

namespace XMainClient
{

	internal class Process_RpcC2G_ReqGetLoginReward
	{

		public static void OnReply(LoginRewardGetReq oArg, LoginRewardGetRet oRes)
		{
			bool flag = oRes.ret == ErrorCode.ERR_INVALID_REQUEST;
			if (flag)
			{
				string fullName = MethodBase.GetCurrentMethod().ReflectedType.FullName;
				XSingleton<UiUtility>.singleton.OnGetInvalidRequest(fullName);
			}
			else
			{
				XSevenLoginDocument specificDocument = XDocuments.GetSpecificDocument<XSevenLoginDocument>(XSevenLoginDocument.uuID);
				specificDocument.ReceiveLoginReward(oArg, oRes);
			}
		}

		public static void OnTimeout(LoginRewardGetReq oArg)
		{
		}
	}
}
