using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Memory : System.Object
{
	private List<Fact>[] m_Facts = new List<Fact>[(int)Fact.E_FactType.E_COUNT];

	public void AddFact(Fact fact)
	{
		int i = (int)fact.FactType;
		if (m_Facts[i] == null)
			m_Facts[i] = new List<Fact>();

        fact.Deleted = false;
		m_Facts[i].Add(fact);
	}

	public void RemoveFact(Fact fact)
	{
		m_Facts[(int)fact.FactType].Remove(fact);
		fact.Deleted = true;

        FactsFactory.Return(fact);

	}


	public Fact GetFact(Fact query)
	{
		int index = (int)query.FactType;

        if (m_Facts[index] == null || m_Facts[index].Count == 0)
            return null;

        for (int i = m_Facts[index].Count -1; i >= 0; i--)
		{
            if (m_Facts[index][i].MatchesQuery(query))
                return m_Facts[index][i];
		}
		return null;
	}

	public void Reset()
	{
		for(Fact.E_FactType i = 0; i < Fact.E_FactType.E_COUNT; i++)
		{
			if (m_Facts[(int)i] == null)
				continue;

			while (m_Facts[(int)i].Count > 0)
				RemoveFact(m_Facts[(int)i][0]);
		}
	}

	/**
	* Updates the Memory, decrease belief
	*/
	public void Update()
	{
        for (int i = 0; i < (int) Fact.E_FactType.E_COUNT; i++)
        {
            if (m_Facts[i] == null)
                continue;

            for (int ii = m_Facts[i].Count -1 ; ii >= 0 ; ii--)
            {
                if (m_Facts[i][ii] == null)
                    continue;

                
                m_Facts[i][ii].DecreaseBelief();

              //  Debug.Log(m_Facts[i][ii].ToString() + " " + m_Facts[i][ii].Belief.ToString());

                if(m_Facts[i][ii].Belief == 0)
                    RemoveFact(m_Facts[i][ii]);
            }
        }

	
	}

	void CleanupFacts()
	{
	}


}

