using System;
namespace CrazyEightsCardLib
{
	public class Deck
	{
		private int[] CardArray;
		private int _size;
		private int checkSequence;

		public int Size
		{
			get { return CardArray.Length;  }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Deck"/> class.
		/// </summary>
		/// <param name="size">The size.</param>
		public Deck(int size)
		{
			this.CardArray = new int[size];
			for (int i = 0; i < size; i++)
			{
				this.CardArray[i] = i;
			}
			this._size = size;
		}

		private Random _random = new Random();

		/// <summary>
		/// Shuffles this instance.
		/// </summary>
		public void Shuffle()
		{
			//int[] array = new int[this._size];
			//bool[] array2 = new bool[this._size];
			//for (int i = 0; i < 10; i++)
			//{
			//    for (int j = 0; j < this._size; j++)
			//    {
			//        array2[j] = false;
			//    }
			//    Random random = new Random();
			//    int k = 0;
			//    while (k < this._size)
			//    {
			//        int num = random.Next(0, this._size);
			//        if (!array2[num])
			//        {
			//            array[k] = num;
			//            array2[num] = true;
			//            k++;
			//        }
			//    }
			//}
			//this.CardArray = array;

			var random = _random;
			for (int i = CardArray.Length; i > 1; i--)
			{
				// Pick random element to swap.
				int j = random.Next(i); // 0 <= j <= i-1

				// Swap.
				int tmp = CardArray[j];
				CardArray[j] = CardArray[i - 1];
				CardArray[i - 1] = tmp;
			}
		}

		/// <summary>
		/// Gets the card.
		/// </summary>
		/// <param name="arrayNum">The array num.</param>
		/// <returns></returns>
		public Card GetCard(int arrayNum)
		{
			if (arrayNum >= 0 && arrayNum <= this._size - 1)
			{
				return new Card(this.CardArray[arrayNum] % 52, CardMode.RankCollated, this.CardArray[arrayNum]);
			}
			throw new ArgumentOutOfRangeException("arrayNum", arrayNum, "Value must be between 0 and " + this._size + ".");
		}

		/// <summary>
		/// Shuffles the specified exclusion list.
		/// </summary>
		/// <param name="exclusionList">The exclusion list.</param>
		public void Shuffle(Card[] exclusionList)
		{
			this.Shuffle();

			int num = 0;
			for (int i = 0; i < exclusionList.Length; i++)
			{
				Card card = exclusionList[i];
				this.checkSequence = card.SequenceNumber;
				int num2 = Array.FindIndex<int>(this.CardArray, new Predicate<int>(this.IsSame));
				if (num2 != -1)
				{
					int num3 = this.CardArray[num2];
					this.CardArray[num2] = this.CardArray[num];
					this.CardArray[num] = num3;
					num++;
				}
			}
		}

		/// <summary>
		/// Determines whether the specified sequence is same.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <returns>
		///   <c>true</c> if the specified sequence is same; otherwise, <c>false</c>.
		/// </returns>
		public bool IsSame(int sequence)
		{
			return sequence == this.checkSequence;
		}
	}
}
