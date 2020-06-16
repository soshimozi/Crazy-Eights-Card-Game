using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CrazyEights.Properties;
using CrazyEightsCardLib;
using Fireworks;
using Timer = System.Threading.Timer;

namespace CrazyEights
{
    public partial class FrmMain : Form
    {
        #region Private Delegates
        private delegate void ShowWildcardDelegate(CardSuit overrideSuit);
        private delegate void ShowHandWonDelegate(int winner);
        private delegate void ShowGameWonDelegate(int winner);
        private delegate void EmptyDelegate();
        private delegate CardSuit GetSuitOverrideDelegate();
        #endregion

        #region Private Constants

        private const int CardSpacing = 18;
        private const int WaitLength = 750;
        private const int FrameWaitLength = 100000;

        private const int TicksPerSecond = 10000000;

        private const int FireworksLaunchTime = TicksPerSecond * 2;
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

        #endregion

        #region Private Instance Fields
        private readonly ManualResetEvent _fireworksEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _stopGameEvent = new ManualResetEvent(false);

        private Hand.PlayDirection _direction;
        private GameData _data;
        private GameState _state;
        private string _prompt = string.Empty;
        private bool _mouseUp;
        private bool _deckSelected;
        private CardSuit _suitOverride = CardSuit.None;
        //private bool _wildCard = false;
        private bool _playerWon;
        private bool _playerHasDrawn;

        private int _winningOpponent;
        private int _currentPlayer;
        private int _startingPlayer;
        private bool _skipFlag;

        private int _undoPointer;

        private readonly CardCanvas _cardCanvas = new CardCanvas();

        private int _numOpponents = 3;

        private int _lastMouseX;
        private int _lastMouseY;
        private MouseButtons _lastMouseButton = MouseButtons.None;

        private Card _lastSelectedCard;
        private readonly List<Card> _lastCardsPlayed = new List<Card>();
        private readonly List<Card> _lastDrawnCards = new List<Card>();
        private readonly int[] _scores = { 0, 0, 0, 0 };
        private readonly string[] _playerNames = { "Patrick", "Betty", "Roberto" };

        private readonly RocketQueue _rocketQueue = new RocketQueue(new Random(), new Size(0, 0));
        private readonly SparkQueue _sparkQueue = new SparkQueue();
        #endregion

        #region Constructors
        public FrmMain()
        {
            InitializeComponent();

            FormClosed += Form1_FormClosed;

            _data = new GameData(_numOpponents, 1);

            SetStyle(
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.DoubleBuffer, true);

            _state = GameState.NotPlaying;

            _rocketQueue.setCanvas(Size);

            //_rocketQueue = new RocketQueue(new Random(), Size);

            _direction = Hand.PlayDirection.Forward;

            var gameLoopThread = new Thread(GameLoop);
            gameLoopThread.Start();

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
                        PaintPrompt(e.Graphics, _currentPlayer == 0 ? Brushes.White : Brushes.Yellow);
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
        private const int OpponentOneTop = 225;
        private const int OpponentTwoTop = 30;
        private const int OpponentThreeTop = 225;
        private const int CardsPerHand = 5;
        private const int SelectedOffset = 10;
        private const int ScoreWidth = 250;
        private const int ScoreHeight = 54;
        private const int PlayerRow = 403;
        private const int HandMaxWidth = 180;

        private int _borderWidth = 2;
        //private Point _tableTLC = new Point(339, 225);
        //private Point _deckTLC = new Point(258, 225);
        private int _tableCenter;

        private readonly Font _gameOverFont = new Font(FontFamily.GenericSansSerif, 36.0f, FontStyle.Bold, GraphicsUnit.Point);
        private readonly Font _overrideFont = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold, GraphicsUnit.Point);
        private readonly Font _promptFont = new Font(FontFamily.GenericSansSerif, 14.0f, FontStyle.Bold, GraphicsUnit.Point);
        private readonly Font _nameFont = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold, GraphicsUnit.Point);
        private readonly Font _directionFont = new Font(FontFamily.GenericSansSerif, 8.0f, FontStyle.Bold, GraphicsUnit.Point);
        private Font _scoreFont = new Font(FontFamily.GenericSansSerif, 7.0f, FontStyle.Bold, GraphicsUnit.Point);
        private readonly Color _scoreBackgroundColor = Color.FromArgb(64, 255, 255, 255);
        private readonly Color _scoreBorderColor = Color.FromArgb(255, 0, 32, 0);
        private readonly Color _scoreForegroundColor = Color.FromArgb(255, 255, 255, 255);


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
            var text = _playerWon ? "You Won!" : "Game Over";

            var textSize = graphics.MeasureString(text, _gameOverFont);
            graphics.DrawString(text, _gameOverFont, Brushes.Yellow, new PointF((Width - textSize.Width)/2, (Height - textSize.Height)/2));

            // only paint sparks if window is not minimized
            if (WindowState == FormWindowState.Minimized) return;
            _sparkQueue.CullDeadSparks(true, true);
            _rocketQueue.makeSparks(_sparkQueue);
            _sparkQueue.Paint(graphics);
        }

        private void ShowDrawTwoDialog()
        {
            if (InvokeRequired)
            {
                Invoke(new EmptyDelegate(ShowDrawTwoDialog));
            }
            else
            {
                MessageBox.Show(this, @"Draw Two!", @"Crazy Eights");
            }
        }

        private void ShowDrawFourDialog()
        {
            if (InvokeRequired)
            {
                Invoke(new EmptyDelegate(ShowDrawFourDialog));
            }
            else
            {
                MessageBox.Show(this, @"Draw Four!", @"Crazy Eights");
            }
        }
        private void ShowWildcardDialog(CardSuit overrideSuit)
        {
            if( InvokeRequired )
            {
                Invoke( new ShowWildcardDelegate(ShowWildcardDialog), overrideSuit);
            }
            else
            {
                MessageBox.Show(this, string.Format(Resources.Caption_OverrideSuit, overrideSuit.ToString()), Resources.Caption_CrazyEights);
            }
        }

        private void ShowGameWonDialog(int winner)
        {
            if (InvokeRequired)
            {
                Invoke(new ShowGameWonDelegate(ShowGameWonDialog), winner);
            }
            else
            {
                MessageBox.Show(
                    winner == 0
                        ? Resources.Text_YouWonGame
                        : string.Format(Resources.Text_OpponentWonGame, _playerNames[winner - 1]),
                    Resources.Caption_CrazyEights);
            }
        }

        private void ShowHandWonDialog(int winner)
        {
            if (InvokeRequired)
            {
                Invoke(new ShowHandWonDelegate(ShowHandWonDialog), winner);
            }
            else
            {
                MessageBox.Show(
                    winner == 0
                        ? Resources.Text_YouWonHand
                        : string.Format(Resources.Text_OpponentWonHand, _playerNames[winner - 1]),
                    Resources.Caption_CrazyEights);
            }
        }

        private void PaintScore(Graphics graphics)
        {
            Rectangle scoreRect = new Rectangle(_tableCenter - (ScoreWidth / 2), PlayerRow + CardCanvas.DefaultHeight + (CardSpacing / 2), ScoreWidth, ScoreHeight);
            Rectangle clientRect = ShrinkRect(scoreRect, 1);

            PaintScoreBox(graphics, scoreRect, clientRect);

            if (_state == GameState.GameOver && !_playerWon)
                _scoreFont = new Font(_scoreFont, FontStyle.Strikeout);
            else
                _scoreFont = new Font(_scoreFont, FontStyle.Bold);

            string text = Resources.Caption_Score;
            SizeF textSize = graphics.MeasureString(text, _scoreFont);
            graphics.DrawString(string.Format(Resources.Text_ScoreDisplay, "You", _scores[0]), _scoreFont, new SolidBrush(_scoreForegroundColor), new Point(clientRect.X, clientRect.Y + 8));

            if (_state == GameState.GameOver && (_winningOpponent != 0 || _playerWon))
                _scoreFont = new Font(_scoreFont, FontStyle.Strikeout);
            else
                _scoreFont = new Font(_scoreFont, FontStyle.Bold);

            graphics.DrawString(string.Format(Resources.Text_ScoreDisplay, _playerNames[0], _scores[1]), _scoreFont, new SolidBrush(_scoreForegroundColor), new Point(clientRect.X, clientRect.Y + clientRect.Height - 16));

            if (_state == GameState.GameOver && (_winningOpponent != 1 || _playerWon))
                _scoreFont = new Font(_scoreFont, FontStyle.Strikeout);
            else
                _scoreFont = new Font(_scoreFont, FontStyle.Bold);

            string scoreText = string.Format(Resources.Text_ScoreDisplay, _playerNames[1], _scores[2]);
            SizeF scoreTextSize = graphics.MeasureString(scoreText, _scoreFont);
            graphics.DrawString(scoreText, _scoreFont, new SolidBrush(_scoreForegroundColor), new PointF((clientRect.X + clientRect.Width) - scoreTextSize.Width - 1, clientRect.Y + 8));

            if (_state == GameState.GameOver && (_winningOpponent != 2 || _playerWon))
                _scoreFont = new Font(_scoreFont, FontStyle.Strikeout);
            else
                _scoreFont = new Font(_scoreFont, FontStyle.Bold);

            scoreText = string.Format(Resources.Text_ScoreDisplay, _playerNames[2], _scores[3]);
            scoreTextSize = graphics.MeasureString(scoreText, _scoreFont);
            graphics.DrawString(scoreText, _scoreFont, new SolidBrush(_scoreForegroundColor), new PointF((clientRect.X + clientRect.Width) - scoreTextSize.Width - 1, ((clientRect.Y + clientRect.Height) - 16)));

            _scoreFont = new Font(_scoreFont, FontStyle.Bold);
            graphics.DrawString(Resources.Caption_Score, _scoreFont, new SolidBrush(_scoreForegroundColor),
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
                var suitImage = GetSuitImage(_suitOverride);

                var text = string.Format(Resources.Caption_OverrideSuit, "");
                var textSize = graphics.MeasureString(text, _overrideFont);

                var deckTlc = new Point(258, 225) {X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2};

                Image directionImage = (_direction == Hand.PlayDirection.Forward ? Resources.PNG_Forward : Resources.PNG_Reverse);
                var deckRow = (float)(deckTlc.Y + CardCanvas.DefaultHeight + _borderWidth + (directionImage.Height * 1.5));

                var size = textSize.Width + suitImage.Width;
                var leftEdge = _tableCenter - (size / 2);
                graphics.DrawString(text, _overrideFont, Brushes.White, new PointF(leftEdge, deckRow));

              //textPoint.X += textSize.Width;

              if (_overrideFont == null) return;
              var centerPoint = deckRow + (_overrideFont.Height/2.0f) - (suitImage.Height/2.0f);
              graphics.DrawImage(suitImage, new PointF(leftEdge + textSize.Width, centerPoint));
            }
        }

        private void PaintDirection(Graphics graphics)
        {
            var deckTlc = new Point(258, 225) {X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2};

            var y = deckTlc.Y + CardCanvas.DefaultHeight + _borderWidth;
            var directionImage = (_direction == Hand.PlayDirection.Forward ? Resources.PNG_Forward : Resources.PNG_Reverse);

            var textSize = graphics.MeasureString("Direction", _directionFont);

            var renderPoint = new PointF(_tableCenter - (directionImage.Width + textSize.Width + 5)/ 2, y);
            graphics.DrawString("Direction", _directionFont, Brushes.GhostWhite, new PointF(renderPoint.X, y + ((directionImage.Height - textSize.Height) / 2)));
            graphics.DrawImage(directionImage, new PointF(renderPoint.X + textSize.Width + 5, renderPoint.Y));

        }

        private static Image GetSuitImage(CardSuit suit)
        {
            Image suitImage;
            switch (suit)
            {
                case CardSuit.Diamonds:
                    suitImage = Resources.PNG_DiamondsSuit;
                    break;

                case CardSuit.Hearts:
                    suitImage = Resources.PNG_HeartsSuit;
                    break;

                case CardSuit.Spades:
                    suitImage = Resources.PNG_SpadesSuit;
                    break;

                case CardSuit.Clubs:
                    suitImage = Resources.PNG_ClubsSuit;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(suit), suit, null);
            }

            return suitImage;
        }

        private void PaintPrompt(Graphics g, Brush brush)
        {

            var deckTlc = new Point(258, 225) {X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2};

            if (string.IsNullOrEmpty(_prompt)) return;
            var textSize = g.MeasureString(_prompt, _promptFont);
            g.DrawString(_prompt, _promptFont, brush, new PointF(_tableCenter - textSize.Width/2, deckTlc.Y - 50));
        }

        private void PaintDeck()
        {
            //_deckTLC.Y = (Size.Height - CardCanvas.DefaultHeight) / 2;
            var deckTlc = new Point(258, 225) {X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2};

            _cardCanvas.DrawCardBack(deckTlc, !_data.DeckManager.IsEmpty ? GetSelectedBack() : CardBack.The_X);
        }


        private void PaintDeckSelection(Graphics graphics)
        {
            var deckTlc = new Point(258, 225) {X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2};
            if (!_deckSelected) return;

            graphics.DrawRectangle(Pens.Gold, ShrinkRect(new Rectangle(deckTlc, new Size(CardCanvas.DefaultWidth, CardCanvas.DefaultHeight)), -1));
            graphics.DrawRectangle(Pens.Gold, ShrinkRect(new Rectangle(deckTlc, new Size(CardCanvas.DefaultWidth, CardCanvas.DefaultHeight)), -2));
        }


        private void PaintTable()
        {
            var tableTlc = new Point(339, 225) {X = _tableCenter + CardSpacing / 2};
            if (_data.DeckManager.Table.Count > 0)
            {
                var topCard = _data.DeckManager.Table[_data.DeckManager.Table.Count - 1];
                _cardCanvas.DrawCard(tableTlc, topCard.CardIndex);
            }
            else
            {
                _cardCanvas.DrawCardBack(tableTlc, CardBack.The_O);
            }
        }

        private void PaintOpponentHands()
        {
            var deckTlc = new Point(258, 225) {X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2};

            var tableTlc = new Point(339, 225) {X = _tableCenter + CardSpacing / 2};

            var opponent1Tlc = new Point(20, OpponentOneTop);
            var opponent2Tlc = new Point();
            var opponent3Tlc = new Point();

            opponent2Tlc.Y = OpponentTwoTop;
            opponent3Tlc.Y = OpponentThreeTop;

            var center = _tableCenter; // _tableTLC.X + (CardCanvas.DefaultWidth / 2);
            opponent2Tlc.X = center - ((((_data.OpponentsHands[1].Cards.Count - 1) * CardSpacing) + CardCanvas.DefaultWidth) / 2);

            opponent1Tlc.X = deckTlc.X - (CardSpacing * 5) - HandMaxWidth;

            if (opponent1Tlc.X < 20)
            {
                opponent1Tlc.X = 20;
            }
            int minX = tableTlc.X + CardCanvas.DefaultWidth + CardSpacing;
            opponent3Tlc.X = tableTlc.X + CardCanvas.DefaultHeight + (CardSpacing * 5); // -(((_data.OpponentsHands[2].Cards.Count - 1) * CardSpacing) + CardCanvas.DefaultWidth);
            if (opponent3Tlc.X < minX)
            {
                opponent3Tlc.X = minX;
            }
        
            // array of locations for opponent hands
            var opponentTlc = new[] {opponent1Tlc, opponent2Tlc, opponent3Tlc};
            var maxWidths = new[] { HandMaxWidth, Width, HandMaxWidth };

            // draw each opponent hands
            for( var i=0; i<3; i++)
            {
                if (_data.OpponentsHands[i].Cards.Count > 0)
                {
                    PaintHand(_data.OpponentsHands[i].Cards.ToArray(), opponentTlc[i], false, maxWidths[i]);
                }
            }

        }

        private void PaintPlayerNames(Graphics graphics)
        {
            var deckTlc = new Point(258, 225) {X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2};

            var tableTlc = new Point(339, 225) {X = _tableCenter + CardSpacing / 2};

            var playerNameText = _playerNames[0];
            var playerNameSize = graphics.MeasureString(playerNameText, _nameFont);

            float leftEdge = deckTlc.X - (CardSpacing * 5) - HandMaxWidth;
            leftEdge += ((HandMaxWidth - playerNameSize.Width) / 2);

            var brush = _currentPlayer == 1 ? Brushes.Yellow : Brushes.White;

            graphics.DrawString(playerNameText, _nameFont, brush, new PointF(leftEdge, (OpponentOneTop - playerNameSize.Height)));

            playerNameText = _playerNames[1];
            playerNameSize = graphics.MeasureString(playerNameText, _nameFont);

            var center = _tableCenter; // tableTLC.X + (CardCanvas.DefaultWidth / 2);
            leftEdge = center - (playerNameSize.Width / 2);

            brush = _currentPlayer == 2 ? Brushes.Yellow : Brushes.White;

            graphics.DrawString(playerNameText, _nameFont, brush, new PointF(leftEdge, (OpponentTwoTop + CardCanvas.DefaultHeight)));

            playerNameText = _playerNames[2];
            playerNameSize = graphics.MeasureString(playerNameText, _nameFont);

            leftEdge = tableTlc.X + CardCanvas.DefaultHeight + (CardSpacing * 5);
            leftEdge += ((HandMaxWidth - playerNameSize.Width) / 2);

            brush = _currentPlayer == 3 ? Brushes.Yellow : Brushes.White;

            graphics.DrawString(playerNameText, _nameFont, brush, new PointF(leftEdge, (OpponentThreeTop - playerNameSize.Height)));
        }


        private void PaintPlayerHand()
        {
            Point playerCardPoint = new Point();

            int center = _tableCenter; // _tableTLC.X + (CardCanvas.DefaultWidth / 2);

            playerCardPoint.X = center - ((((_data.PlayerHand.Cards.Count - 1) * CardSpacing) + CardCanvas.DefaultWidth) / 2);
            playerCardPoint.Y = PlayerRow;
            if (_data.PlayerHand.Cards.Count > 0)
            {
                PaintHand(_data.PlayerHand.Cards.ToArray(), playerCardPoint, true, -1);
            }
        }

        private void PaintHand(Card[] hand, Point tlc, bool showCard, int maxWidth)
        {
            int currentX = tlc.X;
            int currentY = tlc.Y;
            foreach (var t in hand)
            {
                if (maxWidth >= 0 && Math.Abs((currentX + CardCanvas.DefaultWidth)- tlc.X) > maxWidth)
                {
                    currentY += CardSpacing;
                    currentX = tlc.X;
                }

                var cardPoint = new Point(currentX, currentY);
                if (showCard)
                {
                    if (t.Selected)
                    {
                        cardPoint.Y -= SelectedOffset;
                    }
                    _cardCanvas.DrawCard(cardPoint, t.CardIndex);
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
            StartFireworksThread();
            //AskStartGame();
        }

        private void AskStartGame()
        {
            bool startGame = true;
            if (_state == GameState.Playing)
            {
                if (MessageBox.Show(Resources.Prompt_AbandonGame, Resources.Caption_GameInProgress, MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    startGame = false;
                }
            }

            if (startGame)
            {
                _state = GameState.StartGame;
            }
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
                if (MessageBox.Show(this, Resources.Prompt_EndGame, Resources.Caption_CrazyEights, MessageBoxButtons.YesNoCancel, MessageBoxIcon.None) != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmMain_MouseMove(object sender, MouseEventArgs e)
        {
            _lastMouseX = e.X;
            _lastMouseY = e.Y;
        }

        private void cardBackToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // find any other checked item
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
        private void InitializeHand()
        {
            var done = false;

            while (!done)
            {
                _data.PlayerHand.Cards.Clear();
                _data.DeckManager.Shuffle();

                foreach (var t in _data.OpponentsHands)
                {
                    t.Cards.Clear();
                }

                var hands = new Hand[_data.OpponentsHands.Length + 1];
                _data.OpponentsHands.CopyTo(hands, 0);
                hands[_data.OpponentsHands.Length] = _data.PlayerHand;

                _data.DeckManager.Deal(hands, CardsPerHand);

                if (_data.DeckManager.Table[0] != null && _data.DeckManager.Table[0].Rank != SpecialCard.WildCard)
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
            var thread = new Thread(FireworksThread);
            thread.Start();
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

                if ((DateTime.Now.Ticks - timerTick) > FireworksLaunchTime)
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
            long lastFrameTime = 0;
            do
            {
                if ((DateTime.Now.Ticks - lastFrameTime) > FrameWaitLength)
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
                                InitializeHand();
                                _state = GameState.Playing;
                                break;
                        }

                        lastFrameTime = DateTime.Now.Ticks;
                    }
                }

                Thread.Sleep(0);

            } while (!_stopGameEvent.WaitOne(10, true));
        }

        private void OpponentTurn()
        {
            SetPrompt();

            ClearMouseButtons();
            ClearSelectedCard();
            _deckSelected = false;

            Invalidate();

            // wait - simulate thinking
            Thread.Sleep(WaitLength);

            // send table to ai, with current hand
            // to evaluate next move, which will be added
            // to the table by the ai
            AiManager.MoveInfo info = AiManager.EvaluateMove(_data.DeckManager, _data.OpponentsHands[_currentPlayer - 1], _suitOverride);

            if (info.DrawCard)
            {
                if (DrawCard(_currentPlayer, out _))
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
            _prompt = _currentPlayer == 0 ? Resources.Caption_YourTurn : string.Format(Resources.Text_OpponentTurn, _playerNames[_currentPlayer - 1]);
        }

        private int NextPlayer()
        {
            var player = _currentPlayer;
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

            if (!_skipFlag) return player;
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

            return player;
        }

        private void PlayerTurn()
        {
            //SetPrompt();

            if (CardHitCheck(out var selectedColumn))
            {
                // bingo, play card
                if (CheckValidCard(_data.PlayerHand.Cards[selectedColumn]))
                {
                    _suitOverride = CardSuit.None;
                    if (_data.PlayerHand.Cards[selectedColumn].Rank == SpecialCard.WildCard)
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
                    _prompt = Resources.Text_InvalidCard;

                    var t = new Timer(Timer_Callback);
                    t.Change(2000, Timeout.Infinite);
                }
            }
            else if (DeckHitCheck())
            {
                if (DrawCard(0, out _))
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
            var deckTlc = new Point(258, 225) {X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2};

            //var point = new Point(339, 225) {X = _tableCenter + CardSpacing / 2};

            var hitCheck = false;
            if (!_mouseUp || _lastMouseButton != MouseButtons.Left || _state != GameState.Playing) return false;
            // check for deck hit
            if (_lastMouseX >= deckTlc.X && _lastMouseX <= deckTlc.X + CardCanvas.DefaultWidth && _lastMouseY >= deckTlc.Y && _lastMouseY <= deckTlc.Y + CardCanvas.DefaultHeight)
            {
                hitCheck = true;
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
            var deckTlc = new Point(258, 225) {X = _tableCenter - CardCanvas.DefaultWidth - CardSpacing / 2};

            //var point = new Point(339, 225) {X = _tableCenter + CardSpacing / 2};

            var hand = _data.PlayerHand.Cards.ToArray();
            var column = GetCardColumn(hand, _lastMouseX, _lastMouseY);

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
            if (_lastMouseX >= deckTlc.X && _lastMouseX <= deckTlc.X + CardCanvas.DefaultWidth && _lastMouseY >= deckTlc.Y && _lastMouseY <= deckTlc.Y + CardCanvas.DefaultHeight)
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
            var validPlay = false;

            var topCard = _data.DeckManager.Table[_data.DeckManager.Table.Count - 1];

            // if there is only one top card, then we match normal
            if (_data.DeckManager.Table.Count == 1)
            {
                // if an eight, make sure we don't have any other suits
                if (cardToPlay.Rank == SpecialCard.WildCard)
                {
                    //var suitCount = _data.PlayerHand.Cards.Count(c => c.Suit == topCard.Suit && c.Rank != SpecialCard.WildCard);

                    // if there are no other suits but the eight, then we have a valid play
                    // otherwise, we should play one of the other suits
                    //if (suitCount == 0)
                    //{
                    //    validPlay = true;
                    //}

                    validPlay = true;
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
            else if (topCard.Rank == SpecialCard.WildCard)
            {
                if (cardToPlay.Rank == SpecialCard.WildCard || cardToPlay.Suit == _suitOverride)
                {
                    validPlay = true;
                }
            }
            else if (topCard.Rank == SpecialCard.DrawTwo && !_playerHasDrawn)
            {
                if (cardToPlay.Rank == SpecialCard.DrawTwo)
                {
                    validPlay = true;
                }
            }
            else
            {
                if (cardToPlay.Rank == SpecialCard.WildCard)
                {
                    //var suitCount = _data.PlayerHand.Cards.Count(c => c.Suit == topCard.Suit && c.Rank != SpecialCard.WildCard);

                    // if there are no other suits but the eight, then we have a valid play
                    // otherwise, we should play one of the other suits
                    //if (suitCount == 0)
                    //{
                    //    validPlay = true;
                    //}

                    validPlay = true;
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
            if (InvokeRequired)
            {
                selectedSuit = (CardSuit)Invoke(new GetSuitOverrideDelegate(GetSuitOverride));
            }
            else
            {
                var dlg = new SuitOverrideDialog {StartPosition = FormStartPosition.CenterParent};

                if (dlg.ShowDialog(this) == DialogResult.OK)
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

            if (cardToPlay.Rank == SpecialCard.Reverse)
            {
                _direction = _direction == Hand.PlayDirection.Forward ? Hand.PlayDirection.Reverse : Hand.PlayDirection.Forward;
            }
            else if (cardToPlay.Rank == SpecialCard.DrawFour && !_data.DeckManager.IsEmpty)
            {
                _playerHasDrawn = false;

                // increment the two's count
                //_deuceCount++;

                // draw two
                var nextPlayer = NextPlayer();

                // get hand of next player
                //var nextPlayerHand = nextPlayer == 0 ? _data.PlayerHand : _data.OpponentsHands[nextPlayer - 1];

                //if (nextPlayerHand.Cards.Find(c => c.Rank == SpecialCard.DrawTwo) != null) return;
                _lastDrawnCards.Clear();

                for (int i = 0; i < 4; i++)
                {
                    DrawCard(nextPlayer, out var drawCard);

                    _lastDrawnCards.Add(drawCard);
                }

                _playerHasDrawn = true;

                // if next is player and player doesn't have any two's
                if (nextPlayer == 0)
                {
                    ShowDrawFourDialog();
                }

            }
            else if (cardToPlay.Rank == SpecialCard.DrawTwo && !_data.DeckManager.IsEmpty)
            {
                _playerHasDrawn = false;

                // increment the two's count
                //_deuceCount++;

                // draw two
                var nextPlayer = NextPlayer();

                // get hand of next player
                //var nextPlayerHand = nextPlayer == 0 ? _data.PlayerHand : _data.OpponentsHands[nextPlayer - 1];

                //if (nextPlayerHand.Cards.Find(c => c.Rank == SpecialCard.DrawTwo) != null) return;
                _lastDrawnCards.Clear();

                for (int i = 0; i < 2; i++)
                {
                    DrawCard(nextPlayer, out var drawCard);

                    _lastDrawnCards.Add(drawCard);

                }

                _playerHasDrawn = true;

                // if next is player and player doesn't have any two's
                if (nextPlayer == 0)
                {
                    ShowDrawTwoDialog();
                }

                //_deuceCount = 0;
            }
            else if (cardToPlay.Rank == SpecialCard.Queen)
            {
                // trick here, increment player so when
                // we increment next player, it will skip
                _skipFlag = true;
            }

            //_redoPointer = _lastCardsPlayed.Count;
        }

        private void EndHand()
        {
            // force a redraw before displaying message box
            Invalidate();

            ShowHandWonDialog(_currentPlayer);

            var winner = false;

            // check if top card is a 2, then make sure we subtract last drawn cards from current player
            var topCard = _data.DeckManager.Table[_data.DeckManager.Table.Count - 1];

            if (topCard.Rank == SpecialCard.DrawTwo)
            {
                Hand hand = _currentPlayer == 0 ? _data.PlayerHand : _data.OpponentsHands[_currentPlayer - 1];

                foreach (var card in _lastDrawnCards)
                {
                    hand.Cards.Remove(card);
                }
            }

            // update scores from each hand
            for (var i = 0; i < 4; i++)
            {
                if (i != _currentPlayer)
                {
                    // get hand score
                    _scores[i] += ScoreHand(i);
                }
            }

            // now check if anyone is over 500
            for (var i = 0; i < 4; i++)
            {
                if (_scores[i] >= 500)
                {
                    winner = true;
                }
            }

            if (winner)
            {
                var winningPlayer = 0;
                var lowestScore = 500;
                for (var i = 0; i < 4; i++)
                {
                    // lowest player wins
                    if (_scores[i] >= lowestScore) continue;
                    lowestScore = _scores[i];
                    winningPlayer = i;
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
                    _winningOpponent = winningPlayer - 1;
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
                InitializeHand();
            }
        }

        private int ScoreHand(int handIndex)
        {
            var score = 0;
            var cards = handIndex == 0 ? _data.PlayerHand.Cards.ToArray() : _data.OpponentsHands[handIndex - 1].Cards.ToArray();

            foreach (var card in cards)
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

        //private bool PointInRect(RectangleF rect, Point point)
        //{
        //    if (point.X >= rect.X && point.X <= (rect.X + rect.Width) && point.Y >= rect.Y && point.Y <= (rect.Y + rect.Height))
        //        return true;
        //    return false;
        //}

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
                playerCardPoint.Y = PlayerRow;

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
                        if ((mouseY < playerCardPoint.Y - SelectedOffset) || (mouseY > playerCardPoint.Y + CardCanvas.DefaultHeight - SelectedOffset))
                        {
                            column = -1;
                        }
                    }
                    else
                    {
                        if ((mouseY < playerCardPoint.Y) || (mouseY > playerCardPoint.Y + CardCanvas.DefaultHeight - SelectedOffset))
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