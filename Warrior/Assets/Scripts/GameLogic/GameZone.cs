using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof(BoxCollider))]
public class GameZone : BaseGameZone
{

	public enum E_State
	{
		WaitStart,
		InProcess,
		Complished,
		Finished,
	}

	[SerializeField]
	public SpawnZone[] SpawnZones = null;

	//可破坏的物品
	public BreakableObject[] BreakableObjects = null;

	//交互对象
	public InteractionGameObject[] InteractionObjects = null;
        
	//下一关
	public LoadNextLevel LoadNextLevel;

	E_State State = E_State.WaitStart;

	void Start ()
	{
		if (LoadNextLevel != null) {
			LoadNextLevel.gameObject.SetActive (false);
		}
	}

	public void Enable ()
	{
		gameObject.SetActive (true);

		State = E_State.WaitStart;
		for (int i = 0; i < SpawnZones.Length; i++) {
			SpawnZones [i].Enable ();
		}

	}

	public void Disable ()
	{

	}

	public override void SetInProgress ()
	{
		base.SetInProgress ();
		State = E_State.InProcess;
	}

	/// <summary>
	/// 关卡重置
	/// </summary>
	public override void Reset ()
	{
		base.Reset ();
		StopAllCoroutines ();

		if (LoadNextLevel != null)
			LoadNextLevel.gameObject.SetActive (false);

		if (SpawnZones != null)
			for (int i = 0; i < SpawnZones.Length; i++) {
				//todo: add Reset
				SpawnZones [i].enabled = true;
			}

		if (InteractionObjects != null)
			for (int i = 0; i < InteractionObjects.Length; i++) {
				InteractionObjects [i].Restart ();
			}

		if (BreakableObjects != null)
			for (int i = 0; i < BreakableObjects.Length; i++) {
				BreakableObjects [i].Restart ();///
			}
	}

	void FixedUpdate ()
	{
		for (int i = 0; i < Enemies.Count; i++) {
			if (Enemies [i].IsAlive == false) {
				Enemies.RemoveAt (i);
				break;
			}
		}

		if (State == E_State.InProcess) {
			bool finished = true;
			for (int i = 0; i < SpawnZones.Length; i++) {
				if (SpawnZones [i].State == SpawnZone.E_State.Finshed)
					continue;
				finished = false;
			}

			if (finished == true) {
				State = E_State.Complished;
				if (LoadNextLevel != null) {
					LoadNextLevel.gameObject.SetActive (true);
				} else {
					//解锁下一个GameZone，MissionZone里面解锁GameZone
					MissionZone.Instance.UnLockNextGameZone ();

					//重置玩家生命状态
					Player.Instance.HealToFullHealth();
				}
			}
		}
	}

	public void EnableAllActiveInteraction(bool enable)
	{
		for (int i = InteractionObjects.Length - 1; i >= 0; i--)
		{
			InteractionGameObject o = InteractionObjects[i];
			if (o.IsActive == false)
				continue;

			o.Enable(enable);
		}
	}

	/// <summary>
	/// 攻击可破碎对象
	/// </summary>
	/// <param name="attacker"></param>
	public override void BreakBreakableObjects (Agent attacker)
	{
		if (attacker.IsPlayer == false)
			return;
		BreakableObject breakObj = null;
		Vector3 dir;

		for (int i = 0; i < BreakableObjects.Length; i++) {
			breakObj = BreakableObjects [i];
			if (breakObj.IsActive == false || breakObj.enabled == false)
				continue;
			dir = breakObj.Position - attacker.Position;

			//todo: 玩家攻击范围外就不能破碎物品
			if (dir.sqrMagnitude > 10)
				continue;

			breakObj.Break ();
		}
	}

	/// <summary>
	/// 获取最近操作杆
	/// </summary>
	/// <param name="center"></param>
	/// <param name="maxLen"></param>
	/// <returns></returns>
	public override InteractionGameObject GetNearestInteractionObject (Vector3 center, float maxLen)
	{
		float len;
		float nearestLen = maxLen * maxLen;
		InteractionGameObject best = null;
		for (int i = 0; i < InteractionObjects.Length; i++) {
			InteractionGameObject igo = InteractionObjects [i];
			if (igo.IsActive == false || igo.IsEnabled == false || (igo is InteractionLever) == false)
				continue;

			len = (igo.Position - center).sqrMagnitude;
			if (len < nearestLen) {
				nearestLen = len;
				best = igo;
			}
		}
		return best;
	}



	void OnTriggerEnter (Collider other)
	{
		if (State != E_State.WaitStart || other.CompareTag ("Player") == false)
			return;
		//触发关卡初始化，保存关卡数据，设置场景隐藏，锁住前一关卡

		MissionZone.Instance.CurrentGameZone = this;

		MissionZone.Instance.LockPrevGameZone ();
	}

	void OnTriggerExit ()
	{
		//没有出怪的情况下触发
		if (SpawnZones == null || SpawnZones.Length == 0) {
			MissionZone.Instance.UnLockNextGameZone ();
			//
			//重置玩家生命状态
		}
	}


	public void EnemiesRecvImpuls(Agent attacker, float angle, float range, float impuls)
	{
		Vector3 dirToEnemy;
		Vector3 center = attacker.Position + attacker.Forward * attacker.CharacterController.radius;
		Vector3 attackerDir = attacker.Forward;
		Agent enemy;
		bool hit = false;

		for (int i = 0; i < Enemies.Count; i++)
		{
			enemy = Enemies[i];

			if (enemy == attacker || enemy.IsAlive == false || enemy.enabled == false || enemy.BlackBoard.MotionType == E_MotionType.Knockdown)
				continue;

			dirToEnemy = enemy.Position - center;
			float len = dirToEnemy.sqrMagnitude;
			if (len > range * range)
				continue; //too far

			dirToEnemy.Normalize();

			if (len > 0.5f * 0.5f && Vector3.Angle(attackerDir, dirToEnemy) > angle)
				continue;

			hit = true;

			enemy.ReceiveImpuls(attacker, dirToEnemy * impuls);
		}

		if (hit)
			attacker.Sound.PlayAttackHit();
	}

	public void DoMeleeDamage(Agent attacker, Agent mainTarget, E_WeaponType byWeapon, AnimAttackData data, bool critical, bool knockdown)
	{
		if (attacker == Player.Instance.Agent)
		{
			StartCoroutine(EnemiesRecvDamage(attacker, mainTarget, byWeapon, data, critical, knockdown));
		}
		else
		{
			bool hit = false;
			Vector3 dirToEnemy;
			Vector3 attackerDir = attacker.Forward;

			if (mainTarget == null)
				mainTarget = Player.Instance.Agent;

			if (mainTarget.IsInvulnerable == false && mainTarget.BlackBoard.MotionType != E_MotionType.Roll)
			{
				dirToEnemy = mainTarget.Position - attacker.Position;

				float len = dirToEnemy.sqrMagnitude;

				if (len < attacker.BlackBoard.sqrWeaponRange)
				{
					dirToEnemy.Normalize();

					if (len < 0.5f * 0.5f || data.HitAngle == -1 || Vector3.Angle(attackerDir, dirToEnemy) < data.HitAngle)
					{
						if(Game.Instance.GameDifficulty == E_GameDifficulty.Hard)
							mainTarget.ReceiveDamage(attacker, byWeapon, data.HitDamage * 1.2f, data);
						else 
							mainTarget.ReceiveDamage(attacker, byWeapon, data.HitDamage, data);
						hit = true;
					}
				}
			}

			if (hit)
				attacker.Sound.PlayAttackHit();
			else
				attacker.Sound.PlayAttackMiss();
		}
	}

	public void DoDamageToPlayer(Agent attacker, AnimAttackData data, float distance)
	{
		Agent mainTarget = Player.Instance.Agent;

		if (mainTarget.BlackBoard.MotionType != E_MotionType.Roll)
		{
			if ((mainTarget.Position - attacker.Position).sqrMagnitude < distance * distance)
			{
				mainTarget.ReceiveDamage(attacker, E_WeaponType.Body, data.HitDamage, data);
				attacker.Sound.PlayAttackHit();
			}
		}
	}

	public void DoDamageFatality(Agent attacker, Agent mainTarget, E_WeaponType byWeapon, AnimAttackData data)
	{
		if (mainTarget.IsAlive == false || mainTarget.enabled == false)
			return;

		mainTarget.ReceiveDamage(attacker, byWeapon, 1000, data);
	}


	IEnumerator EnemiesRecvDamage(Agent attacker, Agent mainTarget, E_WeaponType byWeapon, AnimAttackData data, bool critical, bool knockdown)
	{
		bool hit = false;
		bool block = false;
		bool knock = false;

		Vector3 dirToEnemy;
		Vector3 center = attacker.Position;
		Vector3 attackerDir = attacker.Forward;
		Agent enemy;

		yield return new WaitForEndOfFrame();

		for (int i = 0; i < Enemies.Count; i++)
		{
			enemy = Enemies[i];

			if (enemy.IsAlive == false || enemy.enabled == false || enemy.IsKnockedDown == true)
				continue;

			dirToEnemy = enemy.Position - center;
			float len = dirToEnemy.magnitude;
			dirToEnemy.Normalize();

			if (enemy.IsInvulnerable || (enemy.BlackBoard.DamageOnlyFromBack && Vector3.Angle(attackerDir, enemy.Forward) > 80))
			{
				// Debug.Log(enemy.name + " high angle " + Vector3.Angle(attackerDir, enemy.Forward));
				enemy.ReceiveHitCompletelyBlocked(attacker);
				block = true;
				continue;
			}

			if (len > attacker.BlackBoard.WeaponRange)
			{
				if (data.HitAreaKnockdown == true && knockdown && len < attacker.BlackBoard.WeaponRange * 1.2f)
				{
					knock = true;
					enemy.ReceiveKnockDown(attacker, dirToEnemy * data.HitMomentum);
				}
				else if (data.UseImpuls && len < attacker.BlackBoard.WeaponRange * 1.4f)
				{
					enemy.ReceiveImpuls(attacker, dirToEnemy * data.HitMomentum);
				}
				continue; //too far
			}

			if (enemy.IsInvulnerable || (enemy.BlackBoard.DamageOnlyFromBack && Vector3.Angle(attackerDir, enemy.Forward) > 80))
			{
				// Debug.Log(enemy.name + " high angle " + Vector3.Angle(attackerDir, enemy.Forward));
				enemy.ReceiveHitCompletelyBlocked(attacker);
				block = true;
				continue;
			}

			if (len > 0.5f && Vector3.Angle(attackerDir, dirToEnemy) > data.HitAngle)
			{

				if (data.UseImpuls)
				{
					//Debug.Log(enemy.name + " impuls");
					enemy.ReceiveImpuls(attacker, dirToEnemy * data.HitMomentum);
				}
				continue;
			}

			if (enemy.BlackBoard.CriticalAllowed && data.HitCriticalType != E_CriticalHitType.None && Vector3.Angle(attackerDir, enemy.Forward) < 45) // from behind
			{
				// Debug.Log(enemy.name + " critical from behind");
				enemy.ReceiveCriticalHit(attacker, data.HitCriticalType);
				hit = true;
			}
			else if (enemy.IsBlocking)
			{
				//Debug.Log(enemy.name + " block ");
				enemy.ReceiveBlockedHit(attacker, byWeapon, data.HitDamage, data);
				block = true;
			}
			else if (enemy.BlackBoard.CriticalAllowed && critical && (mainTarget == enemy || (data.HitCriticalType == E_CriticalHitType.Horizontal && Random.Range(0, 100) < 30)))
			{
				//   Debug.Log(enemy.name + " critical by chance");
				enemy.ReceiveCriticalHit(attacker, data.HitCriticalType);
				hit = true;
			}
			else if (data.HitAreaKnockdown == true && knockdown)
			{
				//Debug.Log(Time.timeSinceLevelLoad + " " + enemy.name + " knockdown");
				enemy.ReceiveKnockDown(attacker, dirToEnemy * (1 - (len / attacker.BlackBoard.WeaponRange) + data.HitMomentum));
				knock = true;
			}
			else
			{
				//  Debug.Log(enemy.name + " damage");
				enemy.ReceiveDamage(attacker, byWeapon, data.HitDamage, data);
				hit = true;
			}



			yield return new WaitForEndOfFrame();
		}

		if(knock)
			attacker.Sound.PlayKnockdown();
		else if (block)
			attacker.Sound.PlayAttackBlock();
		else if (hit)
			attacker.Sound.PlayAttackHit();
		else
			attacker.Sound.PlayAttackMiss();

	}

	void OnDrawGizmos ()
	{
		BoxCollider box = GetComponent<BoxCollider> ();
		if (box != null) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube (box.transform.position + box.center, box.size);
		}
	}
}
