#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CardGame_Prototype_01;

public class Game1 : Game
{
    private const int CardWidth = 90;
    private const int CardHeight = 122;
    private const int TableauStartX = 52;
    private const int TableauStepX = 112;
    private const int TopRowY = 96;
    private const int TableauStartY = 274;

    private static readonly Color Felt = new(24, 91, 70);
    private static readonly Color FeltLight = new(31, 103, 78);
    private static readonly Color CardFace = new(250, 248, 239);
    private static readonly Color CardOutline = new(217, 220, 210);
    private static readonly Color CardRed = new(185, 55, 56);
    private static readonly Color CardBlack = new(36, 43, 46);
    private static readonly Color Gold = new(235, 193, 111);
    private static readonly Color Muted = new(192, 213, 201);
    private static readonly Color SlotColor = new(18, 70, 56);
    private static readonly Dictionary<char, string[]> Glyphs = new()
    {
        ['A'] = ["01110", "10001", "10001", "11111", "10001", "10001", "10001"],
        ['B'] = ["11110", "10001", "10001", "11110", "10001", "10001", "11110"],
        ['C'] = ["01111", "10000", "10000", "10000", "10000", "10000", "01111"],
        ['D'] = ["11110", "10001", "10001", "10001", "10001", "10001", "11110"],
        ['E'] = ["11111", "10000", "10000", "11110", "10000", "10000", "11111"],
        ['F'] = ["11111", "10000", "10000", "11110", "10000", "10000", "10000"],
        ['G'] = ["01111", "10000", "10000", "10111", "10001", "10001", "01111"],
        ['H'] = ["10001", "10001", "10001", "11111", "10001", "10001", "10001"],
        ['I'] = ["11111", "00100", "00100", "00100", "00100", "00100", "11111"],
        ['J'] = ["00111", "00010", "00010", "00010", "10010", "10010", "01100"],
        ['K'] = ["10001", "10010", "10100", "11000", "10100", "10010", "10001"],
        ['L'] = ["10000", "10000", "10000", "10000", "10000", "10000", "11111"],
        ['M'] = ["10001", "11011", "10101", "10101", "10001", "10001", "10001"],
        ['N'] = ["10001", "11001", "10101", "10011", "10001", "10001", "10001"],
        ['O'] = ["01110", "10001", "10001", "10001", "10001", "10001", "01110"],
        ['P'] = ["11110", "10001", "10001", "11110", "10000", "10000", "10000"],
        ['Q'] = ["01110", "10001", "10001", "10001", "10101", "10010", "01101"],
        ['R'] = ["11110", "10001", "10001", "11110", "10100", "10010", "10001"],
        ['S'] = ["01111", "10000", "10000", "01110", "00001", "00001", "11110"],
        ['T'] = ["11111", "00100", "00100", "00100", "00100", "00100", "00100"],
        ['U'] = ["10001", "10001", "10001", "10001", "10001", "10001", "01110"],
        ['V'] = ["10001", "10001", "10001", "10001", "10001", "01010", "00100"],
        ['W'] = ["10001", "10001", "10001", "10101", "10101", "10101", "01010"],
        ['X'] = ["10001", "10001", "01010", "00100", "01010", "10001", "10001"],
        ['Y'] = ["10001", "10001", "01010", "00100", "00100", "00100", "00100"],
        ['Z'] = ["11111", "00001", "00010", "00100", "01000", "10000", "11111"],
        ['0'] = ["01110", "10001", "10011", "10101", "11001", "10001", "01110"],
        ['1'] = ["00100", "01100", "00100", "00100", "00100", "00100", "01110"],
        ['2'] = ["01110", "10001", "00001", "00010", "00100", "01000", "11111"],
        ['3'] = ["11110", "00001", "00001", "01110", "00001", "00001", "11110"],
        ['4'] = ["00010", "00110", "01010", "10010", "11111", "00010", "00010"],
        ['5'] = ["11111", "10000", "10000", "11110", "00001", "00001", "11110"],
        ['6'] = ["01110", "10000", "10000", "11110", "10001", "10001", "01110"],
        ['7'] = ["11111", "00001", "00010", "00100", "01000", "01000", "01000"],
        ['8'] = ["01110", "10001", "10001", "01110", "10001", "10001", "01110"],
        ['9'] = ["01110", "10001", "10001", "01111", "00001", "00001", "01110"],
        [':'] = ["00000", "00100", "00100", "00000", "00100", "00100", "00000"],
        ['.'] = ["00000", "00000", "00000", "00000", "00000", "00110", "00110"],
        ['-'] = ["00000", "00000", "00000", "11111", "00000", "00000", "00000"],
        ['!'] = ["00100", "00100", "00100", "00100", "00100", "00000", "00100"]
    };

    private readonly GraphicsDeviceManager _graphics;
    private readonly List<Card> _stock = [];
    private readonly List<Card> _waste = [];
    private readonly List<Card>[] _tableau = Enumerable.Range(0, 7).Select(_ => new List<Card>()).ToArray();
    private readonly List<Card>[] _foundations = Enumerable.Range(0, 4).Select(_ => new List<Card>()).ToArray();
    private SpriteBatch _spriteBatch = null!;
    private Texture2D _pixel = null!;
    private MouseState _previousMouse;
    private KeyboardState _previousKeyboard;
    private Selection? _selection;
    private string _status = "CLICK THE STOCK PILE TO DEAL A CARD";

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 800;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.Title = "Solitaire";
    }

    protected override void Initialize()
    {
        NewGame();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    protected override void Update(GameTime gameTime)
    {
        var mouse = Mouse.GetState();
        var keyboard = Keyboard.GetState();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        if (keyboard.IsKeyDown(Keys.R) && _previousKeyboard.IsKeyUp(Keys.R))
        {
            NewGame();
        }

        if (mouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released)
        {
            HandleClick(mouse.X, mouse.Y);
        }

        _previousMouse = mouse;
        _previousKeyboard = keyboard;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Felt);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        Fill(new Rectangle(0, 0, 1280, 64), new Color(19, 76, 60));
        Fill(new Rectangle(0, 64, 1280, 2), new Color(45, 119, 91));
        DrawText("SOLITAIRE", 52, 20, Gold, 3);
        DrawText("KLONDIKE  /  BUILD FOUR SUIT SEQUENCES", 245, 28, Muted, 1);
        DrawText("NEW GAME  R", 1084, 27, Gold, 1);

        DrawText("STOCK", 52, 76, Muted, 1);
        DrawText("WASTE", 164, 76, Muted, 1);
        DrawText("FOUNDATIONS", 806, 76, Muted, 1);
        DrawSlot(52, TopRowY, _stock.Count == 0);
        DrawSlot(164, TopRowY, _waste.Count == 0);
        for (var i = 0; i < 4; i++)
        {
            DrawSlot(806 + i * 110, TopRowY, _foundations[i].Count == 0);
        }

        if (_stock.Count > 0)
        {
            DrawCard(_stock[^1], 52, TopRowY, false);
        }

        if (_waste.Count > 0)
        {
            DrawCard(_waste[^1], 164, TopRowY, IsSelected(SourceKind.Waste, 0));
        }

        for (var i = 0; i < 4; i++)
        {
            if (_foundations[i].Count > 0)
            {
                DrawCard(_foundations[i][^1], 806 + i * 110, TopRowY, IsSelected(SourceKind.Foundation, i));
            }
        }

        DrawText("TABLEAU", 52, 238, Muted, 1);
        for (var column = 0; column < _tableau.Length; column++)
        {
            var x = TableauStartX + column * TableauStepX;
            var pile = _tableau[column];
            if (pile.Count == 0)
            {
                DrawSlot(x, TableauStartY, true);
                continue;
            }

            var y = TableauStartY;
            for (var cardIndex = 0; cardIndex < pile.Count; cardIndex++)
            {
                var selected = _selection is { Kind: SourceKind.Tableau } selection &&
                    selection.PileIndex == column && cardIndex >= selection.CardIndex;
                DrawCard(pile[cardIndex], x, y, selected);
                if (cardIndex < pile.Count - 1)
                {
                    y += pile[cardIndex].FaceUp ? 28 : 18;
                }
            }
        }

        Fill(new Rectangle(52, 742, 1176, 1), new Color(55, 127, 98));
        DrawText(_status, 52, 760, Gold, 1);
        DrawText("CLICK A CARD, THEN A DESTINATION  |  R TO RESTART", 720, 760, Muted, 1);
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void NewGame()
    {
        _stock.Clear();
        _waste.Clear();
        foreach (var pile in _tableau)
        {
            pile.Clear();
        }

        foreach (var pile in _foundations)
        {
            pile.Clear();
        }

        var deck = new List<Card>(52);
        foreach (var suit in Enum.GetValues<Suit>())
        {
            for (var rank = 1; rank <= 13; rank++)
            {
                deck.Add(new Card(rank, suit));
            }
        }

        for (var i = deck.Count - 1; i > 0; i--)
        {
            var swapIndex = Random.Shared.Next(i + 1);
            (deck[i], deck[swapIndex]) = (deck[swapIndex], deck[i]);
        }

        var nextCard = 0;
        for (var column = 0; column < _tableau.Length; column++)
        {
            for (var cardIndex = 0; cardIndex <= column; cardIndex++)
            {
                var card = deck[nextCard++];
                card.FaceUp = cardIndex == column;
                _tableau[column].Add(card);
            }
        }

        for (; nextCard < deck.Count; nextCard++)
        {
            deck[nextCard].FaceUp = false;
            _stock.Add(deck[nextCard]);
        }

        _selection = null;
        _status = "CLICK THE STOCK PILE TO DEAL A CARD";
    }

    private void HandleClick(int x, int y)
    {
        if (y < 64 && x > 1060)
        {
            NewGame();
            return;
        }

        if (IsInside(x, y, 52, TopRowY, CardWidth, CardHeight))
        {
            if (_selection is not null)
            {
                _selection = null;
            }

            if (_stock.Count > 0)
            {
                var card = _stock[^1];
                _stock.RemoveAt(_stock.Count - 1);
                card.FaceUp = true;
                _waste.Add(card);
                _status = "CHOOSE A CARD OR DEAL AGAIN";
            }
            else if (_waste.Count > 0)
            {
                for (var i = _waste.Count - 1; i >= 0; i--)
                {
                    var card = _waste[i];
                    card.FaceUp = false;
                    _stock.Add(card);
                }

                _waste.Clear();
                _status = "STOCK RECYCLED";
            }
            else
            {
                _status = "NO CARDS LEFT TO DEAL";
            }

            return;
        }

        if (_waste.Count > 0 && IsInside(x, y, 164, TopRowY, CardWidth, CardHeight))
        {
            SelectOrMove(new Selection(SourceKind.Waste, 0, _waste.Count - 1));
            return;
        }

        for (var i = 0; i < _foundations.Length; i++)
        {
            var foundationX = 806 + i * 110;
            if (IsInside(x, y, foundationX, TopRowY, CardWidth, CardHeight))
            {
                if (_selection is not null)
                {
                    TryMoveToFoundation(i);
                }
                else if (_foundations[i].Count > 0)
                {
                    SelectOrMove(new Selection(SourceKind.Foundation, i, _foundations[i].Count - 1));
                }

                return;
            }
        }

        for (var column = 0; column < _tableau.Length; column++)
        {
            var pileX = TableauStartX + column * TableauStepX;
            if (x < pileX || x >= pileX + CardWidth || y < TableauStartY)
            {
                continue;
            }

            var pile = _tableau[column];
            var clickedCardIndex = GetClickedCardIndex(pile, y);
            if (_selection is not null)
            {
                if (TryMoveToTableau(column))
                {
                    return;
                }

                if (clickedCardIndex >= 0 && pile[clickedCardIndex].FaceUp)
                {
                    SelectOrMove(new Selection(SourceKind.Tableau, column, clickedCardIndex));
                }
                else
                {
                    _status = "THAT MOVE IS NOT ALLOWED";
                }

                return;
            }

            if (clickedCardIndex >= 0 && pile[clickedCardIndex].FaceUp)
            {
                SelectOrMove(new Selection(SourceKind.Tableau, column, clickedCardIndex));
            }
            else if (pile.Count == 0)
            {
                _status = "SELECT A KING TO START AN EMPTY PILE";
            }

            return;
        }

        _selection = null;
    }

    private void SelectOrMove(Selection selection)
    {
        if (_selection == selection)
        {
            _selection = null;
            return;
        }

        _selection = selection;
        _status = "NOW CHOOSE A TABLEAU OR FOUNDATION";
    }

    private bool TryMoveToTableau(int destinationIndex)
    {
        if (_selection is not { } selection)
        {
            return false;
        }

        if (selection.Kind == SourceKind.Tableau && selection.PileIndex == destinationIndex)
        {
            _selection = null;
            return false;
        }

        var sourcePile = GetSourcePile(selection);
        if (sourcePile is null || selection.CardIndex < 0 || selection.CardIndex >= sourcePile.Count)
        {
            _selection = null;
            return false;
        }

        var destination = _tableau[destinationIndex];
        var movingCard = sourcePile[selection.CardIndex];
        if (destination.Count == 0)
        {
            if (movingCard.Rank != 13)
            {
                return false;
            }
        }
        else
        {
            var topCard = destination[^1];
            if (!topCard.FaceUp || IsRed(topCard.Suit) == IsRed(movingCard.Suit) ||
                topCard.Rank != movingCard.Rank + 1)
            {
                return false;
            }
        }

        MoveSelectedCards(sourcePile, selection.CardIndex, destination);
        return true;
    }

    private void TryMoveToFoundation(int destinationIndex)
    {
        if (_selection is not { } selection)
        {
            return;
        }

        var sourcePile = GetSourcePile(selection);
        if (sourcePile is null || selection.CardIndex != sourcePile.Count - 1)
        {
            _status = "ONLY A SINGLE TOP CARD CAN MOVE TO A FOUNDATION";
            return;
        }

        var card = sourcePile[selection.CardIndex];
        var destination = _foundations[destinationIndex];
        if (destination.Count == 0 ? card.Rank != 1 :
            destination[^1].Suit != card.Suit || destination[^1].Rank + 1 != card.Rank)
        {
            _status = "FOUNDATIONS BUILD UP BY SUIT, STARTING WITH AN ACE";
            return;
        }

        MoveSelectedCards(sourcePile, selection.CardIndex, destination);
    }

    private void MoveSelectedCards(List<Card> source, int cardIndex, List<Card> destination)
    {
        var movingCards = source.GetRange(cardIndex, source.Count - cardIndex);
        source.RemoveRange(cardIndex, source.Count - cardIndex);
        destination.AddRange(movingCards);
        FlipTableauTop(source);
        _selection = null;
        _status = _foundations.Sum(pile => pile.Count) == 52
            ? "YOU WIN!  PRESS R TO PLAY AGAIN"
            : "NICE MOVE";
    }

    private static void FlipTableauTop(List<Card> pile)
    {
        if (pile.Count > 0 && !pile[^1].FaceUp)
        {
            pile[^1].FaceUp = true;
        }
    }

    private List<Card>? GetSourcePile(Selection selection) => selection.Kind switch
    {
        SourceKind.Waste => _waste,
        SourceKind.Foundation => _foundations[selection.PileIndex],
        SourceKind.Tableau => _tableau[selection.PileIndex],
        _ => null
    };

    private bool IsSelected(SourceKind kind, int pileIndex) =>
        _selection is { } selection && selection.Kind == kind && selection.PileIndex == pileIndex;

    private static int GetClickedCardIndex(List<Card> pile, int y)
    {
        var cardY = TableauStartY;
        for (var i = 0; i < pile.Count; i++)
        {
            var card = pile[i];
            var cardBottom = i == pile.Count - 1
                ? cardY + CardHeight
                : cardY + (card.FaceUp ? 28 : 18);
            if (y >= cardY && y < cardBottom)
            {
                return i;
            }

            if (i < pile.Count - 1)
            {
                cardY += card.FaceUp ? 28 : 18;
            }
        }

        return -1;
    }

    private void DrawSlot(int x, int y, bool showOutline)
    {
        Fill(new Rectangle(x, y, CardWidth, CardHeight), SlotColor);
        if (showOutline)
        {
            Outline(new Rectangle(x, y, CardWidth, CardHeight), new Color(67, 133, 105), 2);
        }
    }

    private void DrawCard(Card card, int x, int y, bool selected)
    {
        Fill(new Rectangle(x + 3, y + 4, CardWidth, CardHeight), new Color(12, 48, 40, 100));
        if (!card.FaceUp)
        {
            Fill(new Rectangle(x, y, CardWidth, CardHeight), new Color(45, 81, 142));
            Outline(new Rectangle(x, y, CardWidth, CardHeight), new Color(220, 226, 229), 2);
            Outline(new Rectangle(x + 5, y + 5, CardWidth - 10, CardHeight - 10), new Color(151, 180, 211), 1);
            for (var stripeY = y + 12; stripeY < y + CardHeight - 12; stripeY += 12)
            {
                Fill(new Rectangle(x + 12, stripeY, CardWidth - 24, 2), new Color(80, 117, 177));
            }
        }
        else
        {
            Fill(new Rectangle(x, y, CardWidth, CardHeight), CardFace);
            Outline(new Rectangle(x, y, CardWidth, CardHeight), CardOutline, 2);
            var ink = IsRed(card.Suit) ? CardRed : CardBlack;
            DrawText(RankLabel(card.Rank), x + 8, y + 7, ink, 2);
            DrawSuit(card.Suit, x + 8, y + 27, ink, 1);
            DrawText(SuitLabel(card.Suit), x + 34, y + 46, ink, 5);
            DrawText(RankLabel(card.Rank), x + CardWidth - 19, y + CardHeight - 22, ink, 1);
        }

        if (selected)
        {
            Outline(new Rectangle(x - 3, y - 3, CardWidth + 6, CardHeight + 6), Gold, 3);
        }
    }

    private void DrawSuit(Suit suit, int x, int y, Color color, int scale) =>
        DrawText(SuitLabel(suit), x, y, color, scale);

    private void DrawText(string text, int x, int y, Color color, int scale)
    {
        var cursorX = x;
        foreach (var character in text)
        {
            if (character == ' ')
            {
                cursorX += 4 * scale;
                continue;
            }

            if (Glyphs.TryGetValue(char.ToUpperInvariant(character), out var glyph))
            {
                for (var row = 0; row < glyph.Length; row++)
                {
                    for (var column = 0; column < glyph[row].Length; column++)
                    {
                        if (glyph[row][column] == '1')
                        {
                            Fill(new Rectangle(cursorX + column * scale, y + row * scale, scale, scale), color);
                        }
                    }
                }
            }

            cursorX += 6 * scale;
        }
    }

    private void Fill(Rectangle rectangle, Color color) =>
        _spriteBatch.Draw(_pixel, rectangle, color);

    private void Outline(Rectangle rectangle, Color color, int thickness)
    {
        Fill(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
        Fill(new Rectangle(rectangle.X, rectangle.Bottom - thickness, rectangle.Width, thickness), color);
        Fill(new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
        Fill(new Rectangle(rectangle.Right - thickness, rectangle.Y, thickness, rectangle.Height), color);
    }

    private static bool IsInside(int x, int y, int left, int top, int width, int height) =>
        x >= left && x < left + width && y >= top && y < top + height;

    private static bool IsRed(Suit suit) => suit is Suit.Hearts or Suit.Diamonds;

    private static string RankLabel(int rank) => rank switch
    {
        1 => "A",
        11 => "J",
        12 => "Q",
        13 => "K",
        _ => rank.ToString()
    };

    private static string SuitLabel(Suit suit) => suit switch
    {
        Suit.Clubs => "C",
        Suit.Diamonds => "D",
        Suit.Hearts => "H",
        Suit.Spades => "S",
        _ => "?"
    };

    private enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    private enum SourceKind
    {
        Waste,
        Foundation,
        Tableau
    }

    private sealed class Card(int rank, Suit suit)
    {
        public int Rank { get; } = rank;
        public Suit Suit { get; } = suit;
        public bool FaceUp { get; set; }
    }

    private sealed record Selection(SourceKind Kind, int PileIndex, int CardIndex);
}
