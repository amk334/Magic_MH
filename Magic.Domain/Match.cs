﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	[DebuggerDisplay("{Player1.Name}({Player1.ID}) {Player1Wins}-{Player2Wins} {Player2.Name}({Player2.ID})")]
	public class Match
	{
		public Player Player1;
		//private string _player1Name;
		//public string Player1Name
		//{
		//	get { return Player1 != null ? Player1.Name : _player1Name; }
		//}
		public Player Player2;
		//private string _player2Name;
		//public string Player2Name
		//{
		//	get { return Player2 != null ? Player2.Name : _player2Name; }
		//}
		public int Round;
		public string Event;
		public int Player1Wins;
		public int Player2Wins;
		public int Draws;
		public bool IsPreview;

		public int Player1ID;
		public int Player2ID;

		public dbMatch myDbMatch;

		public Match()
		{ }

		public Match(int p1, int p2, string eventName, int round, int p1wins, int p2wins, int draws)
		{
			Player1ID = p1;
			Player2ID = p2;

			Event = eventName;
			Round = round;
			Player1Wins = p1wins;
			Player2Wins = p2wins;
			Draws = draws;
		}

		public Match(Player p1, Player p2, int p1id, int p2id, string eventName, int round, int p1wins, int p2wins, int draws)
		{
			Player1 = p1;
			//_player1Name = player1name;
			Player2 = p2;
			//_player2Name = player2name;
			Player1ID = p1id;
			Player2ID = p2id;
			Event = eventName;
			Round = round;
			Player1Wins = p1wins;
			Player2Wins = p2wins;
			Draws = draws;

			//if (String.IsNullOrEmpty(_player1Name))
			//	throw new Exception();
			//if (String.IsNullOrEmpty(_player2Name))
			//	throw new Exception();

			if (Player1 == null)
				throw new Exception();
			if (Player2 == null)
				throw new Exception();
		}

		public Match(dbMatch m)
						: this(m.Player1ID, m.Player2ID, m.Event, m.Round, m.Player1Wins, m.Player2Wins, m.Draws)
		{
			myDbMatch = m;
		}

		public void Copy(Match m)
		{
			Player1 = m.Player1;
			//_player1Name = m._player1Name;
			Player2 = m.Player2;
			//_player2Name = m._player2Name;
			Event = m.Event;
			Round = m.Round;
			Player1Wins = m.Player1Wins;
			Player2Wins = m.Player2Wins;
			Draws = m.Draws;

			//if (String.IsNullOrEmpty(_player1Name))
			//	throw new Exception();
			//if (String.IsNullOrEmpty(_player2Name))
			//	throw new Exception();

			if (Player1 == null)
				throw new Exception();
			if (Player2 == null)
				throw new Exception();
		}

		public Match Flipped()
		{
			//return new Match(p1id: Player2ID, p2id: Player1ID, p1: Player2, player1name: Player2Name, p2: Player1, player2name: Player1Name, p1wins: Player2Wins, p2wins: Player1Wins, eventName: Event, round: Round, draws: Draws);
			return new Match(p1id: Player2ID, p2id: Player1ID, p1: Player2, p2: Player1, p1wins: Player2Wins, p2wins: Player1Wins, eventName: Event, round: Round, draws: Draws);
		}

		public Match WithPlayerOneAs(int playerID)
		{
			if (Player1ID == playerID)
				return this;
			else if (Player2ID == playerID)
				return this.Flipped();
			else
				throw new InvalidOperationException("Bad Parameter for Flipped(): Name did not match either player in match");
		}

		public bool DidPlayerWin(int playerID)
		{
			if (Player1ID == playerID)
			{
				return Player1Wins > Player2Wins;
			}
			else if (Player2ID == playerID)
			{
				return Player2Wins > Player1Wins;
			}
			else
			{
				throw new Exception("Player not in match");
			}
		}

		public void SetPlayerOneTo(int playerID)
		{
			Copy(WithPlayerOneAs(playerID));
		}
	}
}
