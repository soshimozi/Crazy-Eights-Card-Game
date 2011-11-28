using System;
using System.Linq;
using System.Collections.Generic;

namespace CrazyEightsCardLib
{
	public class CrazyEightsHand : Hand
	{
		/// <summary>
		/// Determines whether this instance has eight.
		/// </summary>
		/// <returns>
		///   <c>true</c> if this instance has eight; otherwise, <c>false</c>.
		/// </returns>
		public bool HasEight()
		{
			return Cards
                .Where(c => c.Rank == CardRank.Eight).FirstOrDefault() == null ? false : true;
		}

		/// <summary>
		/// Finds the eight.
		/// </summary>
		/// <returns></returns>
		public Card FindEight()
		{
			return Cards
				.Where(c => c.Rank == CardRank.Eight)
				.FirstOrDefault();
		}

		/// <summary>
		/// Ranks the count.
		/// </summary>
		/// <param name="rank">The rank.</param>
		/// <returns></returns>
		public override int GetRankCount(CardRank rank)
		{
			return Cards
				.Count(c => c.Rank == rank);
		}

		/// <summary>
		/// Suits the count.
		/// </summary>
		/// <param name="suit">The suit.</param>
		/// <returns></returns>
		public override int GetSuitCount(CardSuit suit)
		{
			return Cards
				.Count(c => c.Suit == suit);
		}

		/// <summary>
		/// Finds the rank.
		/// </summary>
		/// <param name="cardRank">The card rank.</param>
		/// <returns></returns>
		public override Card FindRank(CardRank cardRank)
		{
			return Cards
				.Where(c => c.Rank == cardRank)
				.FirstOrDefault();
		}

		/// <summary>
		/// Finds the suit.
		/// </summary>
		/// <param name="cardSuit">The card suit.</param>
		/// <returns></returns>
		public override Card FindSuit(CardSuit cardSuit)
		{
			return Cards
				.Where(c => c.Suit == cardSuit)
				.FirstOrDefault();
		}
	}
}
