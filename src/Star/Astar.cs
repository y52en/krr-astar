using System.Collections.Generic;
using System;

namespace Star
{
	// Token: 0x0200051C RID: 1308
	public class AStar : IPathFinding
	{
		// Token: 0x060019DF RID: 6623 RVA: 0x00086750 File Offset: 0x00084B50
		public AStar(int w, int h)
		{
			this.m_Width = w;
			this.m_Height = h;
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060019E0 RID: 6624 RVA: 0x0008679D File Offset: 0x00084B9D
		public List<AStar.ANode> OpenList
		{
			get
			{
				return this.m_OpenList;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060019E1 RID: 6625 RVA: 0x000867A5 File Offset: 0x00084BA5
		public List<AStar.ANode> CloseList
		{
			get
			{
				return this.m_CloseList;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060019E2 RID: 6626 RVA: 0x000867AD File Offset: 0x00084BAD
		public AStar.ANode CurrentNode
		{
			get
			{
				return this.m_CurrentNode;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060019E3 RID: 6627 RVA: 0x000867B5 File Offset: 0x00084BB5
		public Object LockObject
		{
			get
			{
				return this.m_LockObject;
			}
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x000867BD File Offset: 0x00084BBD
		public bool IsPassable(Vector2 pt)
		{
			return this.GetCost(pt) >= 0;
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x000867D0 File Offset: 0x00084BD0
		public int GetCost(Vector2 pt)
		{
			if (pt.x < 0f || pt.x >= (float)this.m_Width || pt.y < 0f || pt.y >= (float)this.m_Height)
			{
				return -1;
			}
			if (this.m_CostData == null)
			{
				return 1;
			}
			return this.m_CostData[(int)pt.x, (int)pt.y];
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x00086850 File Offset: 0x00084C50
		public virtual List<Vector2> GetPassableNeighbours(AStar.ANode node)
		{
			int num = (int)node.Pos.x;
			int num2 = (int)node.Pos.y;
			List<Vector2> list = new List<Vector2>();
			if (this.IsPassable(new Vector2((float)(num + 1), (float)num2)))
			{
				list.Add(new Vector2((float)(num + 1), (float)num2));
			}
			if (this.IsPassable(new Vector2((float)(num - 1), (float)num2)))
			{
				list.Add(new Vector2((float)(num - 1), (float)num2));
			}
			if (this.IsPassable(new Vector2((float)num, (float)(num2 + 1))))
			{
				list.Add(new Vector2((float)num, (float)(num2 + 1)));
			}
			if (this.IsPassable(new Vector2((float)num, (float)(num2 - 1))))
			{
				list.Add(new Vector2((float)num, (float)(num2 - 1)));
			}
			if (this.m_AllowDiagonal)
			{
				if (this.IsPassable(new Vector2((float)(num + 1), (float)(num2 + 1))))
				{
					list.Add(new Vector2((float)(num + 1), (float)(num2 + 1)));
				}
				if (this.IsPassable(new Vector2((float)(num + 1), (float)(num2 - 1))))
				{
					list.Add(new Vector2((float)(num + 1), (float)(num2 - 1)));
				}
				if (this.IsPassable(new Vector2((float)(num - 1), (float)(num2 + 1))))
				{
					list.Add(new Vector2((float)(num - 1), (float)(num2 + 1)));
				}
				if (this.IsPassable(new Vector2((float)(num - 1), (float)(num2 - 1))))
				{
					list.Add(new Vector2((float)(num - 1), (float)(num2 - 1)));
				}
			}
			return list;
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x000869E4 File Offset: 0x00084DE4
		public void OpenNeighbours(AStar.ANode parent)
		{
			object lockObject = this.LockObject;
			lock (lockObject)
			{
				this.m_OpenList.Remove(parent);
				this.m_CloseList.Add(parent);
				List<Vector2> passableNeighbours = this.GetPassableNeighbours(parent);
				foreach (Vector2 pos in passableNeighbours)
				{
					this.OpenNode(pos, parent);
				}
			}
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x00086A84 File Offset: 0x00084E84
		public AStar.ANode OpenNode(Vector2 pos, AStar.ANode parent)
		{
			if (this.GetCost(pos) <= 0)
			{
				return null;
			}
			int num = (parent == null) ? 0 : (parent.G_cost + this.GetCost(pos));
			int num2 = this.CalcHeuristicCost(pos, this.m_DestPos);
			if (base.FindingLimitCost > 0 && num > base.FindingLimitCost)
			{
				return null;
			}
			AStar.ANode anode = null;
			object lockObject = this.LockObject;
			lock (lockObject)
			{
				if (this.m_Pool.ContainsKey(pos))
				{
					anode = this.m_Pool[pos];
					if (this.m_CloseList.Contains(anode))
					{
						if (anode.F_cost <= num + num2)
						{
							return null;
						}
						this.m_CloseList.Remove(anode);
					}
					else if (this.m_OpenList.Contains(anode) && anode.F_cost <= num + num2)
					{
						return null;
					}
				}
				else
				{
					anode = new AStar.ANode(pos);
					this.m_Pool[pos] = anode;
				}
				if (!this.m_OpenList.Contains(anode))
				{
					this.m_OpenList.Add(anode);
				}
				anode.Open(num, num2, parent);
			}
			return anode;
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x00086BD0 File Offset: 0x00084FD0
		public virtual int CalcHeuristicCost(Vector2 start, Vector2 end)
		{
			if (this.m_AllowDiagonal)
			{
				int a = Math.Abs((int)end.x - (int)start.x);
				int b = Math.Abs((int)end.y - (int)start.y);
				return Math.Max(a, b);
			}
			int num = Math.Abs((int)end.x - (int)start.x);
			int num2 = Math.Abs((int)end.y - (int)start.y);
			return num + num2;
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x00086C4F File Offset: 0x0008504F
		public AStar.ANode GetMinCostNodeInOpenList()
		{
			return this.GetMinCostNode(this.m_OpenList);
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x00086C60 File Offset: 0x00085060
		public AStar.ANode GetMinCostNode(List<AStar.ANode> list)
		{
			int num = 999;
			int num2 = 999;
			AStar.ANode result = null;
			for (int i = 0; i < list.Count; i++)
			{
				AStar.ANode anode = list[i];
				int f_cost = anode.F_cost;
				int h_cost = anode.H_cost;
				if (f_cost <= num)
				{
					if (f_cost != num || h_cost < num2)
					{
						num = f_cost;
						num2 = h_cost;
						result = anode;
					}
				}
			}
			return result;
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x00086CDC File Offset: 0x000850DC
		public AStar.ANode GetMinCostNodeInCloseList()
		{
			int num = 999;
			AStar.ANode result = null;
			for (int i = 0; i < this.m_CloseList.Count; i++)
			{
				AStar.ANode anode = this.m_CloseList[i];
				int h_cost = anode.H_cost;
				if (h_cost <= num)
				{
					num = h_cost;
					result = anode;
				}
			}
			return result;
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x00086D38 File Offset: 0x00085138
		public override List<Vector2> Find(Vector2 v1, Vector2 v2)
		{
			base.Finding = true;
			this.m_StartPos = v1;
			this.m_DestPos = v2;
			object lockObject = this.LockObject;
			lock (lockObject)
			{
				this.m_OpenList.Clear();
				this.m_CloseList.Clear();
				this.m_Pool.Clear();
				this.m_Path = null;
			}
			AStar.ANode anode = this.OpenNode(this.m_StartPos, null);
			while (this.m_OpenList.Count != 0)
			{
				this.OpenNeighbours(anode);
				anode = this.GetMinCostNodeInOpenList();
				if (anode == null)
				{
					anode = this.GetMinCostNodeInCloseList();
					if (anode != null)
					{
						this.m_Path = anode.GetPath();
						this.ClipPathInCost();
					}
					break;
				}
				if ((int)anode.Pos.x == (int)this.m_DestPos.x && (int)anode.Pos.y == (int)this.m_DestPos.y)
				{
					this.m_Path = anode.GetPath();
					this.ClipPathInCost();
					break;
				}
			}
			base.Finding = false;
			return this.m_Path;
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x00086E6C File Offset: 0x0008526C
		protected void ClipPathInCost()
		{
			if (this.m_Path == null || base.UsableCost <= 0)
			{
				return;
			}
			int num = 0;
			List<Vector2> list = new List<Vector2>();
			list.Add(this.m_Path[0]);
			for (int i = 1; i < this.m_Path.Count; i++)
			{
				Vector2 vector = this.m_Path[i];
				int cost = this.GetCost(vector);
				if (num + cost > base.UsableCost)
				{
					break;
				}
				num += cost;
				list.Add(vector);
			}
			this.m_Path = list;
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x00086F04 File Offset: 0x00085304
		// public override void FindAsync(Vector2 v1, Vector2 v2)
		// {
		// 	base.Finding = true;
		// 	this.m_StartPos = v1;
		// 	this.m_DestPos = v2;
		// 	this.m_Thread = new Thread(new ThreadStart(this.FindThread));
		// 	this.m_Thread.Start();
		// }

		// Token: 0x060019F0 RID: 6640 RVA: 0x00086F40 File Offset: 0x00085340
		// public virtual void FindThread()
		// {
		// 	object lockObject = this.LockObject;
		// 	lock (lockObject)
		// 	{
		// 		this.m_OpenList.Clear();
		// 		this.m_CloseList.Clear();
		// 		this.m_Pool.Clear();
		// 		this.m_Path = null;
		// 	}
		// 	this.m_CurrentNode = this.OpenNode(this.m_StartPos, null);
		// 	while (this.m_OpenList.Count != 0)
		// 	{
		// 		if (this.m_AbortRequest)
		// 		{
		// 			break;
		// 		}
		// 		this.OpenNeighbours(this.m_CurrentNode);
		// 		this.m_CurrentNode = this.GetMinCostNodeInOpenList();
		// 		if (this.m_CurrentNode == null)
		// 		{
		// 			this.m_CurrentNode = this.GetMinCostNodeInCloseList();
		// 			if (this.m_CurrentNode != null)
		// 			{
		// 				this.m_Path = this.m_CurrentNode.GetPath();
		// 				this.ClipPathInCost();
		// 			}
		// 			break;
		// 		}
		// 		if ((int)this.m_CurrentNode.Pos.x == (int)this.m_DestPos.x && (int)this.m_CurrentNode.Pos.y == (int)this.m_DestPos.y)
		// 		{
		// 			this.m_Path = this.m_CurrentNode.GetPath();
		// 			this.ClipPathInCost();
		// 			break;
		// 		}
		// 		if (base.SleepTime > 0)
		// 		{
		// 			Thread.Sleep(base.SleepTime);
		// 		}
		// 	}
		// 	this.m_AbortRequest = false;
		// 	base.Finding = false;
		// }

		// Token: 0x060019F1 RID: 6641 RVA: 0x000870B8 File Offset: 0x000854B8
		// public override void Abort()
		// {
		// 	object lockObject = this.LockObject;
		// 	lock (lockObject)
		// 	{
		// 		if (base.Finding)
		// 		{
		// 			this.m_AbortRequest = true;
		// 		}
		// 	}
		// }

		// Token: 0x04002085 RID: 8325
		protected int m_Width;

		// Token: 0x04002086 RID: 8326
		protected int m_Height;

		// Token: 0x04002087 RID: 8327
		protected List<AStar.ANode> m_OpenList = new List<AStar.ANode>();

		// Token: 0x04002088 RID: 8328
		protected List<AStar.ANode> m_CloseList = new List<AStar.ANode>();

		// Token: 0x04002089 RID: 8329
		protected Dictionary<Vector2, AStar.ANode> m_Pool = new Dictionary<Vector2, AStar.ANode>();

		// Token: 0x0400208A RID: 8330
		protected AStar.ANode m_CurrentNode;

		// Token: 0x0400208B RID: 8331
		protected Vector2 m_DestPosOriginal;

		// Token: 0x0400208C RID: 8332
		protected Vector2 m_DestPos;

		// Token: 0x0400208D RID: 8333
		protected Vector2 m_StartPos;

		// Token: 0x0400208E RID: 8334
		protected bool m_AllowDiagonal;

		// Token: 0x0400208F RID: 8335
		// protected Thread m_Thread;

		// Token: 0x04002090 RID: 8336
		protected Object m_LockObject = new Object();

		// Token: 0x04002091 RID: 8337
		protected bool m_AbortRequest;

		// Token: 0x0200051D RID: 1309
		public class ANode
		{
			// Token: 0x060019F2 RID: 6642 RVA: 0x00087100 File Offset: 0x00085500
			public ANode(Vector2 pos)
			{
				this.Pos = pos;
			}

			// Token: 0x170001A0 RID: 416
			// (get) Token: 0x060019F3 RID: 6643 RVA: 0x0008710F File Offset: 0x0008550F
			// (set) Token: 0x060019F4 RID: 6644 RVA: 0x00087117 File Offset: 0x00085517
			public AStar.ANode Parent { get; private set; }

			// Token: 0x170001A1 RID: 417
			// (get) Token: 0x060019F5 RID: 6645 RVA: 0x00087120 File Offset: 0x00085520
			// (set) Token: 0x060019F6 RID: 6646 RVA: 0x00087128 File Offset: 0x00085528
			public int ID { get; private set; }

			// Token: 0x170001A2 RID: 418
			// (get) Token: 0x060019F7 RID: 6647 RVA: 0x00087131 File Offset: 0x00085531
			// (set) Token: 0x060019F8 RID: 6648 RVA: 0x00087139 File Offset: 0x00085539
			public Vector2 Pos { get; private set; }

			// Token: 0x170001A3 RID: 419
			// (get) Token: 0x060019F9 RID: 6649 RVA: 0x00087142 File Offset: 0x00085542
			// (set) Token: 0x060019FA RID: 6650 RVA: 0x0008714A File Offset: 0x0008554A
			public int G_cost { get; private set; }

			// Token: 0x170001A4 RID: 420
			// (get) Token: 0x060019FB RID: 6651 RVA: 0x00087153 File Offset: 0x00085553
			// (set) Token: 0x060019FC RID: 6652 RVA: 0x0008715B File Offset: 0x0008555B
			public int H_cost { get; private set; }

			// Token: 0x170001A5 RID: 421
			// (get) Token: 0x060019FD RID: 6653 RVA: 0x00087164 File Offset: 0x00085564
			public int F_cost
			{
				get
				{
					return this.G_cost + this.H_cost;
				}
			}

			// Token: 0x060019FE RID: 6654 RVA: 0x00087173 File Offset: 0x00085573
			public void Open(int G_cost, int H_cost, AStar.ANode parent)
			{
				this.G_cost = G_cost;
				this.H_cost = H_cost;
				this.Parent = parent;
			}

			// Token: 0x060019FF RID: 6655 RVA: 0x0008718C File Offset: 0x0008558C
			public List<Vector2> GetPath()
			{
				List<Vector2> list = new List<Vector2>();
				list.Insert(0, this.Pos);
				if (this.Parent != null)
				{
					this.Parent.GetPath(list);
				}
				return list;
			}

			// Token: 0x06001A00 RID: 6656 RVA: 0x000871C4 File Offset: 0x000855C4
			private void GetPath(List<Vector2> list)
			{
				list.Insert(0, this.Pos);
				if (this.Parent != null)
				{
					this.Parent.GetPath(list);
				}
			}
		}
	}
}
