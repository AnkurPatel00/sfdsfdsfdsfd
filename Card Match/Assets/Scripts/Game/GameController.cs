using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Game.CommonModules.Pooling;
using Game.CommonModules.Audio;
using Game.Grid;
using Game.Installers;
using Game.Model;
using Game.Utility;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Controller
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private int rows = 4;
        [SerializeField] private int columns = 4;
        [SerializeField] private float cardSpacing = 1.5f;
        [SerializeField] private float viewTime;

        [Header("Sounds")] [SerializeField] private string winSoundId;
        [SerializeField] private string loseSoundId;
        [SerializeField] private string gameOverSoundId;

        [SerializeField] private bool deletePrefs;

        private List<CardController> flippedCards = new(); // Track flipped cards
        private int score;
        private List<CardDataStorage> cardDataStorage = new();

        private IPoolManager poolManager;
        private GameConfigVO gameConfig;
        private IAudioPlayer audioPlayer;
        private AudioMapConfigVO audioMapConfigVO;
        private GridManager gridManager;

        public Action<int> OnScoreChanged;
        public Action<float> OnAccuracyUpdated;

        [HideInInspector] public bool CanClick;
        [HideInInspector] public bool isGameOver;
        [HideInInspector] public int TotalClicked;

        private const string STORAGE_KEY = "cardData";

        [Inject]
        private void Init(IPoolManager poolManager, GameConfigVO gameConfig, IAudioPlayer audioPlayer,
            AudioMapConfigVO audioMapConfigVO, GridManager gridManager)
        {
            this.poolManager = poolManager;
            this.gameConfig = gameConfig;
            this.audioPlayer = audioPlayer;
            this.audioMapConfigVO = audioMapConfigVO;
            this.gridManager = gridManager;
        }

        private void Start()
        {
            InitializeCards();
        }

        private void InitializeCards()
        {
            if (deletePrefs)
                PlayerPrefs.DeleteAll();

            SetGrid();

            bool hasData = LoadFromStorage();

            if (hasData) return;
            
            var totalCards = rows * columns;
            var cardValues = GenerateCardValues(totalCards / 2);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var cardController = SpawnCard(j, i);

                    int randomIndex = Random.Range(0, cardValues.Count);
                    int cardValue = cardValues[randomIndex];
                    cardController.SetCardValue(cardValue, gameConfig.CardDataConfig[cardValue].CardImage,
                        new Vector2(j, i));
                    cardValues.RemoveAt(randomIndex);

                    cardDataStorage.Add(new CardDataStorage(new Vector2(j, i), cardValue));
                }
            }
        }

        private bool LoadFromStorage()
        {
            bool hasData = false;
            
            if (!PlayerPrefs.HasKey(STORAGE_KEY)) return hasData;
            
            var storageHolder = JsonUtility.FromJson<CardStorageHolder>(PlayerPrefs.GetString(STORAGE_KEY));
            cardDataStorage = storageHolder.cardDataStorage;
            if (cardDataStorage.Count > 0)
            {
                hasData = true;
            }

            if (hasData)
            {
                score = storageHolder.Score;
                TotalClicked = storageHolder.TotalClicked;
                    
                foreach (var cardData in cardDataStorage)
                {
                    var cardController = SpawnCard(cardData.coord.x, cardData.coord.y);
                    cardController.SetCardValue(cardData.cardIndex,
                        gameConfig.CardDataConfig[cardData.cardIndex].CardImage, cardData.coord);
                }
            }

            return hasData;
        }

        private CardController SpawnCard(float x, float y)
        {
            GameObject newCard = poolManager.GetFromPool("card").gameObject;
            newCard.name = $"Card [{x},{y}]";
            newCard.transform.position = gridManager.GetPosition((int)x, (int)y);
            var cardController = newCard.GetComponent<CardController>();
            StartCoroutine(DelayedCall(viewTime, () =>
            {
                cardController.FlipCard(true);
                CanClick = true;
            }));
            return cardController;
        }

        private void SetGrid()
        {
            gridManager.SetLength(columns, rows);
            gridManager.SetOffset(cardSpacing);
            gridManager.SetSprite(cardPrefab.GetComponent<CardController>().GetFrontRenderer());
            gridManager.Init();
            gridManager.SetCamera();
        }

        private IEnumerator DelayedCall(float delay, Action onComplete)
        {
            yield return new WaitForSeconds(delay);
            onComplete?.Invoke();
        }

        private List<int> GenerateCardValues(int numPairs)
        {
            var cardValues = new List<int>();
            for (int i = 0; i < numPairs; i++)
            {
                int index = gameConfig.CardDataConfig.Random().CardIndex;
                cardValues.Add(index);
                cardValues.Add(index);
            }

            return cardValues;
        }

        public void OnCardFlipped(CardController card, bool init)
        {
            if (init)
                return;

            if (!flippedCards.Contains(card))
                flippedCards.Add(card);

            if (flippedCards.Count >= 2)
            {
                StartCoroutine(CheckMatch());
            }
        }

        private IEnumerator CheckMatch()
        {
            yield return new WaitForSeconds(1f);

            Dictionary<int, List<CardController>> flippedCardMap = new();

            for (int i = 0; i < flippedCards.Count; i++)
            {
                var controller = flippedCards[i];

                if (flippedCardMap.ContainsKey(controller.GetCardValue()))
                {
                    flippedCardMap[controller.GetCardValue()].Add(controller);
                }
                else
                {
                    flippedCardMap.Add(controller.GetCardValue(), new List<CardController> { controller });
                }
            }

            foreach (var pair in flippedCardMap)
            {
                while (pair.Value.Count > 1)
                {
                    score += 20;
                    audioPlayer.Play(audioMapConfigVO.GetAudioClip(winSoundId));

                    flippedCards.Remove(pair.Value[0]);
                    flippedCards.Remove(pair.Value[1]);

                    cardDataStorage.RemoveAll(x =>
                        x.coord == pair.Value[0].GetCoord() || x.coord == pair.Value[1].GetCoord());

                    pair.Value[0].Reset();
                    pair.Value[1].Reset();

                    pair.Value.RemoveAt(0);
                    pair.Value.RemoveAt(0);
                }
            }

            if (flippedCards.Count > 1)
            {
                score -= 5 * flippedCards.Count;
                audioPlayer.Play(audioMapConfigVO.GetAudioClip(loseSoundId));

                flippedCards.ForEach(x => x.FlipCard());
                flippedCards.Clear();
            }

            OnScoreChanged?.Invoke(score);

            var accuracy = ((rows * columns) - cardDataStorage.Count) * 100 / (float)TotalClicked;
            OnAccuracyUpdated?.Invoke(TotalClicked == 0 ? 0 : accuracy);

            if (cardDataStorage.Count == 0)
            {
                isGameOver = true;
                score = 0;
                CanClick = false;
                TotalClicked = 0;
                audioPlayer.Play(audioMapConfigVO.GetAudioClip(gameOverSoundId));
            }
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void SaveData()
        {
            if (!isGameOver)
            {
                var storage = new CardStorageHolder(cardDataStorage)
                {
                    Score = score,
                    TotalClicked = TotalClicked
                };
                var json = JsonUtility.ToJson(storage);

                PlayerPrefs.SetString(STORAGE_KEY, json);
            }
            else
            {
                PlayerPrefs.DeleteKey(STORAGE_KEY);
            }
        }
    }
}