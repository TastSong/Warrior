using UnityEngine;
using System.Collections;

/// <summary>
/// Trigger show assets.
/// </summary>
public class TriggerShowAssets: MonoBehaviour
{
	public GameObject[] GameObjectsList;

    
	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag ("Player") == false)
			return;

		GetComponent<BoxCollider> ().enabled = false;

		//设置需要显示的场景对象
		MissionZone.Instance.SetManagedActive(IsInList);

	}

	/// <summary>
	/// 是否在需要显示的列表中
	/// </summary>
	/// <returns><c>true</c> if this instance is in list the specified gameObject; otherwise, <c>false</c>.</returns>
	/// <param name="gameObject">Game object.</param>
	bool IsInList(GameObject gameObject)
	{
		for (int ii = 0; ii < GameObjectsList.Length; ii++)
		{
			if (gameObject == GameObjectsList[ii])
				return true;
		}
		return false;
	}

}
