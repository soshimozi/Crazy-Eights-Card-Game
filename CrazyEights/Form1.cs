using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Threading;
using GDIDB;
using Fireworks;
using CrazyEightsCardLib;

namespace CrazyEights
{
    public partial class frmMain : Form
    {
        #region Private Delegates
        private delegate void ShowWildcardDelegate(CardSuit overrideSuit);
        private delegate void ShowHandWonDelegate(int winner);
        private delegate void ShowGameWonDelegate(int winner);
        private delegate void ShowDrawTwoDelegate();
        private delegate CardSuit GetSuitOverrideDelegate();
        #endregion

        #region Private Constants
        private const int NumberOpponents = 3;
        private const int CardSpacing = 18;
        private const int BoxWidth = 315;
        private const int BoxHeight = 210;
        private const int WAIT_LENGTH = 750;
        private const int FRAME_WAIT_LENGTH = 100000;
        private const int TICKS_PER_SECOND = 10000000;
        private const int FIREWORKS_LAUNCH_TIME = TICKS_PER_SECOND * 2;
        #endregion

        #region Enums
        private enum GameState
        {
            NotPlaying,
            StartGame,
            Playing,
            GameOver
        }

        //private enum TurnState
        //{
        //    PlayerTurn,
        //    OpponentTurn,
        //}

        private enum GameType
        {
            CrazyEights,
            GoFish,
            War,
            Rummy
        }

        #endregion

        #region Private Instance Fields
        private ManualResetEvent _fireworksEvent = new ManualResetEvent(false);
        private ManualResetEvent _stopGameEvent = new ManualResetEvent(false);
        private Thread _gameLoopThread = null;
        private Thread _fireworksThread = null;

        private Hand.PlayDirection _direction;
        private GameData _data;
        private GameState _state;
        private string _prompt = string.Empty;
        private bool _mouseUp = false;
        private bool _deckSelected = false;
        private CardSuit _suitOverride = CardSuit.None;
        //private bool _wildCard = false;
        private bool _playerWon = false;
        private bool _playerHasDrawn = false;

        private int _winningOpponnent = 0;
        private int _currentPlayer = 0;
        private int _startingPlayer = 0;
        private bool _skipFlag = false;

        private int _undoPointer = 0;

        private CardCanvas _cardCanvas = new CardCanvas();

        private int _numOpponents = 3;

        private int _lastMouseX = 0;
        private int _lastMouseY = 0;
        private MouseButtons _lastMouseButton = MouseButtons.None;

        private Card _lastSelectedCard = null;
        private List<Card> _lastCardsPlayed = new List<Card>();
        private List<Card> _lastDrawnCards = new List<Card>();
        private int[] _scores = new int[4] { 0, 0, 0, 0 };
        private string[] _playerNames = new string[3] { "Patrick", "Betty", "Roberto" };

        private int _dueceCount = 0;

        private RocketQueue _rocketQueue = new RocketQueue(new Random(), new Size(0, 0));
        private SparkQueue _sparkQueue = new SparkQueue();
        #endregion

        #region Constructors
        public frmMain()
        {
            InitializeComponent();

            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);

            _data = new GameData(_numOpponents, 1);

            this.SetStyle(
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.DoubleBuffer, true);

            _state = GameState.NotPlaying;

            _rocketQueue.setCanvas(Size);

            //_rocketQueue = new RocketQueue(new Random(), Size);

            _direction = Hand.PlayDirection.Forward;

            _gameLoopThread = new System.Threading.Thread(GameLoop);
            _gameLoopThread.Start();

            _tableCenter = Size.Width / 2;

        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _stopGameEvent.Set();
            _fireworksEvent.Set();
        }

        #endregion

        #region Overrides
        protected override void OnPaint(PaintEventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                switch (_state)
                {
                    case GameState.NotPlaying:
                    case GameState.StartGame:
                        break;

                    case GameState.GameOver:
                        PaintTable(e.Graphics);
                        PaintGameOver(e.Graphics);
                        break;

                    case GameState.Playing:
                        PaintTable(e.Graphics);
                        PaintPlayerNames(e.Graphics);
                        PaintSuitOverride(e.Graphics);
                        if( _currentPlayer == 0 )
                            PaintPrompt(e.Graphics, Brushes.White);
                        else
                        {
                            PaintPrompt(e.Graphics, Brushes.Yellow);
                        }
                        PaintDeckSelection(e.Graphics);
                        PaintDirection(e.Graphics);
                        break;
                }

            }
            else
            {
                base.OnPaint(e);
            }
        }

        #endregion

        #region Drawing Code
        private const int OPPONENT_ONE_TOP = 225;
        private const int OPPONENT_TWO_TOP = 30;
        private const int OPPONENT_THREE_TOP = 225;
        private const int CARDS_PER_HAND = 5;
        private const int SELECTED_OFFSET = 10;
        private const int SCORE_WIDTH = 250;
        private const int SCORE_HEIGHT = 54;
        private const int PLAYER_ROW = 403;
        private const int HAND_MAX_WIDTH = 180;
        private const int DECK_ROW = 225;
        private int _borderWidth = 2;
        //private Point _tableTLC = new Point(339, 225);
        //private Point _deckTLC = new Point(258, 225);
        private int _tableCenter = 334;

        private Font _gameOverFont = new Font(FontFamily.GenericSansSerif, 36.0f, FontStyle.Bold, GraphicsUnit.Point);
        private Font _overrideFont = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold, GraphicsUnit.Point);
        private Font _promptFont = new Font(FontFamily.GenericSansSerif, 14.0f, FontStyle.Bold, GraphicsUnit.Point);
        private Font _nameFont = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold, GraphicsUnit.Point);
        private Font _directionFont = new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Bold, GraphicsUnit.Point);
        private Font _scoreFont = new Font(FontFamily.GenericSansSerif, 7.0f, FontStyle.Bold, GraphicsUnit.Point);
        private Color _scoreBackgroundColor = Color.FromArgb(64, 255, 255, 255);
        private Color _scoreBorderColor = Color.FromArgb(255, 0, 32, 0);
        private Color _scoreForegroundColor = Color.FromArgb(255, 255, 255, 255);


        private void PaintTable(Graphics graphics)
        {
            _cardCanvas.BeginPaint(graphics);

            PaintPlayerHand();
            PaintOpponentHands();
            PaintTable();
            PaintDeck();

            // free up graphics
            _cardCanvas.EndPaint();

            PaintScore(graphics);
        }
        
        private void PaintGameOver(Graphics graphics)
        {
            string text = "";
            if (_playerWon)
            {
                text = "You Won!";
            }
            else
            {
                text = "Game Over";
            }

            SizeF textSize = graphics.MeasureString(text, _gameOverFont);
            graphics.DrawString(text, _gameOverFont, Brushes.Yellow, new PointF((Width - textSize.Width)/2, (Height - textSize.Height)/2));

            // only paint sparks if window is not minimized
            if (WindowState != FormWindowState.Minimized)
            {
                _sparkQueue.CullDeadSparks(true, true);
                _rocketQueue.makeSparks(_sparkQueue);
                _sparkQueue.Paint(graphics);
            }
        }

        private void ShowDrawTwoDialog()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ShowDrawTwoDelegate(ShowDrawTwoDialog));
            }
            else
            {
                MessageBox.Show(this, "Draw Two!", "Crazy Eights");

            }
        }

        private void ShowWildcardDialog(CardSuit overrideSuit)
        {
            if( this.InvokeRequired )
            {
                this.Invoke( new ShowWildcardDelegate(ShowWildcardDialog), overrideSuit);
            }
            else
            {
                MessageBox.Show(this, string.Format(Properties.Resources.Caption_OverrideSuit, overrideSuit.ToString()), Properties.Resources.Caption_CrazyEights);
            }
        }

        private void ShowGameWonDialog(int winner)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ShowGameWonDelegate(ShowGameWonDialog), winner);
            }
            else
            {
                if (winner == 0)
                {
                    MessageBox.Show(Properties.Resources.Text_YouWonGame, Properties.Resources.Caption_CrazyEights);
                }
                else
                {
                    MessageBox.Show(string.Format(Properties.Resources.Text_OpponentWonGame, _playerNames[winner - 1]), Properties.Resources.Caption_CrazyEights);
                }
            }
        }

        private void ShowHandWonDialog(int winner)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ShowHandWonDelegate(ShowHandWonDialog), winner);
            }
            else
            {
                if (winner == 0)
                {
                    MessageBox.Show(Properties.Resources.Text_YouWonHand, Properties.Resources.Caption_CrazyEights);
                }
                else
                {
                    MessageBox.Show(string.Format(Properties.Resources.Text_OpponentWonHand, _playerNames[winner - 1]), Properties.Resources.Caption_CrazyEights);
                }
            }
        }

        private void PaintScore(Graphics graphics)
        {
            Rectangle scoreRect = new Rectangle(_tableCenter - (SCORE_WIDTH / 2), PLAYER_ROW + CardCanvas.DefaultHeight + (CardSpacing / 2), SCORE_WIDTH, SCORE_HEIGHT);
            Rectangle clientRect = ShrinkRect(scoreRect, 1);

            PaintScoreBox(graphics, scoreRect, clientRect);

            if (_state == GameState.GameOver && !_playerWon)
                _scoreFont = new Font(_scoreFont, FontStyle.Strikeout);
            else
                _scoreFont = new Font(_scoreFont, FontStyle.Bold);

            string text = Properties.Resources.Caption_Score;
            SizeF textSize = graphics.MeasureString(text, _scoreFont);
            graphics.DrawString(string.Format(Properties.Resources.Text_ScoreDisplay, "You", _scores[0]), _scoreFont, new SolidBrush(_scoreForegroundColor), new Point(clientRect.X, clientRect.Y + 8));

            if (_state == GameState.GameOver && (_winningOpponnent != 0 || _playerWon))
                _scoreFont = new Font(_scoreFont, FontStyle.Strikeout);
            else
                _scoreFont = new Font(_scoreFont, FontStyle.Bold);

            graphics.DrawString(string.Format(Properties.Resources.Text_ScoreDisplay, _playerNames[0], _scores[1]), _scoreFont, new SolidBrush(_scoreForegroundColor), new Point(clientRect.X, clientRect.Y + clientRect.Height - 16));

            if (_state == GameState.GameOver && (_winningOpponnent != 1 || _playerWon))
                _scoreFont = new Font(_scoreFont, FontStyle.Strikeout);
            else
                _scoreFont = new Font(_scoreFont, FontStyle.Bold);

            string scoreText = string.Format(Properties.Resources.Text_ScoreDisplay, _playerNames[1], _scores[2]);
            SizeF scoreTextSize = graphics.MeasureString(scoreText, _scoreFont);
            graphics.DrawString(scoreText, _scoreFont, new SolidBrush(_scoreForegroundColor), new PointF((clientRect.X + clientRect.Width) - scoreTextSize.Width - 1, clientRect.Y + 8));

            if (_state == GameState.GameOver && (_winningOpponnent != 2 || _playerWon))
                _scoreFont = new Font(_scoreFont, FontStyle.Strikeout);
            else
                _scoreFont = new Font(_scoreFont, FontStyle.Bold);

            scoreText = string.Format(Properties.Resources.Text_ScoreDisplay, _playerNames[2], _scores[3]);
            scoreTextSize = graphics.MeasureString(scoreText, _scoreFont);
            graphics.DrawString(scoreText, _scoreFont, new SolidBrush(_scoreForegroundColor), new PointF((clientRect.X + clientRect.Width) - scoreTextSize.Width - 1, ((clientRect.Y + clientRect.Height) - 16)));

            _scoreFont = new Font(_scoreFont, FontStyle.Bold);
            graphics.DrawString(Properties.Resources.Caption_Score, _scoreFont, new SolidBrush(_scoreForegroundColor),
                new PointF(clientRect.X + ((clientRect.Width - textSize.Width) / 2),
                    clientRect.Y));
        }

        private void PaintScoreBox(Graphics graphics, Rectangle scoreRect, Rectangle clientRect)
        {
            graphics.DrawRectangle(new Pen(new SolidBrush(_scoreBorderColor)), scoreRect);
            graphics.FillRectangle(new SolidBrush(_scoreBackgroundColor), clientRect);
        }


        private void PaintSuitOverride(Graphics graphics)
        {
            if( _suitOverride != CardSuit.None )
            {
                Image suitImage = GetSuitImage(_suitOverride);

                string text = string.Format(Properties.Resources.Caption_OverrideSuit, "");
                SizeF textSize = graphics.MeasureString(text, _overrideFont);

                Point deckTLC = new Point(258, 225);
                deckTLC.X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2;

                Image directionImage = (_direction == Hand.PlayDirection.Forward ? Properties.Resources.PNG_Forward : Properties.Resources.PNG_Reverse);
                float deck_row = (float)(deckTLC.Y + CardCanvas.DefaultHeight + _borderWidth + (directionImage.Height * 1.5));

                float size = textSize.Width + suitImage.Width;
                float leftEdge = _tableCenter - (size / 2);
                graphics.DrawString(text, _overrideFont, Brushes.White, new PointF(leftEdge, deck_row));

              //textPoint.X += textSize.Width;

                float centerPoint = deck_row + (_overrideFont.Height/2) - (suitImage.Height/2);
                graphics.DrawImage(suitImage, new PointF(leftEdge + textSize.Width, centerPoint));
            }
        }

        private void PaintDirection(Graphics graphics)
        {
            Point deckTLC = new Point(258, 225);
            deckTLC.X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2;

            int y = deckTLC.Y + CardCanvas.DefaultHeight + _borderWidth;
            Image directionImage = (_direction == Hand.PlayDirection.Forward ? Properties.Resources.PNG_Forward : Properties.Resources.PNG_Reverse);

            SizeF textSize = graphics.MeasureString("Direction", _directionFont);

            PointF renderPoint = new PointF(_tableCenter - (directionImage.Width + textSize.Width + 5)/ 2, y);
            graphics.DrawString("Direction", _directionFont, Brushes.GhostWhite, new PointF(renderPoint.X, y + ((directionImage.Height - textSize.Height) / 2)));
            graphics.DrawImage(directionImage, new PointF(renderPoint.X + textSize.Width + 5, renderPoint.Y));

        }

        private Image GetSuitImage(CardSuit suit)
        {
            Image suitImage = null;
            switch (suit)
            {
                case CardSuit.Diamonds:
                    suitImage = Properties.Resources.PNG_DiamondsSuit;
                    break;

                case CardSuit.Hearts:
                    suitImage = Properties.Resources.PNG_HeartsSuit;
                    break;

                case CardSuit.Spades:
                    suitImage = Properties.Resources.PNG_SpadesSuit;
                    break;

                default:
                    suitImage = Properties.Resources.PNG_ClubsSuit;
                    break;
            }
            return suitImage;
        }

        private void PaintPrompt(Graphics g, Brush brush)
        {

            Point deckTLC = new Point(258, 225);
            deckTLC.X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2;

            if (!string.IsNullOrEmpty(_prompt))
            {
                SizeF textSize = g.MeasureString(_prompt, _promptFont);
                g.DrawString(_prompt, _promptFont, brush, new PointF(_tableCenter - textSize.Width/2, deckTLC.Y - 50));
            }
        }

        private void PaintDeck()
        {
            //_deckTLC.Y = (Size.Height - CardCanvas.DefaultHeight) / 2;
            Point deckTLC = new Point(258, 225);

            deckTLC.X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing/2;
            if (!_data.DeckManager.IsEmpty)
                _cardCanvas.DrawCardBack(deckTLC, GetSelectedBack());
            else
                _cardCanvas.DrawCardBack(deckTLC, CardBack.The_X);
        }


        private void PaintDeckSelection(Graphics graphics)
        {
            Point deckTLC = new Point(258, 225);
            deckTLC.X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2;
            if (_deckSelected)
            {
                graphics.DrawRectangle(Pens.Gold, ShrinkRect(new Rectangle(deckTLC, new Size(CardCanvas.DefaultWidth, CardCanvas.DefaultHeight)), -1));
                graphics.DrawRectangle(Pens.Gold, ShrinkRect(new Rectangle(deckTLC, new Size(CardCanvas.DefaultWidth, CardCanvas.DefaultHeight)), -2));
            }
        }


        private void PaintTable()
        {
            Point tableTLC = new Point(339, 225);
            tableTLC.X = _tableCenter + CardSpacing/2;
            if (_data.DeckManager.Table.Count > 0)
            {
                Card topCard = _data.DeckManager.Table[_data.DeckManager.Table.Count - 1];
                _cardCanvas.DrawCard(tableTLC, topCard.CardIndex);
            }
            else
            {
                _cardCanvas.DrawCardBack(tableTLC, CardBack.The_O);
            }
        }

        private void PaintOpponentHands()
        {
            Point deckTLC = new Point(258, 225);
            deckTLC.X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2;

            Point tableTLC = new Point(339, 225);
            tableTLC.X = _tableCenter + CardSpacing / 2;

            Point opponent1TLC = new Point(20, OPPONENT_ONE_TOP);
            Point opponent2TLC = new Point();
            Point opponent3TLC = new Point();

            opponent2TLC.Y = OPPONENT_TWO_TOP;
            opponent3TLC.Y = OPPONENT_THREE_TOP;

            int center = _tableCenter; // _tableTLC.X + (CardCanvas.DefaultWidth / 2);
            opponent2TLC.X = center - ((((_data.OpponentsHands[1].Cards.Count - 1) * CardSpacing) + CardCanvas.DefaultWidth) / 2);

            opponent1TLC.X = deckTLC.X - (CardSpacing * 5) - HAND_MAX_WIDTH;

            if (opponent1TLC.X < 20)
            {
                opponent1TLC.X = 20;
            }
            int minX = tableTLC.X + CardCanvas.DefaultWidth + CardSpacing;
            opponent3TLC.X = tableTLC.X + CardCanvas.DefaultHeight + (CardSpacing * 5); // -(((_data.OpponentsHands[2].Cards.Count - 1) * CardSpacing) + CardCanvas.DefaultWidth);
            if (opponent3TLC.X < minX)
            {
                opponent3TLC.X = minX;
            }
        
            // array of locations for opponent hands
            Point[] opponentTLC = new Point[3] {opponent1TLC, opponent2TLC, opponent3TLC};
            int[] maxWidths = new int[3] { HAND_MAX_WIDTH, Width, HAND_MAX_WIDTH };

            // draw each opponent hands
            for( int i=0; i<3; i++)
            {
                if (_data.OpponentsHands[i].Cards.Count > 0)
                {
                    PaintHand(_data.OpponentsHands[i].Cards.ToArray(), opponentTLC[i], true, maxWidths[i]);
                }
            }

        }

        private void PaintPlayerNames(Graphics graphics)
        {
            Point deckTLC = new Point(258, 225);
            deckTLC.X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2;

            Point tableTLC = new Point(339, 225);
            tableTLC.X = _tableCenter + CardSpacing / 2;

            string playerNameText = _playerNames[0];
            SizeF playerNameSize = graphics.MeasureString(playerNameText, _nameFont);

            float leftEdge = deckTLC.X - (CardSpacing * 5) - HAND_MAX_WIDTH;
            leftEdge = leftEdge + ((HAND_MAX_WIDTH - playerNameSize.Width) / 2);

            Brush brush;
            if (_currentPlayer == 1)
                brush = Brushes.Yellow;
            else
                brush = Brushes.White;

            graphics.DrawString(playerNameText, _nameFont, brush, new PointF(leftEdge, (OPPONENT_ONE_TOP - playerNameSize.Height)));

            playerNameText = _playerNames[1];
            playerNameSize = graphics.MeasureString(playerNameText, _nameFont);

            int center = _tableCenter; // tableTLC.X + (CardCanvas.DefaultWidth / 2);
            leftEdge = center - (playerNameSize.Width / 2);

            if (_currentPlayer == 2)
                brush = Brushes.Yellow;
            else
                brush = Brushes.White;

            graphics.DrawString(playerNameText, _nameFont, brush, new PointF(leftEdge, (OPPONENT_TWO_TOP + CardCanvas.DefaultHeight)));

            playerNameText = _playerNames[2];
            playerNameSize = graphics.MeasureString(playerNameText, _nameFont);

            leftEdge = tableTLC.X + CardCanvas.DefaultHeight + (CardSpacing * 5);
            leftEdge = leftEdge + ((HAND_MAX_WIDTH - playerNameSize.Width) / 2);

            if (_currentPlayer == 3)
                brush = Brushes.Yellow;
            else
                brush = Brushes.White;

            graphics.DrawString(playerNameText, _nameFont, brush, new PointF(leftEdge, (OPPONENT_THREE_TOP - playerNameSize.Height)));
        }


        private void PaintPlayerHand()
        {
            Point playerCardPoint = new Point();

            int center = _tableCenter; // _tableTLC.X + (CardCanvas.DefaultWidth / 2);

            playerCardPoint.X = center - ((((_data.PlayerHand.Cards.Count - 1) * CardSpacing) + CardCanvas.DefaultWidth) / 2);
            playerCardPoint.Y = PLAYER_ROW;
            if (_data.PlayerHand.Cards.Count > 0)
            {
                PaintHand(_data.PlayerHand.Cards.ToArray(), playerCardPoint, true, -1);
            }
        }

        private void PaintHand(Card[] hand, Point TLC, bool showCard, int maxWidth)
        {
            int currentX = TLC.X;
            int currentY = TLC.Y;
            for (int i = 0; i < hand.Length; i++)
            {
                if (maxWidth >= 0 && Math.Abs((currentX + CardCanvas.DefaultWidth)- TLC.X) > maxWidth)
                {
                    currentY += CardSpacing;
                    currentX = TLC.X;
                }

                Point cardPoint = new Point(currentX, currentY);
                if (showCard)
                {
                    if (hand[i].Selected)
                    {
                        cardPoint.Y -= SELECTED_OFFSET;
                    }
                    _cardCanvas.DrawCard(cardPoint, hand[i].CardIndex);
                }
                else
                {
                    _cardCanvas.DrawCardBack(cardPoint, GetSelectedBack());
                }

                currentX += CardSpacing;
            }
        }
        #endregion

        #region Message Handlers
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AskStartGame();
        }

        private bool AskStartGame()
        {
            bool startGame = true;
            if (_state == GameState.Playing)
            {
                if (MessageBox.Show(Properties.Resources.Prompt_AbandonGame, Properties.Resources.Caption_GameInProgress, MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    startGame = false;
                }
            }

            if (startGame)
            {
                _state = GameState.StartGame;
            }

            return startGame;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            _lastMouseX = e.X;
            _lastMouseY = e.Y;
            _mouseUp = true;
            _lastMouseButton = e.Button;
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            _rocketQueue.setCanvas(Size);
            _tableCenter = Size.Width / 2;
            Invalidate();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.WindowsShutDown)
            {
                if (MessageBox.Show(this, Properties.Resources.Prompt_EndGame, Properties.Resources.Caption_CrazyEights, MessageBoxButtons.YesNoCancel, MessageBoxIcon.None) != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_MouseMove(object sender, MouseEventArgs e)
        {
            _lastMouseX = e.X;
            _lastMouseY = e.Y;
        }

        private void cardBackToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // find any other checked itme
            foreach (ToolStripMenuItem item in cardBackToolStripMenuItem.DropDownItems)
            {
                if (item.Tag != e.ClickedItem.Tag && item.Checked)
                {
                    item.Checked = false;
                }
            }

        }
        #endregion

        #region Game State
        private void InitalizeHand()
        {
            bool done = false;

            while (!done)
            {
                _data.PlayerHand.Cards.Clear();
                _data.DeckManager.Shuffle();

                for (int i = 0; i < _data.OpponentsHands.Length; i++)
                {
                    _data.OpponentsHands[i].Cards.Clear();
                }

                Hand[] hands = new Hand[_data.OpponentsHands.Length + 1];
                _data.OpponentsHands.CopyTo(hands, 0);
                hands[_data.OpponentsHands.Length] = _data.PlayerHand;

                _data.DeckManager.Deal(hands, CARDS_PER_HAND);

                if (_data.DeckManager.Table[0] != null && _data.DeckManager.Table[0].Rank != CardRank.Eight)
                {
                    done = true;
                }
            }

            
            _currentPlayer = _startingPlayer;
            _suitOverride = CardSuit.None;
            _mouseUp = false;
            _prompt = "";
            _undoPointer = 0;
            _lastMouseButton = MouseButtons.None;
            _lastCardsPlayed.Clear();
            _direction = Hand.PlayDirection.Forward;
            _skipFlag = false;

            SetPrompt();
            //if (_currentPlayer == 0)
            //{
            //    _prompt = Properties.Resources.Caption_YourTurn;
            //}
        }


        private void InitializeGame()
        {
            StopFireworksThread();

            for (int i = 0; i < _scores.Length; i++)
            {
                _scores[i] = 0;
            }

            _currentPlayer = _startingPlayer = 0;

            _data = new GameData(_numOpponents, 2);
        }

        private void StartFireworksThread()
        {
            _fireworksEvent.Reset();
            _fireworksThread = new Thread(new ThreadStart(FireworksThread));
           // _fireworksThread.Start();
        }

        private void StopFireworksThread()
        {
            _fireworksEvent.Set();
            _rocketQueue.Clear();
            _sparkQueue.Clear();
        }

        private void FireworksThread()
        {
            long timerTick = DateTime.Now.Ticks;
            int salvoCount = 0;
            bool done = false;

            while (!_fireworksEvent.WaitOne(10, true))
            {
                Invalidate();

                if ((DateTime.Now.Ticks - timerTick) > FIREWORKS_LAUNCH_TIME)
                {
                    if (!done)
                    {
                        if (salvoCount == 5)
                        {
                            done = true;
                            _rocketQueue.addRandomRocket();
                            _rocketQueue.addRandomRocket();
                            _rocketQueue.addRandomRocket();
                        }
                        else
                        {
                            _rocketQueue.addRandomRocket();
                            salvoCount++;
                        }
                    }

                    timerTick = DateTime.Now.Ticks;
                }
            }
        }

        private void GameLoop()
        {
            bool gameComplete = false;
            long lastFrameTime = 0;
            do
            {
                if ((DateTime.Now.Ticks - lastFrameTime) > FRAME_WAIT_LENGTH)
                {
                    // only update state if not minimized
                    if (WindowState != FormWindowState.Minimized)
                    {
                        switch (_state)
                        {
                            case GameState.NotPlaying:
                            case GameState.GameOver:
                                Invalidate(); // repaint
                                break;

                            case GameState.Playing:
                                if (_currentPlayer == 0)
                                {
                                    PlayerTurn();

                                    // clear any mouse clicks
                                    ClearMouseButtons();

                                    // check for mouse over on any cards
                                    CheckMouseOver();
                                }
                                else
                                {
                                    OpponentTurn();
                                    SetPrompt();
                                }

                                // repaint
                                Invalidate();
                                break;

                            case GameState.StartGame:
                                InitializeGame();
                                InitalizeHand();
                                _state = GameState.Playing;
                                break;
                        }

                        lastFrameTime = DateTime.Now.Ticks;
                    }
                }

                Thread.Sleep(0);

            } while (!_stopGameEvent.WaitOne(10, true) && !gameComplete);
        }

        private void OpponentTurn()
        {
            SetPrompt();

            ClearMouseButtons();
            ClearSelectedCard();
            _deckSelected = false;

            Invalidate();

            // wait - simulate thinking
            Thread.Sleep(WAIT_LENGTH);

            // send table to ai, with current hand
            // to evaluate next move, which will be added
            // to the table by the ai
            AIManager.MoveInfo info = AIManager.EvaluateMove(_data.DeckManager, _data.OpponentsHands[_currentPlayer - 1], _suitOverride);

            if (info.DrawCard)
            {
                Card card;
                if (DrawCard(_currentPlayer, out card))
                {
                    _currentPlayer = NextPlayer();
                }
            }
            else
            {
                PlayCard(info.SelectedCard, _data.OpponentsHands[_currentPlayer - 1]);
                _suitOverride = CardSuit.None;

                // check if this guy won
                if (_data.OpponentsHands[_currentPlayer - 1].Cards.Count == 0)
                {
                    EndHand();
                }
                else
                {
                    _currentPlayer = NextPlayer();
                }

                if (info.WildCardUsed)
                {
                    _suitOverride = info.OverrideSuit;
                    if (_currentPlayer == 0)
                    {
                        ShowWildcardDialog(info.OverrideSuit);
                    }
                }
            }

        }

        private void SetPrompt()
        {
            if (_currentPlayer == 0)
            {
                _prompt = Properties.Resources.Caption_YourTurn;
            }
            else
            {
                _prompt = string.Format(Properties.Resources.Text_OpponentTurn, _playerNames[_currentPlayer - 1]);
            }

        }

        private int NextPlayer()
        {
            int player = _currentPlayer;
            if (_direction == Hand.PlayDirection.Forward)
            {
                player++;
            }
            else
            {
                player--;
            }

            if (player < 0)
            {
                // warp around to last opponent
                player = _data.OpponentsHands.Length;
            }
            else if (player > _data.OpponentsHands.Length)
            {
                // wrap around to player
                player = 0;
            }

            if (_skipFlag)
            {
                _skipFlag = false;

                if (_direction == Hand.PlayDirection.Forward)
                {
                    player++;
                }
                else
                {
                    player--;
                }

                if (player < 0)
                {
                    // warp around to last opponent
                    player = _data.OpponentsHands.Length;
                }
                else if (player > _data.OpponentsHands.Length)
                {
                    // wrap around to player
                    player = 0;
                }
            }

            return player;
        }

        private void PlayerTurn()
        {
            //SetPrompt();

            int selectedColumn;
            if (CardHitCheck(out selectedColumn))
            {
                // bingo, play card
                if (CheckValidCard(_data.PlayerHand.Cards[selectedColumn]))
                {
                    _suitOverride = CardSuit.None;
                    if (_data.PlayerHand.Cards[selectedColumn].Rank == CardRank.Eight)
                    {
                        // display dialog for suit override
                        _suitOverride = GetSuitOverride();
                    }

                    PlayCard(_data.PlayerHand.Cards[selectedColumn], _data.PlayerHand);

                    // check if you won
                    if (_data.PlayerHand.Cards.Count == 0)
                    {
                        EndHand();
                    }
                    else
                    {
                        _currentPlayer = NextPlayer();
                    }

                }
                else
                {
                    _prompt = Properties.Resources.Text_InvalidCard;

                    var t = new System.Threading.Timer(new TimerCallback(Timer_Callback));
                    t.Change(2000, Timeout.Infinite);
                }
            }
            else if (DeckHitCheck())
            {
                Card card;
                if (DrawCard(0, out card))
                {
                    // player has to pass
                    _currentPlayer = NextPlayer();
                }
            }

            RearrangeHand();
        }

        private void RearrangeHand()
        {
            var query =
                  from c in _data.PlayerHand.Cards
                  group c by c.Suit into grp
                  select new { Suit = grp.Key, Cards = grp.OrderBy( c2 => c2.Rank ) }; 

            List<Card> newHand = new List<Card>();
            foreach (var g in query)
            {
                foreach (var card in g.Cards)
                {
                    newHand.Add(card);
                }
            }

            // now we have the cards grouped
            _data.PlayerHand.Cards.Clear();
            _data.PlayerHand.Cards.InsertRange(0, newHand);

        }

        private void ClearMouseButtons()
        {
            _mouseUp = false;
            _lastMouseButton = MouseButtons.None;
        }

        private void ClearSelectedCard()
        {
            if (_lastSelectedCard != null)
            {
                _lastSelectedCard.Selected = false;
                _lastSelectedCard = null;
            }
        }

        private bool DeckHitCheck()
        {
            Point deckTLC = new Point(258, 225);
            deckTLC.X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2;

            Point tableTLC = new Point(339, 225);
            tableTLC.X = _tableCenter + CardSpacing / 2;

            bool hitCheck = false;
            if (_mouseUp && _lastMouseButton == MouseButtons.Left && _state == GameState.Playing)
            {
                // check for deck hit
                if (_lastMouseX >= deckTLC.X && _lastMouseX <= deckTLC.X + CardCanvas.DefaultWidth && _lastMouseY >= deckTLC.Y && _lastMouseY <= deckTLC.Y + CardCanvas.DefaultHeight)
                {
                    hitCheck = true;
                }
            }

            return hitCheck;
        }

        private bool CardHitCheck(out int column)
        {
            // hit check
            bool hitCheck = false;

            column = -1;
            if (_mouseUp && _lastMouseButton == MouseButtons.Left && _state == GameState.Playing)
            {
                Card[] hand = _data.PlayerHand.Cards.ToArray();
                column = GetCardColumn(hand, _lastMouseX, _lastMouseY);

                // now check column
                if (column >= 0 && column < hand.Length)
                {
                    hitCheck = true;
                }
            }

            return hitCheck;
        }

        private void CheckMouseOver()
        {
            Point deckTLC = new Point(258, 225);
            deckTLC.X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2;

            Point tableTLC = new Point(339, 225);
            tableTLC.X = _tableCenter + CardSpacing / 2;

            Card[] hand = _data.PlayerHand.Cards.ToArray();
            int column = GetCardColumn(hand, _lastMouseX, _lastMouseY);

            if (column >= 0 && column < hand.Length)
            {
                if (_lastSelectedCard != null)
                {
                    _lastSelectedCard.Selected = false;
                }
                _lastSelectedCard = null;

                _lastSelectedCard = _data.PlayerHand.Cards[column];
                _lastSelectedCard.Selected = true;
            }
            else
            {
                if (_lastSelectedCard != null)
                {
                    _lastSelectedCard.Selected = false;
                }
                _lastSelectedCard = null;

            }

            // check for deck hit
            if (_lastMouseX >= deckTLC.X && _lastMouseX <= deckTLC.X + CardCanvas.DefaultWidth && _lastMouseY >= deckTLC.Y && _lastMouseY <= deckTLC.Y + CardCanvas.DefaultHeight)
            {
                // if so, make deck selected
                _deckSelected = true;
            }
            else
            {
                _deckSelected = false;
            }

            Invalidate();
        }

        private bool CheckValidCard(Card cardToPlay)
        {
            bool validPlay = false;

            Card topCard = _data.DeckManager.Table[_data.DeckManager.Table.Count - 1];

            // if there is only one top card, then we match normal
            if (_data.DeckManager.Table.Count == 1)
            {
                // if an eight, make sure we don't have any other suits
                if (cardToPlay.Rank == CardRank.Eight)
                {
                    var suitCount = _data.PlayerHand.Cards.Count(c => c.Suit == topCard.Suit && c.Rank != CardRank.Eight);

                    // if there are no other suits but the eight, then we have a valid play
                    // otherwise, we should play one of the other suits
                    if (suitCount == 0)
                    {
                        validPlay = true;
                    }
                }
                else
                {
                    // otherwise see if it matches suit and rank
                    if (cardToPlay.Suit == topCard.Suit || cardToPlay.Rank == topCard.Rank)
                    {
                        validPlay = true;
                    }
                }
            }
            // if top card is an eight, rules are different, can only match rank or _suitOverride suit
            else if (topCard.Rank == CardRank.Eight)
            {
                if (cardToPlay.Rank == CardRank.Eight || cardToPlay.Suit == _suitOverride)
                {
                    validPlay = true;
                }
            }
            else if (topCard.Rank == CardRank.Two && !_playerHasDrawn)
            {
                if (cardToPlay.Rank == CardRank.Two)
                {
                    validPlay = true;
                }
            }
            else
            {
                if (cardToPlay.Rank == CardRank.Eight)
                {
                    var suitCount = _data.PlayerHand.Cards.Count(c => c.Suit == topCard.Suit && c.Rank != CardRank.Eight);

                    // if there are no other suits but the eight, then we have a valid play
                    // otherwise, we should play one of the other suits
                    if (suitCount == 0)
                    {
                        validPlay = true;
                    }
                }
                else
                {
                    if (_suitOverride == CardSuit.None)
                    {
                        // normal check
                        if (cardToPlay.Suit == topCard.Suit || cardToPlay.Rank == topCard.Rank)
                        {
                            validPlay = true;
                        }
                    }
                    else
                    {
                        if (cardToPlay.Suit == _suitOverride)
                        {
                            validPlay = true;
                        }
                    }
                }
            }
            return validPlay;
        }

        private CardSuit GetSuitOverride()
        {
            CardSuit selectedSuit = CardSuit.Clubs;
            if (this.InvokeRequired)
            {
                selectedSuit = (CardSuit)this.Invoke(new GetSuitOverrideDelegate(GetSuitOverride));
            }
            else
            {
                SuitOverrideDialog dlg = new SuitOverrideDialog();

                dlg.StartPosition = FormStartPosition.CenterParent;
                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    selectedSuit = dlg.Suit;
                }
            }
            return selectedSuit;
        }

        private bool DrawCard(int player, out Card card)
        {
            bool pass = false;

            card = null;
            if (player == 0)
            {
                card = _data.DeckManager.DrawCard();
                if (card != null)
                {
                    _data.PlayerHand.Cards.Add(card);
                }
            }
            else
            {
                card = _data.DeckManager.DrawCard();
                if (card != null)
                {
                    _data.OpponentsHands[player - 1].Cards.Add(card);
                }
            }

            if (card == null)
            {
                //ReShuffle();

                //if (_data.DeckManager.IsEmpty)
                //{
                //    pass = true;
                //}
                //else
                //{
                //    DrawCard(player);
                //}

                pass = true;
            }

            return pass;
        }

        private void ReShuffle()
        {
            // make exlusion list
            int exlusionLength = _data.PlayerHand.Cards.Count;

            for (int i = 0; i < _numOpponents; i++)
            {
                exlusionLength += _data.OpponentsHands[i].Cards.Count;
            }

            Card topCard = null;
            if (_data.DeckManager.Table.Count > 0)
            {
                topCard = _data.DeckManager.Table[_data.DeckManager.Table.Count - 1];
                exlusionLength += 1;
            }

            if (exlusionLength > 0)
            {

                Card[] exlusionList = new Card[exlusionLength];
                int copyIndex = 0;

                Array.Copy(_data.PlayerHand.Cards.ToArray(), exlusionList, _data.PlayerHand.Cards.Count);

                copyIndex += _data.PlayerHand.Cards.Count;
                for (int i = 0; i < _numOpponents; i++)
                {
                    Array.Copy(_data.OpponentsHands[i].Cards.ToArray(), 0, exlusionList, copyIndex, _data.OpponentsHands[i].Cards.Count);
                    copyIndex += _data.OpponentsHands[i].Cards.Count;
                }

                if (topCard != null)
                {
                    Card[] topCardArray = new Card[1] { topCard };

                    Array.Copy(topCardArray, 0, exlusionList, copyIndex, 1);
                }

                _data.DeckManager.Shuffle(exlusionList);

            }
            else
            {
                _data.DeckManager.Shuffle();
            }

            if (topCard != null)
            {
                _data.DeckManager.Table.Add(topCard);
            }
        }

        private void PlayCard(Card cardToPlay, CrazyEightsHand hand)
        {
            // put card onto table
            _data.DeckManager.Table.Add(cardToPlay);

            // remove from player hand
            //_data.OpponentsHands[_currentPlayer - 1].Cards.Remove(cardToPlay);
            hand.Cards.Remove(cardToPlay);

            if (_undoPointer < _lastCardsPlayed.Count - 1)
            {
                // prune list past _redoPointer
                _lastCardsPlayed.RemoveRange(_undoPointer + 1, _lastCardsPlayed.Count - (_undoPointer + 1));
            }

            _lastCardsPlayed.Add(cardToPlay);
            _undoPointer = _lastCardsPlayed.Count - 1;

            if (cardToPlay.Rank == CardRank.Ace)
            {
                if (_direction == Hand.PlayDirection.Forward)
                {
                    _direction = Hand.PlayDirection.Reverse;
                }
                else
                {
                    _direction = Hand.PlayDirection.Forward;
                }
            }
            else if (cardToPlay.Rank == CardRank.Two && !_data.DeckManager.IsEmpty)
            {
                _playerHasDrawn = false;

                // increment the two's count
                _dueceCount++;

                // draw two
                int nextPlayer = NextPlayer();

                // get hand of next player
                CrazyEightsHand nextPlayerHand = null;
                if (nextPlayer == 0)
                {
                    nextPlayerHand = _data.PlayerHand;
                }
                else
                {
                    nextPlayerHand = _data.OpponentsHands[nextPlayer - 1];
                }

                if (nextPlayerHand.Cards.Find(c => c.Rank == CardRank.Two) == null)
                {
                    _lastDrawnCards.Clear();

                    for (int i = 0; i < _dueceCount; i++)
                    {
                        Card drawCard;

                        DrawCard(nextPlayer, out drawCard);

                        _lastDrawnCards.Add(drawCard);

                        DrawCard(nextPlayer, out drawCard);

                        _lastDrawnCards.Add(drawCard);
                    }

                    _playerHasDrawn = true;

                    // if next is player and player doesn't have any two's
                    if (nextPlayer == 0)
                    {
                        ShowDrawTwoDialog();
                    }

                    _dueceCount = 0;
                }
            }
            else if (cardToPlay.Rank == CardRank.Queen)
            {
                // trick here, increment player so when
                // we increment next player, it will skip
                _skipFlag = true;
            }

            //_redoPointer = _lastCardsPlayed.Count;
        }

        private void EndHand()
        {
            // force a redraw before displaying messagebox
            Invalidate();

            ShowHandWonDialog(_currentPlayer);

            bool winner = false;

            // check if top card is a 2, then make sure we subtract last drawn cards from current player
            Card topCard = _data.DeckManager.Table[_data.DeckManager.Table.Count - 1];

            if (topCard.Rank == CardRank.Two)
            {
                Hand hand = null;
                if( _currentPlayer == 0)
                {
                    hand = _data.PlayerHand;
                }
                else 
                {
                    hand = _data.OpponentsHands[_currentPlayer - 1];
                }

                foreach (var card in _lastDrawnCards)
                {
                    hand.Cards.Remove(card);
                }
            }

            // update scores from each hand
            for (int i = 0; i < 4; i++)
            {
                if (i != _currentPlayer)
                {
                    // get hand score
                    _scores[i] += ScoreHand(i);
                }
            }

            // now check if anyone is over 500
            for (int i = 0; i < 4; i++)
            {
                if (_scores[i] >= 500)
                {
                    winner = true;
                }
            }

            if (winner)
            {
                int winningPlayer = 0;
                int lowestScore = 500;
                for (int i = 0; i < 4; i++)
                {
                    // lowest player wins
                    if (_scores[i] < lowestScore)
                    {
                        lowestScore = _scores[i];
                        winningPlayer = i;
                    }
                }

                ShowGameWonDialog(winningPlayer);
                if (winningPlayer == 0)
                {
                    _playerWon = true;
                    StartFireworksThread();
                }
                else
                {
                    _playerWon = false;
                    _winningOpponnent = winningPlayer - 1;
                }
                _state = GameState.GameOver;
            }
            else // keep playing
            {
                // wrap round starting player if at end of list
                if (_startingPlayer >= _data.OpponentsHands.Length)
                {
                    _startingPlayer = 0;
                }
                else
                {
                    // increment starting player
                    _startingPlayer++;
                }

                // now start new turn
                InitalizeHand();
            }
        }

        private int ScoreHand(int handIndex)
        {
            int score = 0;
            Card[] cards;
            if (handIndex == 0)
            {
                cards = _data.PlayerHand.Cards.ToArray() ;
            }
            else
            {
                cards = _data.OpponentsHands[handIndex - 1].Cards.ToArray();
            }

            foreach (Card card in cards)
            {
                if ((int)card.Rank > 10)
                {
                    score += 10;
                }
                else
                {
                    score += (int)card.Rank + 1;
                }
            }

            return score;
            
        }
        #endregion

        #region Utility Functions
        private CardBack GetSelectedBack()
        {
            CardBack selectedBack = CardBack.Crosshatch;
            foreach (ToolStripMenuItem item in cardBackToolStripMenuItem.DropDownItems)
            {
                if (item.Checked)
                {
                    selectedBack = (CardBack)Enum.Parse(typeof(CardBack), (string)item.Tag);
                }
            }

            return selectedBack;
        }

        private Rectangle ShrinkRect(Rectangle rectangle, int amount)
        {
            rectangle.X += amount;
            rectangle.Height -= (amount * 2);
            rectangle.Y += amount;
            rectangle.Width -= (amount * 2);
            return rectangle;
        }

        private bool PointInRect(RectangleF rect, Point point)
        {
            if (point.X >= rect.X && point.X <= (rect.X + rect.Width) && point.Y >= rect.Y && point.Y <= (rect.Y + rect.Height))
                return true;
            else
                return false;
        }

        //private bool IsPlayerTurnNext()
        //{
        //    if (_direction == PlayDirection.Left)
        //    {
        //        return _currentPlayer == _data.OpponentsHands.Length;
        //    }
        //    else
        //    {
        //        return _currentPlayer == 1;
        //    }
        //}

        private int GetCardColumn(Card[] hand, int mouseX, int mouseY)
        {

            //Point deckTLC = new Point(258, 225);
            //deckTLC.X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2;

            //Point tableTLC = new Point(339, 225);
            //tableTLC.X = _tableCenter + CardSpacing / 2;

            int column = -1;

            if (_state == GameState.Playing)
            {
                Point playerCardPoint = new Point();

                int center = _tableCenter; // tableTLC.X + (CardCanvas.DefaultWidth / 2);

                playerCardPoint.X = center - ((((_data.PlayerHand.Cards.Count - 1) * CardSpacing) + CardCanvas.DefaultWidth) / 2);
                playerCardPoint.Y = PLAYER_ROW;

                // special case if on last card
                if ((mouseX > (CardSpacing * hand.Length) + playerCardPoint.X) && (mouseX < (CardSpacing * (hand.Length - 1) + playerCardPoint.X) + CardCanvas.DefaultWidth))
                    column = hand.Length - 1;
                else if ((mouseX >= playerCardPoint.X) && (mouseX <= (CardSpacing * hand.Length) + playerCardPoint.X))
                    column = ((mouseX - playerCardPoint.X) / CardSpacing);

                if (column >= _data.PlayerHand.Cards.Count)
                {
                    column = -1;
                }

                if (column != -1 && _data.PlayerHand.Cards.Count > 0)
                {
                    if (_data.PlayerHand.Cards[column].Selected)
                    {
                        if ((mouseY < playerCardPoint.Y - SELECTED_OFFSET) || (mouseY > playerCardPoint.Y + CardCanvas.DefaultHeight - SELECTED_OFFSET))
                        {
                            column = -1;
                        }
                    }
                    else
                    {
                        if ((mouseY < playerCardPoint.Y) || (mouseY > playerCardPoint.Y + CardCanvas.DefaultHeight - SELECTED_OFFSET))
                        {
                            column = -1;
                        }
                    }
                }
            }

            return column;
        }
        #endregion

        private void optionsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (_currentPlayer != 0)
            {
                undoToolStripMenuItem.Enabled = false;
            }
            else
            {
                if (_undoPointer >= 0 && _lastCardsPlayed.Count > 0)
                {
                    undoToolStripMenuItem.Enabled = true;
                }
                else
                {
                    undoToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_undoPointer >= 0 && _currentPlayer == 0)
            {
                // remove last turn of cards
                int opponent = 2;
                for( int i=0; i<3; i++ )
                {
                    _data.DeckManager.Table.Remove(_lastCardsPlayed[_undoPointer]);
                    _data.OpponentsHands[opponent].Cards.Add(_lastCardsPlayed[_undoPointer]);
                    opponent--;
                    _undoPointer--;
                }

                _data.DeckManager.Table.Remove(_lastCardsPlayed[_undoPointer]);
                _data.PlayerHand.Cards.Add(_lastCardsPlayed[_undoPointer]);
                _undoPointer--;
            }
        }

        private void aboutCrazyEightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutForm = new AboutBox1();
            aboutForm.ShowDialog(this);
        }

        private void frmMain_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, "crazyeights.chm");
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "crazyeights.chm");
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelpIndex(this, "crazyeights.chm");
        }


        public void Timer_Callback(object state)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(SetPrompt));
            }
            else
            {
                SetPrompt();
            }

        }

        private void dealToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _state = GameState.StartGame;
        }
    }
}