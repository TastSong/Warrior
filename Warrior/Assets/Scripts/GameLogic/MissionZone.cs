using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// Mission zone.
/// 副本管理器
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SoundDataManager))]
[RequireComponent(typeof(SpriteEffectsManager))]
[RequireComponent(typeof(MissionBlackBoard))]

public class MissionZone : MonoBehaviour
{
	//该场景全部关卡
	public GameZone[] GameZones = new GameZone[1];
	//该场景全部场景资源
	[SerializeField]
	private GameObject[] ManagedObjList = new GameObject[1];
	//当前进行的关卡
	[System.NonSerialized]
	public GameZone CurrentGameZone;
	//当前关卡索引
	public int GameZoneIndex {
		get {
			for (int i = 0; i < GameZones.Length; i++) {
				if (GameZones [i] == CurrentGameZone)
					return i;
			}
			//error
			return 0;
		}
	}
	public AudioSource Music;
	const float MaxMusicVolume = 0.0f;

	[SerializeField]
	private LiveHumanCache[] HumanCaches;
	[SerializeField]
	private DeadHumanCache[] DeadCaches;

	public static MissionZone Instance;

	void Awake ()
	{
		Instance = this;
	}

	void Start()
	{
		
		for (int i = 0; i < HumanCaches.Length; i++) {
			HumanCaches[i].Init();
		}

		for (int ii= 0; ii < DeadCaches.Length; ii++) {
			DeadCaches[ii].Init();
		}

		for (int iii = 0; iii < GameZones.Length; iii++) {
			if (iii == GameZoneIndex)
			{
				GameZones[iii].Enable();
				CurrentGameZone = GameZones[iii];
			}else
			{
				GameZones[iii].Disable();
			}
		}
	}

	public void EndOfMission(bool success)
	{
		if (Game.Instance.GameType == E_GameType.Survival)
		{
//			if(success == false)
//				GuiManager.Instance.ShowMessage(GuiManager.E_HudMessageType.E_DEATH);


			StartCoroutine(EndOfSurvivalMode(3));
		}
		else
		{
			if (success)
			{
				StartCoroutine(FadeOutMusic(1));
				//GuiManager.Instance.FadeOut();
			}
			else
			{
				//GuiManager.Instance.ShowMessage(GuiManager.E_HudMessageType.E_DEATH);
				//StartCoroutine(LoadLastSave(3));
			}
		}
	}


	IEnumerator EndOfSurvivalMode(float delay)
	{
		Debug.Log("mission - end of survival  " + Game.Instance.GameType);
		Player.Instance.Agent.BlackBoard.Stop = true;

		StartCoroutine(FadeOutMusic(2));

		yield return new WaitForSeconds(delay);

		//GuiManager.Instance.ShowMessage(GuiManager.E_HudMessageType.E_NONE);

		//GuiManager.Instance.FadeOut();

		//Game.Instance.Save_Clear();

		yield return new WaitForSeconds(1.1f);

		//Game.Instance.LoadScoreScreen();
	}


/*
    IEnumerator FadeInMusic(float time)
    {
        float volume = 0;
        StopCoroutine("FadeOutMusic");
        Music.Play();

        if (time == 0)
        {
            Music.volume = MaxMusicVolume;
            yield break;
        }


        //Debug.Log("Fade in music");
        while (volume < MaxMusicVolume)
        {
            volume += 1 / time * Time.deltaTime * MaxMusicVolume;
            if (volume > MaxMusicVolume)
                volume = MaxMusicVolume;

            Music.volume = volume;

            yield return new WaitForEndOfFrame();
        }
    }
*/

	public IEnumerator FadeOutMusic(float time)
	{
		StopCoroutine("FadeInMusic");

		if (time == 0)
		{
			Music.volume = 0;
			Music.Stop();
			yield break;
		}

		float volume = MaxMusicVolume;
		while (volume > 0)
		{
			volume -= 1 / time * Time.deltaTime * MaxMusicVolume;
			if (volume < 0)
				volume = 0;

			Music.volume = volume;

			yield return new WaitForEndOfFrame();
		}
		Music.Stop();
	}



	//锁住前一关卡
	public void LockPrevGameZone ()
	{
		int i = GameZoneIndex;
		if (i > 0)
			GameZones [i - 1].Disable ();
	}
	//开启后一关卡
	public void UnLockNextGameZone ()
	{
		int i = GameZoneIndex;
		if (i + 1 < GameZones.Length)
			GameZones [i + 1].Enable ();
	}

	/// <summary>
	/// Sets the managed active.
	/// 设置需要显示的场景资源，等价于遮挡剔除的功能
	/// </summary>
	/// <param name="obj">Object.</param>
	public void SetManagedActive(Func<GameObject, bool> func)
	{
		for (int i = 0; i < ManagedObjList.Length; i++)
		{
			ManagedObjList[i].SetActive(func(ManagedObjList[i]));
		}

	}


	/// <summary>
	/// Gets the human.
	/// 获取怪物对象
	/// </summary>
	/// <returns>The human.</returns>
	/// <param name="enemyType">Enemy type.</param>
	/// <param name="tran">Tran.</param>
	public Agent GetHuman(E_EnemyType enemyType, Transform tran)
	{
		int len = HumanCaches.Length;
		GameObject go;
		for (int i = 0; i < len; i++) {
			if (HumanCaches[i].EnemyType == enemyType)
			{
				go = HumanCaches[i].Get();
				if (go != null)
				{//初始化相关
					Agent a = go.GetComponent<Agent>();
					a.Enable();
					//设置位置
					a.SetPosition(tran);
					//设置阴影

					return a;
				}
			}
		}
		return null;
	}

	/// <summary>
	/// Returns the human.
	/// 放回怪物对象资源
	/// </summary>
	/// <param name="go">Go.</param>
	public void ReturnHuman(GameObject go)
	{
		if (go == null)
			return;
		IEDable ed = go.GetComponent<IEDable>();
		ed.Disable();

		int len = HumanCaches.Length;
		for (int i = 0; i < len; i++) {
			if (HumanCaches[i].Return(go) == true)
				return;
		}
	}


	/// <summary>
	/// Gets the dead.
	/// 获取死亡模型资源
	/// </summary>
	public GameObject GetDead(Agent agent, E_DeadBodyType deadType)
	{
		int len = DeadCaches.Length;
		GameObject go;
		for (int i = 0; i < len; i++) {
			if (DeadCaches[i].EnemyType == agent.EnemyType)
			{
				go = DeadCaches[i].Get(deadType);
				if (go != null)
				{
					ComponentChoppedBody ed = go.GetComponent<ComponentChoppedBody>();
					ed.Enable(agent.transform);
				}
				return go;
			}
		}
		return null;
	}

	/// <summary>
	/// Returns the dead.
	/// 放回死亡模型资源
	/// </summary>
	/// <param name="go">Go.</param>
	public void ReturnDead(GameObject go)
	{
		if (go == null)
			return;
		ComponentChoppedBody ed = go.GetComponent<ComponentChoppedBody>();
		ed.Disable();

		int len = DeadCaches.Length;
		for (int i = 0; i < len; i++) {
			if (DeadCaches[i].Return(go) == true)
				return;
		}
	}


	public Agent GetBestTarget(Agent Owner, Agent Last, bool hasToBeKnockdown = false)
	{
		if (CurrentGameZone == null)
			return null;

		List<Agent> enemies = CurrentGameZone.Enemies;

		float[] EnemyCoeficient = new float[enemies.Count];
		Agent enemy;
		Vector3 dirToEnemy;

		for (int i = 0; i < enemies.Count; i++)
		{
			EnemyCoeficient[i] = 0;
			enemy = enemies[i];

			if (hasToBeKnockdown && enemy.BlackBoard.MotionType != E_MotionType.Knockdown)
				continue;

			if (enemy.BlackBoard.Invulnerable)
				continue;

			dirToEnemy = (enemy.Position - Owner.Position);

			float distance = dirToEnemy.magnitude;

			if (distance > 5.0f)
				continue;

			dirToEnemy.Normalize();

			float angle = Vector3.Angle(dirToEnemy, Owner.Forward);

			if (enemy == Last)
				EnemyCoeficient[i] += 0.1f;

			EnemyCoeficient[i] += 0.2f - ((angle / 180.0f) * 0.2f);

			angle = Vector3.Angle(dirToEnemy, Owner.BlackBoard.DesiredDirection);
			EnemyCoeficient[i] += 0.5f - ((angle / 180.0f) * 0.5f);

			EnemyCoeficient[i] += 0.2f - ((distance / 5) * 0.2f);
		}

		float bestValue = 0;
		int best = -1;
		for (int i = 0; i < enemies.Count; i++)
		{
			if (EnemyCoeficient[i] <= bestValue)
				continue;
			best = i;
			bestValue = EnemyCoeficient[i];
		}

		if (best >= 0)
			return enemies[best];

		return null;
	}


}
