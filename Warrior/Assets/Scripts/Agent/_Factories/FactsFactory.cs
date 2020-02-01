using System;
using System.Collections.Generic;
using UnityEngine;


static public class FactsFactory
{
	static private Queue<Fact> m_Unused = new Queue<Fact>();

	// DEBUG !!!!!!!
	static private List<Fact> m_InAction = new List<Fact>();


	static public Fact Create(Fact.E_FactType type)
	{
		Fact o;

		if (m_Unused.Count > 0)
		{
			o = m_Unused.Dequeue();
			o.FactType = type;
		}
		else
			o = new Fact(type);

		m_InAction.Add(o);
		return o;
	}

	static public void Return(Fact f)
	{
		m_Unused.Enqueue(f);
		m_InAction.Remove(f);
	}

    static public void Report()
    {
        Debug.Log("Fact Factory in action " + m_InAction.Count);
    }
}

