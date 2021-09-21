﻿using System;
using System.Collections.Generic;
using XUtliPoolLib;

namespace XMainClient
{
	// Token: 0x020008CA RID: 2250
	internal class ArtifactSuitMgr
	{
		// Token: 0x17002A95 RID: 10901
		// (get) Token: 0x0600881F RID: 34847 RVA: 0x00118D88 File Offset: 0x00116F88
		public List<ArtifactSuit> Suits
		{
			get
			{
				return this.m_Suits;
			}
		}

		// Token: 0x06008820 RID: 34848 RVA: 0x00118DA0 File Offset: 0x00116FA0
		public ArtifactSuitMgr(ArtifactSuitTable.RowData[] datas)
		{
			foreach (ArtifactSuitTable.RowData rowData in datas)
			{
				ArtifactSuit artifactSuit = new ArtifactSuit();
				artifactSuit.Id = rowData.ArtifactSuitID;
				artifactSuit.Name = rowData.Name;
				artifactSuit.Level = rowData.Level;
				artifactSuit.SuitId = rowData.ArtifactSuitID;
				artifactSuit.ElementType = rowData.ElementType;
				artifactSuit.SuitQuality = rowData.SuitQuality;
				artifactSuit.IsCreateShow = (rowData.IsCreateShow != 0);
				artifactSuit.effects[1] = default(SeqListRef<uint>);
				artifactSuit.effects[2] = ((rowData.Effect2.Count > 0) ? rowData.Effect2 : default(SeqListRef<uint>));
				artifactSuit.effects[3] = ((rowData.Effect3.Count > 0) ? rowData.Effect3 : default(SeqListRef<uint>));
				artifactSuit.effects[4] = ((rowData.Effect4.Count > 0) ? rowData.Effect4 : default(SeqListRef<uint>));
				artifactSuit.effects[5] = ((rowData.Effect5.Count > 0) ? rowData.Effect5 : default(SeqListRef<uint>));
				artifactSuit.effects[6] = ((rowData.Effect6.Count > 0) ? rowData.Effect6 : default(SeqListRef<uint>));
				for (int j = 0; j < artifactSuit.effects.Length; j++)
				{
					bool flag = artifactSuit.effects[j].Count > 0;
					if (flag)
					{
						artifactSuit.activeCount.Add(j);
						artifactSuit.MaxSuitEffectCount = (uint)j;
					}
				}
				this.m_Suits.Add(artifactSuit);
			}
		}

		// Token: 0x06008821 RID: 34849 RVA: 0x00118F8C File Offset: 0x0011718C
		public ArtifactSuit GetSuitByArtifactId(uint artifactId)
		{
			for (int i = 0; i < this.m_Suits.Count; i++)
			{
				bool flag = this.m_Suits[i].Artifacts.Contains(artifactId);
				if (flag)
				{
					return this.m_Suits[i];
				}
			}
			return null;
		}

		// Token: 0x06008822 RID: 34850 RVA: 0x00118FE8 File Offset: 0x001171E8
		public ArtifactSuit GetSuitBySuitId(uint suitId)
		{
			for (int i = 0; i < this.m_Suits.Count; i++)
			{
				bool flag = this.m_Suits[i].Id == suitId;
				if (flag)
				{
					return this.m_Suits[i];
				}
			}
			return null;
		}

		// Token: 0x06008823 RID: 34851 RVA: 0x00119040 File Offset: 0x00117240
		public bool WillChangeEquipedCount(int suitItemID, int newItemID)
		{
			ArtifactSuit suitByArtifactId = this.GetSuitByArtifactId((uint)suitItemID);
			bool flag = suitByArtifactId == null;
			return !flag && suitByArtifactId.WillChangeEquipedCount(newItemID, suitItemID);
		}

		// Token: 0x04002AF2 RID: 10994
		private List<ArtifactSuit> m_Suits = new List<ArtifactSuit>();
	}
}
