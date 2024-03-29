﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;
using ProtoBuf;

namespace KKSG
{

	[ProtoContract(Name = "HorseRank")]
	[Serializable]
	public class HorseRank : IExtensible
	{

		[ProtoMember(1, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
		public uint rank
		{
			get
			{
				return this._rank ?? 0U;
			}
			set
			{
				this._rank = new uint?(value);
			}
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool rankSpecified
		{
			get
			{
				return this._rank != null;
			}
			set
			{
				bool flag = value == (this._rank == null);
				if (flag)
				{
					this._rank = (value ? new uint?(this.rank) : null);
				}
			}
		}

		private bool ShouldSerializerank()
		{
			return this.rankSpecified;
		}

		private void Resetrank()
		{
			this.rankSpecified = false;
		}

		[ProtoMember(2, IsRequired = false, Name = "turns", DataFormat = DataFormat.TwosComplement)]
		public uint turns
		{
			get
			{
				return this._turns ?? 0U;
			}
			set
			{
				this._turns = new uint?(value);
			}
		}

		[XmlIgnore]
		[Browsable(false)]
		public bool turnsSpecified
		{
			get
			{
				return this._turns != null;
			}
			set
			{
				bool flag = value == (this._turns == null);
				if (flag)
				{
					this._turns = (value ? new uint?(this.turns) : null);
				}
			}
		}

		private bool ShouldSerializeturns()
		{
			return this.turnsSpecified;
		}

		private void Resetturns()
		{
			this.turnsSpecified = false;
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		private uint? _rank;

		private uint? _turns;

		private IExtension extensionObject;
	}
}
