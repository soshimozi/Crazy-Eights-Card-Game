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
			return Cards.FirstOrDefault(c => c.Rank == SpecialCard.WildCard) != null;
		}

		/// <summary>
		/// Finds the eight.
		/// </summary>
		/// <returns></returns>
		public Card FindEight()
		{
			return Cards
                .FirstOrDefault(c => c.Rank == SpecialCard.WildCard);
		}

		/// <summary>
		/// Ranks the count.
		/// </summary>
		/// <param name="rank">The rank.</param>
		/// <returns></returns>
		public override int GetRankCount(SpecialCard rank)
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
		/// <param name="specialCard">The card rank.</param>
		/// <returns></returns>
		public override Card FindRank(SpecialCard specialCard)
		{
			return Cards
                .FirstOrDefault(c => c.Rank == specialCard);
		}

		/// <summary>
		/// Finds the suit.
		/// </summary>
		/// <param name="cardSuit">The card suit.</param>
		/// <returns></returns>
		public override Card FindSuit(CardSuit cardSuit)
		{
			return Cards
                .FirstOrDefault(c => c.Suit == cardSuit);
		}
	}
}
