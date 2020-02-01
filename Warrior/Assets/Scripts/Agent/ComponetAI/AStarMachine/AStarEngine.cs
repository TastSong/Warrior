/***************************************************************
 * Class Name : AStarEngine
 * Function   : Generic AStar engine using abstract classes for searching.. \used for creating GOAP plan or navigation path
 *				
 * Created by : Marek Rabas
 *
 **************************************************************/

//

using System;
using UnityEngine;

class AStarEngine : System.Object
{

	private AStarGoal Goal;
	private AStarMap Map;
	private AStarStorage Storage;
	public AStarNode CurrentNode;// { get { return CurrentNode; } private set { CurrentNode = value; } }
	
	public short Start;
	public short End;


	/**
	 * Initialise the A Star machine to run a search
	 * @param the Goal we are searching towards
	 * @param the Storage which will store the open and closed lists
	 * @param the map which will get the neighbours, heuristic/action costs for the search
	 */
	public void Setup(AStarGoal _goal, AStarStorage _storage, AStarMap _aStarMap)
	{
		Goal = _goal;
		Storage = _storage;

		Map = _aStarMap;
		Storage.ResetStorage(Map);
	}

/**
 * Runs a generic AStar search
 * The search can look through action nodes or pathfinding nodes and find a path from the start to the Goal
 * @param the ai module,needed to generate neighbours
 */

	public void RunAStar(Agent ai)
	{
		float g, h, f;//The g,h and f values needed for the formula f = g + h
		int numberOfNeighbours = 0;
		short neighbour;
		AStarNode neighbourNode;

		CurrentNode = Map.CreateANode(End);
		//Add the first node to the open list
		Storage.AddToOpenList(CurrentNode,Map);

		h = Goal.GetHeuristicDistance(ai,CurrentNode,true);
		CurrentNode.G = 0.0f;
		CurrentNode.H = h;
		CurrentNode.F = h;

        //if (ai.debugGOAP) Debug.Log(Time.timeSinceLevelLoad + " RunAStar " + CurrentNode.NodeID);
		/**
		 * We come to the main body of A*, we continue until a solution is found
		 */
		while(true)
		{	
			//Get the node with the lowest f from the front of the Storage
			CurrentNode = Storage.RemoveCheapestOpenNode(Map);

			//Add the node to the closed list
			if(CurrentNode != null)
				Storage.AddToClosedList(CurrentNode,Map);
			else
				break;

			//Check whether we've reached our Goal
			//The abstract Goal handles the different Goal conditions for nav and planning
			if(Goal.IsAStarFinished(CurrentNode))
				break;//We've finished, break out

			/**
			 * Next job is to get the neighbours of the current node
			 * Iterate over them calculate their f,g,h and add to the open list if required
			 */ 

			numberOfNeighbours = Map.GetNumAStarNeighbours(CurrentNode);

			for(short i = 0;i  < numberOfNeighbours;i++)
			{	
				neighbour = Map.GetAStarNeighbour(CurrentNode,i);
				if (neighbour == -1)				//If neighbour is invalid quit
					break;
	
				AStarNode.E_AStarFlags flags = Map.GetAStarFlags(neighbour);
				//check whether the node is flagged as open
				if(flags == AStarNode.E_AStarFlags.Open)
				{	
					neighbourNode = Storage.FindInOpenList(neighbour);	
				}
				else if(flags == AStarNode.E_AStarFlags.Closed)
				{	
					/**
					 * Get node from the closed list
					 * If the new f for this node is better than previously then remove the node from the closed list
					 * If it is greater than previously then skip this neighbour
					 */
					neighbourNode = Storage.FindInClosedList(neighbour);
										
				}
				else if(Goal.IsAStarNodePassable(neighbour))
					neighbourNode = Map.CreateANode(neighbour);
				else
					continue;
				
				//If out best path so far is through the neighbour
				//then theres no need to re-assess
				if(neighbourNode == null || CurrentNode.Parent == neighbourNode )
					continue;

				/**
				 * Get the new g,h and f for this neighbour node
				 */
				g = CurrentNode.G + (Goal.GetActualCost(CurrentNode,neighbourNode));
				h = Goal.GetHeuristicDistance(ai,neighbourNode,false);

				f = g + h;

				/**
				 * Now need to check if the new f is more expensive to get to this
				 * neighbor node from the current node than from its previous parent.
				 */
				if(f >= neighbourNode.F)
					continue;


				neighbourNode.F = f;
				neighbourNode.G = g;
				neighbourNode.H = h;
				
				if(flags  == AStarNode.E_AStarFlags.Closed)
					Storage.RemoveFromClosedList(neighbourNode.NodeID,Map);
				
			
				// Finally add the neighbour to the open list
				Storage.AddToOpenList(neighbourNode,Map);

				neighbourNode.Parent = CurrentNode;
			}
		}
	}

	/**
	 * Cleanup
	 */

	public void Cleanup() 
	{
		Goal = null;
		Map = null;
		Storage = null;

		CurrentNode = null;
		Start = 0;
		End = 0;
	}

}
