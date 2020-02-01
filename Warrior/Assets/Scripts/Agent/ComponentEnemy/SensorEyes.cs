using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SensorEyes : MonoBehaviour
{

	Agent Owner;
	Agent Target;
	Transform Transform;

	public float EyeRange = 6;
	public float FieldOfView = 120;

	float sqrEyeRange { get { return EyeRange * EyeRange; } }

	void Awake ()
	{
		Transform = transform;
		Owner = GetComponent ("Agent") as Agent;
        

	}
	// Use this for initialization
	void Start ()
	{
		if (Owner.IsPlayer == false) {
			Owner.BlackBoard.DesiredTarget = null;
			Target = Player.Instance.Agent;
		}

	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (Owner.IsVisible == false)
			return;

		if (Owner.IsPlayer == true)
			UpdatePlayer();
		else
			UpdateEnemy();

	}

	/// <summary>
	/// Updates the player.
	/// 如果是玩家
	/// </summary>
	void UpdatePlayer()
	{
		if (MissionZone.Instance.CurrentGameZone == null)
		{
			Owner.WorldState.SetWSProperty(E_PropKey.E_ALERTED, false);
			return;
		}

		List<Agent> enemies  = MissionZone.Instance.CurrentGameZone.Enemies;

		if (Owner.WorldState.GetWSProperty(E_PropKey.E_ALERTED).GetBool() == true)
		{
			if (enemies.Count == 0)
				Owner.WorldState.SetWSProperty(E_PropKey.E_ALERTED, false);
		}
		else
		{
			for (int i = 0; i < enemies.Count; i++)
			{
				if ((Owner.Position - enemies[i].Position).sqrMagnitude < sqrEyeRange)
				{
					Owner.WorldState.SetWSProperty(E_PropKey.E_ALERTED, true);
					return;
				}
			}

			Owner.WorldState.SetWSProperty(E_PropKey.E_ALERTED, false);
		}
	}

	/// <summary>
	/// Updates the enemy.
	/// 如果是怪
	/// </summary>
	void UpdateEnemy()
	{


		if (Target.IsAlive == false) {
			Owner.WorldState.SetWSProperty (E_PropKey.E_ALERTED, false);
			Owner.WorldState.SetWSProperty (E_PropKey.E_ATTACK_TARGET, false);
			return;
		}

		//reset some shits
		Owner.WorldState.SetWSProperty (E_PropKey.E_LOOKING_AT_TARGET, false);
		Owner.WorldState.SetWSProperty (E_PropKey.E_AHEAD_OF_ENEMY, false);
		Owner.WorldState.SetWSProperty (E_PropKey.E_BEHIND_ENEMY, false);


		Vector3 dirToEnemy = Target.Position - Owner.Position;

		Owner.BlackBoard.DistanceToTarget = dirToEnemy.magnitude;

		Owner.WorldState.SetWSProperty (E_PropKey.E_IN_WEAPONS_RANGE, Owner.BlackBoard.DistanceToTarget < Owner.BlackBoard.WeaponRange);
		Owner.WorldState.SetWSProperty (E_PropKey.E_IN_COMBAT_RANGE, Owner.BlackBoard.DistanceToTarget < Owner.BlackBoard.CombatRange);

		Owner.WorldState.SetWSProperty (E_PropKey.E_BEHIND_ENEMY, Vector3.Angle (dirToEnemy, Target.Forward) > 180);

		if (Owner.WorldState.GetWSProperty (E_PropKey.E_ALERTED).GetBool () == false) {
			if (Owner.BlackBoard.DistanceToTarget < EyeRange && Vector3.Angle (Owner.Forward, dirToEnemy) < FieldOfView) {
				Owner.WorldState.SetWSProperty (E_PropKey.E_ALERTED, true);
				Owner.WorldState.SetWSProperty (E_PropKey.E_ATTACK_TARGET, true);
				Owner.BlackBoard.DesiredTarget = Target;
			}
		} else {
			if (Vector3.Angle (Owner.Forward, dirToEnemy) < 10)
				Owner.WorldState.SetWSProperty (E_PropKey.E_LOOKING_AT_TARGET, true);
		}

		float angleToEnemyForward = Vector3.Angle (dirToEnemy, Target.Forward);

		if (angleToEnemyForward > 135 && angleToEnemyForward < 225)
			Owner.WorldState.SetWSProperty (E_PropKey.E_AHEAD_OF_ENEMY, true);
		else if (angleToEnemyForward > 315 || angleToEnemyForward < 45)
			Owner.WorldState.SetWSProperty (E_PropKey.E_BEHIND_ENEMY, true);
		
	}
}
