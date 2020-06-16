using CrazyEightsCardLib;

namespace CrazyEights
{
    public class GameData
    {
        public GameData(int opponents, int numDecks)
        {
            OpponentsHands = new CrazyEightsHand[opponents];
            for (var i = 0; i < opponents; i++)
            {
                OpponentsHands[i] = new CrazyEightsHand();
            }

            DeckManager = new CrazyEightDeckManager(numDecks);
        }

        public CrazyEightsHand[] OpponentsHands { get; set; }

        public CrazyEightDeckManager DeckManager { get; set; }

        public CrazyEightsHand PlayerHand { get; set; } = new CrazyEightsHand();
    }
}
