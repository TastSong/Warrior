using UnityEngine;
using System.Collections;


public class BreakableExperience : BreakableObject
{
    public GameObject InteractionObject;
    public int Experience;

    private Agent Owner;

	void Awake () 
    {
        base.Initialize();
	}


    protected override void OnStart()
    {
        //Game.Instance.NumberOfBarrels++;
        //Player.Instance.AddExperience(Experience, 1);
		// of	unlockAchievement
		//if(Game.Instance.NumberOfBarrels >= 50) {
		//	Achievements.UnlockAchievement(0);
		//}
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

}
