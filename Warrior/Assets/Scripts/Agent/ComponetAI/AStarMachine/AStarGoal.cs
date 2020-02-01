using System;


////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////		Abstract Goal Node
///////////////////////////////////////////////////////////////////////////////////////////////////

// A* 目标抽象基类
/// <summary>
/// A* 目标抽象基类
///  star goal.
/// </summary>
abstract class AStarGoal : System.Object
{
		//destination 目标
		/// <summary>
		/// 设置目标节点
		/// Sets the destination node.
		/// </summary>
		/// <param name="destNode">Destination node.</param>
	public abstract void SetDestNode(AStarNode destNode);
		//探索的距离  
		/// <summary>
		/// 获取探索的代价
		/// Gets the heuristic distance.
		/// </summary>
		/// <returns>The heuristic distance.</returns>
		/// <param name="ai">Ai.</param>
		/// <param name="pAStarNode">P A star node.</param>
		/// <param name="firstRun">If set to <c>true</c> first run.</param>
	public abstract float GetHeuristicDistance(Agent ai, AStarNode pAStarNode, bool firstRun);
		//实施代价
		/// <summary>
		/// one和two节点之间的代价
		/// Gets the actual cost.
		/// </summary>
		/// <returns>The actual cost.</returns>
		/// <param name="nodeOne">Node one.</param>
		/// <param name="nodeTwo">Node two.</param>
	public abstract float GetActualCost(AStarNode nodeOne, AStarNode nodeTwo);
		/// <summary>
		/// currNode是否完成
		/// etermines whether this instance is A star finished the specified currNode.
		/// </summary>
		/// <returns><c>true</c> if this instance is A star finished the specified currNode; otherwise, <c>false</c>.</returns>
		/// <param name="currNode">Curr node.</param>
	public abstract bool IsAStarFinished(AStarNode currNode);
		/// <summary>
		/// 与node是否可连通
		/// etermines whether this instance is A star node passable the specified node.
		/// </summary>
		/// <returns><c>true</c> if this instance is A star node passable the specified node; otherwise, <c>false</c>.</returns>
		/// <param name="node">Node.</param>
	public abstract bool IsAStarNodePassable(int node);
		/// <summary>
		/// 清理节点
		/// Cleanup this instance.
		/// </summary>
	public abstract void Cleanup();

}
