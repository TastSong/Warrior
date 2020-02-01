//
// /**************************************************************************
//
// ComponentSounds.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 2016/8/2
//
// Description:Provide  functions  to connect Oracle
//
// Copyright (c) 2016 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

/// <summary>
/// E sound type.
/// 播放声音的类型
/// </summary>
public enum E_SoundType
{
	None,
	Spawn,
	Step,
	Roll,
	Berserk,
	PrepareAttack,
	Miss,
	Hit,
	Block,
	WeaponOn,
	WeaponOff,
	Knockdown,
	Fatality,
}

/// <summary>
/// Component sounds.
/// 声音播放组件
/// </summary>
public class ComponentSounds : MonoBehaviour
{
	private AudioSource Audio;
	
	#region Sounds
	[SerializeField]
	private AudioClip[] SpawnSounds = null;
	[SerializeField]
	private AudioClip[] StepSounds = null;
	[SerializeField]
	private AudioClip[] RollSounds = null;
	[SerializeField]
	private AudioClip[] BerserkSounds = null;

	[SerializeField]
	private AudioClip[] PrepareAttackSounds= null;
	[SerializeField]
	private AudioClip[] AttackMissSounds = null;
	[SerializeField]
	private AudioClip[] AttackHitSounds = null;
	[SerializeField]
	private AudioClip[] AttackBlockSounds = null;


	[SerializeField]
	private AudioClip WeaponOn = null;
	[SerializeField]
	private AudioClip WeaponOff = null;


	public AudioClip SpawnSound
	{
		get
		{
			if (SpawnSounds ==null || SpawnSounds.Length == 0)
				return null;
			return SpawnSounds[Random.Range(0,SpawnSounds.Length)];
		}
	}

	public AudioClip StepSound
	{
		get
		{
			if (StepSounds ==null || StepSounds.Length == 0)
				return null;
			return StepSounds[Random.Range(0,StepSounds.Length)];
		}
	}

	public AudioClip RollSound
	{
		get
		{
			if (RollSounds ==null || RollSounds.Length == 0)
				return null;
			return RollSounds[Random.Range(0,RollSounds.Length)];
		}
	}

	public AudioClip BerserkSound
	{
		get
		{
			if (BerserkSounds ==null || BerserkSounds.Length == 0)
				return null;
			return BerserkSounds[Random.Range(0,BerserkSounds.Length)];
		}
	}

	public AudioClip PrepareAttackSound
	{
		get
		{
			if (PrepareAttackSounds ==null || PrepareAttackSounds.Length == 0)
				return null;
			return PrepareAttackSounds[Random.Range(0,PrepareAttackSounds.Length)];
		}
	}

	public AudioClip AttackMissSound
	{
		get
		{
			if (AttackMissSounds ==null || AttackMissSounds.Length == 0)
				return null;
			return AttackMissSounds[Random.Range(0,AttackMissSounds.Length)];
		}
	}

	public AudioClip AttackHitSound
	{
		get
		{
			if (AttackHitSounds ==null || AttackHitSounds.Length == 0)
				return null;
			return AttackHitSounds[Random.Range(0,AttackHitSounds.Length)];
		}
	}

	public AudioClip AttackBlockSound
	{
		get
		{
			if (AttackBlockSounds ==null || AttackBlockSounds.Length == 0)
				return null;
			return AttackBlockSounds[Random.Range(0,AttackBlockSounds.Length)];
		}
	}

	#endregion


	void Awake()
	{
		Audio = GetComponent<AudioSource>();
	}

	/// <summary>
	/// Plaies the sound.
	/// </summary>
	/// <param name="sound">Sound.</param>
	public void PlaySound(E_SoundType sound)
	{
		switch (sound) {
		case E_SoundType.Fatality:
			PlaySound(SoundDataManager.Instance.FatalitySound);
			break;
		case E_SoundType.Berserk:
			PlaySound(BerserkSound);
			break;
		case E_SoundType.Block:
			PlaySound(AttackBlockSound);
			break;
		case E_SoundType.Hit:
			PlaySound(AttackHitSound);
			break;
		case E_SoundType.Knockdown:
			PlaySound(SoundDataManager.Instance.KnockDownSound);
			break;
		case E_SoundType.Miss:
			PlaySound(AttackMissSound);
			break;		
		case E_SoundType.PrepareAttack:
			PlaySound(PrepareAttackSound);
			break;
		case E_SoundType.Roll:
			PlaySound(RollSound);
			break;
		case E_SoundType.Spawn:
			PlaySound(SpawnSound);
			break;
		case E_SoundType.Step:
			PlaySound(StepSound);
			break;
		case E_SoundType.WeaponOff:
			PlaySound(WeaponOff);
			break;
		case E_SoundType.WeaponOn:
			PlaySound(WeaponOn);
			break;
		default:
			Debug.Log("Error Sound Type:" + sound.ToString());
			break;
		}
	}

	#region Play Sound
	/// <summary>
	/// Plaies the spawn.
	/// 出生
	/// </summary>
	public void PlaySpawn()
	{
		PlaySound(SpawnSound);
	}
	/// <summary>
	/// Plaies the step.
	/// 移动
	/// </summary>
	public void PlayStep()
	{
		PlaySound(StepSound);
	}
	/// <summary>
	/// Plaies the roll.
	/// 翻滚
	/// </summary>
	public void PlayRoll()
	{
		PlaySound(RollSound);
	}
	/// <summary>
	/// Plaies the berserk.
	/// 爆击
	/// </summary>
	public void PlayBerserk()
	{
		PlaySound(BerserkSound);
	}

	/// <summary>
	/// Plaies the weapon on.
	/// 使用武器
	/// </summary>
	public void PlayWeaponOn()
	{
		PlaySound(WeaponOn);
	}

	/// <summary>
	/// Plaies the weapon off.
	/// 卸载武器
	/// </summary>
	public void PlayWeaponOff()
	{
		PlaySound(WeaponOff);
	}

	/// <summary>
	/// Plaies the prepare attack.
	/// 准备攻击
	/// </summary>
	public void PlayPrepareAttack()
	{
		PlaySound(PrepareAttackSound);
	}

	/// <summary>
	/// Plaies the attack miss.
	/// 攻击Miss
	/// </summary>
	public void PlayAttackMiss()
	{
		PlaySound(AttackMissSound);
	}

	/// <summary>
	/// Plaies the attack hit.
	/// 攻击击中
	/// </summary>
	public void PlayAttackHit()
	{
		PlaySound(AttackHitSound);
	}

	/// <summary>
	/// Plaies the attack block.
	/// 攻击格挡
	/// </summary>
	public void PlayAttackBlock()
	{
		PlaySound(AttackBlockSound);
	}

	/// <summary>
	/// Plaies the knockdown.
	/// 攻击击倒
	/// </summary>
	public void PlayKnockdown()
	{
		PlaySound(SoundDataManager.Instance.KnockDownSound);
	}

	#endregion

	/// <summary>
	/// Plaies the sound.
	/// </summary>
	/// <param name="clip">Clip.</param>
	public void PlaySound(AudioClip clip)
	{
		if (clip != null && Audio != null)
			Audio.PlayOneShot(clip);
	}

	/// <summary>
	/// Plaies the loop sound.
	/// </summary>
	/// <param name="clip">声音片段Clip.</param>
	/// <param name="delay">播放延时Delay.</param>
	/// <param name="time">播放时长Time.</param>
	/// <param name="fadeInTime">渐显时间Fade in time.</param>
	/// <param name="fadeOutTime">渐隐时间Fade out time.</param>
	public void PlayLoopSound(AudioClip clip, float delay, float time, float fadeInTime, float fadeOutTime)
	{
		StartCoroutine(_PlayLoopSound(clip,delay,time,fadeInTime,fadeOutTime));
	}

	IEnumerator _PlayLoopSound(AudioClip clip, float delay, float time, float fadeInTime, float fadeOutTime)
	{
		Audio.volume = 0;
		Audio.loop = true;
		Audio.clip = clip;

		yield return new WaitForEndOfFrame();

		yield return new WaitForSeconds(delay);

		Audio.Play();

		float step = 1/fadeInTime;
		while(Audio.volume < 1)
		{
			Audio.volume = Mathf.Min(1.0f, Audio.volume + step*Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(time - fadeInTime -fadeOutTime);

		step = 1/ fadeOutTime;
		while(Audio.volume > 0)
		{
			Audio.volume = Mathf.Max(0.0f, Audio.volume - step*Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}

		Audio.Stop();

		yield return new WaitForEndOfFrame();

		Audio.volume = 1;

	}
}

