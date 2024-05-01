using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Model
{
    [Serializable]
    public class CardStorageHolder
    {
        public List<CardDataStorage> cardDataStorage;
        public int Score;
        public int TotalClicked;

        public CardStorageHolder(List<CardDataStorage> cardDataStorages)
        {
            cardDataStorage = cardDataStorages;
        }
    }

    [Serializable]
    public class CardDataStorage
    {
        public Vector2 coord;
        public int cardIndex;

        public CardDataStorage(Vector2 coord, int cardIndex)
        {
            this.coord = coord;
            this.cardIndex = cardIndex;
        }
    }
}