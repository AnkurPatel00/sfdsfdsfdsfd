using Game.Controller;
using Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text accuracyText;

        private GameController gameController;
            
        [Inject]
        private void Init(GameController gameController)
        {
            this.gameController = gameController;
            gameController.OnScoreChanged += UpdateScore;
            gameController.OnAccuracyUpdated += UpdateAccuracy;
        }

        private void UpdateAccuracy(float obj)
        {
            accuracyText.text = $"Accuracy: {CUtility.RoundingToFloat(obj, 0)}%";
        }

        private void UpdateScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        public void RestartGame()
        {
            if (gameController.isGameOver)
                SceneManager.LoadScene("SampleScene");
        }
    }
}