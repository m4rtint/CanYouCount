using TMPro;
using UnityEngine;

namespace CanYouCount
{
    public class UIManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField]
        private GameObject _timerPrefab = null;
        [SerializeField]
        private GameObject _nextTilePrefab = null;
        
        private Game _game = null;
        private TMP_Text _timerText = null;
        private TMP_Text _nextTileText = null;

        /// <summary>
        /// Initialize the specified game.
        /// </summary>
        /// <param name="game">Game.</param>
        public void Initialize(Game game)
        {
            _game = game;
            SetupTimerUI();
            SetupNextTileUI();
            _game.OnCorrectTileTapped += _game_OnCorrectTileTapped;
        }

        /// <summary>
        /// Updates the user interface.
        /// </summary>
        public void UpdateUI()
        {
            _timerText.text = string.Format(GameUIContent.TwoDecimalPoint, _game?.Timer);
        }

        /// <summary>
        /// Cleans up.
        /// </summary>
        public void CleanUp()
        {
            Destroy(_timerText.gameObject);
            Destroy(_nextTileText.gameObject);
        }

        private void SetupNextTileUI()
        {
            GameObject _nextTileObj = Instantiate<GameObject>(_nextTilePrefab, transform.parent);
            _nextTileText = _nextTileObj.GetComponent<TMP_Text>();
            if (_nextTileText == null)
            {
                _nextTileObj.AddComponent<TMP_Text>();
            }
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

        private void _game_OnCorrectTileTapped(Tile originalTile, Tile arg2)
        {
            _nextTileText.text = string.Format(GameUIContent.NextTile, originalTile.TileValue + 1);
        }
    }
}
