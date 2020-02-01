using System;
using System.Collections.Generic;
using UnityEngine;




static public class AgentOrderFactory
{
	static private Queue<AgentOrder> m_UnusedOrders = new Queue<AgentOrder>();

	// DEBUG !!!!!!!
	static private List<AgentOrder> m_InAction = new List<AgentOrder>();


	static public AgentOrder Create(AgentOrder.E_OrderType type)
	{
		AgentOrder o;

		if (m_UnusedOrders.Count > 0)
		{
			o = m_UnusedOrders.Dequeue();
			o.Type = type;
		}
		else
			o = new AgentOrder(type);

		m_InAction.Add(o);
		return o;
	}


	static public void Return(AgentOrder order)
	{
		m_UnusedOrders.Enqueue(order);

		m_InAction.Remove(order);
	}

    static public void Report()
    {
        Debug.Log("Order Factory in action " + m_InAction.Count);
    }
}

