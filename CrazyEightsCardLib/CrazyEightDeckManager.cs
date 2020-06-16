using System;
using System.Collections.Generic;

namespace CrazyEightsCardLib
{
	public class CrazyEightDeckManager
	{
		private readonly int _numCards;
		private int _currentDrawCard;

		/// <summary>
		/// Gets a value indicating whether this instance is empty.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
		/// </value>
		public bool IsEmpty => _currentDrawCard >= _numCards;

        /// <summary>
		/// Gets the table.
		/// </summary>
		public List<Card> Table { get; } = new List<Card>();

		/// <summary>
		/// Gets the cards.
		/// </summary>
		public Deck Cards { get; }

		/// <summary>
		/// Gets the discard pile.
		/// </summary>
		public List<Card> DiscardPile { get; } = new List<Card>();

		/// <summary>
		/// Initializes a new instance of the <see cref="CrazyEightDeckManager"/> class.
		/// </summary>
		/// <param name="numDecks">The num decks.</param>
		public CrazyEightDeckManager(int numDecks)
		{
			_numCards = numDecks * 52;
			Cards = new Deck(_numCards);
		}

		/// <summary>
		/// Shuffles this instance.
		/// </summary>
		public void Shuffle()
		{
			_currentDrawCard = 0;
			DiscardPile.Clear();
			Table.Clear();
			Cards.Shuffle();
		}

        /// <summary>
		/// Deals the specified hands.
		/// </summary>
		/// <param name="hands">The hands.</param>
		/// <param name="cardsInHand">The cards in hand.</param>
		public void Deal(Hand[] hands, int cardsInHand)
		{
			if (hands.Length * cardsInHand > _numCards)
			{
				throw new ArgumentOutOfRangeException(nameof(hands), "Number of hands can't exceed deck size.");
			}
			Table.Clear();
			for (var i = 1; i <= hands.Length * cardsInHand; i += hands.Length)
            {
                foreach (var t in hands)
                {
                    t.Cards.Add(DrawCard());
                }
            }

			Table.Add(DrawCard());
		}

        /// <summary>
		/// Draws the card.
		/// </summary>
		/// <returns></returns>
		public Card DrawCard()
		{
            if (_currentDrawCard >= Cards.Size) return null;
            var result = Cards.GetCard(_currentDrawCard);
            _currentDrawCard++;

            return result;
		}
	}
}
