using System;
using System.Collections.Generic;
namespace CrazyEightsCardLib
{
	public class CrazyEightDeckManager
	{
		private Deck _deck;
		private List<Card> _discardPile = new List<Card>();
		private List<Card> _table = new List<Card>();
		private int _numCards;
		private int currentDrawCard;

		/// <summary>
		/// Gets a value indicating whether this instance is empty.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
		/// </value>
		public bool IsEmpty
		{
			get
			{
				return this.currentDrawCard >= this._numCards;
			}
		}

		/// <summary>
		/// Gets the table.
		/// </summary>
		public List<Card> Table
		{
			get
			{
				return this._table;
			}
		}

		/// <summary>
		/// Gets the cards.
		/// </summary>
		public Deck Cards
		{
			get
			{
				return this._deck;
			}
		}

		/// <summary>
		/// Gets the discard pile.
		/// </summary>
		public List<Card> DiscardPile
		{
			get
			{
				return this._discardPile;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CrazyEightDeckManager"/> class.
		/// </summary>
		/// <param name="numDecks">The num decks.</param>
		public CrazyEightDeckManager(int numDecks)
		{
			this._numCards = numDecks * 52;
			this._deck = new Deck(this._numCards);
		}

		/// <summary>
		/// Shuffles this instance.
		/// </summary>
		public void Shuffle()
		{
			this.currentDrawCard = 0;
			DiscardPile.Clear();
			Table.Clear();
			Cards.Shuffle();
		}

		/// <summary>
		/// Shuffles the specified exclusion list.
		/// </summary>
		/// <param name="exclusionList">The exclusion list.</param>
		public void Shuffle(Card[] exclusionList)
		{
			DiscardPile.Clear();
			Table.Clear();
			Cards.Shuffle(exclusionList);
			currentDrawCard = exclusionList.Length;
		}

		/// <summary>
		/// Deals the specified hands.
		/// </summary>
		/// <param name="hands">The hands.</param>
		/// <param name="cardsInHand">The cards in hand.</param>
		public void Deal(Hand[] hands, int cardsInHand)
		{
			if (hands.Length * cardsInHand > this._numCards)
			{
				throw new ArgumentOutOfRangeException("hands", "Number of hands can't exceed deck size.");
			}
			this.Table.Clear();
			for (int i = 1; i <= hands.Length * cardsInHand; i += hands.Length)
			{
				for (int j = 0; j < hands.Length; j++)
				{
					hands[j].Cards.Add(this.DrawCard());
				}
			}

			Table.Add(this.DrawCard());
		}

		private void BurnCard(Card drawCard)
		{
			// find a spot in middle of deck
			// we should put card there
			
		}

		/// <summary>
		/// Draws the card.
		/// </summary>
		/// <returns></returns>
		public Card DrawCard()
		{
			Card result = null;
			if (this.currentDrawCard < this.Cards.Size)
			{
				result = this.Cards.GetCard(this.currentDrawCard);
				this.currentDrawCard++;
			}
			else
			{
				var size = this.Cards.Size;

				// should we throw something here?
			}
			return result;
		}
	}
}
