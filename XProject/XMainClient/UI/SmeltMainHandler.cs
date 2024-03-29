﻿using System;
using System.Collections.Generic;
using System.Text;
using UILib;
using UnityEngine;
using XMainClient.UI.UICommon;
using XUtliPoolLib;

namespace XMainClient.UI
{

	internal class SmeltMainHandler : DlgHandlerBase
	{

		private XSmeltDocument m_doc
		{
			get
			{
				return XSmeltDocument.Doc;
			}
		}

		public string EffectPath
		{
			get
			{
				bool flag = string.IsNullOrEmpty(this.m_effectPath);
				if (flag)
				{
					this.m_effectPath = XSingleton<XGlobalConfig>.singleton.GetValue("SmeltEffectPath");
				}
				return this.m_effectPath;
			}
		}

		protected override string FileName
		{
			get
			{
				return "ItemNew/SmeltMainHandler";
			}
		}

		protected override void Init()
		{
			base.Init();
			this.m_AttrParentGo = base.PanelObject.transform.FindChild("AttrParentGo").gameObject;
			Transform transform = base.PanelObject.transform.FindChild("AttrTpl");
			this.m_AttrTplPool.SetupPool(this.m_AttrParentGo, transform.gameObject, 3U, false);
			transform = base.PanelObject.transform.FindChild("Top");
			this.m_SmeltItemGo = transform.FindChild("SmeltItem").gameObject;
			this.m_tips1Lab = (transform.FindChild("Tips1").GetComponent("XUILabel") as IXUILabel);
			this.m_tips2Lab = (transform.FindChild("Tips2").GetComponent("XUILabel") as IXUILabel);
			this.m_Help = (base.transform.Find("Help").GetComponent("XUIButton") as IXUIButton);
			this.m_ClosedBtn = (base.PanelObject.transform.FindChild("Close").GetComponent("XUIButton") as IXUIButton);
			this.m_TittleLab = (base.PanelObject.transform.FindChild("TittleLab").GetComponent("XUILabel") as IXUILabel);
			transform = base.PanelObject.transform.FindChild("Bottom");
			this.m_SmeltBtn = (transform.FindChild("SmeltBtn").GetComponent("XUIButton") as IXUIButton);
			this.m_NeedGoldLab = (transform.FindChild("NeedGoldLab").GetComponent("XUILabel") as IXUILabel);
			this.m_btnRedDot = this.m_SmeltBtn.gameObject.transform.FindChild("RedPoint").gameObject;
			this.m_resultGo = base.PanelObject.transform.FindChild("Result").gameObject;
			bool flag = this.m_itemGoList == null;
			if (flag)
			{
				transform = transform.FindChild("Items");
				this.m_itemGoList = new List<GameObject>();
				for (int i = 0; i < transform.childCount; i++)
				{
					this.m_itemGoList.Add(transform.GetChild(i).gameObject);
				}
			}
			this.m_doc.View = this;
			this.m_tips1Lab.SetText(XSingleton<XStringTable>.singleton.GetString("SmeltNewTips1"));
			this.m_tips2Lab.SetText(XSingleton<XStringTable>.singleton.GetString("SmeltNewTips2"));
		}

		public override void RegisterEvent()
		{
			base.RegisterEvent();
			this.m_ClosedBtn.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnCloseClicked));
			this.m_SmeltBtn.RegisterPressEventHandler(new ButtonPressEventHandler(this.OnIconPress));
			this.m_Help.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnHelpClicked));
		}

		public bool OnHelpClicked(IXUIButton button)
		{
			DlgBase<XCommonHelpTipView, XCommonHelpTipBehaviour>.singleton.ShowHelp(XSysDefine.XSys_Item_Smelting);
			return true;
		}

		protected override void OnShow()
		{
			base.OnShow();
			this.RefreshData();
		}

		protected override void OnHide()
		{
			this.m_doc.Clear();
			bool flag = DlgBase<ItemSystemDlg, TabDlgBehaviour>.singleton._EmblemEquipHandler != null;
			if (flag)
			{
				DlgBase<ItemSystemDlg, TabDlgBehaviour>.singleton._EmblemEquipHandler.RegisterItemClickEvents(null);
			}
			DlgBase<ItemSystemDlg, TabDlgBehaviour>.singleton.OnPopHandlerSetVisible(false, null);
			DlgBase<ItemSystemDlg, TabDlgBehaviour>.singleton.StackRefresh();
			bool flag2 = DlgBase<ItemSystemDlg, TabDlgBehaviour>.singleton._equipHandler != null;
			if (flag2)
			{
				DlgBase<ItemSystemDlg, TabDlgBehaviour>.singleton._equipHandler.SelectEquip(0UL);
				DlgBase<ItemSystemDlg, TabDlgBehaviour>.singleton._equipHandler.RegisterItemClickEvents(null);
			}
			bool flag3 = this.m_fx != null;
			if (flag3)
			{
				this.m_fx.Stop();
				this.m_fx.SetActive(false);
			}
			this.m_bStatus = false;
			base.OnHide();
		}

		public override void OnUnload()
		{
			this.m_doc.View = null;
			this.m_doc.MesIsBack = true;
			bool flag = this.m_fx != null;
			if (flag)
			{
				XSingleton<XFxMgr>.singleton.DestroyFx(this.m_fx, true);
				this.m_fx = null;
			}
			base.OnUnload();
		}

		public override void RefreshData()
		{
			base.RefreshData();
			XItem itemByUID = XBagDocument.BagDoc.GetItemByUID(this.m_doc.CurUid);
			bool flag = itemByUID != null;
			if (flag)
			{
				bool flag2 = DlgBase<ItemSystemDlg, TabDlgBehaviour>.singleton._equipHandler != null && DlgBase<ItemSystemDlg, TabDlgBehaviour>.singleton._equipHandler.IsVisible() && itemByUID.Type == ItemType.EQUIP;
				if (flag2)
				{
					DlgBase<ItemSystemDlg, TabDlgBehaviour>.singleton._equipHandler.RegisterItemClickEvents(new SpriteClickEventHandler(this.OnEquipClicked));
				}
				else
				{
					bool flag3 = itemByUID.Type == ItemType.EMBLEM && DlgBase<ItemSystemDlg, TabDlgBehaviour>.singleton._EmblemEquipHandler != null && DlgBase<ItemSystemDlg, TabDlgBehaviour>.singleton._EmblemEquipHandler.IsVisible();
					if (flag3)
					{
						DlgBase<ItemSystemDlg, TabDlgBehaviour>.singleton._EmblemEquipHandler.RegisterItemClickEvents(new SpriteClickEventHandler(this.OnEquipClicked));
					}
				}
			}
			this.ShowUi();
		}

		public override void StackRefresh()
		{
			this.RefreshData();
			base.StackRefresh();
		}

		public void RefreshUi(bool randTips)
		{
			this.m_bIsInit = false;
			this.m_bIsNeedRandTips = randTips;
			this.FillContent();
		}

		public void ShowUi()
		{
			this.m_bIsInit = true;
			this.GetShowIndex();
			this.FillContent();
		}

		public void UpdateUi(bool randTips)
		{
			this.m_bIsInit = true;
			this.m_bIsNeedRandTips = randTips;
			this.FillContent();
		}

		private void FillContent()
		{
			this.FillTop();
			this.FillAttrList();
			this.FillResultPanel();
			this.FillBottom();
		}

		private void FillTop()
		{
			XItem itemByUID = XBagDocument.BagDoc.GetItemByUID(this.m_doc.CurUid);
			bool flag = itemByUID == null;
			if (flag)
			{
				XSingleton<XDebug>.singleton.AddErrorLog("not find uid : ", this.m_doc.CurUid.ToString(), null, null, null, null);
			}
			else
			{
				bool flag2 = itemByUID.Type == ItemType.EQUIP;
				if (flag2)
				{
					XSingleton<XItemDrawerMgr>.singleton.DrawItem(this.m_SmeltItemGo, itemByUID as XEquipItem);
				}
				else
				{
					bool flag3 = itemByUID.Type == ItemType.EMBLEM;
					if (flag3)
					{
						XSingleton<XItemDrawerMgr>.singleton.DrawItem(this.m_SmeltItemGo, itemByUID as XEmblemItem);
					}
					else
					{
						XSingleton<XItemDrawerMgr>.singleton.DrawItem(this.m_SmeltItemGo, itemByUID);
					}
				}
				IXUISprite ixuisprite = this.m_SmeltItemGo.transform.FindChild("Icon").GetComponent("XUISprite") as IXUISprite;
				ixuisprite.ID = this.m_doc.CurUid;
				ixuisprite.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(this.OnSelectedItemClicked));
				bool flag4 = itemByUID.Type == ItemType.EQUIP;
				if (flag4)
				{
					this.m_TittleLab.SetText(XStringDefineProxy.GetString("EquipSmelt"));
				}
				else
				{
					this.m_TittleLab.SetText(XStringDefineProxy.GetString("EmbleSmelt"));
				}
			}
		}

		private void FillAttrList()
		{
			this.m_AttrTplPool.ReturnAll(false);
			bool flag = this.m_fx != null;
			if (flag)
			{
				this.m_fx.Stop();
				this.m_fx.SetActive(false);
			}
			bool flag2 = this.m_doc.SmeltAttrList == null || this.m_doc.SmeltAttrList.Count == 0;
			if (!flag2)
			{
				for (int i = 0; i < this.m_doc.SmeltAttrList.Count; i++)
				{
					GameObject gameObject = this.m_AttrTplPool.FetchGameObject(false);
					gameObject.transform.localPosition = new Vector3(0f, (float)(-57 * i), 0f);
					this.FillAttrItem(gameObject, this.m_doc.GetSmeltAttr(i), i);
				}
			}
		}

		private void FillAttrItem(GameObject go, SmeltAttr attr, int index)
		{
			bool flag = attr == null;
			if (!flag)
			{
				IXUILabel ixuilabel = go.transform.FindChild("Name").GetComponent("XUILabel") as IXUILabel;
				string text = string.Format("[{0}]{1}[-]", attr.ColorStr, XAttributeCommon.GetAttrStr((int)attr.AttrID));
				ixuilabel.SetText(text);
				text = string.Format("[{0}]{1}[-]", attr.ColorStr, attr.RealValue);
				ixuilabel = (go.transform.FindChild("Name/Value").GetComponent("XUILabel") as IXUILabel);
				ixuilabel.SetText(text);
				text = string.Format("[{0}]{1}[{2}-{3}][-]", new object[]
				{
					attr.ColorStr,
					XStringDefineProxy.GetString("SmeltRange"),
					attr.Min,
					attr.Max
				});
				ixuilabel = (go.transform.FindChild("RangeVlue").GetComponent("XUILabel") as IXUILabel);
				ixuilabel.SetText(text);
				go.transform.FindChild("Select").gameObject.SetActive(false);
				go.transform.FindChild("Select1").gameObject.SetActive(false);
				IXUISprite ixuisprite = go.transform.FindChild("Bg").GetComponent("XUISprite") as IXUISprite;
				ixuisprite.ID = (ulong)((long)index);
				ixuisprite.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(this.OnClickCheckBox));
				go.transform.FindChild("RedPoint").gameObject.SetActive(false);
				bool flag2 = index == this.m_doc.SelectIndex;
				if (flag2)
				{
					this.m_curTra = go.transform;
					this.m_curTra.FindChild("Select").gameObject.SetActive(true);
					this.m_curTra.FindChild("Select1").gameObject.SetActive(true);
					bool flag3 = !this.m_bIsInit;
					if (flag3)
					{
						bool flag4 = this.m_fx == null;
						if (flag4)
						{
							this.m_fx = XSingleton<XFxMgr>.singleton.CreateFx(this.EffectPath, null, true);
						}
						else
						{
							this.m_fx.SetActive(true);
						}
						this.m_fx.Play(go.transform.FindChild("effect"), Vector3.zero, Vector3.one, 1f, true, false);
					}
					this.FillResultPanel();
				}
			}
		}

		private void FillResultPanel()
		{
			SmeltAttr smeltAttr = this.m_doc.GetSmeltAttr(this.m_doc.SelectIndex);
			bool flag = smeltAttr == null;
			if (flag)
			{
				this.m_resultGo.SetActive(false);
			}
			else
			{
				this.m_resultGo.SetActive(true);
				IXUILabel ixuilabel = this.m_resultGo.transform.FindChild("Tips").GetComponent("XUILabel") as IXUILabel;
				bool bIsNeedRandTips = this.m_bIsNeedRandTips;
				if (bIsNeedRandTips)
				{
					ixuilabel.SetText(this.GetTips());
				}
				else
				{
					this.m_bIsNeedRandTips = true;
				}
				this.m_btnRedDot.SetActive(false);
				ixuilabel = (this.m_resultGo.transform.FindChild("Name").GetComponent("XUILabel") as IXUILabel);
				string text = string.Format("[{0}]{1}[-]", smeltAttr.ColorStr, XAttributeCommon.GetAttrStr((int)smeltAttr.AttrID));
				ixuilabel.SetText(text);
				bool flag2 = smeltAttr.LastValue == -1;
				if (flag2)
				{
					ixuilabel = (this.m_resultGo.transform.FindChild("NowValue").GetComponent("XUILabel") as IXUILabel);
					ixuilabel.SetText(string.Format("[{0}]{1}[-]", smeltAttr.ColorStr, this.GetAttrValue()));
					ixuilabel = (this.m_resultGo.transform.FindChild("AfterValue").GetComponent("XUILabel") as IXUILabel);
					ixuilabel.SetText(string.Format("[{0}]{1}[-]", smeltAttr.ColorStr, "???"));
					ixuilabel.gameObject.transform.FindChild("Up").gameObject.SetActive(false);
					ixuilabel.gameObject.transform.FindChild("Down").gameObject.SetActive(false);
				}
				else
				{
					ixuilabel = (this.m_resultGo.transform.FindChild("NowValue").GetComponent("XUILabel") as IXUILabel);
					ixuilabel.SetText(string.Format("[{0}]{1}[-]", smeltAttr.ColorStr, smeltAttr.LastValue));
					ixuilabel = (this.m_resultGo.transform.FindChild("AfterValue").GetComponent("XUILabel") as IXUILabel);
					bool isReplace = smeltAttr.IsReplace;
					if (isReplace)
					{
						ixuilabel.SetText(string.Format("[63ff85]{0}[-]", smeltAttr.SmeltResult));
						ixuilabel.gameObject.transform.FindChild("Down").gameObject.SetActive(false);
						ixuilabel = (ixuilabel.gameObject.transform.FindChild("Up").GetComponent("XUILabel") as IXUILabel);
						ixuilabel.SetText(string.Format("[63ff85]{0}[-]", (long)((ulong)smeltAttr.SmeltResult - (ulong)((long)smeltAttr.LastValue))));
						ixuilabel.gameObject.SetActive(true);
					}
					else
					{
						ixuilabel.SetText(string.Format("[ff3e3e]{0}[-]", smeltAttr.SmeltResult));
						ixuilabel.gameObject.transform.FindChild("Up").gameObject.SetActive(false);
						ixuilabel = (ixuilabel.gameObject.transform.FindChild("Down").GetComponent("XUILabel") as IXUILabel);
						ixuilabel.SetText(string.Format("[ff3e3e]{0}[-]", (long)smeltAttr.LastValue - (long)((ulong)smeltAttr.SmeltResult)));
						ixuilabel.gameObject.SetActive(true);
					}
				}
			}
		}

		private string GetTips()
		{
			SmeltAttr smeltAttr = this.m_doc.GetSmeltAttr(this.m_doc.SelectIndex);
			bool flag = smeltAttr == null || smeltAttr.LastValue == -1;
			string @string;
			if (flag)
			{
				@string = XStringDefineProxy.GetString("SmeltBadNotReplace");
			}
			else
			{
				int num = UnityEngine.Random.Range(0, 10);
				bool isReplace = smeltAttr.IsReplace;
				if (isReplace)
				{
					@string = XStringDefineProxy.GetString(XSingleton<XCommon>.singleton.StringCombine("SmeltSucceed", num.ToString()));
				}
				else
				{
					@string = XStringDefineProxy.GetString(XSingleton<XCommon>.singleton.StringCombine("SmeltLost", num.ToString()));
				}
			}
			return @string;
		}

		private uint GetAttrValue()
		{
			SmeltAttr smeltAttr = this.m_doc.GetSmeltAttr(this.m_doc.SelectIndex);
			bool flag = smeltAttr == null;
			uint result;
			if (flag)
			{
				result = 0U;
			}
			else
			{
				result = smeltAttr.RealValue;
			}
			return result;
		}

		public void UpdateNeedItem()
		{
			this.FillBottom();
		}

		private void FillBottom()
		{
			this.m_NeedGoldLab.SetText(this.m_doc.GetNeedGold().ToString());
			SeqListRef<uint> needItem = this.m_doc.GetNeedItem();
			this.m_needItemIsEnough = true;
			for (int i = 0; i < this.m_itemGoList.Count; i++)
			{
				GameObject gameObject = this.m_itemGoList[i];
				bool flag = gameObject == null;
				if (!flag)
				{
					bool flag2 = i >= needItem.Count;
					if (flag2)
					{
						XSingleton<XItemDrawerMgr>.singleton.normalItemDrawer.DrawItem(gameObject, null, 0, false);
					}
					else
					{
						XSingleton<XItemDrawerMgr>.singleton.normalItemDrawer.DrawItem(gameObject, (int)needItem[i, 0], (int)needItem[i, 1], false);
						ulong itemCount = XBagDocument.BagDoc.GetItemCount((int)needItem[i, 0]);
						bool flag3 = itemCount >= (ulong)needItem[i, 1];
						bool flag4 = i == 0;
						if (flag4)
						{
							int num = 0;
							this.m_NeedSmeltStoneLst = this.m_doc.GetShouldShowItems((int)needItem[i, 0], (int)needItem[i, 1], ref num);
							flag3 = ((long)num >= (long)((ulong)needItem[i, 1]));
							this.m_smeltItemId = (int)needItem[i, 0];
						}
						bool flag5 = !flag3;
						if (flag5)
						{
							this.m_needItemIsEnough = false;
						}
						IXUILabel ixuilabel = gameObject.transform.FindChild("Num").GetComponent("XUILabel") as IXUILabel;
						ixuilabel.gameObject.SetActive(true);
						bool flag6 = flag3;
						if (flag6)
						{
							ixuilabel.SetText(string.Format(XStringDefineProxy.GetString("COMMON_COUNT_TOTAL_ENOUGH_FMT"), itemCount, needItem[i, 1]));
						}
						else
						{
							ixuilabel.SetText(string.Format(XStringDefineProxy.GetString("COMMON_COUNT_TOTAL_NOTENOUGH_FMT"), itemCount, needItem[i, 1]));
						}
						IXUISprite ixuisprite = gameObject.transform.FindChild("Icon").GetComponent("XUISprite") as IXUISprite;
						ixuisprite.ID = (ulong)needItem[i, 0];
						bool flag7 = flag3;
						if (flag7)
						{
							ixuisprite.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(XSingleton<UiUtility>.singleton.OnItemClick));
						}
						else
						{
							ixuisprite.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(this.OnGetItemAccess));
						}
					}
				}
			}
		}

		private void GetShowIndex()
		{
			this.m_doc.SelectIndex = 0;
			bool flag = this.m_doc.SmeltAttrList == null || this.m_doc.SmeltAttrList.Count == 0;
			if (!flag)
			{
				for (int i = 0; i < this.m_doc.SmeltAttrList.Count; i++)
				{
					bool flag2 = !this.m_doc.SmeltAttrList[i].IsFull;
					if (flag2)
					{
						this.m_doc.SelectIndex = i;
						break;
					}
				}
			}
		}

		private string GetTipsStr()
		{
			StringBuilder stringBuilder = new StringBuilder();
			ItemList.RowData itemConf;
			for (int i = 0; i < this.m_NeedSmeltStoneLst.Count; i++)
			{
				itemConf = XBagDocument.GetItemConf(this.m_NeedSmeltStoneLst[i].Item1);
				bool flag = itemConf != null;
				if (flag)
				{
					stringBuilder.Append(this.m_NeedSmeltStoneLst[i].Item2).Append(XSingleton<XStringTable>.singleton.GetString("Ge")).Append("[00ff00]").Append(itemConf.ItemName[0]).Append("[-]");
					bool flag2 = i != this.m_NeedSmeltStoneLst.Count;
					if (flag2)
					{
						stringBuilder.Append(",");
					}
				}
			}
			itemConf = XBagDocument.GetItemConf(this.m_smeltItemId);
			bool flag3 = itemConf != null;
			string result;
			if (flag3)
			{
				result = string.Format(XSingleton<XStringTable>.singleton.GetString("SmeltStoneExchangedTips"), itemConf.ItemName[0], stringBuilder);
			}
			else
			{
				result = "";
			}
			return result;
		}

		private bool OnCloseClicked(IXUIButton btn)
		{
			base.SetVisible(false);
			return true;
		}

		private bool Smelt()
		{
			uint needGold = this.m_doc.GetNeedGold();
			bool flag = (ulong)needGold >= XBagDocument.BagDoc.GetItemCount(1);
			bool result;
			if (flag)
			{
				int level = (int)XSingleton<XAttributeMgr>.singleton.XPlayerData.Level;
				XRechargeDocument specificDocument = XDocuments.GetSpecificDocument<XRechargeDocument>(XRechargeDocument.uuID);
				int vipLevel = (int)specificDocument.VipLevel;
				XPurchaseDocument xpurchaseDocument = XSingleton<XGame>.singleton.Doc.GetXComponent(XPurchaseDocument.uuID) as XPurchaseDocument;
				XPurchaseInfo purchaseInfo = xpurchaseDocument.GetPurchaseInfo(level, vipLevel, ItemEnum.GOLD);
				bool flag2 = purchaseInfo.totalBuyNum > purchaseInfo.curBuyNum;
				if (flag2)
				{
					DlgBase<XPurchaseView, XPurchaseBehaviour>.singleton.ReqQuickCommonPurchase(ItemEnum.GOLD);
				}
				else
				{
					XSingleton<UiUtility>.singleton.ShowSystemTip(XStringDefineProxy.GetString("ERR_LACKCOIN"), "fece00");
				}
				this.m_bStatus = false;
				result = true;
			}
			else
			{
				bool flag3 = !this.m_needItemIsEnough;
				if (flag3)
				{
					XSingleton<UiUtility>.singleton.ShowSystemTip(XStringDefineProxy.GetString("ERR_SMELTING_LACKMONEY"), "fece00");
					this.m_bStatus = false;
					result = true;
				}
				else
				{
					SmeltAttr smeltAttr = this.m_doc.GetSmeltAttr(this.m_doc.SelectIndex);
					bool flag4 = smeltAttr == null;
					if (flag4)
					{
						this.m_bStatus = false;
						result = true;
					}
					else
					{
						bool isFull = smeltAttr.IsFull;
						if (isFull)
						{
							XSingleton<UiUtility>.singleton.ShowSystemTip(XStringDefineProxy.GetString("SmeltAttrFull"), "fece00");
							this.m_bStatus = false;
							result = true;
						}
						else
						{
							bool flag5 = !smeltAttr.IsCanSmelt;
							if (flag5)
							{
								XSingleton<UiUtility>.singleton.ShowSystemTip(XStringDefineProxy.GetString("ThisAttrCannotSmelt"), "fece00");
								this.m_bStatus = false;
								result = true;
							}
							else
							{
								XOptionsDocument specificDocument2 = XDocuments.GetSpecificDocument<XOptionsDocument>(XOptionsDocument.uuID);
								bool flag6 = (this.m_NeedSmeltStoneLst.Count == 1 && this.m_NeedSmeltStoneLst[0].Item1 == this.m_smeltItemId) || specificDocument2.GetValue(XOptionsDefine.OD_NO_SMELTSTONE_EXCHANGED_CONFIRM) == 1;
								if (flag6)
								{
									this.m_doc.ReqSmelt();
								}
								else
								{
									XSingleton<UiUtility>.singleton.ShowModalDialog(this.GetTipsStr(), XStringDefineProxy.GetString(XStringDefine.COMMON_OK), XStringDefineProxy.GetString(XStringDefine.COMMON_CANCEL), new ButtonClickEventHandler(this.DoOK), new ButtonClickEventHandler(this.DoCancel), false, XTempTipDefine.OD_SMELTSTONE_EXCHANGED, 50);
									this.m_bStatus = false;
								}
								result = true;
							}
						}
					}
				}
			}
			return result;
		}

		private bool DoCancel(IXUIButton btn)
		{
			XOptionsDocument specificDocument = XDocuments.GetSpecificDocument<XOptionsDocument>(XOptionsDocument.uuID);
			specificDocument.SetValue(XOptionsDefine.OD_NO_SMELTSTONE_EXCHANGED_CONFIRM, DlgBase<ModalDlg, ModalDlgBehaviour>.singleton.GetTempTip(XTempTipDefine.OD_SMELTSTONE_EXCHANGED) ? 1 : 0, false);
			XSingleton<UiUtility>.singleton.CloseModalDlg();
			return true;
		}

		private bool DoOK(IXUIButton btn)
		{
			this.m_doc.ReqSmelt();
			bool flag = DlgBase<ModalDlg, ModalDlgBehaviour>.singleton.IsVisible();
			if (flag)
			{
				XOptionsDocument specificDocument = XDocuments.GetSpecificDocument<XOptionsDocument>(XOptionsDocument.uuID);
				specificDocument.SetValue(XOptionsDefine.OD_NO_SMELTSTONE_EXCHANGED_CONFIRM, DlgBase<ModalDlg, ModalDlgBehaviour>.singleton.GetTempTip(XTempTipDefine.OD_SMELTSTONE_EXCHANGED) ? 1 : 0, false);
			}
			XSingleton<UiUtility>.singleton.CloseModalDlg();
			return true;
		}

		private void OnSelectedItemClicked(IXUISprite iSp)
		{
			this.m_bStatus = false;
			ulong id = iSp.ID;
			XSingleton<UiUtility>.singleton.ShowTooltipDialog(XSingleton<XGame>.singleton.Doc.XBagDoc.GetItemByUID(id), null, iSp, false, 0U);
		}

		private void OnGetItemAccess(IXUISprite iSp)
		{
			this.m_bStatus = false;
			int itemid = (int)iSp.ID;
			XSingleton<UiUtility>.singleton.ShowItemAccess(itemid, null);
		}

		public void OnEquipClicked(IXUISprite iSp)
		{
			bool flag = !this.m_doc.MesIsBack;
			if (!flag)
			{
				this.m_bStatus = false;
				this.m_doc.SelectEquip(iSp.ID);
			}
		}

		private void OnClickCheckBox(IXUISprite iSp)
		{
			bool flag = !this.m_doc.MesIsBack;
			if (!flag)
			{
				this.m_bStatus = false;
				this.m_doc.SelectIndex = (int)iSp.ID;
				bool flag2 = this.m_curTra != null;
				if (flag2)
				{
					this.m_curTra.FindChild("Select").gameObject.SetActive(false);
					this.m_curTra.FindChild("Select1").gameObject.SetActive(false);
				}
				this.m_curTra = iSp.gameObject.transform.parent;
				this.m_curTra.FindChild("Select").gameObject.SetActive(true);
				this.m_curTra.FindChild("Select1").gameObject.SetActive(true);
				this.FillResultPanel();
			}
		}

		private void OnIconPress(IXUIButton btn, bool state)
		{
			XSingleton<XDebug>.singleton.AddGreenLog("icon press", null, null, null, null, null);
			this.m_bStatus = state;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			bool flag = this.m_bStatus && this.m_doc.MesIsBack;
			if (flag)
			{
				bool flag2 = Time.realtimeSinceStartup - this.m_lastClickTime >= this.m_cdTime;
				if (flag2)
				{
					this.m_lastClickTime = Time.realtimeSinceStartup;
					this.Smelt();
				}
			}
		}

		private XUIPool m_AttrTplPool = new XUIPool(XSingleton<XGameUI>.singleton.m_uiTool);

		private IXUIButton m_ClosedBtn;

		private IXUIButton m_SmeltBtn;

		private IXUIButton m_Help;

		private IXUILabel m_TittleLab;

		private IXUILabel m_NeedGoldLab;

		private IXUILabel m_tips1Lab;

		private IXUILabel m_tips2Lab;

		public GameObject m_btnRedDot;

		public GameObject m_resultGo;

		private GameObject m_SmeltItemGo;

		private GameObject m_AttrParentGo;

		private List<GameObject> m_itemGoList;

		private Transform m_curTra = null;

		private bool m_needItemIsEnough = true;

		private bool m_bIsInit = true;

		private bool m_bIsNeedRandTips = true;

		private List<XTuple<int, int>> m_NeedSmeltStoneLst = new List<XTuple<int, int>>();

		private int m_smeltItemId = 0;

		private XFx m_fx;

		private string m_effectPath = string.Empty;

		private bool m_bStatus = false;

		private float m_cdTime = 0.2f;

		private float m_lastClickTime = 0f;
	}
}
