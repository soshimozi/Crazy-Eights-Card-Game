using System;
using System.Security.Cryptography;

namespace CrazyEightsCardLib
{
	public class Deck
	{
		private readonly int[] _cardArray;
		private readonly int _size;

        public int Size => _cardArray.Length;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public Deck(int size)
		{
			_cardArray = new int[size];
			for (var i = 0; i < size; i++)
			{
				_cardArray[i] = i;
			}
			_size = size;
        }

        /// <summary>
		/// Shuffles this instance.
		/// </summary>
		public void Shuffle()
		{
            for (var i = 0; i < 5; i++)
            {
                var n = _cardArray.Length;
				while(n > 1)
                {
                    // Pick random element to swap.
                    var k = GetRandomNumber(n); // 0 <= j <= i-1
                    n--;

                    // Swap.
                    var tmp = _cardArray[k];
                    _cardArray[k] = _cardArray[n];
                    _cardArray[n] = tmp;
                }
            }
        }

        private int GetRandomNumber(int max)
        {
            return GetRandomNumber(0, max);
        }

        private static int GetRandomNumber(int min, int max)
        {
            var rnd = GetRandomNumber();
            return (int)(rnd / (float)uint.MaxValue * (max - min) + min);
        }

        private static uint GetRandomNumber()
        {
            using (var rg = new RNGCryptoServiceProvider())
            {
                var rno = new byte[4];
                rg.GetBytes(rno);
                return BitConverter.ToUInt32(rno, 0);
            }
		}

		/// <summary>
		/// Gets the card.
		/// </summary>
		/// <param name="arrayNum">The array num.</param>
		/// <returns></returns>
		public Card GetCard(int arrayNum)
		{
			if (arrayNum >= 0 && arrayNum <= _size - 1)
			{
				return new Card(_cardArray[arrayNum] % 52, CardMode.RankCollated, _cardArray[arrayNum]);
			}
			throw new ArgumentOutOfRangeException(nameof(arrayNum), arrayNum, $"Value must be between 0 and {_size}.");
		}
    }
}
