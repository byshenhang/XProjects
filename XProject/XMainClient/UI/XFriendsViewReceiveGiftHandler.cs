﻿using System;
using System.Collections.Generic;
using KKSG;
using UILib;
using UnityEngine;
using XMainClient.UI.UICommon;
using XUtliPoolLib;

namespace XMainClient.UI
{

	internal class XFriendsViewReceiveGiftHandler : DlgHandlerBase
	{

		private void _ResetReceiveGiftRank(List<XFriendData> friendData)
		{
			this.friendGiftSortData.Clear();
			for (int i = 0; i < friendData.Count; i++)
			{
				this.friendGiftSortData.Add(friendData[i]);
			}
			bool flag = this.friendGiftSortData.Count > 0;
			if (flag)
			{
				this.friendGiftSortData[0].receiveNo = 1;
				bool flag2 = this.friendGiftSortData.Count > 1;
				if (flag2)
				{
					this.friendGiftSortData.Sort(new Comparison<XFriendData>(this.CompareFriendGiftData));
					this.friendGiftSortData[0].receiveNo = 1;
					for (int j = 1; j < this.friendGiftSortData.Count; j++)
					{
						bool flag3 = this.friendGiftSortData[j].receiveAll == this.friendGiftSortData[j - 1].receiveAll;
						if (flag3)
						{
							this.friendGiftSortData[j].receiveNo = this.friendGiftSortData[j - 1].receiveNo;
						}
						else
						{
							this.friendGiftSortData[j].receiveNo = j + 1;
						}
					}
				}
			}
		}

		private void _SortItemByReceiveTime(List<XFriendData> list)
		{
			uint num = (uint)XFastEnumIntEqualityComparer<FriendGiftReceive>.ToInt(FriendGiftReceive.FriendGift_ReceiveTaken);
			uint num2 = (uint)XFastEnumIntEqualityComparer<FriendGiftReceive>.ToInt(FriendGiftReceive.FriendGift_Received);
			List<XFriendData> list2 = new List<XFriendData>();
			List<XFriendData> list3 = new List<XFriendData>();
			for (int i = 0; i < list.Count; i++)
			{
				bool flag = list[i].receiveGiftState == num;
				if (flag)
				{
					list2.Add(list[i]);
				}
				else
				{
					bool flag2 = list[i].receiveGiftState == num2;
					if (flag2)
					{
						list3.Add(list[i]);
					}
				}
			}
			list2.Sort(new Comparison<XFriendData>(this.CompareFriendGiftDataByTime));
			list3.Sort(new Comparison<XFriendData>(this.CompareFriendGiftDataByTime));
			list.Clear();
			list.AddRange(list3);
			list.AddRange(list2);
		}

		private int CompareFriendGiftData(XFriendData a, XFriendData b)
		{
			return b.receiveAll.CompareTo(a.receiveAll);
		}

		private int CompareFriendGiftDataByTime(XFriendData a, XFriendData b)
		{
			return (int)(b.receivetime - a.receivetime);
		}

		public void RefreshList(List<XFriendData> list)
		{
			bool flag = !base.IsVisible();
			if (!flag)
			{
				bool flag2 = list == null;
				if (!flag2)
				{
					this._ResetReceiveGiftRank(list);
					this.mList.Clear();
					uint num = (uint)XFastEnumIntEqualityComparer<FriendGiftReceive>.ToInt(FriendGiftReceive.FriendGift_ReceiveNone);
					for (int i = 0; i < this.friendGiftSortData.Count; i++)
					{
						bool flag3 = this.friendGiftSortData[i].receiveGiftState != num;
						if (flag3)
						{
							this.mList.Add(this.friendGiftSortData[i]);
						}
					}
					this._SortItemByReceiveTime(this.mList);
					this.RefreshList();
				}
			}
		}

		public void RefreshList()
		{
			this._SortItemByReceiveTime(this.mList);
			this.mListWrapContent.SetContentCount(this.mList.Count, false);
			this.mListScrollView.ResetPosition();
			int num = 0;
			uint num2 = (uint)XFastEnumIntEqualityComparer<FriendGiftReceive>.ToInt(FriendGiftReceive.FriendGift_ReceiveTaken);
			for (int i = 0; i < this.mList.Count; i++)
			{
				bool flag = this.mList[i].receiveGiftState == num2;
				if (flag)
				{
					num++;
				}
			}
			this.lbNum.SetText(string.Format(XSingleton<XFriendsStaticData>.singleton.CommonCountTotalFmt, DlgBase<XFriendsView, XFriendsBehaviour>.singleton.TodayReceiveCount, XSingleton<XFriendsStaticData>.singleton.ReceiveGifMaxTimes));
		}

		protected override void Init()
		{
			base.Init();
			this.mTweenTool = (base.PanelObject.GetComponent("XUIPlayTween") as IXUITweenTool);
			Transform transform = base.PanelObject.transform.Find("Bg");
			this.lbNum = (transform.Find("Num").GetComponent("XUILabel") as IXUILabel);
			this.mListScrollView = (transform.Find("List").GetComponent("XUIScrollView") as IXUIScrollView);
			this.mListWrapContent = (this.mListScrollView.gameObject.transform.Find("WrapContent").GetComponent("XUIWrapContent") as IXUIWrapContent);
			this.mListTempView = new XPlayerInfoChildBaseView();
		}

		public override void RegisterEvent()
		{
			base.RegisterEvent();
			IXUISprite ixuisprite = base.PanelObject.transform.Find("BgBlack").GetComponent("XUISprite") as IXUISprite;
			ixuisprite.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(this.OnClose));
			this.mListWrapContent.RegisterItemUpdateEventHandler(new WrapItemUpdateEventHandler(this._RankWrapListUpdated));
		}

		protected override void OnShow()
		{
			base.OnShow();
		}

		protected override void OnHide()
		{
		}

		private void OnHideTweenFinished(IXUITweenTool tween)
		{
		}

		private void OnClose(IXUISprite sprClose)
		{
			base.SetVisible(false);
		}

		private void _RankWrapListUpdated(Transform t, int i)
		{
			XFriendData xfriendData = this.mList[i];
			XPlayerInfoChildBaseView xplayerInfoChildBaseView = this.mListTempView;
			xplayerInfoChildBaseView.FindFrom(t);
			IXUITexture tencentImage = t.Find("tencent").GetComponent("XUITexture") as IXUITexture;
			DlgBase<XFriendsView, XFriendsBehaviour>.singleton.SetTencentImage(tencentImage);
			t.Find("Received").gameObject.SetActive((ulong)xfriendData.receiveGiftState == (ulong)((long)XFastEnumIntEqualityComparer<FriendGiftReceive>.ToInt(FriendGiftReceive.FriendGift_ReceiveTaken)));
			Transform transform = t.Find("Receive");
			bool flag = (ulong)xfriendData.receiveGiftState == (ulong)((long)XFastEnumIntEqualityComparer<FriendGiftReceive>.ToInt(FriendGiftReceive.FriendGift_Received));
			if (flag)
			{
				transform.gameObject.SetActive(true);
				IXUIButton ixuibutton = transform.GetComponent("XUIButton") as IXUIButton;
				ixuibutton.ID = xfriendData.roleid;
				ixuibutton.RegisterClickEventHandler(new ButtonClickEventHandler(DlgBase<XFriendsView, XFriendsBehaviour>.singleton.OnClickReceiveGiftFromFriend));
			}
			else
			{
				transform.gameObject.SetActive(false);
			}
			t.Find("Returned").gameObject.SetActive((ulong)xfriendData.sendGiftState == (ulong)((long)XFastEnumIntEqualityComparer<FriendGiftSend>.ToInt(FriendGiftSend.FriendGift_Sended)));
			transform = t.Find("Return");
			bool flag2 = (ulong)xfriendData.sendGiftState == (ulong)((long)XFastEnumIntEqualityComparer<FriendGiftSend>.ToInt(FriendGiftSend.FriendGift_SendNone));
			if (flag2)
			{
				transform.gameObject.SetActive(true);
				IXUIButton ixuibutton2 = transform.GetComponent("XUIButton") as IXUIButton;
				ixuibutton2.ID = xfriendData.roleid;
				ixuibutton2.RegisterClickEventHandler(new ButtonClickEventHandler(DlgBase<XFriendsView, XFriendsBehaviour>.singleton.OnClickSendGiftToFriend));
			}
			else
			{
				transform.gameObject.SetActive(false);
			}
			IXUILabel ixuilabel = t.Find("Num").GetComponent("XUILabel") as IXUILabel;
			ixuilabel.SetText(xfriendData.receiveAll.ToString());
			IXUILabel ixuilabel2 = t.Find("No").GetComponent("XUILabel") as IXUILabel;
			ixuilabel2.SetText(xfriendData.receiveNo.ToString());
			IXUILabel ixuilabel3 = t.Find("Level").GetComponent("XUILabel") as IXUILabel;
			bool flag3 = xfriendData.degreeAll < XSingleton<XFriendsStaticData>.singleton.MaxFriendlyEvaluation;
			if (flag3)
			{
				ixuilabel3.SetText(xfriendData.degreeAll.ToString());
			}
			else
			{
				ixuilabel3.SetText("MAX");
			}
			IXUISprite ixuisprite = ixuilabel3.gameObject.transform.Find("Mark").GetComponent("XUISprite") as IXUISprite;
			float num = xfriendData.degreeAll;
			num /= XSingleton<XFriendsStaticData>.singleton.MaxFriendlyEvaluation;
			ixuisprite.SetFillAmount(1f - num);
			ixuisprite.ID = (ulong)xfriendData.degreeAll;
			ixuisprite.RegisterSpriteClickEventHandler(new SpriteClickEventHandler(DlgBase<XFriendsView, XFriendsBehaviour>.singleton.OnClickDegreeHeart));
			xplayerInfoChildBaseView.sprHead.SetSprite(XSingleton<XProfessionSkillMgr>.singleton.GetProfHeadIcon2((int)xfriendData.profession));
			xplayerInfoChildBaseView.lbName.InputText = XSingleton<XCommon>.singleton.StringCombine(XTitleDocument.GetTitleWithFormat(xfriendData.titleID, xfriendData.name), XStringDefineProxy.GetString("FRIEND_RECEIVE_GIFT_INFO"), XRechargeDocument.GetVIPIconString(xfriendData.viplevel));
		}

		private IXUITweenTool mTweenTool;

		private IXUIWrapContent mListWrapContent;

		private IXUIScrollView mListScrollView;

		private IXUILabel lbNum;

		private XPlayerInfoChildBaseView mListTempView;

		private List<XFriendData> mList = new List<XFriendData>();

		public List<XFriendData> friendGiftSortData = new List<XFriendData>();
	}
}
