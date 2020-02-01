using System;
using System.Collections;
using System.Collections.Generic;

class PrecedenceComparer: IComparer<GOAPAction>
{
	public int Compare(GOAPAction x, GOAPAction y)
	{
		return x.Precedence - y.Precedence;
	}
}

class AStarGOAPMap : AStarMap
{
	private Agent Ai;
		
	private List<GOAPAction>[] m_EffectsTable = new List<GOAPAction>[(int)E_PropKey.E_COUNT];
	private List<GOAPAction>  m_Neighbours = new List<GOAPAction>();
	
	public void Initialise(Agent ai)
	{
		Ai = ai;
		m_Neighbours.Clear();
	}
	
		/// <summary>
		/// 构建效果地图
		/// Builds the actions effects table.
		/// </summary>
	public void BuildActionsEffectsTable()
	{
		//Go through every effect and add all the actions that have this effect to the effectsTable
		for(E_GOAPAction i = 0; i < E_GOAPAction.count ;i++)
		{
            GOAPAction action = Ai.GetAction(i);

            if (action == null)
                continue;

			//go through all world effects
			for(uint j = 0;j < (int)E_PropKey.E_COUNT;j++)
			{	
				if (action.WorldEffects.IsWSPropertySet((E_PropKey)j)) // if set
				{
					if (m_EffectsTable[j] == null)
						m_EffectsTable[j] = new List<GOAPAction>();

					m_EffectsTable[j].Add(action);// store it
				}
			}
		}
	}

		/// <summary>
		/// Gets the number A star neighbours.
		/// 获取指定节点的邻居节点数量
		/// </summary>
		/// <returns>The number A star neighbours.</returns>
		/// <param name="aStarNode">A star node.</param>
	public override int	GetNumAStarNeighbours(  AStarNode aStarNode )
	{
		if (aStarNode == null)//If the node is invalid then there's no neighbours
			return 0;
		
		//New search about to occur, clear the previous neighbour records
		m_Neighbours.Clear();

		/**
		 * Go through each world state property and test if it is in the goal and current state
		 * If it isn't then we look through the actionEffects table and see if the actions for this effect can solve the goal state
		 */
		AStarGOAPNode node = (AStarGOAPNode)aStarNode;
		BitArray currBitSet = node.CurrentState.GetPropBitSet();
		BitArray goalBitSet = node.GoalState.GetPropBitSet();

		WorldStateProp currProp;
		WorldStateProp goalProp;
		GOAPAction action;

		for(E_PropKey i = 0;i < E_PropKey.E_COUNT;i++)
		{	
			// First test if the effect isn't in both the goal and the current state

			if(!(currBitSet.Get((int)i) && goalBitSet.Get((int)i)))
				continue; //If not skip this effect

			/**
			 * Now test if the two world state properties are the same
			 * If not we continue
			 */
			currProp = node.CurrentState.GetWSProperty(i);
			goalProp = node.GoalState.GetWSProperty(i);

			if(currProp != null && goalProp != null)
			{	
				if(!(currProp == goalProp))
				{	for(int j = 0;m_EffectsTable[(int)i] != null && j < m_EffectsTable[(int)i].Count;j++)
					{	
						action = m_EffectsTable[(int)i][j];

						if (action == null)
							continue;

						//Are the context preconditions valid?
						if(!action.ValidateContextPreconditions(Ai))
							continue;

						m_Neighbours.Add(action);
					}
				}
			}
		}
		//Sort the returned neighbour actions by precedence
		PrecedenceComparer c = new PrecedenceComparer();
		m_Neighbours.Sort(c);

		return m_Neighbours.Count;
	}

	/**
	 * Returns the neighbours action converted to a node
	* @return a neighbour
	*/

	public override short GetAStarNeighbour(AStarNode AStarNode, short neighbourCount)
	{
		return ((short)m_Neighbours[neighbourCount].Type);
	}

	/**
	 * Creates an a-star node
	* @param the id of the new node
	* @return a new a-star node
	*/

	public override AStarNode CreateANode(short id)
	{	
		AStarGOAPNode newNode = new AStarGOAPNode();
		newNode.NodeID = id;
		
		return newNode;
	}


	

	/**
	* Returns the A Star Flags
	* @return the unchecked flag
	*/
	public override AStarNode.E_AStarFlags GetAStarFlags( short node ){	return  AStarNode.E_AStarFlags.Unchecked; }


	/**
	 * compares the two nodes for precedence, returns true if node one has higher precedence than node two
	 */
	public override bool CompareNodes(AStarNode node1, AStarNode node2)
	{	
		GOAPAction action1 = GetAction(node1.NodeID);
		GOAPAction action2 = GetAction(node2.NodeID);

		return (action1.Precedence < action2.Precedence);
	}	

	/**
	 * gets the AI action
	 * @return the AI action for this action type
	 */
	public GOAPAction GetAction(short nodeID)
	{
		return  Ai.GetAction((E_GOAPAction)nodeID);
	}

	public override void Cleanup()
	{

	}
}