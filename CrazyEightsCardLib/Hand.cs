using System;
using System.Collections.Generic;

namespace CrazyEightsCardLib
{
	public abstract class Hand
	{
        public enum PlayDirection
		{
			Forward,
			Reverse
		}

		/// <summary>
		/// Gets the cards.
		/// </summary>
		public List<Card> Cards { get; } = new List<Card>();

        /// <summary>
		/// Gets the rank count.
		/// </summary>
		/// <param name="rank">The rank.</param>
		/// <returns></returns>
		public abstract int GetRankCount(SpecialCard rank);

		/// <summary>
		/// Gets the suit count.
		/// </summary>
		/// <param name="suit">The suit.</param>
		/// <returns></returns>
		public abstract int GetSuitCount(CardSuit suit);

		/// <summary>
		/// Finds the rank.
		/// </summary>
		/// <param name="specialCard">The card rank.</param>
		/// <returns></returns>
		public abstract Card FindRank(SpecialCard specialCard);

		/// <summary>
		/// Finds the suit.
		/// </summary>
		/// <param name="cardSuit">The card suit.</param>
		/// <returns></returns>
		public abstract Card FindSuit(CardSuit cardSuit);
	}
}
