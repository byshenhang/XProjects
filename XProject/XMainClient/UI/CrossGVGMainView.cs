﻿using System;
using System.Collections.Generic;
using KKSG;
using UILib;
using XMainClient.UI.UICommon;
using XMainClient.Utility;
using XUtliPoolLib;

namespace XMainClient.UI
{

	internal class CrossGVGMainView : TabDlgBase<CrossGVGMainView>
	{

		public override int sysid
		{
			get
			{
				return XFastEnumIntEqualityComparer<XSysDefine>.ToInt(XSysDefine.XSys_CrossGVG);
			}
		}

		public override string fileName
		{
			get
			{
				return "Guild/CrossGVG/CrossGVGArenaDlg";
			}
		}

		public override int layer
		{
			get
			{
				return 1;
			}
		}

		public override int group
		{
			get
			{
				return 1;
			}
		}

		public override bool pushstack
		{
			get
			{
				return true;
			}
		}

		public override bool hideMainMenu
		{
			get
			{
				return true;
			}
		}

		public override bool fullscreenui
		{
			get
			{
				return true;
			}
		}

		public override bool autoload
		{
			get
			{
				return true;
			}
		}

		protected override void Init()
		{
			base.Init();
			this._doc = XDocuments.GetSpecificDocument<XCrossGVGDocument>(XCrossGVGDocument.uuID);
			this.m_Help = (base.uiBehaviour.m_root.FindChild("Help").GetComponent("XUIButton") as IXUIButton);
			this.m_maskSprite = (base.uiBehaviour.m_root.FindChild("Mask").GetComponent("XUISprite") as IXUISprite);
			this.RegisterHandler<CrossGVGHallHandle>(GuildArenaTab.Hall);
			this.RegisterHandler<CrossGVGDuelHandler>(GuildArenaTab.Duel);
			this.RegisterHandler<CrossGVGCombatHandler>(GuildArenaTab.Combat);
		}

		private void RegisterHandler<T>(GuildArenaTab index) where T : DlgHandlerBase, new()
		{
			bool flag = !this.m_handlers.ContainsKey(index);
			if (flag)
			{
				T t = default(T);
				t = DlgHandlerBase.EnsureCreate<T>(ref t, base.uiBehaviour.m_root, false, this);
				this.m_handlers.Add(index, t);
			}
		}

		private void RemoveHandler(GuildArenaTab index)
		{
			DlgHandlerBase dlgHandlerBase;
			bool flag = this.m_handlers.TryGetValue(index, out dlgHandlerBase);
			if (flag)
			{
				DlgHandlerBase.EnsureUnload<DlgHandlerBase>(ref dlgHandlerBase);
				this.m_handlers.Remove(index);
			}
		}

		protected override void OnUnload()
		{
			this.RemoveHandler(GuildArenaTab.Hall);
			this.RemoveHandler(GuildArenaTab.Duel);
			this.RemoveHandler(GuildArenaTab.Combat);
			base.OnUnload();
		}

		public override void RegisterEvent()
		{
			base.RegisterEvent();
			base.uiBehaviour.m_Close.RegisterClickEventHandler(new ButtonClickEventHandler(this._CloseClickHandle));
			this.m_Help.RegisterClickEventHandler(new ButtonClickEventHandler(this._OnHelpClick));
		}

		protected override void OnShow()
		{
			base.OnShow();
			this.InitTabTableControl();
			this._doc.SendCrossGVGData();
		}

		public void SelectTabIndex(GuildArenaTab tab)
		{
			bool flag = !base.IsVisible();
			if (!flag)
			{
				ulong num = (ulong)((long)XFastEnumIntEqualityComparer<GuildArenaTab>.ToInt(tab));
				IXUICheckBox byCheckBoxId = this.m_uiBehaviour.m_tabcontrol.GetByCheckBoxId(num);
				bool flag2 = byCheckBoxId == null;
				if (!flag2)
				{
					byCheckBoxId.bChecked = true;
					this._OnTabControlUpdate(num);
				}
			}
		}

		protected override void OnHide()
		{
			this.SetHandlerVisible(this._doc.SelectTabIndex, false);
			this._doc.SendCrossGVGOper(CrossGvgOperType.CGOT_LeaveUI, 0UL);
			base.OnHide();
		}

		public void RefreshData()
		{
			DlgHandlerBase dlgHandlerBase;
			bool flag = this.m_handlers.TryGetValue(this._doc.SelectTabIndex, out dlgHandlerBase) && dlgHandlerBase.IsVisible();
			if (flag)
			{
				dlgHandlerBase.RefreshData();
			}
		}

		private void InitTabTableControl()
		{
			List<int> list = new List<int>
			{
				1,
				2,
				3
			};
			List<string> list2 = new List<string>();
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				list2.Add(string.Format("CrossGVG_Tab{0}", list[i]));
				i++;
			}
			base.uiBehaviour.m_tabcontrol.SetupTabs(list, list2, new XUITabControl.UITabControlCallback(this._OnTabControlUpdate), true, 1f, -1, true);
		}

		private void SetHandlerVisible(GuildArenaTab handlerID, bool isVisble)
		{
			DlgHandlerBase dlgHandlerBase;
			bool flag = this.m_handlers.TryGetValue(handlerID, out dlgHandlerBase);
			if (flag)
			{
				dlgHandlerBase.SetVisible(isVisble);
				if (isVisble)
				{
					this._doc.SelectTabIndex = handlerID;
				}
			}
		}

		private void _OnTabControlUpdate(ulong handId)
		{
			this.SetHandlerVisible(this._doc.SelectTabIndex, false);
			this.SetHandlerVisible((GuildArenaTab)handId, true);
			this.m_maskSprite.SetAlpha((this._doc.SelectTabIndex == GuildArenaTab.Hall) ? 0f : 1f);
		}

		private bool _OnHelpClick(IXUIButton btn)
		{
			DlgBase<XCommonHelpTipView, XCommonHelpTipBehaviour>.singleton.ShowHelp(XSysDefine.XSys_CrossGVG);
			return true;
		}

		private bool _CloseClickHandle(IXUIButton btn)
		{
			this.SetVisibleWithAnimation(false, null);
			return false;
		}

		private XCrossGVGDocument _doc;

		private Dictionary<GuildArenaTab, DlgHandlerBase> m_handlers = new Dictionary<GuildArenaTab, DlgHandlerBase>();

		private IXUIButton m_Help;

		private IXUISprite m_maskSprite;
	}
}
