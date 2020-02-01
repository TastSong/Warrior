using System;

////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////	A STAR NODE
///////////////////////////////////////////////////////////////////////////////////////////

//
/// <summary>
/// A*节点基类
/// A star node.
/// </summary>
class AStarNode : System.Object
{
		//节点状态
	public enum E_AStarFlags
	{
		Unchecked = 0,
		Open = 1,
		Closed = 2,
		NotPassable = 3,
	}

	public AStarNode()
	{
		NodeID = -1;
		G = 0;
		H = 0;
		F = float.MaxValue;

		Flag = E_AStarFlags.Unchecked;
	}

		/// <summary>
		/// Node 唯一ID
		/// The node I.
		/// </summary>
	public short NodeID;
		/// <summary>
		/// G是从开始点A到达当前方块的移动量
		/// The g.
		/// </summary>
	public float G;
		/// <summary>
		/// H值是从当前方块到终点的移动量估算值
		/// The h.
		/// </summary>
	public float H;
		/// <summary>
		/// 一个G+H 和值
		/// The f.
		/// </summary>
	public float F;
		/// <summary>
		/// 后一节点
		/// The next.
		/// </summary>
	public AStarNode Next;
		/// <summary>
		/// 前一节点
		/// The previous.
		/// </summary>
	public AStarNode Previous;
		/// <summary>
		/// 父节点
		/// The parent.
		/// </summary>
	public AStarNode Parent;
		/// <summary>
		/// 当前节点状态
		/// The flag.
		/// </summary>
	public E_AStarFlags Flag;


};
