using System;

class AStarGOAPNode : AStarNode
{
	public WorldState CurrentState;// { get { return CurrentState; } private set { CurrentState = value; } }
	public WorldState GoalState;// { get { return GoalState; } private set { GoalState = value; } }

	public AStarGOAPNode()
	{
		CurrentState = new WorldState();
		GoalState = new WorldState();
	}
}