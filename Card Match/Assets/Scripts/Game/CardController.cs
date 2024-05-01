using Game.CommonModules.Pooling;
using Game.CommonModules.Audio;
using Game.Controller;
using Game.Installers;
using UnityEngine;
using Zenject;

namespace Game.Card
{
    public class CardController : PoolableObjectView
    {
        [SerializeField] private float flipTime = 1.0f;
        [SerializeField] private SpriteRenderer frontRenderer;
        [SerializeField] private SpriteRenderer backRenderer;
        [SerializeField] private string flipSoundId;

        private bool isFlipping;
        private bool isFront = true;

        private int cardIndex;
        private Vector2 coord;

        private GameController gameController;
        private IAudioPlayer audioPlayer;
        private AudioMapConfigVO audioMapConfigVO;

        [Inject]
        private void Init(GameController gameController, IAudioPlayer audioPlayer, AudioMapConfigVO audioMapConfigVO)
        {
            this.gameController = gameController;
            this.audioPlayer = audioPlayer;
            this.audioMapConfigVO = audioMapConfigVO;
        }

        public void FlipCard(bool init = false)
        {
            if (isFlipping) return;

            isFlipping = true;
            isFront = !isFront;

            float yRotation = isFront ? 0 : 180;

            if (!init)
            {
                audioPlayer?.Play(audioMapConfigVO.GetAudioClip(flipSoundId));
            }

            LeanTween.rotate(gameObject, new Vector3(0, yRotation, 0), flipTime).setOnComplete(() =>
            {
                isFlipping = false;
                if (isFront)
                    gameController.OnCardFlipped(this, init);
            });
        }

        private void OnMouseDown()
        {
            if (gameController.CanClick)
            {
                gameController.TotalClicked++;
                FlipCard();
            }
        }

        public void Reset()
        {
            isFlipping = false;
            isFront = true;
            cardIndex = -1;
            coord = Vector2.one * -1;

            SendToPool();
        }

        public void SetCardValue(int value, Sprite sprite, Vector2 coord)
        {
            cardIndex = value;
            frontRenderer.sprite = sprite;
            this.coord = coord;
        }

        public int GetCardValue()
        {
            return cardIndex;
        }

        public Vector2 GetCoord()
        {
            return coord;
        }

        public Sprite GetFrontRenderer()
        {
            return frontRenderer.sprite;
        }
    }
}