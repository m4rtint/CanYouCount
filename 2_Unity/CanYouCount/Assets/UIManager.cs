using TMPro;
using UnityEngine;

namespace CanYouCount
{
    public class UIManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField]
        private GameObject _timerPrefab = null;
        private Game _game = null;
        private TMP_Text _timerText = null;

        /// <summary>
        /// Initialize the specified game.
        /// </summary>
        /// <param name="game">Game.</param>
        public void Initialize(Game game)
        {
            _game = game;
            SetupTimerUI();
        }

        /// <summary>
        /// Updates the user interface.
        /// </summary>
        public void UpdateUI()
        {
            _timerText.text = string.Format("{0:0.##}", _game?.Timer);
        }

        private void SetupTimerUI()
        {
            GameObject _timerObj = Instantiate<GameObject>(_timerPrefab, transform.parent);
            _timerText = _timerObj.GetComponent<TMP_Text>();
            if (_timerText == null)
            {
                _timerObj.AddComponent<TMP_Text>();
            }
        }
    }
}
