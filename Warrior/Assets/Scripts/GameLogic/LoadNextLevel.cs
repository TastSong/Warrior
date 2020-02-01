using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class LoadNextLevel : MonoBehaviour {

	public string NextLevel;
    public bool LastMission;
	public float Delay = 1;

	// Use this for initialization
	void Start () {
	
	}

    // We'll draw a gizmo in the scene view, so it can be found....
    void OnDrawGizmos()
    {
        BoxCollider b = GetComponent("BoxCollider") as BoxCollider;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(b.transform.position + b.center, b.size);
    }
	
	void OnTriggerEnter(Collider other)
	{
        if (other != Player.Instance.Agent.CharacterController)
            return;

        //Game.Instance.UnlockMission(Application.loadedLevel);

        Player.Instance.Agent.BlackBoard.Stop = true;

		StartCoroutine(Load());
	}

	IEnumerator Load()
	{
        yield return new WaitForSeconds(Delay * 0.5f);

        //if (GameCenterBinding.isGameCenterAvailable() && Game.Instance.HardwareType != E_HardwareType.iPhone3G)
         //   GameCenterBinding.reportScore(Game.Instance.MoneyPerMission, Leaderboard.ToString());

        if (LastMission && Game.Instance.GameType == E_GameType.SinglePlayer)
        {
            //Game.Instance.Save_Clear();
        }

		yield return new WaitForSeconds(Delay * 0.5f);

		//Mission.Instance.EndOfMission(true);

		yield return new WaitForSeconds(1);
        
        //Game.Instance.NextLevelToLoad = NextLevel;

        //Game.Instance.LoadScoreScreen();
	}
}
