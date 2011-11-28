using System;
using System.Collections.Generic;
using System.Text;
using CrazyEights;
using CrazyEightsCardLib;

namespace WindowsApplication2
{
    public class GameController
    {
        private void OpponentTurn(CrazyEightDeckManager deckManager, Card[] opponentHands, CardSuit suitOverride )
        {
            AIManager.MoveInfo info;

            //SetPrompt();
            //_prompt = string.Format(Properties.Resources.Text_OpponentTurn, _playerNames[_currentPlayer - 1]);

            // move into UI
            //ClearMouseButtons();
            //ClearSelectedCard();
            //_deckSelected = false;

            //Invalidate();

            // wait - simulate thinking
            //Thread.Sleep(WAIT_LENGTH);

            // send table to ai, with current hand
            // to evaluate next move, which will be added
            // to the table by the ai
            //if (_wildCard)
             //   info = AIManager.EvaluateMove(_data.DeckManager, _data.OpponentsHands[_currentPlayer - 1], _suitOverride);
            //else
            //    info = AIManager.EvaluateMove(_data.DeckManager, _data.OpponentsHands[_currentPlayer - 1], CardSuit.None);

            //if (info.DrawCard)
            //{
            //    if (DrawCard(_currentPlayer))
            //    {
            //        _currentPlayer = NextPlayer();

            //        SetPrompt();

            //        //// player has to pass
            //        //if (_currentPlayer == 0)
            //        //{
            //        //    // wrap around to player
            //        //    _prompt = Properties.Resources.Caption_YourTurn;
            //        //}
            //    }
            //}
            //else
            //{
            //    PlayCard(info.SelectedCard, _data.OpponentsHands[_currentPlayer - 1]);
            //    _wildCard = false;

            //    // check if this guy won
            //    if (_data.OpponentsHands[_currentPlayer - 1].Cards.Count == 0)
            //    {
            //        EndHand();
            //    }
            //    else
            //    {
            //        _currentPlayer = NextPlayer();
            //        SetPrompt();

            //        //if (_currentPlayer == 0)
            //        //{
            //        //    _prompt = Properties.Resources.Caption_YourTurn;
            //        //    if (info.WildCardUsed)
            //        //    {
            //        //        ShowWildcardDialog(info.OverrideSuit);
            //        //    }
            //        //}
            //    }

            //    if (info.WildCardUsed)
            //    {
            //        _wildCard = true;
            //        _suitOverride = info.OverrideSuit;

            //        if (_currentPlayer == 0)
            //        {
            //            ShowWildcardDialog(info.OverrideSuit);
            //        }
            //    }

            //}

        }
        //    public class GameCompleteEventArgs
        //    {
        //        bool _playerWon;
        //        int _winningOpponent;

        //        public GameCompleteEventArgs(bool playerWon, int winningOpponent)
        //        {
        //            _playerWon = playerWon;
        //            _winningOpponent = winningOpponent;
        //        }

        //        public int WinningOpponent
        //        {
        //            get { return _winningOpponent; }
        //            set { _winningOpponent = value; }
        //        }

        //        public bool PlayerWon
        //        {
        //            get { return _playerWon; }
        //            set { _playerWon = value; }
        //        }
        //    }

        //    public delegate void TurnCompleteEvent(object sender, EventArgs e);
        //    public delegate void GameCompleteEvent(object sender, GameCompleteEventArgs e);

        //    public TurnCompleteEvent OnTurnComplete = null;
        //    public GameCompleteEvent OnGameComplete = null;

        //    public enum GameState
        //    {
        //        Playing,
        //        NotPlaying
        //    }

        //    private GameData _data;
        //    private GameState _state;
        //    private string _prompt = string.Empty;

        //    private Card.CardSuit _suitOverride = Card.CardSuit.Clubs;
        //    private bool _wildCard = false;

        //    int _resumeCount = 0;
        //    int _currentPlayer = 0;
        //    int _firstPlayer = 0;

        //    public Card.CardSuit SuitOverride
        //    {
        //        get { return _suitOverride; }
        //        set { _suitOverride = value; _prompt = "Suit is " + value.ToString(); }
        //    }

        //    public bool WildCard
        //    {
        //        get { return _wildCard; }
        //        set { _wildCard = value; }
        //    }

        //    /// <summary>
        //    /// index of player who currently has turn
        //    /// 0 == player, otherwise currentPlayer - 1 is index 
        //    /// to opponent array in data instance
        //    /// </summary>
        //    public int CurrentPlayer
        //    {
        //        get { return _currentPlayer; }
        //        set { _currentPlayer = value; }
        //    }

        //    public string Prompt
        //    {
        //        get { return _prompt; }
        //    }

        //    public GameState State
        //    {
        //        get { return _state; }
        //    }

        //    public GameData Data
        //    {
        //        get { return _data; }
        //    }

        //    public GameController(int numOpponents)
        //    {
        //        _data = new GameData(numOpponents);
        //        _state = GameState.NotPlaying;
        //    }

        //    public void NewGame()
        //    {
        //        _data.PlayerHand.Cards.Clear();
        //        _data.DeckManager.Shuffle();

        //        for (int i = 0; i < _data.OpponentsHands.Length; i++)
        //        {
        //            _data.OpponentsHands[i].Cards.Clear();
        //        }

        //        Hand [] hands = new Hand[_data.OpponentsHands.Length + 1];
        //        _data.OpponentsHands.CopyTo(hands, 0);
        //        hands[_data.OpponentsHands.Length] = _data.PlayerHand;

        //        _data.DeckManager.Deal(hands, 4);
        //        _state = GameState.Playing;

        //        _currentPlayer = _firstPlayer = 0;
        //    }

        //    public void StartGameTurn()
        //    {
        //        //_prompt = string.Empty;
        //        _data.DeckManager.Table.Clear();

        //        _currentPlayer = _firstPlayer;

        //        _wildCard = false;

        //        for (int i = 0; i < _data.OpponentsHands.Length + 1; i++)
        //        {
        //            if (_currentPlayer == 0)
        //            {
        //                // save the resume count
        //                _resumeCount = _data.OpponentsHands.Length - i;

        //                _prompt = "It's your turn!";

        //                // bail;
        //                break;
        //            }
        //            else
        //            {
        //                OpponentTurn();

        //                if (!CheckGameComplete())
        //                {
        //                    if (_currentPlayer > _data.OpponentsHands.Length)
        //                    {
        //                        // wrap around to player
        //                        _currentPlayer = 0;
        //                    }
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }


        //    public void PlayerPlayCard(Card cardToPlay)
        //    {
        //        if (_currentPlayer == 0)
        //        {
        //            _prompt = string.Empty;

        //            bool validPlay = false;
        //            if (cardToPlay.Rank == Card.CardRank.Eight)
        //            {
        //                // display dialog for suit override
        //                _wildCard = true;
        //                _suitOverride = GetSuitOverride();

        //                validPlay = true;
        //            }
        //            else
        //            {
        //                if (_data.DeckManager.Table.Count == 0)
        //                {
        //                    validPlay = true;
        //                }
        //                else if (!_wildCard)
        //                {
        //                    Card topCard = _data.DeckManager.Table[_data.DeckManager.Table.Count - 1];

        //                    // normal check
        //                    if (cardToPlay.Suit == topCard.Suit || cardToPlay.Rank == topCard.Rank)
        //                    {
        //                        validPlay = true;
        //                        _wildCard = false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (cardToPlay.Suit == _suitOverride)
        //                    {
        //                        validPlay = true;
        //                    }
        //                }

        //            }

        //            if (validPlay)
        //            {
        //                // put card onto table
        //                _data.DeckManager.Table.Add(cardToPlay);

        //                // remove from player hand
        //                _data.PlayerHand.Cards.Remove(cardToPlay);

        //                ResumePlay();
        //            }
        //            else
        //            {
        //                _prompt = "Invalid card!";
        //            }

        //        }
        //    }

        //    private Card.CardSuit GetSuitOverride()
        //    {
        //        SuitOverrideDialog dlg = new SuitOverrideDialog();
        //        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            return dlg.Suit;
        //        }
        //        else
        //            return Card.CardSuit.Clubs;
        //    }

        //    public void PlayerDrawCard()
        //    {
        //        if (_currentPlayer == 0)
        //        {
        //            Card card = _data.DeckManager.DrawCard();
        //            if (card != null)
        //            {
        //                _data.PlayerHand.Cards.Add(card);
        //            }
        //        }
        //    }

        //    private void ResumePlay()
        //    {
        //        bool complete = false;
        //        _currentPlayer++;
        //        for (int i = 0; i < _resumeCount; i++)
        //        {
        //            OpponentTurn();
        //            if (CheckGameComplete())
        //            {
        //                complete = true;
        //                break;
        //            }
        //        }

        //        if (_currentPlayer > _data.OpponentsHands.Length)
        //        {
        //            _currentPlayer = 0;
        //        }

        //        if (!complete)
        //        {
        //            _firstPlayer++;
        //            if (_firstPlayer > _data.OpponentsHands.Length)
        //            {
        //                _firstPlayer = 0;
        //            }

        //            FireOnTurnComplete();
        //        }
        //    }

        //    private bool CheckGameComplete()
        //    {
        //        bool complete = false;
        //        // figure out if current player
        //        // played last card - if so he wins
        //        if (_currentPlayer == 0)
        //        {
        //            if (_data.PlayerHand.Cards.Count == 0)
        //            {
        //                FireOnGameComplete(true, -1);
        //                complete = true;
        //            }
        //        }
        //        else
        //        {
        //            if (_data.OpponentsHands[_currentPlayer - 1].Cards.Count == 0)
        //            {
        //                FireOnGameComplete(false, _currentPlayer - 1);
        //                complete = true;
        //            }
        //        }

        //        return complete;
        //    }


        //    private void FireOnGameComplete(bool playerWon, int winningOpponent)
        //    {
        //        if (OnGameComplete != null)
        //        {
        //            OnGameComplete(this, new GameCompleteEventArgs(playerWon, winningOpponent));
        //        }
        //    }

        //    private void OpponentTurn()
        //    {
        //        if ((_currentPlayer - 1) < _data.OpponentsHands.Length)
        //        {
        //            // send table to ai, with current hand
        //            // to evaluate next move, which will be added
        //            // to the table by the ai
        //            AIManager.EvaluateMove(this, _data.DeckManager, _data.OpponentsHands[_currentPlayer - 1]);

        //            _currentPlayer++;
        //        }

        //        if (_currentPlayer > _data.OpponentsHands.Length)
        //        {
        //            _currentPlayer = 0;
        //        }
        //    }

        //    private void FireOnTurnComplete()
        //    {
        //        if (OnTurnComplete != null)
        //        {
        //            OnTurnComplete(this, new EventArgs());
        //        }
        //    }

        //    //private void OpponentPlayCard(int opponentIndex, int cardIndex)
        //    //{
        //    //    Card card = _data.OpponentsHands[opponentIndex].Cards[cardIndex];

        //    //    // put card onto table
        //    //    _data.DeckManager.Table.Add(card);

        //    //    // remove from player hand
        //    //    _data.OpponentsHands[opponentIndex].Cards.Remove(card);

        //    //}


        //}
    }
}
