﻿using System;
using System.Collections.Generic;
using UILib;
using UnityEngine;
using XMainClient.UI;
using XMainClient.UI.UICommon;
using XUtliPoolLib;

namespace XMainClient
{
	// Token: 0x02000E90 RID: 3728
	internal class XYYMallCategoryHandler : DlgHandlerBase
	{
		// Token: 0x170034B1 RID: 13489
		// (get) Token: 0x0600C719 RID: 50969 RVA: 0x002C2554 File Offset: 0x002C0754
		protected override string FileName
		{
			get
			{
				return "GameSystem/Welfare/YYMallCategory";
			}
		}

		// Token: 0x0600C71A RID: 50970 RVA: 0x002C256B File Offset: 0x002C076B
		protected override void OnShow()
		{
			base.OnShow();
			this.ShowIllustration();
		}

		// Token: 0x0600C71B RID: 50971 RVA: 0x002C257C File Offset: 0x002C077C
		protected override void Init()
		{
			this.m_TypeList.Clear();
			this.m_TypeTitle.Clear();
			this.m_Close = (base.PanelObject.transform.Find("Bg/Close").GetComponent("XUIButton") as IXUIButton);
			for (int i = 1; i < XFastEnumIntEqualityComparer<YYMallCategory>.ToInt(YYMallCategory.MAX); i++)
			{
				YYMallCategory yymallCategory = (YYMallCategory)i;
				string s = yymallCategory.ToString();
				Transform item = base.PanelObject.transform.Find(XSingleton<XCommon>.singleton.StringCombine("Bg/ScrollView/Table/", s));
				this.m_TypeTitle.Add(item);
				IXUIList ixuilist = base.PanelObject.transform.Find(XSingleton<XCommon>.singleton.StringCombine("Bg/ScrollView/Table/", s, "/Grid")).GetComponent("XUIList") as IXUIList;
				ixuilist.RegisterRepositionHandle(new OnAfterRepostion(this.OnListRefreshFinished));
				this.m_TypeList.Add(ixuilist);
			}
			Transform transform = base.PanelObject.transform.Find("Bg/ScrollView/ItemTpl");
			this.m_SpritePool.SetupPool(transform.parent.gameObject, transform.gameObject, 10U, false);
			this.m_ScrollView = (base.PanelObject.transform.Find("Bg/ScrollView").GetComponent("XUIScrollView") as IXUIScrollView);
			this.m_Table = (base.PanelObject.transform.Find("Bg/ScrollView/Table").GetComponent("XUITable") as IXUITable);
		}

		// Token: 0x0600C71C RID: 50972 RVA: 0x002C270C File Offset: 0x002C090C
		private void OnListRefreshFinished()
		{
			this.m_Table.Reposition();
			this.m_ScrollView.ResetPosition();
		}

		// Token: 0x0600C71D RID: 50973 RVA: 0x002C2727 File Offset: 0x002C0927
		public override void RegisterEvent()
		{
			this.m_Close.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnCloseClicked));
		}

		// Token: 0x0600C71E RID: 50974 RVA: 0x002C2742 File Offset: 0x002C0942
		protected override void OnHide()
		{
			this.m_CategoryList.Clear();
			base.OnHide();
		}

		// Token: 0x0600C71F RID: 50975 RVA: 0x002C2758 File Offset: 0x002C0958
		private void ShowIllustration()
		{
			this.m_CategoryList = XNormalShopDocument.ShopDoc.GetShopItemByPlayLevelAndShopType(XSysDefine.XSys_Welfare_YyMall);
			this.m_ScrollView.ResetPosition();
			this.m_SpritePool.FakeReturnAll();
			this.CreateIcon(YYMallCategory.Resource);
			this.CreateIcon(YYMallCategory.Special);
			this.CreateIcon(YYMallCategory.Privilege);
			this.m_SpritePool.ActualReturnAll(true);
		}

		// Token: 0x0600C720 RID: 50976 RVA: 0x002C27B8 File Offset: 0x002C09B8
		private void CreateIcon(YYMallCategory category)
		{
			int index = XFastEnumIntEqualityComparer<YYMallCategory>.ToInt(category) - 1;
			IXUIList ixuilist = this.m_TypeList[index];
			List<uint> list = this.m_CategoryList[index];
			this.m_TypeTitle[index].gameObject.SetActive(list.Count > 0);
			for (int i = 0; i < list.Count; i++)
			{
				GameObject gameObject = this.m_SpritePool.FetchGameObject(false);
				gameObject.transform.parent = ixuilist.gameObject.transform;
				XSingleton<XItemDrawerMgr>.singleton.normalItemDrawer.DrawItem(gameObject, (int)list[i], 1, false);
				IXUISprite ixuisprite = gameObject.transform.Find("Icon").GetComponent("XUISprite") as IXUISprite;
				ixuisprite.ID = (ulong)list[i];
				ixuisprite.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(this.OnClickItemIcon));
				XSingleton<XGameUI>.singleton.m_uiTool.ChangePanel(gameObject, ixuilist.GetParentUIRect(), ixuilist.GetParentPanel());
			}
			ixuilist.Refresh();
		}

		// Token: 0x0600C721 RID: 50977 RVA: 0x002C28D4 File Offset: 0x002C0AD4
		private void OnClickItemIcon(IXUISprite spr)
		{
			XItem mainItem = XBagDocument.MakeXItem((int)spr.ID, false);
			XSingleton<UiUtility>.singleton.ShowTooltipDialogWithSearchingCompare(mainItem, spr, false, 0U);
		}

		// Token: 0x0600C722 RID: 50978 RVA: 0x002C2900 File Offset: 0x002C0B00
		private void SetItemInfo(GameObject obj, uint itemID)
		{
			IXUISprite ixuisprite = obj.transform.Find("Icon").GetComponent("XUISprite") as IXUISprite;
			ixuisprite.ID = (ulong)itemID;
			XSingleton<XItemDrawerMgr>.singleton.normalItemDrawer.DrawItem(obj, (int)itemID, 0, false);
			ixuisprite.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(XSingleton<UiUtility>.singleton.OnItemClick));
		}

		// Token: 0x0600C723 RID: 50979 RVA: 0x002C2964 File Offset: 0x002C0B64
		private void SetSpriteInfo(GameObject obj, uint spriteID)
		{
			XSingleton<XItemDrawerMgr>.singleton.normalItemDrawer.DrawItem(obj, (int)spriteID, 0, false);
			IXUISprite ixuisprite = obj.transform.Find("Icon").GetComponent("XUISprite") as IXUISprite;
			ixuisprite.ID = (ulong)spriteID;
			ixuisprite.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(this.OnSpriteIconClicked));
		}

		// Token: 0x0600C724 RID: 50980 RVA: 0x002C29C4 File Offset: 0x002C0BC4
		private void OnSpriteIconClicked(IXUISprite spr)
		{
			uint spriteID = (uint)spr.ID;
			XSpriteSystemDocument specificDocument = XDocuments.GetSpecificDocument<XSpriteSystemDocument>(XSpriteSystemDocument.uuID);
			DlgBase<XSpriteDetailView, XSpriteDetailBehaviour>.singleton.ShowDetail(spriteID);
		}

		// Token: 0x0600C725 RID: 50981 RVA: 0x002C29F4 File Offset: 0x002C0BF4
		private bool OnCloseClicked(IXUIButton sp)
		{
			base.SetVisible(false);
			return true;
		}

		// Token: 0x04005757 RID: 22359
		private IXUIButton m_Close;

		// Token: 0x04005758 RID: 22360
		private XUIPool m_SpritePool = new XUIPool(XSingleton<XGameUI>.singleton.m_uiTool);

		// Token: 0x04005759 RID: 22361
		private IXUIScrollView m_ScrollView;

		// Token: 0x0400575A RID: 22362
		private IXUITable m_Table;

		// Token: 0x0400575B RID: 22363
		private List<IXUIList> m_TypeList = new List<IXUIList>();

		// Token: 0x0400575C RID: 22364
		private List<List<uint>> m_CategoryList = new List<List<uint>>();

		// Token: 0x0400575D RID: 22365
		private List<Transform> m_TypeTitle = new List<Transform>();
	}
}
