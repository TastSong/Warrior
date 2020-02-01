using UnityEngine;
using System.Collections;

/*
public enum E_EnemyType
{
		None = -1,
		Monster01,
		Monster02,
}

public enum E_GameDifficulty
{
		Easy,
		Normal,
		Hard,
}
*/

public class SpwanPointEnemy : SpawnPoint
{
	public E_EnemyType EnemyType = E_EnemyType.None;
	public E_GameDifficulty Difficulty = E_GameDifficulty.Normal;

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawIcon (transform.position, "SpawnPoint.tif");
	}
}
