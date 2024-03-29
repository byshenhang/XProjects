﻿using System;
using UnityEngine;

namespace XUtliPoolLib
{

	public interface IXAIGeneralMgr : IXInterface
	{

		bool IsHPValue(ulong id, int min, int max);

		bool IsHPValue(Transform transform, int min, int max);

		bool IsMPValue(ulong id, int min, int max);

		bool IsMPValue(Transform transform, int min, int max);

		bool IsValid(ulong id);

		bool IsValid(Transform transform);

		bool HasQTE(ulong id, int QTEState);

		bool HasQTE(GameObject go, int QTEState);

		bool IsAtState(GameObject go, int state);

		bool IsAtState(ulong id, int state);

		bool IsRotate(GameObject go);

		bool IsCastSkill(GameObject go);

		bool IsFighting(ulong id);

		bool IsFighting(Transform transform);

		bool IsWoozy(GameObject go);

		bool IsOppoCastingSkill(ulong id);

		bool IsOppoCastingSkill(Transform transform);

		bool IsHurtOppo(ulong id);

		bool IsHurtOppo(Transform transform);

		bool IsFixedInCd(ulong id);

		bool IsFixedInCd(Transform transform);

		bool IsWander(ulong id);

		bool IsWander(Transform transform);

		bool IsSkillChoosed(ulong id);

		bool IsSkillChoosed(Transform transform);

		bool DetectEnimyInSight(GameObject go);

		bool CastQTESkill(GameObject go);

		bool CastDashSkill(GameObject go);

		bool ResetTargets(GameObject go);

		bool FindTargetByDistance(GameObject go, float distance, bool filterImmortal, float angle, float delta, int targettype);

		bool FindTargetByHitLevel(GameObject go, bool filterImmortal);

		bool FindTargetByHartedList(GameObject go, bool filterImmortal);

		bool FindTargetByNonImmortal(GameObject go);

		bool DoSelectNearest(GameObject go);

		bool DoSelectFarthest(GameObject go);

		bool DoSelectRandomTarget(GameObject go);

		void ClearTarget(ulong id);

		void ClearTarget(Transform transform);

		bool TryCastInstallSkill(GameObject go, GameObject targetgo);

		bool TryCastLearnedSkill(GameObject go, GameObject targetgo);

		bool TryCastPhysicalSkill(GameObject go, GameObject targetgo);

		bool NavToTarget(ulong id, GameObject target);

		bool NavToTarget(GameObject go, GameObject target);

		bool FindNavPath(GameObject go);

		bool ActionMove(GameObject go, Vector3 dir, Vector3 dest, float speed);

		bool ActionNav(ulong id, Vector3 dest);

		bool ActionNav(GameObject go, Vector3 dest);

		bool ActionRotate(GameObject go, float degree, float speed, int type);

		bool RotateToTarget(GameObject go);

		bool SelectSkill(GameObject go, FilterSkillArg skillarg);

		bool DoSelectInOrder(GameObject go);

		bool DoSelectRandom(GameObject go);

		bool DoCastSkill(GameObject go, GameObject targetgo);

		bool SendAIEvent(GameObject go, int msgto, int msgtype, int entitytypeid, string msgarg, float delaytime, Vector3 pos);

		string ReceiveAIEvent(GameObject go, int msgType, bool Deprecate);

		bool IsTargetImmortal(ulong id);

		Transform SelectMoveTargetById(GameObject go, int objectid);

		Transform SelectBuffTarget(GameObject go);

		Transform SelectItemTarget(GameObject go);

		bool SelectTargetBySkillCircle(GameObject go);

		bool ResetHartedList(GameObject go);

		bool CallMonster(GameObject go, CallMonsterData data);

		bool CallScript(GameObject go, string script, float delaytime);

		bool AddBuff(int monsterid, int buffid, int buffid2);

		void RunSubTree(GameObject go, string treename);

		bool PlayFx(GameObject go, string fxname, Vector3 fxpos, float delaytime);

		bool StopCastingSkill(GameObject go);

		bool DetectEnemyInRange(GameObject go, ref DetectEnemyInRangeArg arg);

		bool UpdateNavigation(GameObject go, int dir, int oldDir);

		int GetPlayerProf();

		bool IsPointInMap(Vector3 pos);

		bool AIDoodad(GameObject go, int doodadid, int waveid, Vector3 pos, float randompos, float delaytime);
	}
}
