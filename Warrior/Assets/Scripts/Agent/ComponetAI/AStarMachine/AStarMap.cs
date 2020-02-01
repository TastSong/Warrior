using System;


abstract class AStarMap : System.Object
{
		//neighbours 临近； 获取节点临近节点数量
	public abstract int GetNumAStarNeighbours(AStarNode pAStarNode);
		//获取指定临近节点
	public abstract short GetAStarNeighbour(AStarNode pAStarNode, short iNeighbor);
		//创建A*节点
	public abstract AStarNode CreateANode(short id);
		//获取A*节点状态标记
	public abstract AStarNode.E_AStarFlags GetAStarFlags(short NodeID);
		//设置A*节点状态标记
	public virtual void SetAStarFlags(short NodeID, AStarNode.E_AStarFlags flag){}
		//比较A*节点
	public abstract bool CompareNodes(AStarNode node1, AStarNode node2);
		//清空
	public abstract void Cleanup();
}