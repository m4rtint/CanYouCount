using UnityEngine;
using UnityEngine.UI;

namespace CanYouCount
{
	public class MainMenuScreen : BaseScreen
	{
		[SerializeField]
		private Button _playButton = null;

		[SerializeField]
		private Button _quitButton = null;

		public override void InitializeScreen(ApplicationManager appManager)
		{
			base.InitializeScreen(appManager);

			_playButton.onClick.RemoveAllListeners();
			_playButton.onClick.AddListener(() =>
			{
				_applicationManager.StartNewGame();
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
		}
	}
}
