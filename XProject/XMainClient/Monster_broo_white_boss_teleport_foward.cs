﻿using System;
using XUtliPoolLib;

namespace XMainClient
{

	internal class Monster_broo_white_boss_teleport_foward
	{

		public static bool Disappear(XSkill skill)
		{
			bool casting = skill.Casting;
			if (casting)
			{
				bool flag = !Monster_broo_white_boss_teleport_foward._veiled;
				if (flag)
				{
					Monster_broo_white_boss_teleport_foward._veiled = true;
					Monster_broo_white_boss_teleport_foward._token = XSingleton<XTimerMgr>.singleton.SetTimer(Monster_broo_white_boss_teleport_foward._startVeil, new XTimerMgr.ElapsedEventHandler(Monster_broo_white_boss_teleport_foward.Veil), skill.Firer);
				}
			}
			else
			{
				XSingleton<XTimerMgr>.singleton.KillTimer(Monster_broo_white_boss_teleport_foward._token);
				Monster_broo_white_boss_teleport_foward.Unveil(skill.Firer);
				Monster_broo_white_boss_teleport_foward._veiled = false;
			}
			return true;
		}

		private static void Veil(object o)
		{
			XFadeOutEventArgs @event = XEventPool<XFadeOutEventArgs>.GetEvent();
			@event.Out = Monster_broo_white_boss_teleport_foward._fadeOutTime;
			@event.Firer = (o as XEntity);
			XSingleton<XEventMgr>.singleton.FireEvent(@event);
			Monster_broo_white_boss_teleport_foward._token = XSingleton<XTimerMgr>.singleton.SetTimer(Monster_broo_white_boss_teleport_foward._veilDuration, new XTimerMgr.ElapsedEventHandler(Monster_broo_white_boss_teleport_foward.Unveil), o);
		}

		private static void Unveil(object o)
		{
			bool flag = Monster_broo_white_boss_teleport_foward._token > 0U;
			if (flag)
			{
				Monster_broo_white_boss_teleport_foward._token = 0U;
				XFadeInEventArgs @event = XEventPool<XFadeInEventArgs>.GetEvent();
				@event.In = Monster_broo_white_boss_teleport_foward._fadeInTime;
				@event.Firer = (o as XEntity);
				XSingleton<XEventMgr>.singleton.FireEvent(@event);
			}
		}

		private static uint _token = 0U;

		private static bool _veiled = false;

		private static float _startVeil = 0.267f;

		private static float _fadeOutTime = 0.1f;

		private static float _veilDuration = 0.3f;

		private static float _fadeInTime = 0.2f;
	}
}
