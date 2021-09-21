﻿using System;
using System.Collections.Generic;
using KKSG;
using UILib;
using UnityEngine;
using XMainClient.UI;
using XMainClient.UI.UICommon;
using XUtliPoolLib;

namespace XMainClient
{
	// Token: 0x02000CE3 RID: 3299
	internal class GameDescMallHander : DlgHandlerBase
	{
		// Token: 0x0600B8B8 RID: 47288 RVA: 0x00254648 File Offset: 0x00252848
		protected override void Init()
		{
			base.Init();
			this.doc = XDocuments.GetSpecificDocument<XGameMallDocument>(XGameMallDocument.uuID);
			this.m_Snapshot = (base.transform.Find("Bg/avatar").GetComponent("UIDummy") as IUIDummy);
			this.m_lblEnd = (base.transform.Find("Title").GetComponent("XUILabel") as IXUILabel);
			this.m_btnReset = (base.transform.Find("Bg/reset").GetComponent("XUIButton") as IXUIButton);
			this.m_btnAttr = (base.transform.Find("Bg/attr").GetComponent("XUIButton") as IXUIButton);
			this.m_lblDesc = (base.transform.Find("desc").GetComponent("XUILabel") as IXUILabel);
			this.m_sprRBg = (base.transform.Find("Bg").GetComponent("XUISprite") as IXUISprite);
			this.m_lblBuyCnt = (base.transform.Find("Item/T").GetComponent("XUILabel") as IXUILabel);
			this.m_sprTq = (base.transform.Find("tq").GetComponent("XUISprite") as IXUISprite);
			this.m_lblTq = (base.transform.Find("tq/t").GetComponent("XUILabel") as IXUILabel);
			this.m_objNil = base.transform.Find("ailin").gameObject;
			this.m_lblRefresh = (base.transform.Find("Title2").GetComponent("XUILabel") as IXUILabel);
			this.m_objTpl = base.transform.Find("Item").gameObject;
			this.m_tranBp = base.transform.Find("Bg/pb");
			this.m_TotalAttrPanel = base.transform.FindChild("AttrTotal").gameObject;
			DlgHandlerBase.EnsureCreate<FashionAttrTotalHandler>(ref this._attrHandler, this.m_TotalAttrPanel, null, false);
		}

		// Token: 0x0600B8B9 RID: 47289 RVA: 0x0025485C File Offset: 0x00252A5C
		public override void RegisterEvent()
		{
			base.RegisterEvent();
			this.m_btnReset.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnResetClick));
			this.m_btnAttr.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnAttrViewClick));
			this.m_sprRBg.RegisterSpriteDragEventHandler(new SpriteDragEventHandler(this.OnAvatarDrag));
			this.m_lblTq.RegisterLabelClickEventHandler(new LabelClickEventHandler(this.OnMemberPrivilegeClicked));
		}

		// Token: 0x0600B8BA RID: 47290 RVA: 0x002548D1 File Offset: 0x00252AD1
		public override void OnUnload()
		{
			DlgHandlerBase.EnsureUnload<FashionAttrTotalHandler>(ref this._attrHandler);
			this.m_outLook = null;
			base.OnUnload();
		}

		// Token: 0x0600B8BB RID: 47291 RVA: 0x002548EE File Offset: 0x00252AEE
		private void OnMemberPrivilegeClicked(IXUILabel btn)
		{
			DlgBase<XWelfareView, XWelfareBehaviour>.singleton.CheckActiveMemberPrivilege(MemberPrivilege.KingdomPrivilege_Commerce);
		}

		// Token: 0x0600B8BC RID: 47292 RVA: 0x00254900 File Offset: 0x00252B00
		protected override void OnShow()
		{
			base.OnShow();
			base.Alloc3DAvatarPool("GameDescMallHander", 1);
			this.doc = XDocuments.GetSpecificDocument<XGameMallDocument>(XGameMallDocument.uuID);
			bool flag = this.doc.currItemID == 0;
			if (flag)
			{
				this.ShowUI(false);
			}
		}

		// Token: 0x0600B8BD RID: 47293 RVA: 0x0025494C File Offset: 0x00252B4C
		public override void StackRefresh()
		{
			base.StackRefresh();
			base.Alloc3DAvatarPool("GameDescMallHander", 1);
		}

		// Token: 0x0600B8BE RID: 47294 RVA: 0x00254963 File Offset: 0x00252B63
		public override void LeaveStackTop()
		{
			base.LeaveStackTop();
			XSingleton<X3DAvatarMgr>.singleton.EnableCommonDummy(this.m_Dummy, null, false);
		}

		// Token: 0x0600B8BF RID: 47295 RVA: 0x00254980 File Offset: 0x00252B80
		protected override void OnHide()
		{
			base.Return3DAvatarPool();
			this.m_Dummy = null;
			base.OnHide();
		}

		// Token: 0x17003285 RID: 12933
		// (get) Token: 0x0600B8C0 RID: 47296 RVA: 0x00254998 File Offset: 0x00252B98
		private OutLook outlook
		{
			get
			{
				bool flag = this.m_outLook == null;
				if (flag)
				{
					this.m_outLook = new OutLook();
					bool flag2 = this.m_outLook.display_fashion == null;
					if (flag2)
					{
						this.m_outLook.display_fashion = new OutLookDisplayFashion();
					}
				}
				return this.m_outLook;
			}
		}

		// Token: 0x0600B8C1 RID: 47297 RVA: 0x002549F0 File Offset: 0x00252BF0
		private bool OnResetClick(IXUIButton btn)
		{
			bool flag = DlgBase<GameMallDlg, TabDlgBehaviour>.singleton.mallType == MallType.FASHION;
			if (flag)
			{
				XPlayerAttributes xplayerData = XSingleton<XAttributeMgr>.singleton.XPlayerData;
				uint unitType = (uint)XFastEnumIntEqualityComparer<RoleType>.ToInt(xplayerData.Profession);
				this.m_Dummy = XSingleton<X3DAvatarMgr>.singleton.CreateCommonRoleDummy(this.m_dummPool, xplayerData.RoleID, unitType, null, this.m_Snapshot, this.m_Dummy);
				this.m_Dummy.EngineObject.LocalEulerAngles = this.m_dummyAngle;
				this.m_btnReset.SetEnable(false, false);
				this.m_btnAttr.SetEnable(false, false);
			}
			return true;
		}

		// Token: 0x0600B8C2 RID: 47298 RVA: 0x00254A8C File Offset: 0x00252C8C
		private bool OnAttrViewClick(IXUIButton btn)
		{
			int currItemID = this.doc.currItemID;
			CIBShop currCIBShop = this.doc.currCIBShop;
			List<uint> list = ListPool<uint>.Get();
			this.GetFasionItems(currItemID, list);
			bool flag = list == null || list.Count < 1;
			bool result;
			if (flag)
			{
				this.RefreshDefault(currItemID, currCIBShop);
				result = true;
			}
			else
			{
				this._attrHandler.SetDataSource(list);
				this._attrHandler.SetVisible(true);
				result = true;
			}
			return result;
		}

		// Token: 0x0600B8C3 RID: 47299 RVA: 0x00254B04 File Offset: 0x00252D04
		public void Refresh()
		{
			this.m_objNil.SetActive(this.doc.currItemID == 0);
			bool flag = this.doc.currItemID == 0;
			if (flag)
			{
				this.ShowUI(false);
			}
			else
			{
				int currItemID = this.doc.currItemID;
				CIBShop currCIBShop = this.doc.currCIBShop;
				XSingleton<XItemDrawerMgr>.singleton.normalItemDrawer.DrawItem(this.m_objTpl, currItemID, 0, false);
				IXUISprite ixuisprite = this.m_objTpl.transform.FindChild("Icon").GetComponent("XUISprite") as IXUISprite;
				ixuisprite.ID = (ulong)((long)currItemID);
				ixuisprite.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(XSingleton<UiUtility>.singleton.OnItemClick));
				this.m_lblRefresh.SetText(XStringDefineProxy.GetString("MALL_LIMITE"));
				this.m_lblRefresh.SetVisible(MallType.WEEK == DlgBase<GameMallDlg, TabDlgBehaviour>.singleton.mallType);
				this.m_lblEnd.SetVisible(currCIBShop.sinfo.nlimittime > 0U);
				this.m_lblEnd.SetText(XStringDefineProxy.GetString("MALL_DEADLINE", new object[]
				{
					this.UnixDate(currCIBShop.sinfo.nlimittime).ToString("yyyy-MM-dd HH")
				}));
				this.m_sprTq.gameObject.SetActive((long)this.doc.currItemID == (long)((ulong)DlgBase<GameMallDlg, TabDlgBehaviour>.singleton.privilegeID) && currCIBShop.sinfo.nlimitcount > 0U);
				XWelfareDocument specificDocument = XDocuments.GetSpecificDocument<XWelfareDocument>(XWelfareDocument.uuID);
				this.m_sprTq.SetSprite(specificDocument.GetMemberPrivilegeIcon(MemberPrivilege.KingdomPrivilege_Commerce));
				this.m_lblTq.SetText(XStringDefineProxy.GetString("MALL_PRIVILEGE", new object[]
				{
					specificDocument.GetMemberPrivilegeConfig(MemberPrivilege.KingdomPrivilege_Commerce).BuyGreenAgateLimit
				}));
				MallType mallType = DlgBase<GameMallDlg, TabDlgBehaviour>.singleton.mallType;
				MallType mallType2 = mallType;
				if (mallType2 != MallType.FASHION)
				{
					if (mallType2 != MallType.RIDE)
					{
						this.RefreshDefault(currItemID, currCIBShop);
					}
					else
					{
						this.ShowAvatar(true);
						this.m_Snapshot.transform.localScale = 150f * Vector3.one;
						this.m_Snapshot.transform.localPosition = new Vector3(-10f, -190f, 184f);
						uint num = this.TransItemToPresentID((uint)currItemID);
						string rideAnim = this.GetRideAnim((uint)currItemID);
						bool flag2 = num == 0U || string.IsNullOrEmpty(rideAnim);
						if (flag2)
						{
							this.RefreshDefault(currItemID, currCIBShop);
							XSingleton<XDebug>.singleton.AddWarningLog("item id is wrong ", currItemID.ToString(), " presentid: ", num.ToString(), null, null);
						}
						else
						{
							this.m_Dummy = XSingleton<X3DAvatarMgr>.singleton.CreateCommonEntityDummy(this.m_dummPool, num, this.m_Snapshot, this.m_Dummy, 1f);
							bool flag3 = this.m_Dummy != null;
							if (flag3)
							{
								this.m_Dummy.SetAnimation(rideAnim);
							}
						}
					}
				}
				else
				{
					this.m_btnReset.SetEnable(true, false);
					this.m_btnAttr.SetEnable(true, false);
					this.ShowAvatar(true);
					XPlayerAttributes xplayerData = XSingleton<XAttributeMgr>.singleton.XPlayerData;
					this.m_Snapshot.transform.localScale = 250f * Vector3.one;
					this.m_Snapshot.transform.localPosition = new Vector3(-10f, -190f, 184f);
					bool flag4 = xplayerData != null;
					if (flag4)
					{
						List<uint> list = ListPool<uint>.Get();
						this.GetFasionItems(currItemID, list);
						bool flag5 = list == null || list.Count < 1;
						if (flag5)
						{
							this.RefreshDefault(currItemID, currCIBShop);
						}
						else
						{
							this.outlook.display_fashion.display_fashions.Clear();
							this.outlook.display_fashion.display_fashions.AddRange(list);
							uint unitType = (uint)XFastEnumIntEqualityComparer<RoleType>.ToInt(xplayerData.Profession);
							this.m_Dummy = XSingleton<X3DAvatarMgr>.singleton.CreateCommonRoleDummy(this.m_dummPool, xplayerData.RoleID, unitType, this.m_outLook, this.m_Snapshot, this.m_Dummy);
							ListPool<uint>.Release(list);
							bool flag6 = this.m_Dummy != null && this.m_Dummy.EngineObject != null;
							if (flag6)
							{
								this.m_Dummy.EngineObject.LocalEulerAngles = this.m_dummyAngle;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B8C4 RID: 47300 RVA: 0x00254F60 File Offset: 0x00253160
		private void RefreshDefault(int itemid, CIBShop cShop)
		{
			this.ShowAvatar(false);
			ItemList.RowData itemConf = XBagDocument.GetItemConf(itemid);
			bool flag = itemConf != null;
			if (flag)
			{
				this.m_lblDesc.SetText(XSingleton<UiUtility>.singleton.ReplaceReturn(itemConf.ItemDescription));
			}
			this.m_lblBuyCnt.SetVisible(cShop.sinfo.nlimitcount > 0U);
			XWelfareDocument specificDocument = XDocuments.GetSpecificDocument<XWelfareDocument>(XWelfareDocument.uuID);
			float num = 0f;
			bool flag2 = DlgBase<GameMallDlg, TabDlgBehaviour>.singleton.privilegeID == cShop.sinfo.itemid && specificDocument.IsOwnMemberPrivilege(MemberPrivilege.KingdomPrivilege_Commerce);
			if (flag2)
			{
				num = (float)specificDocument.GetMemberPrivilegeConfig(MemberPrivilege.KingdomPrivilege_Commerce).BuyGreenAgateLimit / 100f;
			}
			int num2 = 0;
			PayMemberPrivilege payMemberPrivilege = specificDocument.PayMemberPrivilege;
			bool flag3 = payMemberPrivilege != null;
			if (flag3)
			{
				for (int i = 0; i < payMemberPrivilege.usedPrivilegeShop.Count; i++)
				{
					bool flag4 = (long)payMemberPrivilege.usedPrivilegeShop[i].goodsID == (long)((ulong)cShop.sinfo.goodsid);
					if (flag4)
					{
						num2 = payMemberPrivilege.usedPrivilegeShop[i].usedCount;
						break;
					}
				}
			}
			string key = (cShop.row.refreshtype == 7U && cShop.row.type != 1U) ? "MALL_BUYCNT2" : "MALL_BUYCNT";
			bool flag5 = num > 0f && (float)num2 < cShop.row.buycount * num;
			if (flag5)
			{
				this.m_lblBuyCnt.SetText(XStringDefineProxy.GetString(key, new object[]
				{
					"[ffff00]" + Mathf.FloorToInt(cShop.sinfo.nlimitcount + cShop.row.buycount * num - cShop.sinfo.nbuycount - (float)num2) + "[-]"
				}));
			}
			else
			{
				this.m_lblBuyCnt.SetText(XStringDefineProxy.GetString(key, new object[]
				{
					Mathf.FloorToInt(cShop.sinfo.nlimitcount + cShop.row.buycount * num - cShop.sinfo.nbuycount - (float)num2)
				}));
			}
			bool flag6 = num > 0f;
			this.m_sprTq.SetGrey(flag6);
			this.m_lblTq.SetEnabled(flag6);
		}

		// Token: 0x0600B8C5 RID: 47301 RVA: 0x002551B8 File Offset: 0x002533B8
		private void GetFasionItems(int itemid, List<uint> fashions)
		{
			ItemList.RowData itemConf = XBagDocument.GetItemConf(itemid);
			bool flag = itemConf == null || fashions == null;
			if (!flag)
			{
				bool flag2 = itemConf.ItemType == 2;
				if (flag2)
				{
					ChestList.RowData chestConf = XBagDocument.GetChestConf(itemid);
					bool flag3 = chestConf == null;
					if (flag3)
					{
						XSingleton<XDebug>.singleton.AddErrorLog("chest list is nil ", itemid.ToString(), null, null, null, null);
					}
					else
					{
						List<DropList.RowData> list = new List<DropList.RowData>();
						bool flag4 = XBagDocument.TryGetDropConf(chestConf.DropID, ref list);
						if (flag4)
						{
							for (int i = 0; i < list.Count; i++)
							{
								fashions.Add((uint)list[i].ItemID);
							}
						}
					}
				}
				else
				{
					bool flag5 = itemConf.ItemType == 5;
					if (flag5)
					{
						fashions.Add((uint)itemid);
					}
				}
			}
		}

		// Token: 0x0600B8C6 RID: 47302 RVA: 0x00255290 File Offset: 0x00253490
		private void ShowAvatar(bool show)
		{
			this.m_Snapshot.transform.gameObject.SetActive(show);
			this.m_lblDesc.SetVisible(!show);
			this.m_btnReset.SetVisible(DlgBase<GameMallDlg, TabDlgBehaviour>.singleton.mallType == MallType.FASHION);
			int itemid = (this.doc != null) ? this.doc.currItemID : 0;
			bool flag = XFashionDocument.IsTargetPart(itemid, FashionPosition.Hair);
			this.m_btnAttr.SetVisible(DlgBase<GameMallDlg, TabDlgBehaviour>.singleton.mallType == MallType.FASHION && !flag);
			this.m_lblBuyCnt.SetVisible(!show);
			this.m_objTpl.SetActive(!show);
			this.m_tranBp.gameObject.SetActive(show);
		}

		// Token: 0x0600B8C7 RID: 47303 RVA: 0x00255350 File Offset: 0x00253550
		private void ShowUI(bool show)
		{
			this.m_Snapshot.transform.gameObject.SetActive(show);
			this.m_lblDesc.SetVisible(show);
			this.m_btnReset.SetVisible(show);
			this.m_btnAttr.SetVisible(show);
			this.m_lblBuyCnt.SetVisible(show);
			this.m_objTpl.SetActive(show);
			this.m_tranBp.gameObject.SetActive(show);
		}

		// Token: 0x0600B8C8 RID: 47304 RVA: 0x002553C8 File Offset: 0x002535C8
		protected bool OnAvatarDrag(Vector2 delta)
		{
			bool flag = this.m_Dummy != null;
			if (flag)
			{
				this.m_Dummy.EngineObject.Rotate(Vector3.up, -delta.x / 2f);
				this.m_dummyAngle = this.m_Dummy.EngineObject.LocalEulerAngles;
			}
			else
			{
				XSingleton<X3DAvatarMgr>.singleton.RotateMain(-delta.x / 2f);
			}
			return true;
		}

		// Token: 0x0600B8C9 RID: 47305 RVA: 0x00255440 File Offset: 0x00253640
		private uint TransItemToPresentID(uint itemid)
		{
			uint petID = XPetDocument.GetPetID(itemid);
			return XPetDocument.GetPresentID(petID);
		}

		// Token: 0x0600B8CA RID: 47306 RVA: 0x00255460 File Offset: 0x00253660
		private string GetRideAnim(uint itemid)
		{
			uint petID = XPetDocument.GetPetID(itemid);
			bool flag = petID == 0U;
			string result;
			if (flag)
			{
				XSingleton<XDebug>.singleton.AddWarningLog("NOT FOUND PET ", null, null, null, null, null);
				result = null;
			}
			else
			{
				XPetDocument specificDocument = XDocuments.GetSpecificDocument<XPetDocument>(XPetDocument.uuID);
				PetBubble.RowData petBubble = specificDocument.GetPetBubble(XPetActionFile.IDLE, petID);
				bool flag2 = petBubble == null;
				if (flag2)
				{
					XSingleton<XDebug>.singleton.AddWarningLog(string.Concat(new object[]
					{
						"PetBubble No Find\nitemid:",
						itemid,
						" petid:",
						petID
					}), null, null, null, null, null);
					result = null;
				}
				else
				{
					result = petBubble.ActionFile;
				}
			}
			return result;
		}

		// Token: 0x0600B8CB RID: 47307 RVA: 0x00255508 File Offset: 0x00253708
		private DateTime UnixDate(uint sp)
		{
			DateTime minValue = DateTime.MinValue;
			return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(sp);
		}

		// Token: 0x0400494B RID: 18763
		private XDummy m_Dummy;

		// Token: 0x0400494C RID: 18764
		public IXUILabel m_lblEnd;

		// Token: 0x0400494D RID: 18765
		public IXUILabel m_lblRefresh;

		// Token: 0x0400494E RID: 18766
		public IXUILabel m_lblDesc;

		// Token: 0x0400494F RID: 18767
		public IXUIButton m_btnReset;

		// Token: 0x04004950 RID: 18768
		public IXUIButton m_btnAttr;

		// Token: 0x04004951 RID: 18769
		public IXUISprite m_sprRBg;

		// Token: 0x04004952 RID: 18770
		public GameObject m_objTpl;

		// Token: 0x04004953 RID: 18771
		public IXUILabel m_lblBuyCnt;

		// Token: 0x04004954 RID: 18772
		public IXUISprite m_sprTq;

		// Token: 0x04004955 RID: 18773
		public IXUILabel m_lblTq;

		// Token: 0x04004956 RID: 18774
		public GameObject m_objNil;

		// Token: 0x04004957 RID: 18775
		private IUIDummy m_Snapshot;

		// Token: 0x04004958 RID: 18776
		public Transform m_tranBp;

		// Token: 0x04004959 RID: 18777
		public GameObject m_TotalAttrPanel;

		// Token: 0x0400495A RID: 18778
		private FashionAttrTotalHandler _attrHandler;

		// Token: 0x0400495B RID: 18779
		private OutLook m_outLook;

		// Token: 0x0400495C RID: 18780
		private XGameMallDocument doc;

		// Token: 0x0400495D RID: 18781
		private Vector3 m_dummyAngle = new Vector3(0f, 180f, 0f);
	}
}
