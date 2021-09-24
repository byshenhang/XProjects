﻿using System;
using KKSG;
using UILib;
using UnityEngine;

namespace XMainClient.UI
{

	internal class InnerGVGBattleMember : GVGBattleMemberBase
	{

		protected override void SetupOtherMemberInfo(Transform t, GmfRole role)
		{
			IXUILabel ixuilabel = t.FindChild("Fight").GetComponent("XUILabel") as IXUILabel;
			ixuilabel.SetText(role.pkpoint.ToString());
		}
	}
}
