/***************************************************************
 * Class Name :	Blackboard
 * Function   : Central memory for GOAPController and other subsystems. 
 * 
 * Created by : Marek Rabas
 *
 **************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;


public class MissionBlackBoard : MonoBehaviour
{
    public List<Transform> EnemiesTransform;
    public float LastAttackTime;

    public static MissionBlackBoard Instance = null;


    void Awake()
    {
        Instance = this;
    }


    void Reset()
    {
        LastAttackTime = 0;
    }


}

