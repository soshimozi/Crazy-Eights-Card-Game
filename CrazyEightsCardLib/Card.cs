using System;

namespace CrazyEightsCardLib
{
	public enum CardSuit
	{
		None = -1,
		Clubs,
		Diamonds,
		Hearts,
		Spades
	}

	public enum CardRank
	{
		Ace,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten,
		Jack,
		Queen,
		King
	}
	
	public class Card : IComparable
	{
		private CardSuit _suit;
		private CardRank _rank;
		private bool _selected;
		private int _sequence;
		private int _cardIndex;

		/// <summary>
		/// Gets the index of the card.
		/// </summary>
		/// <value>
		/// The index of the card.
		/// </value>
		public int CardIndex
		{
			get
			{
				return this._cardIndex;
			}
		}

		/// <summary>
		/// Gets the sequence number.
		/// </summary>
		public int SequenceNumber
		{
			get
			{
				return this._sequence;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Card"/> is selected.
		/// </summary>
		/// <value>
		///   <c>true</c> if selected; otherwise, <c>false</c>.
		/// </value>
		public bool Selected
		{
			get
			{
				return this._selected;
			}
			set
			{
				this._selected = value;
			}
		}

		/// <summary>
		/// Gets the suit.
		/// </summary>
		public CardSuit Suit
		{
			get
			{
				return this._suit;
			}
		}

		/// <summary>
		/// Gets the rank.
		/// </summary>
		public CardRank Rank
		{
			get
			{
				return this._rank;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Card"/> class.
		/// </summary>
		/// <param name="cardIndex">Index of the card.</param>
		/// <param name="mode">The mode.</param>
		/// <param name="sequenceNumber">The sequence number.</param>
		public Card(int cardIndex, CardMode mode, int sequenceNumber)
		{
			this._suit = GetSuitFromCardIndex(cardIndex, mode);
			this._rank = GetRankFromIndex(cardIndex, mode);
			this._sequence = sequenceNumber;
			this._cardIndex = cardIndex;
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
			Card card = (Card)o;
			if (this._suit < card.Suit)
			{
				return -1;
			}
			if (this._suit > card.Suit)
			{
				return 1;
			}
			if (this._rank < card.Rank)
			{
				return -1;
			}
			if (this._rank > card.Rank)
			{
				return 1;
			}
			return 0;
		}

		/// <summary>
		/// Toes the index of the card.
		/// </summary>
		/// <param name="suit">The suit.</param>
		/// <param name="rank">The rank.</param>
		/// <param name="mode">The mode.</param>
		/// <returns></returns>
		public static int ToCardIndex(CardSuit suit, CardRank rank, CardMode mode)
		{
			int result = -1;
			switch (mode)
			{
				case CardMode.RankCollated:
				{
					result = ((int)rank * (int)CardRank.Five + (int)suit);
					break;
				}
				case CardMode.SuitCollated:
				{
					result = ((int)CardRank.Two + (int)rank + (13 * (int)suit));
					break;
				}
				default:
				{
					result = 0;
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// Gets the index of the card.
		/// </summary>
		/// <param name="back">The back.</param>
		/// <returns></returns>
		public static int GetCardIndex(CardBack back)
		{
			return (int)back;
		}

		/// <summary>
		/// Gets the index of the suit from card.
		/// </summary>
		/// <param name="cardIndex">Index of the card.</param>
		/// <param name="mode">The mode.</param>
		/// <returns></returns>
		public static CardSuit GetSuitFromCardIndex(int cardIndex, CardMode mode)
		{
			if (mode == CardMode.SuitCollated && cardIndex >= 1 && cardIndex <= 52)
			{
				return (CardSuit)((cardIndex - 1) / 13);
			}
			if (mode == CardMode.RankCollated && cardIndex >= 0 && cardIndex <= 51)
			{
				return (CardSuit)(cardIndex % 4);
			}
			throw new ApplicationException("Suite only valid to SuitCollated, RankCollated modes.");
		}

		/// <summary>
		/// Gets the index of the rank from.
		/// </summary>
		/// <param name="cardIndex">Index of the card.</param>
		/// <param name="mode">The mode.</param>
		/// <returns></returns>
		public static CardRank GetRankFromIndex(int cardIndex, CardMode mode)
		{
			if (mode == CardMode.SuitCollated && cardIndex >= 1 && cardIndex <= 52)
			{
				return (CardRank)((cardIndex - 1) % 13);
			}
			if (mode == CardMode.RankCollated && cardIndex >= 0 && cardIndex <= 51)
			{
				return (CardRank)(cardIndex / 4);
			}
			throw new ApplicationException("Rank only valid to SuitCollated, RankCollated modes.");
		}
	}
}
