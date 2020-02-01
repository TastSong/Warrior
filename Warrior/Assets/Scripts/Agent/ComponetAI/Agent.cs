//
// /**************************************************************************
//
// Agent.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-17
//
// Description:Provide  functions  to connect Oracle
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour , IEDable
{
	public int Experience = 10;

	[System.NonSerialized]
	public Transform Transform;
	[System.NonSerialized]
	public GameObject GameObject;
	[System.NonSerialized]
	public CharacterController CharacterController;
	[System.NonSerialized]
	public ComponentSounds Sound;

	[System.NonSerialized]
	public Memory Memory;

	[System.NonSerialized]
	public AnimSet AnimSet;
	public BlackBoard BlackBoard = new BlackBoard ();

	[System.NonSerialized]
	public WorldState WorldState;

	private GOAPManager GoapManager;
	private Hashtable m_Actions = new Hashtable ();

	public GOAPAction GetAction (E_GOAPAction type)
	{
		return (GOAPAction)m_Actions [type];
	}

	public int GetNumberOfActions ()
	{
		return m_Actions.Count;
	}

	public GOAPGoal CurrentGOAPGoal { get { return GoapManager.CurrentGoal; } }

	public Vector3 Position { get { return Transform.position; } }

	public Vector3 Right { get { return Transform.right; } }

	public Vector3 Forward{ get { return Transform.forward; } }

	public Vector3 ChestPosition { get { return Transform.position + transform.up * 1.5f; } }

	//是否是玩家
	public bool IsPlayer { get { return BlackBoard.IsPlayer; } }
	//是否活着:对象是否隐藏及生命值是否大于0
	public bool IsAlive{ get { return GameObject.activeSelf && true; } }
	//是否可见:渲染组件是否显示
	public bool IsVisible{ get { return Renderer.isVisible; } }
	//是否正在攻击
	public bool IsAttacking { get { return false; } }
	//是否击倒
	public bool IsKnockedDown;
	//是否格挡
	public bool IsBlocking;
	//是否霸体，是否可以被伤害
	public bool IsInvulnerable;

	//怪物类型
	public E_EnemyType EnemyType = E_EnemyType.None;

	//渲染组件
	private SkinnedMeshRenderer Renderer;
	//普通材质
	[SerializeField]
	private Material DiffuseMaterial;
	//带透明通道的材质
	[SerializeField]
	private Material TransparentMaterial;

	private Vector3 CollisionCenter;


	void Awake ()
	{
		Transform = transform;
		GameObject = gameObject;

		AnimSet = GetComponent<AnimSet> ();

		Memory = new Memory ();
		WorldState = new WorldState ();
		GoapManager = new GOAPManager (this);

		CharacterController = Transform.GetComponent<CharacterController> ();
		CollisionCenter = CharacterController.center;

		Sound = Transform.GetComponent<ComponentSounds> ();

		BlackBoard.Owner = this;
		BlackBoard.myGameObject = GameObject;

		ResetAgent();

		SetWorldState ();
		WorldState.SetWSProperty(E_PropKey.E_LOOKING_AT_TARGET, false);
		WorldState.SetWSProperty(E_PropKey.E_USE_WORLD_OBJECT, false);

		EnableCollisions ();
		//t = GameObject.Find ("GameObject").transform;
	}

	// Use this for initialization
	void Start ()
	{
		Renderer = GameObject.GetComponentInChildren<SkinnedMeshRenderer> ();
	}

	public void PrepareForStart()
	{
		BlackBoard.Reset();
	}

	/// <summary>
	/// Sets the state of the world.
	/// 设置角色世界状态
	/// </summary>
	void SetWorldState ()
	{
		WorldState.SetWSProperty(E_PropKey.E_ORDER, AgentOrder.E_OrderType.E_NONE);
		WorldState.SetWSProperty(E_PropKey.E_IDLING, true);
		WorldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
		WorldState.SetWSProperty(E_PropKey.E_ATTACK_TARGET, false);
		WorldState.SetWSProperty(E_PropKey.E_ALERTED, false);
		WorldState.SetWSProperty(E_PropKey.MoveToRight, false);
		WorldState.SetWSProperty(E_PropKey.MoveToLeft, false);
		WorldState.SetWSProperty(E_PropKey.E_TELEPORT, false);
		WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);
		WorldState.SetWSProperty(E_PropKey.E_PLAY_ANIM, false);
		WorldState.SetWSProperty(E_PropKey.E_IN_DODGE, false);
		WorldState.SetWSProperty(E_PropKey.E_IN_WEAPONS_RANGE, false);
		WorldState.SetWSProperty(E_PropKey.E_IN_BLOCK, false);
		WorldState.SetWSProperty(E_PropKey.E_IN_COMBAT_RANGE, false);
		WorldState.SetWSProperty(E_PropKey.E_AHEAD_OF_ENEMY, false);
		WorldState.SetWSProperty(E_PropKey.E_BEHIND_ENEMY, false);
		WorldState.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, false);

	}

	/// <summary>
	/// Sets the position.
	/// </summary>
	/// <param name="t">T.</param>
	public void SetPosition (Transform t)
	{
		RaycastHit hit;
		if (Physics.Raycast (t.position + Vector3.up, -Vector3.up, out hit, 5, 1 << 10) == false)
			Transform.position = t.position;
		else
			Transform.position = hit.point;

		Transform.rotation = t.rotation;
	}


	public void Enable ()
	{
		Reset ();

		SetWorldState ();
		
		StartCoroutine (FadeIn ());

		Sound.PlaySound (E_SoundType.Spawn);
	}


	public void Disable ()
	{
		StopAllCoroutines ();

		GoapManager.Reset ();

		BlackBoard.Reset ();

		Memory.Reset ();
	}

	/// <summary>
	/// Reset this instance.
	/// </summary>
	public void Reset ()
	{
		//处理透明材质
		//开始加载时角色闪烁问题，一开始就把透明度设置为0
		if (TransparentMaterial != null) {
			if (Renderer == null)
				Renderer = GameObject.GetComponentInChildren<SkinnedMeshRenderer> ();

			Renderer.material = TransparentMaterial;

			Color color = new Color (1, 1, 1, 0);
			TransparentMaterial.SetColor ("_Color", color);
		}
		
		ResetAgent ();

		EnableCollisions ();
	}

	//玩家角色更新Agent，非玩家角色设置GOAP目标FindCriticalGoal
	void LateUpdate ()
	{
		if (IsPlayer) {
			UpdateAgent ();
		} else {
			GoapManager.FindCriticalGoal ();
		}
		//UpdateAgent();
	}
	
	//非玩家角色更新Agent
	void FixedUpdate ()
	{
		if (IsPlayer)
			return;
		UpdateAgent ();
		WorldState.SetWSProperty (E_PropKey.E_IDLING, GoapManager.CurrentGoal == null);
	}

	void UpdateAgent ()
	{
		if (BlackBoard.DontUpdate == true)
			return;
		
		//update blackboard
		BlackBoard.Update ();

		GoapManager.UpdateCurrentGoal ();
		GoapManager.ManageGoals ();
		Memory.Update ();
		
	}

	void ResetAgent ()
	{
		WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);

		StopAllCoroutines();
		GoapManager.Reset ();
		BlackBoard.Reset ();
		WorldState.Reset ();
		Memory.Reset ();
	}

	public float GetCriticalChance ()
	{
		return 18;
	}


	#region Receive Damage

	/// <summary>
	/// Receives the damage.
	/// 普通受击
	/// </summary>
	/// <param name="attacker">Attacker.</param>
	/// <param name="byWeapon">By weapon.</param>
	/// <param name="damage">Damage.</param>
	/// <param name="data">Data.</param>
	public void ReceiveDamage (Agent attacker, E_WeaponType byWeapon, float damage, AnimAttackData data)
	{
		//不是存活
		if (IsAlive == false)
			return;
		//根据游戏难度，是否是玩家，死亡次数等处理伤害值
		//攻击者是玩家,简单难度就增加攻击力20%
		if (attacker.IsPlayer == true) {
			Game.Instance.Hits += 1;
			if (Game.Instance.GameDifficulty == E_GameDifficulty.Easy) {
				damage *= 1.2f;
			}
		}

		//受击者是玩家,简单难度伤害减少20%
		if (IsPlayer == true) {
			if (Game.Instance.GameDifficulty == E_GameDifficulty.Easy) {
				damage *= 0.8f;
			}
		}

		//如果有一击必杀，damage无限大

		//黑板上添加攻击者，攻击武器，攻击方向距离等
		BlackBoard.Attacker = attacker;
		BlackBoard.AttackerWeapon = byWeapon;


		//是否击杀
		if (IsKnockedDown) {
			BlackBoard.Health = 0;
			BlackBoard.DamageType = E_DamageType.InKnockdown;
			WorldState.SetWSProperty (E_PropKey.E_EVENT, E_EventTypes.Died);
			//添加击倒特效，镜头等
			//屏幕上飘血点
			CombatEffectsManager.Instance.PlayCriticalEffect (Transform.position, -attacker.Forward);

			StartCoroutine (FadeOut (3));
			//一击必杀声音
			Sound.PlaySound (E_SoundType.Fatality);
			//如果是玩家
			if (attacker.IsPlayer) {
				Game.Instance.Score += Experience;
				//玩家添加经验，金钱等
			}
		} else {
			BlackBoard.Health = Mathf.Max (0, BlackBoard.Health - damage);
			BlackBoard.DamageType = E_DamageType.Front;

			if (IsAlive) {
				WorldState.SetWSProperty (E_PropKey.E_EVENT, E_EventTypes.Hit);
				//地上出现一滩血
				SpriteEffectsManager.Instance.CreateBlood (Transform);
			} else {
				WorldState.SetWSProperty (E_PropKey.E_EVENT, E_EventTypes.Died);
				StartCoroutine (FadeOut (3));
				//如果是玩家
				if (attacker.IsPlayer) {
					Game.Instance.Score += Experience;
					//玩家添加经验，金钱等
				}
			}
		}

		//身上不同伤害飘不同大小的血
		if (damage >= 15) {
			CombatEffectsManager.Instance.PlayBloodBigEffect (Transform.position, -attacker.Forward);
		} else {
			CombatEffectsManager.Instance.PlayBloodEffect (Transform.position, -attacker.Forward);
		}


	}

	/// <summary>
	/// Receives the range damage.
	/// 范围伤害
	/// </summary>
	/// <param name="attack">Attack.</param>
	/// <param name="damage">Damage.</param>
	/// <param name="impuls">Impuls.</param>
	public void ReceiveRangeDamage (Agent attack, float damage, Vector3 impuls)
	{
		BlackBoard.DamageType = E_DamageType.Front;
		BlackBoard.Attacker = attack;
		BlackBoard.AttackerWeapon = E_WeaponType.Bow;
		BlackBoard.Impuls = impuls;

		if (Game.Instance.GameDifficulty == E_GameDifficulty.Easy)
			damage *= 0.8f;

		BlackBoard.Health = Mathf.Max (0, BlackBoard.Health - damage);

		Fact f = FactsFactory.Create (Fact.E_FactType.E_EVENT);
		f.Belief = 1;
		f.EventType = E_EventTypes.Hit;
		Memory.AddFact (f);

		if (IsAlive) {
			WorldState.SetWSProperty (E_PropKey.E_EVENT, E_EventTypes.Hit);
			SpriteEffectsManager.Instance.CreateBlood (Transform);
		} else {
			WorldState.SetWSProperty (E_PropKey.E_EVENT, E_EventTypes.Died);
			StartCoroutine (FadeOut (15));
		}

		CombatEffectsManager.Instance.PlayBloodEffect (Transform.position, impuls);
	
	}

	/// <summary>
	/// Receives the enviroment damage.
	/// 环境伤害
	/// </summary>
	/// <param name="damage">Damage.</param>
	/// <param name="impuls">Impuls.</param>
	void ReceiveEnviromentDamage (float damage, Vector3 impuls)
	{
		if (Game.Instance.GameDifficulty == E_GameDifficulty.Easy)
			damage *= 0.5f;

		BlackBoard.DamageType = E_DamageType.Enviroment;
		BlackBoard.Attacker = null;
		BlackBoard.AttackerWeapon = E_WeaponType.None;
		BlackBoard.Impuls = impuls;

		BlackBoard.Health = Mathf.Max (0, BlackBoard.Health - damage);

		Fact f = FactsFactory.Create (Fact.E_FactType.E_EVENT);
		f.Belief = 1;
		f.EventType = E_EventTypes.Hit;
		Memory.AddFact (f);

		//玩家受到环境攻击播放相应镜头特效
		if (IsPlayer)
			;
		
		if (IsAlive) {
			WorldState.SetWSProperty (E_PropKey.E_EVENT, E_EventTypes.Hit);
			SpriteEffectsManager.Instance.CreateBlood (Transform);
		} else {
			WorldState.SetWSProperty (E_PropKey.E_EVENT, E_EventTypes.Died);
			StartCoroutine (FadeOut (15));
		}

		CombatEffectsManager.Instance.PlayBloodEffect (Transform.position, impuls);

	}

	/// <summary>
	/// Receives the blocked hit.
	/// 格挡
	/// </summary>
	/// <param name="attacker">Attacker.</param>
	/// <param name="byWeapon">By weapon.</param>
	/// <param name="damage">Damage.</param>
	/// <param name="data">Data.</param>
	public void ReceiveBlockedHit (Agent attacker, E_WeaponType byWeapon, float damage, AnimAttackData data)
	{
		BlackBoard.Attacker = attacker;
		BlackBoard.AttackerWeapon = byWeapon;

		WorldState.SetWSProperty (E_PropKey.E_EVENT, E_EventTypes.HitBlocked);

		bool fromBehind = Vector3.Dot (attacker.Forward, Forward) > -0.1f;

		if (fromBehind) {
			BlackBoard.Health = Mathf.Max (1, BlackBoard.Health - damage);
			BlackBoard.DamageType = E_DamageType.BreakBlock;
			CombatEffectsManager.Instance.PlayBloodEffect (Transform.position, -attacker.Forward);
			SpriteEffectsManager.Instance.CreateBlood (Transform);
		} else {
			if (data.BreakBlock) {
				if (attacker.IsPlayer)
					Game.Instance.NumberOfBreakBlocks++;
				BlackBoard.DamageType = E_DamageType.BreakBlock;
				CombatEffectsManager.Instance.PlayBlockBreakEffect (Transform.position, -attacker.Forward);
			} else {
				if (attacker.IsPlayer)
					Game.Instance.NumberOfBlockedHits++;
				BlackBoard.DamageType = E_DamageType.Front;
				CombatEffectsManager.Instance.PlayBlockHitEffect (ChestPosition, -attacker.Forward);
			}

		}
	}


	public void ReceiveKnockDown (Agent attacker, Vector3 impuls)
	{
		if (IsAlive == false || BlackBoard.KnockDown == false)
			return;

		if (attacker.IsPlayer) {
			Game.Instance.Hits += 1;
			Game.Instance.NumberOfKnockdowns++;
		}

		BlackBoard.Attacker = attacker;
		BlackBoard.Impuls = impuls;

		WorldState.SetWSProperty (E_PropKey.E_EVENT, E_EventTypes.Knockdown);

		CombatEffectsManager.Instance.PlayKnockdownEffect (Transform.position, -attacker.Forward);
	}

	/// <summary>
	/// Receives the critical hit.
	/// 重击，一击必杀，出现碎尸效果
	/// </summary>
	/// <param name="attacker">Attacker.</param>
	/// <param name="type">Type.</param>
	/// <param name="effectOnly">If set to <c>true</c> effect only.</param>
	public void ReceiveCriticalHit (Agent attacker, E_CriticalHitType type, bool effectOnly = false)
	{
		//     Legs = 0,   Beheaded,    HalfBody,    SliceFrontBack,    SliceLeftRight,

		if (attacker.IsPlayer) {
			Game.Instance.Hits += 1;

			Game.Instance.Score += Experience;
			Player.Instance.AddExperience (Experience, 1.5f + Game.Instance.Hits * 0.1f);
			Game.Instance.NumberOfCriticals++;
		}

		BlackBoard.Stop = true;
		BlackBoard.Health = 0;

		if (type == E_CriticalHitType.Horizontal) {
			int r = Random.Range (0, 100);
			if (r < 33)
				MissionZone.Instance.GetDead (this, E_DeadBodyType.Legs);
			else if (r < 66)
				MissionZone.Instance.GetDead (this, E_DeadBodyType.Beheaded);
			else
				MissionZone.Instance.GetDead (this, E_DeadBodyType.HalfBody);
		} else {
			float dot = Vector3.Dot (Forward, attacker.Forward);

			if (dot < 0.5 && dot > -0.5f)
				MissionZone.Instance.GetDead (this, E_DeadBodyType.SliceLeftRight);
			else
				MissionZone.Instance.GetDead (this, E_DeadBodyType.SliceFrontBack);
		}

		CombatEffectsManager.Instance.PlayCriticalEffect (Transform.position, -attacker.Forward);

		MissionZone.Instance.ReturnHuman (GameObject);
	}

	public void ReceiveImpuls(Agent attacker, Vector3 impuls)
	{
		BlackBoard.Attacker = attacker;
		BlackBoard.AttackerWeapon =  E_WeaponType.None;
		BlackBoard.Impuls = impuls;

		BlackBoard.DamageType = E_DamageType.Front;

		Fact f = FactsFactory.Create(Fact.E_FactType.E_EVENT);
		f.Belief = 1;
		f.EventType = E_EventTypes.Hit;
		Memory.AddFact(f);

		WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.Hit);

	}

	public void ReceiveHitCompletelyBlocked(Agent attacker)
	{
		CombatEffectsManager.Instance.PlayBlockHitEffect(ChestPosition, -attacker.Forward);

		BlackBoard.Berserk += BlackBoard.BerserkBlockModificator;
		BlackBoard.Rage += BlackBoard.RageBlockModificator;


		if (attacker.IsPlayer)
			Game.Instance.NumberOfBlockedHits++;
	}

	#endregion

	#region Goap ......

	public GOAPGoal AddGoal (E_GOAPGoals newGoal)
	{
		return GoapManager.AddGoal (newGoal);
	}

	public void InitializeGOAP ()
	{
		GoapManager.Initialize ();
	}

	public void AddAction (E_GOAPAction action)
	{
		m_Actions.Add (action, GOAPActionFactory.Create (action, this));
	}

	#endregion

	#region Enable & Disable Collisions

	/// <summary>
	/// Enables the collisions.
	/// 启用碰撞
	/// </summary>
	public void EnableCollisions ()
	{
		CharacterController.detectCollisions = true;
		CharacterController.center = CollisionCenter;
	}

	/// <summary>
	/// Disables the collisions.
	/// 禁用碰撞，死亡或者霸体时使用
	/// </summary>
	public void DisableCollisions ()
	{
		CharacterController.detectCollisions = false;
		CharacterController.center = Vector3.up * -20;
	}

	#endregion

	#region Fade In & Out

	/// <summary>
	/// Fades the in.
	/// </summary>
	/// <returns>The in.</returns>
	protected IEnumerator FadeIn ()
	{
		if (TransparentMaterial == null)
			yield break;

		yield return new WaitForEndOfFrame ();

		Renderer.material = TransparentMaterial;

		Color color = new Color (1, 1, 1, 0);
		TransparentMaterial.SetColor ("_Color", color);

		while (color.a < 1) {
			color.a += Time.deltaTime * 1; //控制出现的速度
			if (color.a > 1)
				color.a = 1;

			TransparentMaterial.SetColor ("_Color", color);
			yield return new WaitForEndOfFrame ();
		}

		color.a = 1;
		TransparentMaterial.SetColor ("_Color", color);

		Renderer.material = DiffuseMaterial;
	}


	protected IEnumerator FadeOut (float delay)
	{
		if (TransparentMaterial == null)
			yield break;

		yield return new WaitForSeconds (Mathf.Abs (delay));

		//释放角色脚下影子

		//如果有消失特效，播放特效

		Renderer.material = TransparentMaterial;

		Color color = new Color (1, 1, 1, 1);
		TransparentMaterial.SetColor ("_Color", color);

		while (color.a > 0) {
			color.a -= Time.deltaTime * 1; //控制消失的速度
			if (color.a < 0)
				color.a = 0;

			TransparentMaterial.SetColor ("_Color", color);
			yield return new WaitForEndOfFrame ();
		}

		color.a = 0;
		TransparentMaterial.SetColor ("_Color", color);

		//放回缓存
		MissionZone.Instance.ReturnHuman (GameObject);
	}

	#endregion
}

