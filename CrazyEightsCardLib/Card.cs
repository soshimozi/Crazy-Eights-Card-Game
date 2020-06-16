using System;

namespace CrazyEightsCardLib
{
    public class Card : IComparable
	{
		/// <summary>
		/// Gets the index of the card.
		/// </summary>
		/// <value>
		/// The index of the card.
		/// </value>
		public int CardIndex { get; }

		/// <summary>
		/// Gets the sequence number.
		/// </summary>
		public int SequenceNumber { get; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Card"/> is selected.
		/// </summary>
		/// <value>
		///   <c>true</c> if selected; otherwise, <c>false</c>.
		/// </value>
		public bool Selected { get; set; }

		/// <summary>
		/// Gets the suit.
		/// </summary>
		public CardSuit Suit { get; }

		/// <summary>
		/// Gets the rank.
		/// </summary>
		public SpecialCard Rank { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Card"/> class.
		/// </summary>
		/// <param name="cardIndex">Index of the card.</param>
		/// <param name="mode">The mode.</param>
		/// <param name="sequenceNumber">The sequence number.</param>
		public Card(int cardIndex, CardMode mode, int sequenceNumber)
		{
			this.Suit = GetSuitFromCardIndex(cardIndex, mode);
			this.Rank = GetRankFromIndex(cardIndex, mode);
			this.SequenceNumber = sequenceNumber;
			this.CardIndex = cardIndex;
		}

		/// <summary>
		/// Compares to.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		public int CompareTo(object o)
		{
			if (!(o is Card))
			{
				throw new ArgumentException("Object is not a Card");
			}
			var card = (Card)o;
			if (this.Suit < card.Suit)
			{
				return -1;
			}
			if (this.Suit > card.Suit)
			{
				return 1;
			}
			if (this.Rank < card.Rank)
			{
				return -1;
			}
			return this.Rank > card.Rank ? 1 : 0;
        }

        /// <summary>
		/// Gets the index of the suit from card.
		/// </summary>
		/// <param name="cardIndex">Index of the card.</param>
		/// <param name="mode">The mode.</param>
		/// <returns></returns>
		public static CardSuit GetSuitFromCardIndex(int cardIndex, CardMode mode)
		{
			switch (mode)
            {
                case CardMode.SuitCollated when cardIndex >= 1 && cardIndex <= 52:
                    return (CardSuit)((cardIndex - 1) / 13);
                case CardMode.RankCollated when cardIndex >= 0 && cardIndex <= 51:
                    return (CardSuit)(cardIndex % 4);
                case CardMode.Highlight:
                    break;
                case CardMode.Ghost:
                    break;
                case CardMode.Remove:
                    break;
                case CardMode.InvisibleGhost:
                    break;
                case CardMode.DeckX:
                    break;
                case CardMode.DeckO:
                    break;
                default:
                    throw new ApplicationException("Suite only valid to SuitCollated, RankCollated modes.");
            }

            return 0;
        }

		/// <summary>
		/// Gets the index of the rank from.
		/// </summary>
		/// <param name="cardIndex">Index of the card.</param>
		/// <param name="mode">The mode.</param>
		/// <returns></returns>
		public static SpecialCard GetRankFromIndex(int cardIndex, CardMode mode)
		{
			switch (mode)
            {
                case CardMode.SuitCollated when cardIndex >= 1 && cardIndex <= 52:
                    return (SpecialCard)((cardIndex - 1) % 13);
                case CardMode.RankCollated when cardIndex >= 0 && cardIndex <= 51:
                    return (SpecialCard)(cardIndex / 4);
                case CardMode.Highlight:
                    break;
                case CardMode.Ghost:
                    break;
                case CardMode.Remove:
                    break;
                case CardMode.InvisibleGhost:
                    break;
                case CardMode.DeckX:
                    break;
                case CardMode.DeckO:
                    break;
                default:
                    throw new ApplicationException("Rank only valid to SuitCollated, RankCollated modes.");
            }

            return 0;
        }
	}
}
