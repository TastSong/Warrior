using System;
using System.Collections;
using UnityEngine;



[System.Serializable]
public class Fact : System.Object
{
	public enum E_FactType
	{
		E_FACT_INVALID = -1,
		E_EVENT,
		E_ENEMY,
		E_COUNT
	}

	public enum E_DataTypes
	{
		E_EVENT,
		E_POS,
		E_DIR,
        E_AGENT,
		E_COUNT
	}


    public static float LiveTime = 0.2f;

	private BitArray  m_DataTypesSet = new BitArray((int)E_DataTypes.E_COUNT);

    public Agent Causer;
	public E_FactType FactType;// { get { return FactType; } private set { FactType = value; } }
	//private float m_TimeCreated;
	public float Belief;

    private E_EventTypes _Event;
    public E_EventTypes EventType { get { return _Event; } set { _Event = value; m_DataTypesSet.Set((int)E_DataTypes.E_EVENT, true); } }
    private Vector3 _Pos;
    public Vector3 Position { get { return _Pos; } set { _Pos = value; m_DataTypesSet.Set((int)E_DataTypes.E_POS, true); } }
    private Vector3 _Dir;
	public Vector3 Direction { get {return _Dir; } set { _Dir = value; m_DataTypesSet.Set((int)E_DataTypes.E_DIR, true); } }
    private Agent _Agent;
    public Agent Agent { get { return _Agent; } set { _Agent = value; m_DataTypesSet.Set((int)E_DataTypes.E_AGENT, true); } }
	public bool Deleted;


	static private int m_TotalNumberOfFacts;

	public Fact(E_FactType type) { FactType = type; m_TotalNumberOfFacts++; }

	public void Reset(E_FactType newType) 
	{
		FactType = newType;
		//m_TimeCreated = Time.timeSinceLevelLoad;
		Belief = 0;
		Position = Vector3.zero;
		Direction  = Vector3.zero;
        Agent = null;
		Deleted = false;
		m_DataTypesSet.SetAll(false);
	}


	public bool MatchesQuery(Fact other)
	{	
		if(Deleted) return false;

		if (other.m_DataTypesSet.Get((int)E_DataTypes.E_EVENT) == true)
			if(EventType != other.EventType)
				return false;

		return true;
	}

    public void DecreaseBelief()
    {
        if (Belief <= 0.0f)
            return;
        Belief -= (1.0f / LiveTime) * Time.deltaTime;
        Belief = Mathf.Max(0, Belief);
    }

    public override string ToString()
    {
        string s = base.ToString() + " : " + FactType.ToString();

        if (m_DataTypesSet.Get((int)E_DataTypes.E_EVENT) == true)
            s += " " +EventType.ToString();

        return s;
    }

}
