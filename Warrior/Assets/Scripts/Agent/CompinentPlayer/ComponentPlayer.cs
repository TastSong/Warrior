//
// /**************************************************************************
//
// ComponentPlayer.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-21
//
// Description:Provide  functions  to connect Oracle
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class Player
{
	public static ComponentPlayer Instance;
}


[RequireComponent (typeof(Agent))]
[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(Animation))]
[RequireComponent (typeof(AnimSetPlayer))]
[RequireComponent (typeof(AnimComponent))]
[RequireComponent (typeof(CameraOffset))]
[RequireComponent (typeof(ComponentSounds))]
[RequireComponent (typeof(SensorEyes))]


public class ComponentPlayer : MonoBehaviour , IActionHandle
{
	public Agent Agent;
	public Transform Transform;

	private List<E_AttackType> ComboProgress = new List<E_AttackType>();
	private Queue<AgentOrder> BufferedOrders = new Queue<AgentOrder>();
	private Agent LastAttacketTarget;
	private AgentActionAttack CurrentAttackAction;
	private AnimSetPlayer AnimSet;
	private Vector3 MoveDirection;
	private float Force;
	private float StepTime;
	private int Experience;
	//private PlayerControls Controls = new PlayerControls();

	private bool UseMode = false;
	public bool InUseMode { get { return UseMode; } }

	void Awake()
	{
		Player.Instance = this;
		Transform = transform;
		Agent = GetComponent<Agent>();
		AnimSet = GetComponent<AnimSetPlayer>();
	}


	// Use this for initialization
	void Start ()
	{

		Agent.BlackBoard.IsPlayer = true;
		Agent.BlackBoard.Rage = 0;
		Agent.BlackBoard.Dodge = 0;
		Agent.BlackBoard.Fear = 0;

		SpriteEffectsManager.Instance.CreateShadow(Transform.Find("root").gameObject, 1.3f, 1.3f);

		Agent.AddAction(E_GOAPAction.gotoPos);
		Agent.AddAction(E_GOAPAction.move);
		Agent.AddAction(E_GOAPAction.gotoMeleeRange);
		Agent.AddAction(E_GOAPAction.weaponShow);
		Agent.AddAction(E_GOAPAction.weaponHide);
		Agent.AddAction(E_GOAPAction.orderAttack);
		Agent.AddAction(E_GOAPAction.orderDodge);
		Agent.AddAction(E_GOAPAction.rollToTarget);
		Agent.AddAction(E_GOAPAction.useLever);
		Agent.AddAction(E_GOAPAction.playAnim);
		Agent.AddAction(E_GOAPAction.teleport);
		Agent.AddAction(E_GOAPAction.injury);
		Agent.AddAction(E_GOAPAction.death);

		Agent.AddGoal(E_GOAPGoals.E_GOTO);
		Agent.AddGoal(E_GOAPGoals.E_ORDER_ATTACK);
		Agent.AddGoal(E_GOAPGoals.E_ORDER_DODGE);
		Agent.AddGoal(E_GOAPGoals.E_ORDER_USE);
		Agent.AddGoal(E_GOAPGoals.E_ALERT);
		Agent.AddGoal(E_GOAPGoals.E_CALM);
		Agent.AddGoal(E_GOAPGoals.E_USE_WORLD_OBJECT);
		Agent.AddGoal(E_GOAPGoals.E_PLAY_ANIM);
		Agent.AddGoal(E_GOAPGoals.E_TELEPORT);
		Agent.AddGoal(E_GOAPGoals.E_REACT_TO_DAMAGE);

		Agent.InitializeGOAP();

		Agent.BlackBoard.AddActionHandle(this);
	}

	public void Enable(Transform t)
	{
		LastAttacketTarget = null;
		//LastTapTime = 0;
		StepTime = 0;

		Agent.BlackBoard.Reset();

		Agent.WorldState.SetWSProperty(E_PropKey.E_ORDER, AgentOrder.E_OrderType.E_NONE);

		Agent.WorldState.SetWSProperty(E_PropKey.E_IDLING, true);
		Agent.WorldState.SetWSProperty(E_PropKey.E_AT_TARGET_POS, true);
		Agent.WorldState.SetWSProperty(E_PropKey.E_ATTACK_TARGET, false);
		Agent.WorldState.SetWSProperty(E_PropKey.E_LOOKING_AT_TARGET, false);
		Agent.WorldState.SetWSProperty(E_PropKey.E_USE_WORLD_OBJECT, false);
		Agent.WorldState.SetWSProperty(E_PropKey.E_PLAY_ANIM, false);

		Agent.WorldState.SetWSProperty(E_PropKey.E_IN_DODGE, false);
		Agent.WorldState.SetWSProperty(E_PropKey.E_IN_WEAPONS_RANGE, false);
		Agent.WorldState.SetWSProperty(E_PropKey.E_WEAPON_IN_HANDS, false);
		Agent.WorldState.SetWSProperty(E_PropKey.E_IN_BLOCK, false);
		Agent.WorldState.SetWSProperty(E_PropKey.E_ALERTED, false);
		Agent.WorldState.SetWSProperty(E_PropKey.E_IN_COMBAT_RANGE, false);
		Agent.WorldState.SetWSProperty(E_PropKey.E_AHEAD_OF_ENEMY, false);
		Agent.WorldState.SetWSProperty(E_PropKey.E_BEHIND_ENEMY, false);
		Agent.WorldState.SetWSProperty(E_PropKey.MoveToRight, false);
		Agent.WorldState.SetWSProperty(E_PropKey.MoveToLeft, false);

		Agent.WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.None);

		ComboProgress.Clear();
		ClearBufferedOrder();

		//Controls.SwitchToCombatMode();

		//GuiManager.Instance.ShowComboProgress(ComboProgress);
	}

	public void Disable()
	{
		ClearBufferedOrder();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Agent.BlackBoard.Stop)
		{
			LastAttacketTarget = null;
			ComboProgress.Clear();
			ClearBufferedOrder();
			CreateOrderStop();

			//Controls.Update();
			return;
		}

		if (BufferedOrders.Count > 0)
		{
			if(CouldAddnewOrder())
				Agent.BlackBoard.AddOrder(BufferedOrders.Dequeue());
		}

		//Controls.Update();

		//input_begin
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		MoveDirection = new Vector3 (h, 0, v);
		Force = Mathf.Clamp(MoveDirection.magnitude, 0, 1);
		MoveDirection.Normalize ();

		if (MoveDirection != Vector3.zero) {
			CreateOrderGoTo (MoveDirection, Force);
		} else {
			CreateOrderStop ();
		}

		if (Input.GetKeyDown(KeyCode.P)) {
			CreateOrderAttack (E_AttackType.X);
		}
		if (Input.GetKeyDown(KeyCode.O)) {
			CreateOrderAttack (E_AttackType.O);
		}
		if (Input.GetKeyDown(KeyCode.I)) {
			CreateOrderDodge ();
		}
		if (Input.GetKeyDown(KeyCode.U)) {
			CreateOrderUse ();
		}
	}


	void LateUpdate()
	{
		if (StepTime < Time.timeSinceLevelLoad)
		{
			if (Agent.BlackBoard.MotionType == E_MotionType.Run)
			{
				Agent.Sound.PlayStep();
				StepTime = Time.timeSinceLevelLoad + Random.Range(0.25f, 0.28f);
			}
			else if (Agent.BlackBoard.MotionType == E_MotionType.Walk)
			{
				Agent.Sound.PlayStep();
				StepTime = Time.timeSinceLevelLoad + Random.Range(0.40f, 0.43f);
			}
		}

		if (CurrentAttackAction != null && CurrentAttackAction.IsActive() == false)
		{// no continue in combos !!!
			if (BufferedOrders.Count == 0 && Agent.WorldState.GetWSProperty(E_PropKey.E_ORDER).GetOrder() != AgentOrder.E_OrderType.E_ATTACK)
			{
				ComboProgress.Clear();
				//GuiManager.Instance.ShowComboProgress(ComboProgress);
			}
			CurrentAttackAction = null;
		}
	}

	void FixedUpdate()
	{
		bool old = UseMode;
		UseMode = MissionZone.Instance.CurrentGameZone.IsInteractionObjectInRange(Agent.Position, 2);

		if (UseMode != old)
		{
//			if (UseMode)
//			{
//				GuiManager.Instance.SwitchToUseMode();
//				Controls.SwitchToUseMode();
//			}
//			else
//			{
//				GuiManager.Instance.SwitchToCombatMode();
//				Controls.SwitchToCombatMode();
//			}
		}
	}

	public void UpdateControlsPosition()
	{
		//Controls.UpdateControlsPosition();
	}

	void OnTriggerEnter(Collider other)
	{
		InteractionTrigger interaction = other.GetComponent<InteractionTrigger>();
		if (interaction != null)
		{
			AgentOrder order = AgentOrderFactory.Create(AgentOrder.E_OrderType.E_USE);
			order.InteractionObject = interaction;
			order.Position = order.InteractionObject.GetEntryTransform().position;
			order.Interaction = E_InteractionType.On;
			Agent.BlackBoard.AddOrder(order);
			return;
		}
	}

	public void HandleAction(AgentAction a)
	{
		if (a is AgentActionAttack)
		{
			CurrentAttackAction = a as AgentActionAttack;
			Agent.WorldState.SetWSProperty(E_PropKey.E_ALERTED, true);
		}
		else if (a is AgentActionInjury)
		{
			Agent.WorldState.SetWSProperty(E_PropKey.E_ORDER, AgentOrder.E_OrderType.E_NONE);
			ComboProgress.Clear();
			ClearBufferedOrder();
			//GuiManager.Instance.ShowComboProgress(ComboProgress);
			Game.Instance.Hits = 0;
			Game.Instance.NumberOfInjuries++;

		}
		else if (a is AgentActionDeath)
		{
			Agent.WorldState.SetWSProperty(E_PropKey.E_ORDER, AgentOrder.E_OrderType.E_NONE);
			ComboProgress.Clear();
			ClearBufferedOrder();
			//GuiManager.Instance.ShowComboProgress(ComboProgress);
			Game.Instance.Hits = 0;
			Game.Instance.NumberOfDeath++;
			MissionZone.Instance.EndOfMission(false);
			// of	unlockAchievement
//			if(Game.Instance.NumberOfDeath >= 100) {
//				Achievements.UnlockAchievement(2);
//			} else if(Game.Instance.NumberOfDeath >= 50) {
//				Achievements.UnlockAchievement(1);
//			}
		}

	}



	private void CreateOrderGoTo(Vector3 dir, float force)
	{
		if (CouldAddnewOrder() == false)
			return;

		AgentOrder order = AgentOrderFactory.Create(AgentOrder.E_OrderType.E_GOTO);
		order.Direction = dir;
		order.MoveSpeedModifier = force;

		Agent.BlackBoard.AddOrder(order);
	}

	private void CreateOrderStop()
	{
		AgentOrder order = AgentOrderFactory.Create(AgentOrder.E_OrderType.E_STOPMOVE);
		Agent.BlackBoard.AddOrder(order);
	}


	private void CreateOrderAttack(E_AttackType type)
	{
		if (CouldBufferNewOrder() == false && CouldAddnewOrder() == false)
		{
			return;
		}

		AgentOrder order = AgentOrderFactory.Create(AgentOrder.E_OrderType.E_ATTACK);

		if (MoveDirection != Vector3.zero)
			order.Direction = MoveDirection;
		else
			order.Direction = Transform.forward;


		order.AnimAttackData = ProcessCombo(type);

		order.Target = MissionZone.Instance.GetBestTarget(Agent, LastAttacketTarget);;

		if (CouldAddnewOrder())
		{
			Agent.BlackBoard.AddOrder(order);
		}
		else
		{
			BufferedOrders.Enqueue(order);
		}
	}

	private void CreateOrderDodge()
	{
		if (Agent.BlackBoard.IsOrderAddPossible(AgentOrder.E_OrderType.E_DODGE) == false)
			return;

		Vector3 rollDir;

		if(MoveDirection != Vector3.zero)
			rollDir = MoveDirection;
		else 
			rollDir = Agent.Forward;

		rollDir.Normalize();

		AgentOrder order = AgentOrderFactory.Create(AgentOrder.E_OrderType.E_DODGE);
		order.Direction = rollDir;


		Agent.BlackBoard.AddOrder(order);

		ComboProgress.Clear();
		ClearBufferedOrder();
		//GuiManager.Instance.ShowComboProgress(ComboProgress);
	}

	public void CreateOrderUse()
	{
		if (Agent.BlackBoard.IsOrderAddPossible(AgentOrder.E_OrderType.E_USE) == false)
			return;

		InteractionGameObject onObject = MissionZone.Instance.CurrentGameZone.GetNearestInteractionObject(Agent.Position, 2);

		if (onObject == null)
			return;

		if (onObject is InteractionLever)
		{
			InteractionLever lever = onObject as InteractionLever;
			if (lever.State != InteractionLever.E_State.E_OFF && lever.State != InteractionLever.E_State.E_OFF)
			{
				return;
			}

			AgentOrder order = AgentOrderFactory.Create(AgentOrder.E_OrderType.E_USE);
			order.InteractionObject = onObject;
			order.Position = order.InteractionObject.GetEntryTransform().position;
			order.Interaction = E_InteractionType.On;
			Agent.BlackBoard.AddOrder(order);

			return;
		}

		ComboProgress.Clear();
		ClearBufferedOrder();
		//GuiManager.Instance.ShowComboProgress(ComboProgress);
	}


	void ClearBufferedOrder()
	{
		while(BufferedOrders.Count > 0)
			AgentOrderFactory.Return(BufferedOrders.Dequeue());
	}

	public bool CouldBufferNewOrder()
	{
		return BufferedOrders.Count <= 0 && CurrentAttackAction != null;
	}


	public bool CouldAddnewOrder()
	{
		AgentOrder.E_OrderType order =  Agent.WorldState.GetWSProperty(E_PropKey.E_ORDER).GetOrder();

		if (order == AgentOrder.E_OrderType.E_DODGE || order == AgentOrder.E_OrderType.E_ATTACK || order == AgentOrder.E_OrderType.E_USE)
			return false;

		AgentAction action;

		for(int i = 0;i < Agent.BlackBoard.ActionCount; i++)
		{
			action = Agent.BlackBoard.GetAction(i);
			if (action is AgentActionAttack && (action as AgentActionAttack).AttackPhaseDone == false)
				return false;
			else if (action is AgentActionRoll)
				return false;
			else if (action is AgentActionUseLever)
				return false;
			else if (action is AgentActionGoTo && (action as AgentActionGoTo).Motion == E_MotionType.Sprint)
				return false;
		}
		return true;
	}

	public void StopMove(bool stop)
	{
//		if (stop)
//			Controls.DisableInput();
//		else
//			Controls.EnableInput();
	}

	public void Teleport(Teleport teleport)
	{
		Agent.BlackBoard.Stop = true;
		Agent.BlackBoard.TeleportDestination = teleport;
		Agent.WorldState.SetWSProperty(E_PropKey.E_TELEPORT, true);
		//Controls.Reset();
	}


	public void AddExperience(int exp, float scoreModifier)
	{
		Game.Instance.Money += exp;
	}

	private AnimAttackData ProcessCombo(E_AttackType attackType)
	{
		if (attackType != E_AttackType.O && attackType != E_AttackType.X)
			return null;
		ComboProgress.Add(attackType);

		for (int i = 0; i <  SkillData.PlayerComboAttacks.Length; i++)
		{// projedem vsechny attacky
			Combo combo = SkillData.PlayerComboAttacks[i];
			if (combo.SwordLevel > Game.Instance.SwordLevel)
				continue; // nema combo...
			bool valid = ComboProgress.Count <= combo.ComboSteps.Length; // 
			for (int ii = 0; ii < ComboProgress.Count && ii < combo.ComboSteps.Length; ii++)
			{
				if (ComboProgress[ii] != combo.ComboSteps[ii].AttackType ||  combo.ComboSteps[ii].ComboLevel >  Game.Instance.ComboLevel[i])
				{// combo nepokracuje timto stepem... nebo step neni available
					valid = false;
					break;
				}
			}
			if (valid)
			{
				combo.ComboSteps[ComboProgress.Count - 1].Data.LastAttackInCombo = NextAttackIsAvailable(E_AttackType.X) == false && NextAttackIsAvailable(E_AttackType.O) == false;
				combo.ComboSteps[ComboProgress.Count - 1].Data.FirstAttackInCombo = false;
				combo.ComboSteps[ComboProgress.Count - 1].Data.ComboIndex = i;
				combo.ComboSteps[ComboProgress.Count - 1].Data.FullCombo = ComboProgress.Count == combo.ComboSteps.Length;
				combo.ComboSteps[ComboProgress.Count - 1].Data.ComboStep = ComboProgress.Count;
				//GuiManager.Instance.ShowComboProgress(ComboProgress);
				return combo.ComboSteps[ComboProgress.Count - 1].Data;
			}
		}

		ComboProgress.Clear();
		ComboProgress.Add(attackType);

		for (int i = 0; i < SkillData.PlayerComboAttacks.Length; i++)
		{// projedem vsechny prvni stepy
			if (SkillData.PlayerComboAttacks[i].ComboSteps[0].AttackType == attackType)
			{
				SkillData.PlayerComboAttacks[i].ComboSteps[0].Data.FirstAttackInCombo = true;
				SkillData.PlayerComboAttacks[i].ComboSteps[0].Data.LastAttackInCombo = false;
				SkillData.PlayerComboAttacks[i].ComboSteps[0].Data.ComboIndex = i;
				SkillData.PlayerComboAttacks[i].ComboSteps[0].Data.FullCombo = false;
				SkillData.PlayerComboAttacks[i].ComboSteps[0].Data.ComboStep = 0;
				//GuiManager.Instance.ShowComboProgress(ComboProgress);
				return SkillData.PlayerComboAttacks[i].ComboSteps[0].Data;
			}
		}
		Debug.LogError("Could not find any combo attack !!! Some shit happens");
		return null;
	}

	private bool NextAttackIsAvailable(E_AttackType attackType)
	{
		if (attackType != E_AttackType.O && attackType != E_AttackType.X)
			return false;

		if (ComboProgress.Count == 5) // ehmm. proste je jich ted sest, tak bacha na to...
			return false;

		List<E_AttackType> progress = new List<E_AttackType>(ComboProgress);

		progress.Add(attackType);

		for (int i = 0; i < SkillData.PlayerComboAttacks.Length; i++)
		{// projedem vsechny attacky

			Combo combo = SkillData.PlayerComboAttacks[i];

			if (combo.SwordLevel > Game.Instance.SwordLevel)
				continue;

			bool valid = true;
			for (int ii = 0; ii < progress.Count; ii++)
			{
				if (progress[ii] != combo.ComboSteps[ii].AttackType || combo.ComboSteps[ii].ComboLevel > Game.Instance.ComboLevel[i])
				{
					valid = false;
					break;
				}
			}

			if (valid)
				return true;
		}
		return false;
	}

	public void HealToFullHealth()
	{
		StartCoroutine(HealingUp());
	}

	IEnumerator HealingUp()
	{
		yield return new WaitForSeconds(1.5f);

		float healingHP = Agent.BlackBoard.RealMaxHealth - Agent.BlackBoard.Health + 1;

		while (healingHP > 0)
		{
			float h = 33 * Time.deltaTime;
			if (healingHP - h < 0)
				h = healingHP;

			healingHP -= h;
			Agent.BlackBoard.Health += h;

			//GuiManager.Instance.SetHealthPercent(Owner.BlackBoard.Health, Owner.BlackBoard.RealMaxHealth);

			yield return new WaitForEndOfFrame();
		}

		if (Agent.BlackBoard.Health > Agent.BlackBoard.RealMaxHealth)
			Agent.BlackBoard.Health = Agent.BlackBoard.RealMaxHealth;

		//GuiManager.Instance.SetHealthPercent(Agent.BlackBoard.Health, Agent.BlackBoard.RealMaxHealth);
	}
}

