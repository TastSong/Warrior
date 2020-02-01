using UnityEngine;
using System.Collections.Generic;

public class SensorBestCombatPositions : MonoBehaviour {

    //Agent Owner;
    //Agent MyEnemy;
    //Transform Transform;

    public float MinRange = 3;
    public float MaxRange = 7;
    public float MaxAngle;

    Vector3 LastEnemyPos;
    
    void Awake()
    {
       // Transform = transform;
      //  Owner = GetComponent("Agent") as Agent;
      //  MyEnemy = Player.Instance.Agent;
    }

	// Use this for initialization
	void Start () {
	
	}


    void OnDrawGizmos()
    {
      /*  for (int i = 0; Owner.BlackBoard.BestPositionsToAttack != null && i < Owner.BlackBoard.BestPositionsToAttack.Count; i++)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(Owner.BlackBoard.BestPositionsToAttack[i], 0.1f); 
        }*/

    }
	// Update is called once per frame
	void FixedUpdate () 
    {
     /*   if (Owner.isVisible == false)
            return;

        if (MyEnemy.IsAlive == false)
        {
            return;
        }

        if ((LastEnemyPos - MyEnemy.Position).sqrMagnitude < 1 * 1)
            return;

        Owner.BlackBoard.BestPositionsToAttack.Clear();


        Owner.BlackBoard.BestPositionsToAttack.Add(MyEnemy.Position + Vector3.forward * 4);
        Owner.BlackBoard.BestPositionsToAttack.Add(MyEnemy.Position + (Vector3.forward + Vector3.right) * 2);
        Owner.BlackBoard.BestPositionsToAttack.Add(MyEnemy.Position + (Vector3.forward - Vector3.right) * 2);
        Owner.BlackBoard.BestPositionsToAttack.Add(MyEnemy.Position - Vector3.forward * 4);
        Owner.BlackBoard.BestPositionsToAttack.Add(MyEnemy.Position + Vector3.right * 4);
        Owner.BlackBoard.BestPositionsToAttack.Add(MyEnemy.Position - Vector3.right * 4);
        Owner.BlackBoard.BestPositionsToAttack.Add(MyEnemy.Position - (Vector3.forward + Vector3.right) * 2);
        Owner.BlackBoard.BestPositionsToAttack.Add(MyEnemy.Position - (Vector3.forward - Vector3.right) * 2);
     */   

      //  LastEnemyPos = MyEnemy.Position;
	}

    bool IsGroundThere(Vector3 pos)
    {
        return Physics.Raycast(pos + Vector3.up, -Vector3.up, 5, 1 << 10);
    }

}
