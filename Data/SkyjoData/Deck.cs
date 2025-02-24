using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Microsoft.VisualBasic;

namespace Data.SkyjoData
{
    public class Deck
    {
        #region Enum
        #endregion

        #region Constants
        private const int NUMBER_OF_DEFAULT_CARDS = 10;
        private const int NUMBER_OF_ZERO_CARDS = 15;
        private const int NUMBER_OF_SPECIAL_CARDS = 5;
        #endregion

        #region Events
        #endregion

        #region Field et Properties
        private Stack<Card> _cards;
        private Card? _lastPlayedCard;
        private readonly Stack<Card> _oldCards;


        /// <summary>
        /// Get the current card of the deck
        /// </summary>
        public Card? LastPlayedCard
        {
            get { return _lastPlayedCard; }
            set
            {
                _lastPlayedCard = value;
            }
        }

        /// <summary>
        /// Get the number of cards remaining
        /// </summary>
        public int RemainingCards => _cards.Count;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Deck()
        {
            _cards = new Stack<Card>();
            _oldCards = new Stack<Card>();
            InitializeDeck();
            Shuffle();
            _lastPlayedCard = DrawCard();
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
        /// <summary>
        /// Initialize Deck
        /// </summary>
        private void InitializeDeck()
        {
            // Add -2 cards
            for (int i = 0; i< NUMBER_OF_SPECIAL_CARDS; i++)
                _cards.Push(new Card(-2));

            // Add -1 cards
            for (int i = 0; i < NUMBER_OF_DEFAULT_CARDS; i++)
                _cards.Push(new Card(-1));

            // Add 0 cards
            for (int i = 0; i < NUMBER_OF_ZERO_CARDS; i++)
                _cards.Push(new Card(0));

            // ADD 1 to 12 cards
            for (int y = 1; y <= 12; y++)
            {
                for (int i = 0; i < NUMBER_OF_DEFAULT_CARDS; i++)
                    _cards.Push(new Card(y));
            }
        }

        /// <summary>
        /// Suffle cards
        /// </summary>
        private void Shuffle()
        {
            var cardsArray = _cards.ToArray();
            var random = new Random();
            for (int i = cardsArray.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (cardsArray[i], cardsArray[j]) = (cardsArray[j], cardsArray[i]);
            }
            _cards = new Stack<Card>(cardsArray);
            Logger.Instance.Log(Logger.ELevelMessage.Info, "La pioche est mélangée");
        }

        public Card? DrawCard()
        {
            if (_cards.Count == 0)
            {
                if (_oldCards.Count == 0)
                {
                    Logger.Instance.Log(Logger.ELevelMessage.Error, "Problème de gestion de la pioche (vide)");
                    return null;
                }

                // Reshuffle old cards into the deck
                ResetDeck();
            }
            return _cards.Pop();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Reset Deck
        /// </summary>
        /// <param name="cards"></param>
        /// <exception cref="ArgumentException"></exception>
        public void ResetDeck()
        {
            _cards = _oldCards;
            _oldCards.Clear();
            Shuffle();
            Logger.Instance.Log(Logger.ELevelMessage.Info, "La pioche a été réiniaitalisé");
        }

        /// <summary>
        /// Restart Deck
        /// </summary>
        public void RestartDeck()
        {
            _cards.Clear();
            _oldCards.Clear();
            InitializeDeck();
            Shuffle();
            _lastPlayedCard = DrawCard();
        }
        #endregion

        #endregion
    }
}
