//
// /**************************************************************************
//
// AnimFSM.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-17
//
// Description:状态机对应状态基类
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimState : System.Object
{
	protected Animation AnimEngine;
	protected Agent Owner;

	/// <summary>
	/// 角色对象Transfrom
	/// </summary>
	protected Transform Transform;

	/// <summary>
	/// 角色骨骼根节点Transfrom
	/// </summary>
	protected Transform RootTransform;
	
	private bool m_IsFinished = true;

	/// <summary>
	/// Initializes a new instance of the <see cref="AnimState"/> class.
	/// </summary>
	/// <param name="_anims">_anims.</param>
	/// <param name="_owner">_owner.</param>
	public AnimState(Animation _anims, Agent _owner)
	{
		AnimEngine = _anims;
		Owner = _owner;
		Transform = Owner.transform;
		// 角色骨骼根节点名字root
		RootTransform = Transform.Find("root");
	}


	protected virtual void Initialize(AgentAction _action)
	{

	}

	/// <summary>
	/// 激活状态
	/// </summary>
	/// <param name="_action">_action.</param>
	public virtual void OnActivate(AgentAction _action)
	{
		
		SetFinished(false);
		
		Initialize(_action);
	}

	/// <summary>
	/// 停用状态
	/// </summary>
	public virtual void OnDeactivate()
	{

	}

	/// <summary>
	/// 结束释放和重置状态信息
	/// </summary>
	public virtual void Release ()
	{
		SetFinished(true);
	}

	/// <summary>
	/// 每帧更新状态.
	/// </summary>
	public virtual void Update()
	{

	}

	/// <summary>
	/// 执行一个新的AgentAction，是该状态执行就返回true，不执行该动作就返回false.
	/// </summary>
	/// <returns><c>true</c>, if new action was handled, <c>false</c> otherwise.</returns>
	/// <param name="_action">_action.</param>
	public virtual bool HandleNewAction(AgentAction _action)
	{
		return false;
	}

	/// <summary>
	/// Determines whether this instance is finished.
	/// </summary>
	/// <returns><c>true</c> if this instance is finished; otherwise, <c>false</c>.</returns>
	public virtual bool IsFinished()
	{
		return m_IsFinished;
	}

	/// <summary>
	/// Sets the finished.
	/// </summary>
	/// <param name="_finished">If set to <c>true</c> _finished.</param>
	public virtual void SetFinished(bool _finished)
	{
		m_IsFinished = _finished;
	}

	protected bool Move(Vector3 velocity, bool slide = true )
	{
		Vector3 old = Transform.position;

		Transform.position += Vector3.up * Time.deltaTime;

		velocity.y -= 9 * Time.deltaTime;
		CollisionFlags flags = Owner.CharacterController.Move(velocity);

		if (slide == false && (flags & CollisionFlags.Sides) != 0)
		{
			Transform.position = old;
			return false;
		}

		if ((flags & CollisionFlags.Below) == 0)
		{
			Transform.position = old;
			return false;
		}

		return true;
	}

	protected bool MoveEx(Vector3 velocity)
	{
		Vector3 old = Transform.position;

		Transform.position += Vector3.up * Time.deltaTime;

		velocity.y -= 9 * Time.deltaTime;
		CollisionFlags flags = Owner.CharacterController.Move(velocity);

		if (flags == CollisionFlags.None)
		{
			RaycastHit hit;
			if (Physics.Raycast(Transform.position, -Vector3.up, out hit, 3, 1 << 10) == false)
			{
				Transform.position = old;
				return false;
			}
		}

		return true;
	}


	protected bool IsGroundThere(Vector3 pos)
	{
		return Physics.Raycast(pos + Vector3.up, -Vector3.up, 5, 1 << 10);
	}

	protected void CrossFade(string anim, float fadeInTime)
	{

		if(AnimEngine.IsPlaying(anim))
			AnimEngine.CrossFadeQueued(anim, fadeInTime, QueueMode.PlayNow);
		else
			AnimEngine.CrossFade(anim, fadeInTime);
	}

	protected void Blend(string anim, float fadeInTime)
	{
		AnimEngine.Blend(anim, 1, fadeInTime);
	}

	protected IEnumerator ShowTrail(AnimAttackData data, float speed, float delay, bool critical, float dustDelay)
	{
		// Time.timeScale = 0.1f;
		if (data.Trail == null)
			yield break;

		if (dustDelay < 0)
			dustDelay = 0;

		//yield return new WaitForSeconds(delay);

		data.Parent.position = Transform.position + Vector3.up * 0.15f;
		data.Parent.rotation = Quaternion.AngleAxis(Transform.rotation.eulerAngles.y, Vector3.up);

		data.Trail.SetActive(true);

		if (data.Dust)
			data.Dust.SetActive(false);

		Color color = Color.white;

		data.Material.SetColor("_TintColor", color);

		if (data.Dust)
		{
			yield return new WaitForSeconds(dustDelay);
			Owner.StartCoroutine(ShowTrailDust(data));
		}

		yield return new WaitForSeconds(delay - dustDelay);

		while (color.a > 0)
		{
			color.a -= Time.deltaTime * speed;
			if (color.a < 0)
				color.a = 0;

			data.Material.SetColor("_TintColor", color);
			yield return new WaitForEndOfFrame();
		}

		data.Trail.SetActive(false);
	}

	public IEnumerator ShowTrailDust(AnimAttackData data)
	{
		Color colorDust = new Color(1,1,1,1);
		data.Dust.SetActive(true);

		data.MaterialDust.SetColor("_TintColor", colorDust);

		data.AnimationDust["Anim_Dust"].speed = 2.0f;
		data.AnimationDust.Play();

		while (colorDust.a > 0)
		{
			colorDust.a -= Time.deltaTime * 3;
			if (colorDust.a < 0)
				colorDust.a = 0;

			data.MaterialDust.SetColor("_TintColor", colorDust);
			yield return new WaitForEndOfFrame();
		}

		data.Dust.SetActive(false);
	}
}


