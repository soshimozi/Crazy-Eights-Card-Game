using System;
using System.Collections.Generic;

namespace CrazyEightsCardLib
{
	public abstract class Hand
	{
		private List<Card> _cards = new List<Card>();
		
		public enum PlayDirection
		{
			Forward,
			Reverse
		}

		/// <summary>
		/// Gets the cards.
		/// </summary>
		public List<Card> Cards
		{
			get
			{
				return this._cards;
			}
		}

		/// <summary>
		/// Gets the rank count.
		/// </summary>
		/// <param name="rank">The rank.</param>
		/// <returns></returns>
		public abstract int GetRankCount(CardRank rank);

		/// <summary>
		/// Gets the suit count.
		/// </summary>
		/// <param name="suit">The suit.</param>
		/// <returns></returns>
		public abstract int GetSuitCount(CardSuit suit);

		/// <summary>
		/// Finds the rank.
		/// </summary>
		/// <param name="cardRank">The card rank.</param>
		/// <returns></returns>
		public abstract Card FindRank(CardRank cardRank);

		/// <summary>
		/// Finds the suit.
		/// </summary>
		/// <param name="cardSuit">The card suit.</param>
		/// <returns></returns>
		public abstract Card FindSuit(CardSuit cardSuit);
	}
}
