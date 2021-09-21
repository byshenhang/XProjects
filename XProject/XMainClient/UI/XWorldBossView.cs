﻿using System;
using System.Collections.Generic;
using KKSG;
using UILib;
using UnityEngine;
using XMainClient.UI.UICommon;
using XUtliPoolLib;

namespace XMainClient.UI
{
	// Token: 0x020018F3 RID: 6387
	internal class XWorldBossView : DlgBase<XWorldBossView, XWorldBossBehaviour>
	{
		// Token: 0x17003A8D RID: 14989
		// (get) Token: 0x06010A54 RID: 68180 RVA: 0x00420D28 File Offset: 0x0041EF28
		public override string fileName
		{
			get
			{
				return "GameSystem/WorldBossDlg";
			}
		}

		// Token: 0x17003A8E RID: 14990
		// (get) Token: 0x06010A55 RID: 68181 RVA: 0x00420D40 File Offset: 0x0041EF40
		public override int layer
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17003A8F RID: 14991
		// (get) Token: 0x06010A56 RID: 68182 RVA: 0x00420D54 File Offset: 0x0041EF54
		public override bool pushstack
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003A90 RID: 14992
		// (get) Token: 0x06010A57 RID: 68183 RVA: 0x00420D68 File Offset: 0x0041EF68
		public override bool fullscreenui
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003A91 RID: 14993
		// (get) Token: 0x06010A58 RID: 68184 RVA: 0x00420D7C File Offset: 0x0041EF7C
		public override bool hideMainMenu
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003A92 RID: 14994
		// (get) Token: 0x06010A59 RID: 68185 RVA: 0x00420D90 File Offset: 0x0041EF90
		public override bool autoload
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06010A5A RID: 68186 RVA: 0x00420DA3 File Offset: 0x0041EFA3
		protected override void Init()
		{
			base.Init();
			this.m_LeftTime = new XLeftTimeCounter(base.uiBehaviour.m_LeftTime, false);
			this._Doc = XDocuments.GetSpecificDocument<XWorldBossDocument>(XWorldBossDocument.uuID);
			this._Doc.WorldBossDescView = this;
		}

		// Token: 0x06010A5B RID: 68187 RVA: 0x00420DE0 File Offset: 0x0041EFE0
		public void ShowView()
		{
			bool flag = !base.IsVisible();
			if (flag)
			{
				this.SetVisibleWithAnimation(true, null);
				int num = 0;
				int num2 = 0;
				this._Doc.GetWorldBossTime(ref num, ref num2);
				string arg = string.Format("{0}:{1}", (num / 100).ToString("D2"), (num % 100).ToString("D2"));
				string arg2 = string.Format("{0}:{1}", (num2 / 100).ToString("D2"), (num2 % 100).ToString("D2"));
				base.uiBehaviour.m_OpenTime.SetText(string.Format(XStringDefineProxy.GetString("WORLDBOSS_OPEN_TIME"), arg, arg2));
			}
		}

		// Token: 0x06010A5C RID: 68188 RVA: 0x00420EA0 File Offset: 0x0041F0A0
		protected override void OnShow()
		{
			base.OnShow();
			this.ShowDropList();
			this.InitAwardPanelInfo();
			base.uiBehaviour.m_GuildRankTab.bChecked = true;
			this.OnRankTabClicked(base.uiBehaviour.m_GuildRankTab);
			this._Doc.ReqWorldBossState();
			base.uiBehaviour.m_LeftTime.SetVisible(false);
			base.uiBehaviour.m_LeftTimeHint.gameObject.SetActive(false);
			base.uiBehaviour.m_RewardPanel.SetActive(false);
			base.uiBehaviour.m_AwardTip.SetText(XStringDefineProxy.GetString("WORLDBOSS_AWARD_TIP"));
			this.RefreshSubscribe();
			this.RefreshPrivilegeInfo();
		}

		// Token: 0x06010A5D RID: 68189 RVA: 0x00420F58 File Offset: 0x0041F158
		private void RefreshPrivilegeInfo()
		{
			XWelfareDocument specificDocument = XDocuments.GetSpecificDocument<XWelfareDocument>(XWelfareDocument.uuID);
			base.uiBehaviour.m_PrivilegeIcon.SetGrey(specificDocument.IsOwnMemberPrivilege(MemberPrivilege.KingdomPrivilege_Commerce));
			base.uiBehaviour.m_PrivilegeIcon.SetSprite(specificDocument.GetMemberPrivilegeIcon(MemberPrivilege.KingdomPrivilege_Commerce));
			base.uiBehaviour.m_PrivilegeName.SetEnabled(specificDocument.IsOwnMemberPrivilege(MemberPrivilege.KingdomPrivilege_Commerce));
			PayMemberTable.RowData memberPrivilegeConfig = specificDocument.GetMemberPrivilegeConfig(MemberPrivilege.KingdomPrivilege_Commerce);
			bool flag = memberPrivilegeConfig != null;
			if (flag)
			{
				BuffTable.RowData buffData = XSingleton<XBuffTemplateManager>.singleton.GetBuffData((int)memberPrivilegeConfig.worldBossbuffid[0], (int)memberPrivilegeConfig.worldBossbuffid[1]);
				bool flag2 = buffData != null;
				if (flag2)
				{
					base.uiBehaviour.m_PrivilegeName.SetText(XStringDefineProxy.GetString("WORLDBOSS_PRIVILEGE_BUFF", new object[]
					{
						buffData.BuffName
					}));
				}
				base.uiBehaviour.m_PrivilegeIcon.SetVisible(buffData != null);
			}
		}

		// Token: 0x06010A5E RID: 68190 RVA: 0x00421039 File Offset: 0x0041F239
		protected override void OnHide()
		{
			base.OnHide();
			base.uiBehaviour.m_BossTexture.SetTexturePath("");
		}

		// Token: 0x06010A5F RID: 68191 RVA: 0x0042105C File Offset: 0x0041F25C
		public override void RegisterEvent()
		{
			base.RegisterEvent();
			base.uiBehaviour.m_BtnClose.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnCloseClick));
			base.uiBehaviour.m_BtnGoBattle.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnGoBattleClick));
			base.uiBehaviour.m_BtnReward.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnRewardClick));
			base.uiBehaviour.m_BtnRewardPanelClose.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnRewardClose));
			base.uiBehaviour.m_DamageRankTab.ID = (ulong)((long)XFastEnumIntEqualityComparer<RankeType>.ToInt(RankeType.WorldBossDamageRank));
			base.uiBehaviour.m_DamageRankTab.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnRankTabClicked));
			base.uiBehaviour.m_GuildRankTab.ID = (ulong)((long)XFastEnumIntEqualityComparer<RankeType>.ToInt(RankeType.WorldBossGuildRank));
			base.uiBehaviour.m_GuildRankTab.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnRankTabClicked));
			base.uiBehaviour.m_BtnSubscribe.ID = 0UL;
			base.uiBehaviour.m_BtnSubscribe.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnSubscribeClick));
			base.uiBehaviour.m_BtnCancelSubscribe.ID = 1UL;
			base.uiBehaviour.m_BtnCancelSubscribe.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnSubscribeClick));
			base.uiBehaviour.m_WrapContent.RegisterItemUpdateEventHandler(new WrapItemUpdateEventHandler(this.DamageRankWrapContentItemUpdated));
			base.uiBehaviour.m_AwardWrapContent.RegisterItemUpdateEventHandler(new WrapItemUpdateEventHandler(this.AwardWrapContentItemUpdated));
			base.uiBehaviour.m_Privilege.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(this.OnPrivilegeClick));
		}

		// Token: 0x06010A60 RID: 68192 RVA: 0x002548EE File Offset: 0x00252AEE
		private void OnPrivilegeClick(IXUISprite btn)
		{
			DlgBase<XWelfareView, XWelfareBehaviour>.singleton.CheckActiveMemberPrivilege(MemberPrivilege.KingdomPrivilege_Commerce);
		}

		// Token: 0x06010A61 RID: 68193 RVA: 0x00421206 File Offset: 0x0041F406
		protected override void OnUnload()
		{
			this.m_LeftTime = null;
			this._Doc.WorldBossDescView = null;
			base.OnUnload();
		}

		// Token: 0x06010A62 RID: 68194 RVA: 0x00421224 File Offset: 0x0041F424
		private bool OnRankTabClicked(IXUICheckBox checkbox)
		{
			bool flag = !checkbox.bChecked;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.m_CurrRankType = (RankeType)checkbox.ID;
				this._Doc.ReqRankData(this.m_CurrRankType, false);
				result = true;
			}
			return result;
		}

		// Token: 0x06010A63 RID: 68195 RVA: 0x00421268 File Offset: 0x0041F468
		private bool OnSubscribeClick(IXUIButton button)
		{
			this.SubscribebuttonID = button.ID;
			PushSubscribeTable.RowData pushSubscribe = XPushSubscribeDocument.GetPushSubscribe(PushSubscribeOptions.WorldBoss);
			XSingleton<UiUtility>.singleton.ShowModalDialog((button.ID == 0UL) ? pushSubscribe.SubscribeDescription : pushSubscribe.CancelDescription, XStringDefineProxy.GetString("COMMON_OK"), XStringDefineProxy.GetString("COMMON_CANCEL"), new ButtonClickEventHandler(this.ReqSubscribeChange));
			return true;
		}

		// Token: 0x06010A64 RID: 68196 RVA: 0x004212D0 File Offset: 0x0041F4D0
		private bool ReqSubscribeChange(IXUIButton button)
		{
			XSingleton<UiUtility>.singleton.CloseModalDlg();
			XPushSubscribeDocument specificDocument = XDocuments.GetSpecificDocument<XPushSubscribeDocument>(XPushSubscribeDocument.uuID);
			specificDocument.ReqSetSubscribe(PushSubscribeOptions.WorldBoss, this.SubscribebuttonID == 0UL);
			return true;
		}

		// Token: 0x06010A65 RID: 68197 RVA: 0x0042130C File Offset: 0x0041F50C
		public void RefreshSubscribe()
		{
			PushSubscribeTable.RowData pushSubscribe = XPushSubscribeDocument.GetPushSubscribe(PushSubscribeOptions.WorldBoss);
			XPushSubscribeDocument specificDocument = XDocuments.GetSpecificDocument<XPushSubscribeDocument>(XPushSubscribeDocument.uuID);
			bool flag = XSingleton<XClientNetwork>.singleton.AccountType == LoginType.LGOIN_WECHAT_PF && pushSubscribe.IsShow && specificDocument.OptionsDefault != null && specificDocument.OptionsDefault.Count != 0;
			if (flag)
			{
				bool curSubscribeStatus = specificDocument.GetCurSubscribeStatus(PushSubscribeOptions.WorldBoss);
				base.uiBehaviour.m_BtnSubscribe.gameObject.SetActive(!curSubscribeStatus);
				base.uiBehaviour.m_BtnCancelSubscribe.gameObject.SetActive(curSubscribeStatus);
			}
			else
			{
				base.uiBehaviour.m_BtnSubscribe.gameObject.SetActive(false);
				base.uiBehaviour.m_BtnCancelSubscribe.gameObject.SetActive(false);
			}
		}

		// Token: 0x06010A66 RID: 68198 RVA: 0x004213CC File Offset: 0x0041F5CC
		public override void OnUpdate()
		{
			base.OnUpdate();
			bool flag = this.m_LeftTime != null;
			if (flag)
			{
				this.m_LeftTime.Update();
				bool flag2 = this.m_LeftTime.GetLeftTime() <= 0;
				if (flag2)
				{
					this.UpdateLeftTimeState(0f, this.mBossHp);
				}
			}
		}

		// Token: 0x06010A67 RID: 68199 RVA: 0x00421425 File Offset: 0x0041F625
		public void SetLeftTime(float time, uint BossHp)
		{
			this.m_LeftTime.SetLeftTime(time, -1);
			base.uiBehaviour.m_LeftTime.SetVisible(time > 0f);
			this.UpdateLeftTimeState(time, BossHp);
		}

		// Token: 0x06010A68 RID: 68200 RVA: 0x00421458 File Offset: 0x0041F658
		private void UpdateLeftTimeState(float time, uint BossHp)
		{
			this.mBossHp = BossHp;
			bool flag = time <= 0f;
			if (flag)
			{
				base.uiBehaviour.m_LeftTimeHint.gameObject.SetActive(true);
				base.uiBehaviour.m_LeftTime.gameObject.SetActive(false);
				bool flag2 = BossHp > 0U;
				if (flag2)
				{
					base.uiBehaviour.m_LeftTimeHint.SetText(XStringDefineProxy.GetString("GUILD_BOSS_CONDITION_TIME"));
				}
				else
				{
					base.uiBehaviour.m_LeftTimeHint.SetText(XStringDefineProxy.GetString("GUILD_BOSS_CONDITION_BOSSDIE"));
				}
			}
			else
			{
				base.uiBehaviour.m_LeftTimeHint.gameObject.SetActive(false);
				bool flag3 = BossHp <= 0U;
				if (flag3)
				{
					base.uiBehaviour.m_LeftTimeHint.gameObject.SetActive(true);
					base.uiBehaviour.m_LeftTime.gameObject.SetActive(false);
					base.uiBehaviour.m_LeftTimeHint.SetText(XStringDefineProxy.GetString("GUILD_BOSS_CONDITION_BOSSDIE"));
				}
			}
		}

		// Token: 0x06010A69 RID: 68201 RVA: 0x00421564 File Offset: 0x0041F764
		private bool OnCloseClick(IXUIButton button)
		{
			this.SetVisibleWithAnimation(false, null);
			return true;
		}

		// Token: 0x06010A6A RID: 68202 RVA: 0x00421580 File Offset: 0x0041F780
		private bool OnRewardClick(IXUIButton button)
		{
			base.uiBehaviour.m_RewardPanel.SetActive(true);
			base.uiBehaviour.m_AwardScrollView.ResetPosition();
			return true;
		}

		// Token: 0x06010A6B RID: 68203 RVA: 0x004215B8 File Offset: 0x0041F7B8
		private bool OnRewardClose(IXUIButton button)
		{
			base.uiBehaviour.m_RewardPanel.SetActive(false);
			return true;
		}

		// Token: 0x06010A6C RID: 68204 RVA: 0x004215E0 File Offset: 0x0041F7E0
		private bool OnGoBattleClick(IXUIButton button)
		{
			bool flag = this.m_LeftTime.GetLeftTime() > 0;
			bool result;
			if (flag)
			{
				XSingleton<UiUtility>.singleton.ShowSystemTip(XStringDefineProxy.GetString("WORLDBOSS_CHANGGLE_NOTOPEN"), "fece00");
				result = false;
			}
			else
			{
				bool flag2 = this.mBossHp <= 0U;
				if (flag2)
				{
					XSingleton<UiUtility>.singleton.ShowSystemTip(XStringDefineProxy.GetString("WORLDBOSS_CHANGGLE_DEAD"), "fece00");
					result = false;
				}
				else
				{
					bool flag3 = XSingleton<XGameSysMgr>.singleton.IsSystemOpened(XSysDefine.XSys_Activity_WorldBoss);
					if (flag3)
					{
						bool flag4 = XTeamDocument.GoSingleBattleBeforeNeed(new ButtonClickEventHandler(this.OnGoBattleClick), button);
						if (flag4)
						{
							return true;
						}
						this._Doc.ReqEnterWorldBossScene();
					}
					else
					{
						int sysOpenLevel = XSingleton<XGameSysMgr>.singleton.GetSysOpenLevel(XFastEnumIntEqualityComparer<XSysDefine>.ToInt(XSysDefine.XSys_Activity_WorldBoss));
						XSingleton<UiUtility>.singleton.ShowSystemTip(XStringDefineProxy.GetString("ERR_WORLDBOSS_NOPEN", new object[]
						{
							sysOpenLevel
						}), "fece00");
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06010A6D RID: 68205 RVA: 0x004216DC File Offset: 0x0041F8DC
		public void RefreshDamageRank()
		{
			bool flag = this.m_CurrRankType == RankeType.WorldBossDamageRank;
			List<XBaseRankInfo> rankList;
			if (flag)
			{
				rankList = this._Doc.DamageRankList.rankList;
			}
			else
			{
				rankList = this._Doc.GuildRankList.rankList;
			}
			bool flag2 = rankList.Count == 0;
			if (flag2)
			{
				base.uiBehaviour.m_RankPanel_EmptyRank.gameObject.SetActive(true);
				base.uiBehaviour.m_RankPanel_EmptyRank.SetText(XStringDefineProxy.GetString("GUILD_BOSS_EMPTY_RANK"));
				base.uiBehaviour.m_WrapContent.gameObject.SetActive(false);
			}
			else
			{
				base.uiBehaviour.m_RankPanel_EmptyRank.gameObject.SetActive(false);
				base.uiBehaviour.m_WrapContent.gameObject.SetActive(true);
				base.uiBehaviour.m_WrapContent.SetContentCount(rankList.Count, false);
				base.uiBehaviour.m_ScrollView.ResetPosition();
			}
		}

		// Token: 0x06010A6E RID: 68206 RVA: 0x004217D0 File Offset: 0x0041F9D0
		private void DamageRankWrapContentItemUpdated(Transform t, int index)
		{
			bool flag = this.m_CurrRankType == RankeType.WorldBossGuildRank;
			List<XBaseRankInfo> rankList;
			if (flag)
			{
				rankList = this._Doc.GuildRankList.rankList;
			}
			else
			{
				rankList = this._Doc.DamageRankList.rankList;
			}
			bool flag2 = index < 0 || index >= rankList.Count;
			if (!flag2)
			{
				XBaseRankInfo xbaseRankInfo = rankList[index];
				bool flag3 = xbaseRankInfo == null;
				if (flag3)
				{
					XSingleton<XDebug>.singleton.AddErrorLog("XGuildDragonView.GuildRankWrapContentItemUpdated  is null ", null, null, null, null, null);
				}
				else
				{
					IXUISprite ixuisprite = t.FindChild("Rank").GetComponent("XUISprite") as IXUISprite;
					IXUILabel ixuilabel = t.FindChild("Rank3").GetComponent("XUILabel") as IXUILabel;
					IXUILabel ixuilabel2 = t.FindChild("Name").GetComponent("XUILabel") as IXUILabel;
					IXUILabel ixuilabel3 = t.FindChild("shanghai").GetComponent("XUILabel") as IXUILabel;
					XWorldBossGuildRankInfo xworldBossGuildRankInfo = xbaseRankInfo as XWorldBossGuildRankInfo;
					XWorldBossDamageRankInfo xworldBossDamageRankInfo = xbaseRankInfo as XWorldBossDamageRankInfo;
					float num = 0f;
					bool flag4 = xworldBossGuildRankInfo != null;
					if (flag4)
					{
						num = xworldBossGuildRankInfo.damage;
					}
					else
					{
						bool flag5 = xworldBossDamageRankInfo != null;
						if (flag5)
						{
							num = xworldBossDamageRankInfo.damage;
						}
					}
					ixuilabel3.SetText(XSingleton<UiUtility>.singleton.NumberFormatBillion((ulong)num));
					ixuilabel2.SetText(xbaseRankInfo.name);
					bool flag6 = index < 3;
					if (flag6)
					{
						string spriteName = string.Format("N{0}", index + 1);
						ixuisprite.spriteName = spriteName;
						ixuisprite.SetVisible(true);
					}
					else
					{
						ixuisprite.SetVisible(false);
						ixuilabel.SetText((index + 1).ToString());
					}
				}
			}
		}

		// Token: 0x06010A6F RID: 68207 RVA: 0x00421984 File Offset: 0x0041FB84
		public void SetMyRankFrame()
		{
			bool flag = this.m_CurrRankType == RankeType.WorldBossGuildRank;
			XBaseRankList xbaseRankList;
			if (flag)
			{
				xbaseRankList = this._Doc.GuildRankList;
			}
			else
			{
				xbaseRankList = this._Doc.DamageRankList;
			}
			GameObject gameObject = base.uiBehaviour.m_RankPanel.transform.FindChild("RankTpl").gameObject;
			GameObject gameObject2 = base.uiBehaviour.m_RankPanel.transform.FindChild("OutOfRange").gameObject;
			bool flag2 = xbaseRankList.rankList.Count == 0;
			if (flag2)
			{
				gameObject2.SetActive(false);
				gameObject.SetActive(false);
			}
			else
			{
				XBaseRankInfo latestMyRankInfo = xbaseRankList.GetLatestMyRankInfo();
				bool flag3 = this.m_CurrRankType == RankeType.WorldBossGuildRank;
				if (flag3)
				{
					XGuildDocument specificDocument = XDocuments.GetSpecificDocument<XGuildDocument>(XGuildDocument.uuID);
					bool flag4 = !specificDocument.bInGuild;
					if (flag4)
					{
						gameObject2.SetActive(false);
						gameObject.SetActive(false);
						return;
					}
					gameObject2.SetActive(latestMyRankInfo.rank == XRankDocument.INVALID_RANK);
					bool flag5 = latestMyRankInfo == null || latestMyRankInfo.rank == XRankDocument.INVALID_RANK;
					if (flag5)
					{
						gameObject.SetActive(false);
						return;
					}
				}
				else
				{
					gameObject2.SetActive(latestMyRankInfo.rank == XRankDocument.INVALID_RANK);
					bool flag6 = latestMyRankInfo == null || latestMyRankInfo.id == 0UL;
					if (flag6)
					{
						gameObject.SetActive(false);
						return;
					}
				}
				gameObject.SetActive(true);
				IXUISprite ixuisprite = gameObject.transform.FindChild("Rank").GetComponent("XUISprite") as IXUISprite;
				IXUILabel ixuilabel = gameObject.transform.FindChild("Name").gameObject.GetComponent("XUILabel") as IXUILabel;
				IXUILabel ixuilabel2 = gameObject.transform.FindChild("shanghai").gameObject.GetComponent("XUILabel") as IXUILabel;
				IXUILabel ixuilabel3 = gameObject.transform.FindChild("Rank3").gameObject.GetComponent("XUILabel") as IXUILabel;
				ixuilabel.SetText(latestMyRankInfo.name);
				XWorldBossGuildRankInfo xworldBossGuildRankInfo = latestMyRankInfo as XWorldBossGuildRankInfo;
				XWorldBossDamageRankInfo xworldBossDamageRankInfo = latestMyRankInfo as XWorldBossDamageRankInfo;
				float num = 0f;
				bool flag7 = xworldBossGuildRankInfo != null;
				if (flag7)
				{
					num = xworldBossGuildRankInfo.damage;
				}
				else
				{
					bool flag8 = xworldBossDamageRankInfo != null;
					if (flag8)
					{
						num = xworldBossDamageRankInfo.damage;
					}
				}
				ixuilabel2.SetText(XSingleton<UiUtility>.singleton.NumberFormatBillion((ulong)num));
				bool flag9 = latestMyRankInfo.rank < 3U;
				if (flag9)
				{
					string spriteName = string.Format("N{0}", latestMyRankInfo.rank + 1U);
					ixuisprite.spriteName = spriteName;
					ixuisprite.SetVisible(true);
				}
				else
				{
					ixuisprite.SetVisible(false);
					ixuilabel3.SetText((latestMyRankInfo.rank + 1U).ToString());
				}
			}
		}

		// Token: 0x06010A70 RID: 68208 RVA: 0x00421C50 File Offset: 0x0041FE50
		private void InitAwardPanelInfo()
		{
			List<WorldBossRewardTable.RowData> awardList = this._Doc.GetAwardList(XSingleton<XAttributeMgr>.singleton.XPlayerData.Level);
			base.uiBehaviour.m_AwardWrapContent.SetContentCount(awardList.Count, false);
			base.uiBehaviour.m_AwardScrollView.ResetPosition();
		}

		// Token: 0x06010A71 RID: 68209 RVA: 0x00421CA4 File Offset: 0x0041FEA4
		private void AwardWrapContentItemUpdated(Transform t, int index)
		{
			List<WorldBossRewardTable.RowData> awardList = this._Doc.GetAwardList(XSingleton<XAttributeMgr>.singleton.XPlayerData.Level);
			bool flag = index < 0 || index >= awardList.Count;
			if (!flag)
			{
				WorldBossRewardTable.RowData rowData = awardList[index];
				IXUILabel ixuilabel = t.FindChild("Bg/Rank/RankNum").GetComponent("XUILabel") as IXUILabel;
				ixuilabel.SetText(rowData.Rank[1].ToString());
				for (int i = 0; i < 4; i++)
				{
					GameObject gameObject = t.FindChild(string.Format("Bg/Grid/ItemTpl{0}", i)).gameObject;
					bool flag2 = i < rowData.ShowReward.Count;
					if (flag2)
					{
						gameObject.gameObject.SetActive(true);
						XSingleton<XItemDrawerMgr>.singleton.normalItemDrawer.DrawItem(gameObject, (int)rowData.ShowReward[i, 0], (int)rowData.ShowReward[i, 1], false);
						IXUISprite ixuisprite = gameObject.transform.FindChild("Icon").GetComponent("XUISprite") as IXUISprite;
						ixuisprite.ID = (ulong)rowData.ShowReward[i, 0];
						ixuisprite.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(XSingleton<UiUtility>.singleton.OnItemClick));
					}
					else
					{
						gameObject.gameObject.SetActive(false);
					}
				}
			}
		}

		// Token: 0x06010A72 RID: 68210 RVA: 0x00421E20 File Offset: 0x00420020
		public void ShowDropList()
		{
			WorldBossRewardTable.RowData dropReward = this._Doc.GetDropReward(XSingleton<XAttributeMgr>.singleton.XPlayerData.Level);
			bool flag = dropReward == null;
			if (!flag)
			{
				base.uiBehaviour.m_DropAwardPool.FakeReturnAll();
				for (int i = 0; i < dropReward.ShowReward.Count; i++)
				{
					this.CreateDropAward((int)dropReward.ShowReward[i, 0], false);
				}
				AuctionHouseDocument specificDocument = XDocuments.GetSpecificDocument<AuctionHouseDocument>(AuctionHouseDocument.uuID);
				List<GuildAuctReward.RowData> guildAuctReward = specificDocument.GetGuildAuctReward(AuctionActType.WorldBoss);
				bool flag2 = guildAuctReward.Count > 0;
				if (flag2)
				{
					uint[] rewardShow = guildAuctReward[0].RewardShow;
					bool flag3 = rewardShow != null;
					if (flag3)
					{
						for (int j = 0; j < rewardShow.Length; j++)
						{
							this.CreateDropAward((int)rewardShow[j], true);
						}
					}
				}
				base.uiBehaviour.m_DropAwardPool.ActualReturnAll(false);
				base.uiBehaviour.m_DropAward.Refresh();
			}
		}

		// Token: 0x06010A73 RID: 68211 RVA: 0x00421F2C File Offset: 0x0042012C
		private void CreateDropAward(int itemID, bool bGuild)
		{
			GameObject gameObject = base.uiBehaviour.m_DropAwardPool.FetchGameObject(false);
			gameObject.transform.parent = base.uiBehaviour.m_DropAward.gameObject.transform;
			IXUILabel ixuilabel = gameObject.transform.Find("Flag").GetComponent("XUILabel") as IXUILabel;
			ixuilabel.SetText(bGuild ? XStringDefineProxy.GetString("GUILDBOSS_ICON_FLAG") : XStringDefineProxy.GetString("WORLDBOSS_ICON_FLAG"));
			XSingleton<XItemDrawerMgr>.singleton.normalItemDrawer.DrawItem(gameObject, itemID, 0, false);
			IXUISprite ixuisprite = gameObject.transform.FindChild("Icon").GetComponent("XUISprite") as IXUISprite;
			ixuisprite.ID = (ulong)((long)itemID);
			ixuisprite.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(XSingleton<UiUtility>.singleton.OnItemClick));
		}

		// Token: 0x06010A74 RID: 68212 RVA: 0x00422004 File Offset: 0x00420204
		public void ShowCurrentBoss(uint BossID)
		{
			string name = XSingleton<XEntityMgr>.singleton.EntityStatistics.GetByID(BossID).Name;
			base.uiBehaviour.m_BossName.SetText(name);
			string str = "";
			string[] andSeparateValue = XSingleton<XGlobalConfig>.singleton.GetAndSeparateValue("WorldBossTexture", XGlobalConfig.ListSeparator);
			for (int i = 0; i < andSeparateValue.Length; i++)
			{
				string[] array = andSeparateValue[i].Split(XGlobalConfig.SequenceSeparator);
				bool flag = array.Length == 2;
				if (flag)
				{
					bool flag2 = uint.Parse(array[0]) == BossID;
					if (flag2)
					{
						str = array[1];
						break;
					}
				}
			}
			base.uiBehaviour.m_BossTexture.SetTexturePath(XSingleton<XGlobalConfig>.singleton.GetValue("WorldBossTextureLocation") + str);
		}

		// Token: 0x04007933 RID: 31027
		private XWorldBossDocument _Doc;

		// Token: 0x04007934 RID: 31028
		private XLeftTimeCounter m_LeftTime;

		// Token: 0x04007935 RID: 31029
		private RankeType m_CurrRankType;

		// Token: 0x04007936 RID: 31030
		private ulong SubscribebuttonID = 0UL;

		// Token: 0x04007937 RID: 31031
		private uint mBossHp = 0U;
	}
}
