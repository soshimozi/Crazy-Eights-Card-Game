using System;
using System.Collections.Generic;
using System.Text;
using CrazyEightsCardLib;

namespace CrazyEights
{
    public class GameData
    {
        private CrazyEightDeckManager deckManager;
        private CrazyEightsHand playerHand = new CrazyEightsHand();
        private CrazyEightsHand[] _opponentsHand;

        public GameData(int opponents, int numDecks)
        {
            _opponentsHand = new CrazyEightsHand[opponents];
            for (int i = 0; i < opponents; i++)
            {
                _opponentsHand[i] = new CrazyEightsHand();
            }

            deckManager = new CrazyEightDeckManager(numDecks);
        }

        public CrazyEightsHand[] OpponentsHands
        {
            get { return _opponentsHand; }
            set { _opponentsHand = value; }
        }

        public CrazyEightDeckManager DeckManager
        {
            get { return deckManager; }
            set { deckManager = value; }
        }

        public CrazyEightsHand PlayerHand
        {
            get { return playerHand; }
            set { playerHand = value; }
        }

    }
}
