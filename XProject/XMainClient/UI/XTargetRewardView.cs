﻿using System;
using UILib;
using UnityEngine;
using XUtliPoolLib;

namespace XMainClient.UI
{
	// Token: 0x020016CE RID: 5838
	internal class XTargetRewardView : DlgHandlerBase
	{
		// Token: 0x0600F0C5 RID: 61637 RVA: 0x00350550 File Offset: 0x0034E750
		protected override void Init()
		{
			base.Init();
			this.popWindow.panelObject = base.PanelObject.transform.Find("Top");
			this.popWindow.closeBtn = (base.PanelObject.transform.Find("Top/Bg/Close").GetComponent("XUIButton") as IXUIButton);
			this.popWindow.wrapContent = (base.PanelObject.transform.FindChild("Top/Bg/detail/WrapContent").GetComponent("XUIWrapContent") as IXUIWrapContent);
			this.popWindow.panelScrollView = (base.PanelObject.transform.FindChild("Top/Bg/detail").GetComponent("XUIScrollView") as IXUIScrollView);
			this.popWindow.init();
			this._doc = XDocuments.GetSpecificDocument<XTargetRewardDocument>(XTargetRewardDocument.uuID);
			this._doc.InitOpenGoalAward();
			int num = 0;
			for (int i = 0; i < this.m_padTabs.Length; i++)
			{
				this.m_padTabs[i] = (base.PanelObject.transform.Find("padTabs/TabList/TabTpl" + i + "/Bg").GetComponent("XUICheckBox") as IXUICheckBox);
				this.m_padPoint[i] = (base.PanelObject.transform.Find("padTabs/TabList/TabTpl" + i + "/Bg/RedPoint").GetComponent("XUISprite") as IXUISprite);
				IXUISprite ixuisprite = base.PanelObject.transform.Find("padTabs/TabList/TabTpl" + i).GetComponent("XUISprite") as IXUISprite;
				this.m_padPoint[i].gameObject.SetActive(false);
				bool flag = this.m_padTabs[i] != null;
				if (flag)
				{
					this.m_padTabs[i].ID = (ulong)i;
					this.m_padTabs[i].RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnTabControlStateChange));
				}
				bool flag2 = ixuisprite != null;
				if (flag2)
				{
					ixuisprite.gameObject.SetActive(this._doc.m_isGoalOpen[i + 1]);
					bool flag3 = this._doc.m_isGoalOpen[i + 1] && num == 0;
					if (flag3)
					{
						num = i;
						this.m_padTabs[i].ForceSetFlag(true);
					}
					else
					{
						this.m_padTabs[i].ForceSetFlag(false);
					}
				}
			}
			this.m_GoalList = (base.PanelObject.transform.Find("padTabs/TabList").GetComponent("XUIList") as IXUIList);
			this.m_PanelScrollView = (base.PanelObject.transform.FindChild("detail/detail").GetComponent("XUIScrollView") as IXUIScrollView);
			this.m_WrapContent = (base.PanelObject.transform.FindChild("detail/detail/WrapContent").GetComponent("XUIWrapContent") as IXUIWrapContent);
			this.m_WrapContent.RegisterItemUpdateEventHandler(new WrapItemUpdateEventHandler(this.WrapContentItemUpdated));
			this.m_GoalList.Refresh();
		}

		// Token: 0x0600F0C6 RID: 61638 RVA: 0x00350860 File Offset: 0x0034EA60
		private void WrapContentItemUpdated(Transform t, int index)
		{
			bool flag = this._doc != null;
			if (flag)
			{
				bool flag2 = index < this._doc.targetAwardDetails.Count && index >= 0;
				if (flag2)
				{
					TargetItemInfo info = this._doc.targetAwardDetails[index];
					this._SetRecord(t, info);
				}
			}
			else
			{
				XSingleton<XDebug>.singleton.AddErrorLog("_doc is nil or index: ", index.ToString(), null, null, null, null);
			}
		}

		// Token: 0x0600F0C7 RID: 61639 RVA: 0x003508DC File Offset: 0x0034EADC
		protected void _SetProgressBar(IXUILabel label, IXUIProgress progressBar, TargetItemInfo info)
		{
			int num = info.subItems.Count - 1;
			int num2 = (int)Math.Min(info.gottenAwardsIndex, info.doneIndex);
			num2 = Math.Min(num2, num);
			GoalAwards.RowData rowData = info.subItems[num2];
			double num3 = info.totalvalue;
			double num4 = rowData.AwardsValue;
			bool flag = (ulong)info.gottenAwardsIndex == (ulong)((long)(num + 1));
			if (flag)
			{
				label.SetVisible(false);
				progressBar.SetVisible(false);
			}
			else
			{
				label.SetVisible(true);
				progressBar.SetVisible(true);
				bool flag2 = rowData.ShowType == 2U;
				if (flag2)
				{
					bool flag3 = info.gottenAwardsIndex < info.doneIndex;
					if (flag3)
					{
						num3 = 1.0;
						num4 = 1.0;
					}
					else
					{
						num3 = 0.0;
						num4 = 1.0;
					}
				}
				label.SetText(XSingleton<UiUtility>.singleton.NumberFormat((ulong)num3) + " / " + XSingleton<UiUtility>.singleton.NumberFormat((ulong)num4));
				bool flag4 = (ulong)info.gottenAwardsIndex < (ulong)((long)(num + 1)) && info.gottenAwardsIndex == info.doneIndex;
				if (flag4)
				{
					bool flag5 = num3 < num4;
					if (flag5)
					{
						progressBar.value = (float)(num3 / num4);
					}
					else
					{
						progressBar.value = 0f;
					}
				}
				else
				{
					progressBar.value = 1f;
				}
			}
		}

		// Token: 0x0600F0C8 RID: 61640 RVA: 0x00350A48 File Offset: 0x0034EC48
		protected void _SetRecord(Transform t, TargetItemInfo info)
		{
			IXUILabel ixuilabel = t.Find("TLabel").GetComponent("XUILabel") as IXUILabel;
			IXUILabel ixuilabel2 = t.Find("DLabel").GetComponent("XUILabel") as IXUILabel;
			IXUILabel ixuilabel3 = t.Find("ch").GetComponent("XUILabel") as IXUILabel;
			IXUIButton ixuibutton = t.Find("Get").GetComponent("XUIButton") as IXUIButton;
			IXUISprite ixuisprite = t.Find("Fini").GetComponent("XUISprite") as IXUISprite;
			IXUISprite ixuisprite2 = t.Find("RedPoint").GetComponent("XUISprite") as IXUISprite;
			IXUIProgress ixuiprogress = t.Find("slider").GetComponent("XUIProgress") as IXUIProgress;
			IXUILabel label = t.Find("slider/PLabel").GetComponent("XUILabel") as IXUILabel;
			Transform[] array = new Transform[this.maxAwardNum];
			for (int i = 0; i < this.maxAwardNum; i++)
			{
				array[i] = t.Find("tmp/ItemTpl1_" + (i + 1));
			}
			int num = info.subItems.Count - 1;
			int num2 = (int)Math.Min(info.gottenAwardsIndex, info.doneIndex);
			num2 = Math.Min(num2, num);
			GoalAwards.RowData rowData = info.subItems[num2];
			ixuilabel.SetText(rowData.Description);
			ixuilabel2.SetText(rowData.Explanation);
			ixuisprite.SetVisible((ulong)info.gottenAwardsIndex == (ulong)((long)(num + 1)));
			ixuisprite2.SetVisible(info.gottenAwardsIndex < info.doneIndex);
			this._SetProgressBar(label, ixuiprogress, info);
			int num3 = Math.Min(this.maxAwardNum, rowData.Awards.Count);
			bool flag = (ulong)info.gottenAwardsIndex == (ulong)((long)(num + 1));
			if (flag)
			{
				num3 = 0;
			}
			for (int j = 0; j < num3; j++)
			{
				array[j].gameObject.SetActive(true);
				int num4 = (int)rowData.Awards[j, 0];
				int itemCount = (int)rowData.Awards[j, 1];
				XSingleton<XItemDrawerMgr>.singleton.normalItemDrawer.DrawItem(array[j].gameObject, num4, itemCount, false);
				IXUISprite ixuisprite3 = array[j].gameObject.transform.Find("Icon").GetComponent("XUISprite") as IXUISprite;
				ixuisprite3.ID = (ulong)((long)num4);
				ixuisprite3.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(XSingleton<UiUtility>.singleton.OnItemClick));
			}
			for (int k = num3; k < this.maxAwardNum; k++)
			{
				array[k].gameObject.SetActive(false);
			}
			bool visible = info.gottenAwardsIndex >= info.doneIndex && (ulong)info.gottenAwardsIndex != (ulong)((long)(num + 1));
			bool visible2 = info.gottenAwardsIndex < info.doneIndex;
			ixuiprogress.SetVisible(visible);
			ixuibutton.SetVisible(visible2);
			ixuibutton.ID = (ulong)info.goalAwardsID;
			ixuibutton.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnBtnClick));
			ixuilabel3.ID = (ulong)info.goalAwardsID;
			ixuilabel3.RegisterLabelClickEventHandler(new LabelClickEventHandler(this.OnLabelClick));
		}

		// Token: 0x0600F0C9 RID: 61641 RVA: 0x00350DB2 File Offset: 0x0034EFB2
		private void OnLabelClick(IXUILabel uiSprite)
		{
			this.ShowDetailView((int)uiSprite.ID);
		}

		// Token: 0x0600F0CA RID: 61642 RVA: 0x00350DC4 File Offset: 0x0034EFC4
		private void ShowDetailView(int goalAwardsID)
		{
			TargetItemInfo info = new TargetItemInfo();
			bool flag = false;
			for (int i = 0; i < this._doc.targetAwardDetails.Count; i++)
			{
				bool flag2 = (ulong)this._doc.targetAwardDetails[i].goalAwardsID == (ulong)((long)goalAwardsID);
				if (flag2)
				{
					flag = true;
					info = this._doc.targetAwardDetails[i];
					break;
				}
			}
			bool flag3 = !flag;
			if (!flag3)
			{
				this.popWindow.ShowPopView(info);
			}
		}

		// Token: 0x0600F0CB RID: 61643 RVA: 0x00350E4C File Offset: 0x0034F04C
		private bool OnBtnClick(IXUIButton btn)
		{
			TargetItemInfo targetItemInfo = new TargetItemInfo();
			bool flag = false;
			for (int i = 0; i < this._doc.targetAwardDetails.Count; i++)
			{
				bool flag2 = (ulong)this._doc.targetAwardDetails[i].goalAwardsID == (ulong)((long)((int)btn.ID));
				if (flag2)
				{
					flag = true;
					targetItemInfo = this._doc.targetAwardDetails[i];
					break;
				}
			}
			bool flag3 = flag && targetItemInfo.gottenAwardsIndex < targetItemInfo.doneIndex;
			if (flag3)
			{
				this._doc.ClaimAchieve((int)btn.ID);
			}
			return true;
		}

		// Token: 0x0600F0CC RID: 61644 RVA: 0x0019EEB0 File Offset: 0x0019D0B0
		public override void RegisterEvent()
		{
			base.RegisterEvent();
		}

		// Token: 0x0600F0CD RID: 61645 RVA: 0x00350EF8 File Offset: 0x0034F0F8
		protected override void OnShow()
		{
			base.OnShow();
			this._doc = XDocuments.GetSpecificDocument<XTargetRewardDocument>(XTargetRewardDocument.uuID);
			this._doc.rwdView = this;
			this.RefreshRedPoint();
			this.m_padTabs[0].ForceSetFlag(true);
			this.ReqDetailInfo(0);
		}

		// Token: 0x0600F0CE RID: 61646 RVA: 0x001E669E File Offset: 0x001E489E
		public override void OnUpdate()
		{
			base.OnUpdate();
		}

		// Token: 0x0600F0CF RID: 61647 RVA: 0x0025083F File Offset: 0x0024EA3F
		protected override void OnHide()
		{
			base.OnHide();
			base.PanelObject.SetActive(false);
		}

		// Token: 0x0600F0D0 RID: 61648 RVA: 0x00350F47 File Offset: 0x0034F147
		public override void OnUnload()
		{
			this._doc = null;
			base.OnUnload();
		}

		// Token: 0x0600F0D1 RID: 61649 RVA: 0x00350F58 File Offset: 0x0034F158
		public bool OnTabControlStateChange(IXUICheckBox chkBox)
		{
			bool bChecked = chkBox.bChecked;
			if (bChecked)
			{
				this.OnTabClick((int)chkBox.ID);
			}
			return true;
		}

		// Token: 0x0600F0D2 RID: 61650 RVA: 0x00350F85 File Offset: 0x0034F185
		private void OnTabClick(int index)
		{
			this.ReqDetailInfo(index);
		}

		// Token: 0x0600F0D3 RID: 61651 RVA: 0x00350F90 File Offset: 0x0034F190
		private void ReqDetailInfo(int index)
		{
			bool flag = this._doc != null;
			if (flag)
			{
				this.m_targetRewardType = index + TargetRewardType.Athletics;
				this._doc.FetchTargetRewardType(this.m_targetRewardType);
			}
		}

		// Token: 0x0600F0D4 RID: 61652 RVA: 0x00350FC8 File Offset: 0x0034F1C8
		public void RefreshDetails()
		{
			this.m_WrapContent.SetContentCount(this._doc.targetAwardDetails.Count, false);
			this.m_PanelScrollView.ResetPosition();
		}

		// Token: 0x0600F0D5 RID: 61653 RVA: 0x00350FF4 File Offset: 0x0034F1F4
		public void RefreshRedPoint()
		{
			for (int i = 0; i < this.m_padPoint.Length; i++)
			{
				this.m_padPoint[i].SetVisible(false);
			}
			for (int j = 0; j < this._doc.m_redList.Count; j++)
			{
				int num = (int)this._doc.m_redList[j];
				bool flag = num <= this.m_padPoint.Length;
				if (flag)
				{
					this.m_padPoint[num - 1].SetVisible(true);
				}
			}
		}

		// Token: 0x040066BC RID: 26300
		private XTargetRewardDocument _doc = null;

		// Token: 0x040066BD RID: 26301
		private XTargetRewardPopWindow popWindow = new XTargetRewardPopWindow();

		// Token: 0x040066BE RID: 26302
		private IXUICheckBox[] m_padTabs = new IXUICheckBox[4];

		// Token: 0x040066BF RID: 26303
		private IXUISprite[] m_padPoint = new IXUISprite[4];

		// Token: 0x040066C0 RID: 26304
		public TargetRewardType m_targetRewardType;

		// Token: 0x040066C1 RID: 26305
		public IXUIWrapContent m_WrapContent;

		// Token: 0x040066C2 RID: 26306
		public IXUIScrollView m_PanelScrollView;

		// Token: 0x040066C3 RID: 26307
		private IXUIList m_GoalList;

		// Token: 0x040066C4 RID: 26308
		private int maxAwardNum = 4;
	}
}
