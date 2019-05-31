﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Magic.Domain;

namespace Magic.Core.Tests
{
	public class PlayerTests
	{
		[Test]
		public void TestOpponents()
		{
			var testPlayerName = "sut";
			var _sut = new Player(testPlayerName);

			var opp1 = new Player("Result1");
			var opp2 = new Player("Result2");
			var opp3 = new Player("Result3");
			var opp4 = new Player("Result4");

			var match1 = new Match(_sut, testPlayerName, opp1, opp1.Name, "TEST", 1, 0, 0, 0);
			var match2 = new Match(_sut, testPlayerName, opp2, opp2.Name, "TEST", 1, 0, 0, 0);
			var match3 = new Match(opp3, opp3.Name, _sut, testPlayerName, "TEST", 1, 0, 0, 0);
			var match4 = new Match(opp4, opp4.Name, _sut, testPlayerName, "TEST", 1, 0, 0, 0);
			_sut.Matches = new List<Match> { match1, match2, match3, match4 };

			var results = _sut.Opponents();

			Assert.AreEqual(4, results.Count());

			Assert.Contains(opp1, results);
			Assert.Contains(opp2, results);
			Assert.Contains(opp3, results);
			Assert.Contains(opp4, results);
		}

		private bool floatComparison(float sourceA, float sourceB, float marginOfError)
		{
			float difference = Math.Abs(sourceA - sourceB);
			return difference < marginOfError;
		}

		[Test]
		public void TestMWP()
		{
			var testPlayerName = "sut";
			var _sut = new Player(testPlayerName);

			var opp1 = new Player("Result1");
			var opp2 = new Player("Result2");
			var opp3 = new Player("Result3");
			var opp4 = new Player("Result4");

			var match1 = new Match(_sut, testPlayerName, opp1, opp1.Name, "TEST", 1, 2, 0, 0); // sut wins 2-0
			var match2 = new Match(_sut, testPlayerName, opp2, opp2.Name, "TEST", 1, 0, 2, 0); // sut loses 0-2
			var match3 = new Match(opp3, opp3.Name, _sut, testPlayerName, "TEST", 1, 1, 1, 0); // sut draws 1-1
			var match4 = new Match(opp4, opp4.Name, _sut, testPlayerName, "TEST", 1, 1, 2, 0); // sut wins 2-1
			_sut.Matches = new List<Match> { match1, match2, match3, match4 };

			var result = _sut.MWP(1);

			Assert.AreEqual(true, floatComparison(result, 58.3f, 0.1f));
		}
		[Test]
		public void TestGWP()
		{
			var testPlayerName = "sut";
			var _sut = new Player(testPlayerName);

			var opp1 = new Player("Result1");
			var opp2 = new Player("Result2");
			var opp3 = new Player("Result3");
			var opp4 = new Player("Result4");

			var match1 = new Match(_sut, testPlayerName, opp1, opp1.Name, "TEST", 1, 2, 0, 0); // sut wins 2-0
			var match2 = new Match(_sut, testPlayerName, opp2, opp2.Name, "TEST", 1, 0, 2, 0); // sut loses 0-2
			var match3 = new Match(opp3, opp3.Name, _sut, testPlayerName, "TEST", 1, 1, 1, 0); // sut draws 1-1
			var match4 = new Match(opp4, opp4.Name, _sut, testPlayerName, "TEST", 1, 1, 2, 0); // sut wins 2-1
			_sut.Matches = new List<Match> { match1, match2, match3, match4 };

			var result = _sut.GWP();

			Assert.AreEqual(true, floatComparison(result, 55.5f, 0.1f)); // 5 out of 9
		}

		[Test]
		public void TestScore()
		{
			var testPlayerName = "sut";
			var _sut = new Player(testPlayerName);

			var opp1 = new Player("Result1");
			var opp2 = new Player("Result2");
			var opp3 = new Player("Result3");
			var opp4 = new Player("Result4");

			var match1 = new Match(_sut, testPlayerName, opp1, opp1.Name, "TEST", 1, 2, 0, 0); // sut wins 2-0
			var match2 = new Match(_sut, testPlayerName, opp2, opp2.Name, "TEST", 1, 0, 2, 0); // sut loses 0-2
			var match3 = new Match(opp3, opp3.Name, _sut, testPlayerName, "TEST", 1, 1, 1, 0); // sut draws 1-1
			var match4 = new Match(opp4, opp4.Name, _sut, testPlayerName, "TEST", 1, 1, 2, 0); // sut wins 2-1
			_sut.Matches = new List<Match> { match1, match2, match3, match4 };

			var result = _sut.Score();

			Assert.AreEqual(7, result);




		}
	}
}
