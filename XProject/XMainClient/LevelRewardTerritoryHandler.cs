﻿using System;
using System.Collections.Generic;
using KKSG;
using UILib;
using UnityEngine;
using XMainClient.UI;
using XUtliPoolLib;

namespace XMainClient
{
	// Token: 0x02000BAD RID: 2989
	internal class LevelRewardTerritoryHandler : DlgHandlerBase
	{
		// Token: 0x17003052 RID: 12370
		// (get) Token: 0x0600AB59 RID: 43865 RVA: 0x001F3774 File Offset: 0x001F1974
		protected override string FileName
		{
			get
			{
				return "Battle/LevelReward/GuildTerritoryResult";
			}
		}

		// Token: 0x0600AB5A RID: 43866 RVA: 0x001F378B File Offset: 0x001F198B
		protected override void Init()
		{
			base.Init();
			this._doc = XDocuments.GetSpecificDocument<XGuildTerritoryDocument>(XGuildTerritoryDocument.uuID);
			this.InitUI();
		}

		// Token: 0x0600AB5B RID: 43867 RVA: 0x001F37AC File Offset: 0x001F19AC
		public void InitUI()
		{
			this.m_btn_close = (base.PanelObject.transform.Find("Bg/Close").GetComponent("XUIButton") as IXUIButton);
			this.mSprGuild = (base.transform.Find("Bg/Town").GetComponent("XUISprite") as IXUISprite);
			this.m_lblGuildName = (base.transform.Find("Bg/Town/GuildName").GetComponent("XUILabel") as IXUILabel);
			this.m_lblTown = (base.transform.Find("Bg/Town/TownName").GetComponent("XUILabel") as IXUILabel);
			this.mRankWrap = (base.transform.Find("Bg/RankPanel/wrap").GetComponent("XUIWrapContent") as IXUIWrapContent);
			this.mGuildWrap = (base.transform.Find("Bg/guilds/wrap").GetComponent("XUIWrapContent") as IXUIWrapContent);
			this.m_lblMyRank = (base.transform.Find("Bg/Reward/Rank").GetComponent("XUILabel") as IXUILabel);
			this.mRwdTpl = base.transform.Find("Bg/Reward/ItemTpl").gameObject;
			this.tplPos = this.mRwdTpl.transform.localPosition;
			this.mRankWrap.RegisterItemUpdateEventHandler(new WrapItemUpdateEventHandler(this.WrapRankItemUpdate));
			this.mGuildWrap.RegisterItemUpdateEventHandler(new WrapItemUpdateEventHandler(this.WrapGuildItemUpdate));
			this.mRwdPool = new XUIPool(XSingleton<XGameUI>.singleton.m_uiTool);
			this.mRwdPool.SetupPool(this.mRwdTpl.transform.parent.gameObject, this.mRwdTpl, 2U, true);
		}

		// Token: 0x0600AB5C RID: 43868 RVA: 0x001F3961 File Offset: 0x001F1B61
		public override void RegisterEvent()
		{
			base.RegisterEvent();
			this.m_btn_close.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnCloseClick));
		}

		// Token: 0x0600AB5D RID: 43869 RVA: 0x001F3984 File Offset: 0x001F1B84
		private bool OnCloseClick(IXUIButton button)
		{
			PtcC2G_LeaveSceneReq proto = new PtcC2G_LeaveSceneReq();
			XSingleton<XClientNetwork>.singleton.Send(proto);
			return true;
		}

		// Token: 0x0600AB5E RID: 43870 RVA: 0x001F39AC File Offset: 0x001F1BAC
		protected override void OnShow()
		{
			base.OnShow();
			SceneTable.RowData sceneData = XSingleton<XSceneMgr>.singleton.GetSceneData(XSingleton<XScene>.singleton.SceneID);
			bool flag = XSingleton<XScene>.singleton.SceneType == SceneType.SCENE_CASTLE_FIGHT || XSingleton<XScene>.singleton.SceneType == SceneType.SCENE_CASTLE_WAIT;
			if (flag)
			{
				this._doc.SendGCFCommonReq(GCFReqType.GCF_FIGHT_RESULT);
			}
		}

		// Token: 0x0600AB5F RID: 43871 RVA: 0x001F3A08 File Offset: 0x001F1C08
		public void RefreshAll()
		{
			this.RefreshTitleInfo();
			this.RefreshGuildsInfo();
			this.RefreshMembersInfo();
			this.RefreshMyselfInfo();
			this.RefreshRwds();
		}

		// Token: 0x0600AB60 RID: 43872 RVA: 0x001F3A30 File Offset: 0x001F1C30
		private void RefreshTitleInfo()
		{
			XGuildTerritoryDocument specificDocument = XDocuments.GetSpecificDocument<XGuildTerritoryDocument>(XGuildTerritoryDocument.uuID);
			TerritoryBattle.RowData byID = XGuildTerritoryDocument.mGuildTerritoryList.GetByID(specificDocument.territoryid);
			bool flag = byID == null;
			if (flag)
			{
				XSingleton<XDebug>.singleton.AddErrorLog("territory is nil, id: " + specificDocument.territoryid, null, null, null, null, null);
			}
			else
			{
				this.mSprGuild.SetSprite(byID.territoryIcon);
				this.m_lblGuildName.SetText(specificDocument.winguild.guildname);
				this.m_lblTown.SetText(byID.territoryname);
			}
		}

		// Token: 0x0600AB61 RID: 43873 RVA: 0x001F3AC8 File Offset: 0x001F1CC8
		public void RefreshGuildsInfo()
		{
			XGuildTerritoryDocument specificDocument = XDocuments.GetSpecificDocument<XGuildTerritoryDocument>(XGuildTerritoryDocument.uuID);
			specificDocument.guilds.Sort(new Comparison<GCFGuild>(this.SortGuild));
			this.mGuildWrap.SetContentCount(specificDocument.guilds.Count, false);
		}

		// Token: 0x0600AB62 RID: 43874 RVA: 0x001F3B14 File Offset: 0x001F1D14
		private int SortGuild(GCFGuild x, GCFGuild y)
		{
			return (int)(y.brief.point - x.brief.point);
		}

		// Token: 0x0600AB63 RID: 43875 RVA: 0x001F3B40 File Offset: 0x001F1D40
		public void RefreshMembersInfo()
		{
			XGuildTerritoryDocument specificDocument = XDocuments.GetSpecificDocument<XGuildTerritoryDocument>(XGuildTerritoryDocument.uuID);
			this.mRankWrap.SetContentCount(specificDocument.roles.Count, false);
		}

		// Token: 0x0600AB64 RID: 43876 RVA: 0x001F3B74 File Offset: 0x001F1D74
		private void RefreshMyselfInfo()
		{
			XGuildTerritoryDocument specificDocument = XDocuments.GetSpecificDocument<XGuildTerritoryDocument>(XGuildTerritoryDocument.uuID);
			this.m_lblMyRank.SetText(specificDocument.mmyinfo.rank.ToString());
		}

		// Token: 0x0600AB65 RID: 43877 RVA: 0x001F3BAC File Offset: 0x001F1DAC
		private void WrapGuildItemUpdate(Transform t, int index)
		{
			XGuildTerritoryDocument specificDocument = XDocuments.GetSpecificDocument<XGuildTerritoryDocument>(XGuildTerritoryDocument.uuID);
			List<GCFGuild> guilds = specificDocument.guilds;
			bool flag = guilds.Count > index;
			if (flag)
			{
				IXUILabel ixuilabel = t.Find("Name").GetComponent("XUILabel") as IXUILabel;
				IXUILabel ixuilabel2 = t.Find("Score").GetComponent("XUILabel") as IXUILabel;
				IXUISprite ixuisprite = t.Find("Icon").GetComponent("XUISprite") as IXUISprite;
				ixuilabel.SetText(specificDocument.guilds[index].brief.guildname);
				ixuilabel2.SetText(specificDocument.guilds[index].brief.point.ToString());
				ixuisprite.SetSprite(XGuildDocument.GetPortraitName((int)specificDocument.guilds[index].brief.guildicon));
			}
		}

		// Token: 0x0600AB66 RID: 43878 RVA: 0x001F3C98 File Offset: 0x001F1E98
		private void WrapRankItemUpdate(Transform t, int index)
		{
			XGuildTerritoryDocument specificDocument = XDocuments.GetSpecificDocument<XGuildTerritoryDocument>(XGuildTerritoryDocument.uuID);
			List<GCFRoleBrief> roles = specificDocument.roles;
			bool flag = roles.Count > index;
			if (flag)
			{
				IXUILabel ixuilabel = t.Find("Rank").GetComponent("XUILabel") as IXUILabel;
				IXUILabel ixuilabel2 = t.Find("Name").GetComponent("XUILabel") as IXUILabel;
				IXUILabel ixuilabel3 = t.Find("Kill").GetComponent("XUILabel") as IXUILabel;
				IXUILabel ixuilabel4 = t.Find("Times").GetComponent("XUILabel") as IXUILabel;
				IXUILabel ixuilabel5 = t.Find("feats").GetComponent("XUILabel") as IXUILabel;
				ixuilabel.SetText(roles[index].rank.ToString());
				ixuilabel2.SetText(roles[index].rolename);
				ixuilabel3.SetText(roles[index].killcount.ToString());
				ixuilabel4.SetText(roles[index].occupycount.ToString());
				ixuilabel5.SetText(roles[index].feats.ToString());
			}
		}

		// Token: 0x0600AB67 RID: 43879 RVA: 0x001F3DDC File Offset: 0x001F1FDC
		private void RefreshRwds()
		{
			this.mRwdPool.ReturnAll(false);
			XGuildTerritoryDocument specificDocument = XDocuments.GetSpecificDocument<XGuildTerritoryDocument>(XGuildTerritoryDocument.uuID);
			List<ItemBrief> rwds = specificDocument.rwds;
			for (int i = 0; i < rwds.Count; i++)
			{
				GameObject gameObject = this.mRwdPool.FetchGameObject(false);
				gameObject.transform.localPosition = new Vector3(this.tplPos.x + (float)(this.mRwdPool.TplWidth * i), this.tplPos.y, this.tplPos.z);
				XItem xitem = XBagDocument.MakeXItem((int)rwds[i].itemID, rwds[i].isbind);
				xitem.itemCount = (int)rwds[i].itemCount;
				XItemDrawerMgr.Param.bBinding = rwds[i].isbind;
				XSingleton<XItemDrawerMgr>.singleton.DrawItem(gameObject, xitem);
				IXUISprite ixuisprite = gameObject.transform.Find("Icon").GetComponent("XUISprite") as IXUISprite;
				ixuisprite.ID = (ulong)rwds[i].itemID;
				ixuisprite.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(XSingleton<UiUtility>.singleton.OnItemClick));
			}
		}

		// Token: 0x0400401E RID: 16414
		private XGuildTerritoryDocument _doc = null;

		// Token: 0x0400401F RID: 16415
		private IXUIButton m_btn_close;

		// Token: 0x04004020 RID: 16416
		private IXUISprite mSprGuild;

		// Token: 0x04004021 RID: 16417
		private IXUILabel m_lblGuildName;

		// Token: 0x04004022 RID: 16418
		private IXUILabel m_lblTown;

		// Token: 0x04004023 RID: 16419
		private IXUIWrapContent mGuildWrap;

		// Token: 0x04004024 RID: 16420
		private IXUIWrapContent mRankWrap;

		// Token: 0x04004025 RID: 16421
		public IXUILabel m_lblMyRank;

		// Token: 0x04004026 RID: 16422
		private XUIPool mRwdPool;

		// Token: 0x04004027 RID: 16423
		private GameObject mRwdTpl;

		// Token: 0x04004028 RID: 16424
		private Vector3 tplPos;
	}
}
