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

        public override void InitializeScreen(ApplicationManager appManager)
        {
            base.InitializeScreen(appManager);

        }

        private void SetupButtons()
        {
            //_menuButton.onClick.AddListener(() => _applicationManager.)
        }
    }
}
