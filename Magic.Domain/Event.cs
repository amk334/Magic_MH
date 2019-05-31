﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	[DebuggerDisplay("{name} r:{CurrentRound} p:{Players.Count} m:{Matches.Count}")]
	public class Event
	{
		public List<Player> Players = new List<Player>();
		public List<Match> Matches = new List<Match>();
		public List<dbRoundPrize> RoundPrizes = new List<dbRoundPrize>();
		public string name = "";
		public int rounds = 1;
		public int CurrentRound = 1;
		public int RoundMatches = 4;
		public DateTime RoundEndDate = DateTime.Today;
		public DateTime EventStartDate = DateTime.Today;
		
		private bool _locked = false;

		public dbEvent myDbEvent = null;

		public bool Locked(int round)
		{
			if (_locked)
				return true;
			if (round < CurrentRound)
				return true;

			return false;
		}

		public int MaxMatchesPlayed(int round)
		{
			try
			{
				return Players.Max(p => p.Matches.Count(m => m.Round == round));
			}
			catch(Exception)
			{
				return 4;
			}
			
		}

		public int RoundMatchCount(int round)
		{
			try
			{
				if (round > 0)
				{
					return Players.Max(p => p.Matches.Count(m => m.Round == round));
				}
				else
				{
					return Players.Max(p => p.Matches.Count());
				}
			}
			catch(Exception)
			{
				return 0;
			}
			
		}

		public int TotalMatches(int round)
		{
			if (round > 0)
			{
				return Matches.Count(m => m.Round == round);
			}
			else
			{
				return Matches.Count();
			}
		}

		public int TotalMatchesPlayed(int round)
		{
			return Players.Sum(p => p.matchesCompleted(round));
		}
	}
}
