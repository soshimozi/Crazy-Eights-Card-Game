using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CrazyEightsCardLib;

namespace CrazyEights
{
    class AIManager
    {
        public class MoveInfo
        {
            public bool DrawCard { get; set; }
            public bool WildCardUsed { get; set; }
            public CardSuit OverrideSuit { get; set; }
            public Card SelectedCard { get; set; }
        }

        internal static MoveInfo EvaluateMove(CrazyEightDeckManager deckManager, CrazyEightsHand hand, CardSuit wildSuit)
        {
            return GetCardChoice(deckManager, hand, wildSuit);
        }

        private static MoveInfo GetCardChoice(CrazyEightDeckManager deckManager, CrazyEightsHand hand, CardSuit wildSuit)
        {
            // super simple rules here
            // 1. Check top card of table
            // 2. Check for Crazy 8
            // 3. Find number of rank matches
            // 4. Find number of suit matches
            // 5. if( have crazy eight ) then
            //      play crazy eight
            //    else if( rank matches > suit matches ) then
            //      play matching rank
            //    else if( suit matches > rank matches ) then
            //      play matching suit
            //    else
            //      play random matching suit or rank
            MoveInfo info = new MoveInfo();

            info.DrawCard = false;
            info.WildCardUsed = false;
            info.SelectedCard = null;

            Card topCard = deckManager.Table[deckManager.Table.Count - 1];

            CardSuit matchSuit = wildSuit == CardSuit.None ? topCard.Suit : wildSuit;
            Card eightCard = hand.FindEight();
            Card duece = hand.FindRank(CardRank.Two);

            // rules change if top card is an eight
            if (topCard.Rank == CardRank.Eight)
            {
                // if we can match rank, do so
                // that will force a play of an eight
                if (eightCard != null)
                {
                    info.SelectedCard = eightCard;
                }
                else if (hand.GetSuitCount(matchSuit) > 0)
                {
                    // get matching suit (order by rank)
                    info.SelectedCard = hand.Cards.Where(c => c.Suit == matchSuit).OrderBy(c1 => c1.Rank).FirstOrDefault();
                }
                else
                {
                    info.DrawCard = true;
                }
            }
            else if (topCard.Rank == CardRank.Two && duece != null)
            {
                // play the duece on a draw two
                info.SelectedCard = duece;
            } 
            else
            {
                // get suit count, without eights
                var suitCount = hand.Cards.Count(c => c.Suit == matchSuit && c.Rank != CardRank.Eight);
                var rankCount = hand.Cards.Count(c => c.Rank == topCard.Rank);

                // no eight, free to play as you wish
                if (suitCount > rankCount)
                {
                    // get matching suit (order by rank)
                    info.SelectedCard = 
                        hand.Cards
                        .Where(c => c.Suit == matchSuit && c.Rank != CardRank.Eight)
                        .OrderBy(c1 => c1.Rank)
                        .FirstOrDefault();
                }
                else if (rankCount > 0)
                {
                    // get matching rank
                    info.SelectedCard = 
                        hand.Cards
                        .Where(c => c.Rank == topCard.Rank)
                        .FirstOrDefault();
                }
                else
                {
                    // no matches above
                    // see if we have an eight
                    if (hand.Cards.Count(c => c.Rank == CardRank.Eight) > 0)
                    {
                        // get first eight
                        info.SelectedCard = hand.Cards.Where(c => c.Rank == CardRank.Eight).FirstOrDefault();
                    }
                    else
                    {
                        info.DrawCard = true;
                    }
                }
            }

            if (info.SelectedCard != null)
            {
                if (info.SelectedCard.Rank == CardRank.Eight)
                {
                    info.WildCardUsed = true;
                    info.OverrideSuit = SelectOverrideSuit(hand);
                }
            }

            return info;
        }

        private static CardSuit SelectOverrideSuit(CrazyEightsHand hand)
        {
            CardSuit overrideSuit = CardSuit.Diamonds;

            int clubCount = hand.GetSuitCount(CardSuit.Clubs);
            int heartCount = hand.GetSuitCount(CardSuit.Hearts);
            int spadeCount = hand.GetSuitCount(CardSuit.Spades);
            int diamondCount = hand.GetSuitCount(CardSuit.Diamonds);

            if (clubCount >= heartCount && clubCount >= spadeCount && clubCount >= diamondCount)
            {
                overrideSuit = CardSuit.Clubs;
            }
            else if (heartCount >= clubCount && heartCount >= spadeCount && heartCount >= diamondCount)
            {
                overrideSuit = CardSuit.Hearts;
            }
            else if (spadeCount >= clubCount && spadeCount >= heartCount && spadeCount >= diamondCount)
            {
                overrideSuit = CardSuit.Spades;
            }
            else if (diamondCount >= clubCount && diamondCount >= heartCount && diamondCount >= spadeCount)
            {
                overrideSuit = CardSuit.Diamonds;
            }

            return overrideSuit;
        }

        //internal static MoveInfo EvaluateMove(CrazyEightDeckManager deckManager, CrazyEightsHand hand)
        //{
        //    return GetCardChoice(deckManager, hand, CardSuit.Clubs, false);
        ////    // super simple rules here
        ////    // 1. Check top card of table
        ////    // 2. Check for Crazy 8
        ////    // 3. Find number of rank matches
        ////    // 4. Find number of suit matches
        ////    // 5. if( have crazy eight ) then
        ////    //      play crazy eight
        ////    //    else if( rank matches > suit matches ) then
        ////    //      play matching rank
        ////    //    else if( suit matches > rank matches ) then
        ////    //      play matching suit
        ////    //    else
        ////    //      play random matching suit or rank
        ////    MoveInfo info = new MoveInfo();

        ////    info.DrawCard = false;
        ////    info.WildCardUsed = false;
        ////    info.SelectedCard = null;

        ////    //if (deckManager.Table.Count == 0)
        ////    //{
        ////    //    // empty table
        ////    //    Card topCard = hand.Cards[0];
        ////    //    info.SelectedCard = topCard;
        ////    //}
        ////    //else
        ////    //{
        ////        bool haveEight = false;
        ////        if (hand.RankCount(CardRank.Eight) > 0)
        ////        {
        ////            haveEight = true;
        ////        }


        ////    //}

        ////    return info;
        //}
    }
}