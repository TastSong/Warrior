//
// /**************************************************************************
//
// Cache.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 2016/7/31
//
// Description:Provide  functions  to connect Oracle
//
// Copyright (c) 2016 xiaohong
//
// **************************************************************************/
using System;
using UnityEngine;
using System.Collections;


/// <summary>
/// Cache.
/// </summary>
public class Cache 
{
	//资源对象
	[SerializeField]
	protected GameObject Prefab;

	//同屏最大数量
	public int MaxCount = 5;

	//在使用中的layer标记
	protected const int cache_inuse = 20;
	//未使用的layer标记
	protected const int cache_free = 21;

	/// <summary>
	/// Hide the specified enemy.
	/// </summary>
	/// <param name="enemy">Enemy.</param>
	protected virtual IEnumerator Hide (GameObject enemy, float delay = 0.5f)
	{
		enemy.SetActive (false);
		yield return new WaitForSeconds (Mathf.Abs(delay));
		enemy.layer = cache_free;
	}

	/// <summary>
	/// Init this instance.
	/// </summary>
	public virtual void Init ()
	{

	}

	/// <summary>
	/// Return the specified enemy.
	/// </summary>
	/// <param name="enemy">Enemy.</param>
	public virtual bool Return (GameObject enemy)
	{
		return false;
	}
}

