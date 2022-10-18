using System.Collections.Generic;

namespace Star
{
    // Token: 0x0200051E RID: 1310
    public abstract class IPathFinding
    {
        // Token: 0x170001A6 RID: 422
        // (get) Token: 0x06001A02 RID: 6658 RVA: 0x000866F8 File Offset: 0x00084AF8
        // (set) Token: 0x06001A03 RID: 6659 RVA: 0x00086700 File Offset: 0x00084B00
        public bool Finding { get; set; }

        // Token: 0x170001A7 RID: 423
        // (get) Token: 0x06001A04 RID: 6660 RVA: 0x00086709 File Offset: 0x00084B09
        // (set) Token: 0x06001A05 RID: 6661 RVA: 0x00086711 File Offset: 0x00084B11
        public int SleepTime { get; set; }

        // Token: 0x170001A8 RID: 424
        // (get) Token: 0x06001A06 RID: 6662 RVA: 0x0008671A File Offset: 0x00084B1A
        // (set) Token: 0x06001A07 RID: 6663 RVA: 0x00086722 File Offset: 0x00084B22
        public int FindingLimitCost { get; set; }

        // Token: 0x170001A9 RID: 425
        // (get) Token: 0x06001A08 RID: 6664 RVA: 0x0008672B File Offset: 0x00084B2B
        // (set) Token: 0x06001A09 RID: 6665 RVA: 0x00086733 File Offset: 0x00084B33
        public int UsableCost { get; set; }

        // Token: 0x170001AA RID: 426
        // (get) Token: 0x06001A0A RID: 6666 RVA: 0x0008673C File Offset: 0x00084B3C
        public List<Vector2> Path
        {
            get
            {
                return this.m_Path;
            }
        }

        // Token: 0x06001A0B RID: 6667 RVA: 0x00086744 File Offset: 0x00084B44
        /// <summary>set(Cost)Data</summary>
        public virtual void SetData(int[,] costData)
        {
            this.m_CostData = costData;
        }

        // Token: 0x06001A0C RID: 6668
        public abstract List<Vector2> Find(Vector2 v1, Vector2 v2);

        // Token: 0x06001A0D RID: 6669
        // public abstract void FindAsync(Vector2 v1, Vector2 v2);

        // Token: 0x06001A0E RID: 6670
        // public abstract void Abort();

        // Token: 0x04002097 RID: 8343
        protected List<Vector2> m_Path;

        // Token: 0x04002098 RID: 8344
        protected int[,] m_CostData;
    }
}
