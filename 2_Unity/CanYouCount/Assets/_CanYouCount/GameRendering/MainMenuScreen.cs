using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CanYouCount
{
	public class MainMenuScreen : BaseScreen
	{
		[SerializeField]
		private Button _playButton = null;

		[SerializeField]
		private Button _leaderboardButton = null;

		[SerializeField]
		private Button _quitButton = null;

		[SerializeField]
		private TMP_InputField _nameInputField = null;

		public override void InitializeScreen(ApplicationManager appManager)
		{
			base.InitializeScreen(appManager);

			_playButton.onClick.RemoveAllListeners();
			_playButton.onClick.AddListener(() =>
			{
				_applicationManager.AudioManager.PlayUIButtonClick();
				_applicationManager.StartNewGame();
			});

			_leaderboardButton.onClick.RemoveAllListeners();
			_leaderboardButton.onClick.AddListener(() =>
			{
				_applicationManager.AudioManager.PlayUIButtonClick();
				_applicationManager.ChangeState(AppStates.Leaderboard);
			});

			_quitButton.onClick.RemoveAllListeners();
			_quitButton.onClick.AddListener(() =>
			{
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit(0);
#endif
			});

			_nameInputField.onEndEdit.RemoveAllListeners();
			_nameInputField.onEndEdit.AddListener((playerNameInput) =>
			{
				_applicationManager.PlayerName = playerNameInput;
				UpdatePlayerName();
			});
			UpdatePlayerName();
		}

		private void UpdatePlayerName()
		{
			_nameInputField.text = _applicationManager.PlayerName;
		}
	}
}
