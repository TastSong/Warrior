//
// /**************************************************************************
//
// Game.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 2016/8/3
//
// Description:Provide  functions  to connect Oracle
//
// Copyright (c) 2016 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
	private static Game _instance;

	public static Game Instance {
		get {
			if (_instance == null) {
				_instance = (new GameObject ("_Game")).AddComponent<Game> ();
				DontDestroyOnLoad(_instance);
			}
			return _instance;
		}
	}

	//玩家生命等级
	public E_HealthLevel HealthLevel = E_HealthLevel.One;
	//当前关卡等级,副本名字
	public string CurrentLevel;
	//当前GameZone的索引
	public int CurrentGameZone;

	public E_GameState GameState = E_GameState.Game; // for editor
	public E_GameType GameType = E_GameType.ChapterOnly; //for editor game play
	public E_GameDifficulty GameDifficulty = E_GameDifficulty.Normal;

	public int Score;
	public int Money;
	private int _NumberOfDeath;
	public int NumberOfDeath { get { return _NumberOfDeath; } set { _NumberOfDeath = value; if (GameState == E_GameState.Game) PlayerPrefs.SetInt("NumberOfDeath", NumberOfDeath); } }
	[System.NonSerialized]
	public int NumberOfInjuries;
	[System.NonSerialized]
	public int NumberOfBlockedHits;
	[System.NonSerialized]
	public int NumberOfKnockdowns;
	[System.NonSerialized]
	public int NumberOfCriticals;
	[System.NonSerialized]
	public int NumberOfBreakBlocks;
	[System.NonSerialized]
	public int NumberOfBarrels;
	//连击次数
	private int _Hits;
	//清楚连击数量的时间
	public float TimeToClearHits;

	[System.NonSerialized]
	public E_ComboLevel[] ComboLevel = { E_ComboLevel.One, E_ComboLevel.One, E_ComboLevel.One, E_ComboLevel.One, E_ComboLevel.One, E_ComboLevel.One };
	[System.NonSerialized]
	public E_SwordLevel SwordLevel = E_SwordLevel.One;

	public int Hits
	{
		get{ return _Hits;}
		set{
			_Hits = value;
			TimeToClearHits = Time.timeSinceLevelLoad +3f;
			// todo:界面更新连击数量

		}
	}


	void FixedUpdate()
	{
		//处理连击倒计时
		if (TimeToClearHits < Time.timeSinceLevelLoad && Hits > 0)
		{
			Hits = 0;
		}
	}
}

