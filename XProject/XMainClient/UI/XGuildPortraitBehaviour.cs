﻿using System;
using UILib;
using UnityEngine;
using XMainClient.UI.UICommon;
using XUtliPoolLib;

namespace XMainClient.UI
{

	internal class XGuildPortraitBehaviour : DlgBehaviourBase
	{

		private void Awake()
		{
			this.m_Close = (base.transform.FindChild("Bg/Close").GetComponent("XUIButton") as IXUIButton);
			this.m_BtnOK = (base.transform.FindChild("Bg/OK").GetComponent("XUIButton") as IXUIButton);
			Transform transform = base.transform.FindChild("Bg/PortraitList/PortraitTpl");
			this.m_PortraitPool.SetupPool(transform.parent.gameObject, transform.gameObject, (uint)XGuildPortraitView.PORTRAIT_COUNT, false);
			Vector3 tplPos = this.m_PortraitPool.TplPos;
			for (int i = 0; i < XGuildPortraitView.PORTRAIT_COUNT; i++)
			{
				GameObject gameObject = this.m_PortraitPool.FetchGameObject(false);
				gameObject.transform.localPosition = new Vector3(tplPos.x + (float)(i % XGuildPortraitView.COL_COUNT * this.m_PortraitPool.TplWidth), tplPos.y - (float)(i / XGuildPortraitView.COL_COUNT * this.m_PortraitPool.TplHeight));
				this.m_PortraitList[i] = gameObject.transform.FindChild("Portrait").gameObject;
				this.m_SelectorList[i] = gameObject.transform.FindChild("Selector").gameObject;
			}
		}

		public IXUIButton m_Close = null;

		public IXUIButton m_BtnOK;

		public GameObject[] m_PortraitList = new GameObject[XGuildPortraitView.PORTRAIT_COUNT];

		public GameObject[] m_SelectorList = new GameObject[XGuildPortraitView.PORTRAIT_COUNT];

		public XUIPool m_PortraitPool = new XUIPool(XSingleton<XGameUI>.singleton.m_uiTool);
	}
}
