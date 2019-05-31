﻿using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	[Table(Name = "Matches")]
	public class dbMatch
	{
		[Column()]
		public int Player1ID;
		[Column()]
		public int Player2ID;
		[Column()]
		protected string Player1;
		[Column()]
		protected string Player2;
		[Column()]
		public int Round;
		[Column()]
		public string Event;
		[Column()]
		public int Player1Wins;
		[Column()]
		public int Player2Wins;
		[Column()]
		public int Draws;

		public dbMatch()
		{ }

		public void Copy(dbMatch m)
		{
			this.Player1ID = m.Player1ID;
			this.Player2ID = m.Player2ID;
			this.Player1 = m.Player1;
			this.Player2 = m.Player2;
			this.Event = m.Event;
			this.Round = m.Round;
			this.Player1Wins = m.Player1Wins;
			this.Player2Wins = m.Player2Wins;
			this.Draws = m.Draws;
		}

		public dbMatch WithPlayerOneAs(int playerID)
		{
			if(Player1ID == playerID)
			{
				var newMatch = new dbMatch();
				newMatch.Copy(this);
				return newMatch;
			}
			else if(Player2ID != playerID)
			{
				throw new ArgumentException("Player not found in match, can't set player as PlayerOne");
			}
			else
			{
				var newMatch = new dbMatch()
				{
					Player1ID = Player1ID,
					Player2ID = Player2ID,
					Player1 = Player2,
					Player2 = Player1,
					Event = Event,
					Round = Round,
					Player1Wins = Player2Wins,
					Player2Wins = Player1Wins,
					Draws = Draws
				};
				return newMatch;
			}
		}

		public bool HasWon(int playerID)
		{
			var normalizedMatch = WithPlayerOneAs(playerID);

			if ((Player1Wins + Draws >= 2) || (Player2Wins + Draws >= 2)) // Match Complete
			{
				if (Player1Wins > Player2Wins)
					return true;
			}

			return false;
		}
	}
}
