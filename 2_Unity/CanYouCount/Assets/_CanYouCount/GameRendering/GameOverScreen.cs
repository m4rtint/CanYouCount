using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace CanYouCount
{
	public class GameOverScreen : BaseScreen
	{
        [Header("UI Binding")]
        [SerializeField]
        private TMP_Text _timeScoreText;

        [Header("Buttons")]
        [SerializeField]
        private Button _menuButton;
        [SerializeField]
        private Button _retryButton;

        /// <summary>
        /// Initializes the screen.
        /// </summary>
        /// <param name="appManager">App manager.</param>
        public override void InitializeScreen(ApplicationManager appManager)
        {
            base.InitializeScreen(appManager);
            SetupButtons();
        }

        /// <summary>
        /// Shows the screen.
        /// </summary>
        public override void ShowScreen()
        {
            base.ShowScreen();
            _timeScoreText.text = string.Format(GameUIContent.TwoDecimalPoint, _applicationManager.Game.Timer);
        }

        private void SetupButtons()
        {
            _menuButton.onClick.AddListener(() => _applicationManager.ChangeState(AppStates.MainMenu));
            _retryButton.onClick.AddListener(() => _applicationManager.StartNewGame());
        }
    }
}
