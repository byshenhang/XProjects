﻿using System;
using UILib;
using XUtliPoolLib;

namespace XMainClient.UI
{

	internal class SuperRiskOnlineBoxHandler : DlgHandlerBase
	{

		protected override void Init()
		{
			this._doc = XSuperRiskDocument.Doc;
			this.m_CancleBtn = (base.PanelObject.transform.Find("no").GetComponent("XUIButton") as IXUIButton);
			this.m_BuyBtn = (base.PanelObject.transform.Find("Buy").GetComponent("XUIButton") as IXUIButton);
			this.m_CostLab = (base.PanelObject.transform.Find("Buy/Cost").GetComponent("XUILabel") as IXUILabel);
			this.m_CostIcon = (base.PanelObject.transform.Find("Buy/Cost/b").GetComponent("XUISprite") as IXUISprite);
			this.m_BoxTween = (base.PanelObject.transform.Find("Box").GetComponent("XUIPlayTween") as IXUITweenTool);
		}

		public override void RegisterEvent()
		{
			this.m_CancleBtn.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnCancleClick));
			this.m_BuyBtn.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnBuyClick));
			base.RegisterEvent();
		}

		protected override void OnHide()
		{
			base.OnHide();
		}

		protected override void OnShow()
		{
			this.FillContent();
			base.OnShow();
		}

		private void FillContent()
		{
			bool flag = this._doc.OnlineBoxCost != null;
			if (flag)
			{
				this.m_CostLab.SetText(this._doc.OnlineBoxCost.itemCount.ToString());
				ItemList.RowData itemConf = XBagDocument.GetItemConf((int)this._doc.OnlineBoxCost.itemID);
				this.m_CostIcon.SetSprite(itemConf.ItemIcon1[0]);
			}
			XSingleton<XAudioMgr>.singleton.PlayUISound("Audio/UI/UI_Anim_DiceGame_OpenChest", true, AudioChannel.Action);
			this.m_BoxTween.SetTweenGroup(0);
			this.m_BoxTween.ResetTweenByGroup(true, 0);
			this.m_BoxTween.PlayTween(true, -1f);
		}

		private bool OnCancleClick(IXUIButton btn)
		{
			base.SetVisible(false);
			return true;
		}

		private bool OnBuyClick(IXUIButton btn)
		{
			bool flag = this._doc.OnlineBoxCost == null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				ulong itemCount = XBagDocument.BagDoc.GetItemCount((int)this._doc.OnlineBoxCost.itemID);
				bool flag2 = (ulong)this._doc.OnlineBoxCost.itemCount > itemCount;
				if (flag2)
				{
					XSingleton<UiUtility>.singleton.ShowItemAccess((int)this._doc.OnlineBoxCost.itemID, null);
					result = true;
				}
				else
				{
					this._doc.ReqBuyOnlineBox();
					result = true;
				}
			}
			return result;
		}

		private XSuperRiskDocument _doc;

		private IXUITweenTool m_BoxTween;

		private IXUIButton m_BuyBtn;

		private IXUIButton m_CancleBtn;

		private IXUILabel m_CostLab;

		private IXUISprite m_CostIcon;
	}
}
