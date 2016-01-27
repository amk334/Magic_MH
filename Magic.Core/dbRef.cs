﻿using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Core.LocalSetup;

///////////////////////////////////////////////////
/// You need a file called LocalSetup.cs which looks like this
/// 
/// namespace Magic.Core.LocalSetup
/// {
///	  class Constants
///	  {
///		  public const string currentConnectionString = CONNECTION STRING GOES HERE;
///	  }
/// }
/// 
/// ////////////////////////////////////////////////

namespace Magic.Core
{
	[System.Data.Linq.Mapping.Table(Name = "Players")]
	public class dbPlayer
	{
		[System.Data.Linq.Mapping.Column(IsPrimaryKey = true)]
		public string Name;

		public static List<dbPlayer> LoadDBPlayers()
		{
			var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);

			var playersTable = db.GetTable<dbPlayer>().ToList();
			return playersTable;
		}

		public void Save()
		{
			//var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);

			//var sqlUpdate = String.Format("UPDATE Players SET CurrentRound={0}, Rounds={1}, RoundMatches={2} WHERE Name='{3}'", currentRound, rounds, roundMatches, Name);
			//db.ExecuteCommand(sqlUpdate);

		}
	}

	[System.Data.Linq.Mapping.Table(Name = "Matches")]
	public class dbMatch
	{
		[System.Data.Linq.Mapping.Column()]
		public string Player1;
		[System.Data.Linq.Mapping.Column()]
		public string Player2;
		[System.Data.Linq.Mapping.Column()]
		public int Round;
		[System.Data.Linq.Mapping.Column()]
		public string Event;
		[System.Data.Linq.Mapping.Column()]
		public int Player1Wins;
		[System.Data.Linq.Mapping.Column()]
		public int Player2Wins;
		[System.Data.Linq.Mapping.Column()]
		public int Draws;
		[System.Data.Linq.Mapping.Column()]
		public bool InProgress;

		public dbMatch(Magic.Core.Match m)
		{
			Player1 = m.Player1Name;
			Player2 = m.Player2Name;
			Event = m.Event;
			Round = m.Round;
			Player1Wins = m.Player1Wins;
			Player2Wins = m.Player2Wins;
			Draws = m.Draws;
			InProgress = m.InProgress;
		}

		public dbMatch()
		{ }

		public void Copy(dbMatch m)
		{
			this.Player1 = m.Player1;
			this.Player2 = m.Player2;
			this.Event = m.Event;
			this.Round = m.Round;
			this.Player1Wins = m.Player1Wins;
			this.Player2Wins = m.Player2Wins;
			this.Draws = m.Draws;
			this.InProgress = m.InProgress;
		}

		public static List<dbMatch> LoadDBMatches(string mtgEvent)
		{
			var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
			var matchesTable = db.GetTable<dbMatch>().Where(m => m.Event == mtgEvent).ToList();
			return matchesTable;
		}

		public void Update()
		{
			var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
			db.ExecuteQuery<dbMatch>(String.Format("UPDATE [Magic]..[Matches] SET [Player1Wins]={0}, [Player2Wins]={1}, [Draws]={2}, [InProgress]='{3}' WHERE [Player1]='{4}' AND [Player2]='{5}' AND [Event]='{6}' AND [Round]={7}", Player1Wins, Player2Wins, Draws, InProgress, Player1, Player2, Event, Round));
			db.ExecuteQuery<dbMatch>(String.Format("UPDATE [Magic]..[Matches] SET [Player2Wins]={0}, [Player1Wins]={1}, [Draws]={2}, [InProgress]='{3}' WHERE [Player2]='{4}' AND [Player1]='{5}' AND [Event]='{6}' AND [Round]={7}", Player1Wins, Player2Wins, Draws, InProgress, Player1, Player2, Event, Round));
		}

		public void Insert()
		{
			var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
			var sql = String.Format("INSERT INTO [Magic]..[Matches](Event,Round,Player1,Player2,Player1Wins,Player2Wins,Draws,InProgress) VALUES('{0}',{1},'{2}','{3}',{4},{5},{6},0)", Event, Round, Player1, Player2, Player1Wins, Player2Wins, Draws);
			db.ExecuteQuery<dbMatch>(sql);
		}

		public void Save()
		{
			if (MatchExists(Event, Round, Player1, Player2))
				Update();
			else
				Insert();

		}

		internal static bool IsMatch(dbMatch checkedMatch, string player1name, string player2name, string eventname, int round)
		{
			if (eventname == checkedMatch.Event && round == checkedMatch.Round)
			{
				if (player1name == checkedMatch.Player1 && player2name == checkedMatch.Player2)
					return true;
				else if (player1name == checkedMatch.Player2 && player2name == checkedMatch.Player1)
					return true;
			}

			return true;
		}

		internal bool MatchExists(string eventName, int round, string p1, string p2)
		{
			var allMatches = dbMatch.LoadDBMatches(eventName);
			var eventMatches = allMatches.Where(m => m.Event == eventName).ToList();
			var roundMatches = eventMatches.Where(m => m.Round == round).ToList();

			var matchesAsP1 = roundMatches.Where(m => m.Player1 == p1 && m.Player2 == p2);
			if (matchesAsP1.Any())
			{
				return true;
			}
			else
			{
				var matchesAsP2 = roundMatches.Where(m => m.Player1 == p2 && m.Player2 == p1);
				if (matchesAsP2.Any())
				{
					return true;
				}
			}
			return false;
		}

		public bool Read(string eventName, int round, string p1, string p2)
		{
			var allMatches = dbMatch.LoadDBMatches(eventName);
			var eventMatches = allMatches.Where(m => m.Event == eventName).ToList();
			var roundMatches = eventMatches.Where(m => m.Round == round).ToList();

			var matchesAsP1 = roundMatches.Where(m => m.Player1 == p1 && m.Player2 == p2);
			if (matchesAsP1.Any())
			{
				var foundMatch = matchesAsP1.First();
				Copy(foundMatch);
				return true;

			}
			else
			{
				var matchesAsP2 = roundMatches.Where(m => m.Player1 == p2 && m.Player2 == p1);
				if (matchesAsP2.Any())
				{
					var foundMatch = matchesAsP2.First();
					Copy(foundMatch);
					return true;
				}
			}

			return false;
		}
	}

	[Table(Name = "Events")]
	public class dbEvent
	{
		[Column()] public string Name;
		[Column()] public DateTime StartDate;
		[Column()] public DateTime RoundEndDate;
		[Column()] public Int32 Rounds;
		[Column()] public Int32 CurrentRound;
		[Column()] public Int32 RoundMatches;
		[Column()] public bool Locked;
		private string dbName;

		public static dbEvent LoadDBEvent(string eventName)
		{
			var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
			var result = db.GetTable<dbEvent>().First(e => e.Name == eventName);
			result.dbName = result.Name;
			return result;
		}

		public static List<dbEvent> LoadAllDBEvents()
		{
			var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
			var results = db.GetTable<dbEvent>().ToList();
			results.ForEach(r=>r.dbName=r.Name);
			return results;
		}

		public void Create()
		{
			var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);

			var sqlCreate = "INSERT INTO Events (Name, CurrentRound, Rounds, RoundMatches, Locked, StartDate, RoundEndDate)";
			var sqlValues = $"VALUES ('{Name}', {CurrentRound}, {Rounds}, {RoundMatches}, {Convert.ToInt32(Locked)}, '{StartDate}', '{RoundEndDate}')";
			var fullSql = sqlCreate + sqlValues;

			try
			{
				db.ExecuteCommand(fullSql);
				dbName = Name;
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		public void Update()
		{
			var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);

			var sqlUpdate = $"UPDATE Events SET Name='{Name}', CurrentRound={CurrentRound}, Rounds={Rounds}, RoundMatches={RoundMatches}, Locked={Convert.ToInt32(Locked)}, StartDate='{StartDate}', RoundEndDate='{RoundEndDate}' WHERE Name='{dbName}'";

			try
			{
				db.ExecuteCommand(sqlUpdate);
				dbName = Name;
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		public void AddPlayer(dbPlayer dbPlayer)
		{
			var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);

			var sqlAddPlayerToPlayers = $"IF NOT EXISTS(SELECT * FROM Players WHERE Players.Name = '{0}') INSERT INTO Players(Name) VALUES('{0}')";
			var sqlAddPlayerToEvent = $"IF NOT EXISTS(SELECT * FROM EventPlayers WHERE Player={0} AND EventName={1}) INSERT INTO EventPlayers(Player, EventName) VALUES({0}, {1})";

			try
			{
				db.ExecuteCommand(sqlAddPlayerToPlayers, dbPlayer.Name);
				db.ExecuteCommand(sqlAddPlayerToEvent, dbPlayer.Name, Name);
			}
			catch(Exception ex)
			{
				throw;
			}
    }
	}

	[Table(Name = "EventPlayers")]
	public class dbEventPlayers
	{
		[Column()]
		public string EventName;

		[Column()]
		public string Player;

		[Column()]
		public int Dropped;

		public static List<dbEventPlayers> LoadDBEventPlayers(string eventName)
		{
			var db = new System.Data.Linq.DataContext(Constants.currentConnectionString);
			return db.GetTable<dbEventPlayers>().Where(ep => ep.EventName == eventName).ToList();
		}
	}
}

