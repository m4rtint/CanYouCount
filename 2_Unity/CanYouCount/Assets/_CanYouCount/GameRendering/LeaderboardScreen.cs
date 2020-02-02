using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CanYouCount
{
	public class LeaderboardScreen : BaseScreen
	{
		private const int NUMBER_OF_LEADERBOARD_ENTRIES = 20;

		[SerializeField]
		private Button _menuButton = null;

		[SerializeField]
		private GameObject _leaderboardEntryPrefab = null;

		[SerializeField]
		private RectTransform _leaderboardEntryContainer = null;

		private List<LeaderboardEntryUI> _leaderboardEntryUIs = new List<LeaderboardEntryUI>();

		public override void InitializeScreen(ApplicationManager appManager)
		{
			base.InitializeScreen(appManager);

			_menuButton.onClick.RemoveAllListeners();
			_menuButton.onClick.AddListener(() =>
			{
				_applicationManager.AudioManager.PlayUIButtonClick();
				_applicationManager.ChangeState(AppStates.MainMenu);
			});

			for (int i = 0; i < NUMBER_OF_LEADERBOARD_ENTRIES; i++)
			{
				var gObj = Instantiate(_leaderboardEntryPrefab, _leaderboardEntryContainer);
				var entryUI = gObj.GetComponent<LeaderboardEntryUI>();
				_leaderboardEntryUIs.Add(entryUI);
			}

			DisableAllLeaderboardEntries();
		}

		public override void ShowScreen(bool isInstant = false)
		{
			base.ShowScreen(isInstant);

			_applicationManager.LeaderboardManager.OnLeaderboardPageRetrieved += HandleLeaderboardRetrieved;
			_applicationManager.LeaderboardManager.GetLeaderboardEntries(0);
		}

		private void HandleLeaderboardRetrieved(System.Collections.Generic.List<LeaderboardEntry> leaderboardEntries)
		{
			_applicationManager.LeaderboardManager.OnLeaderboardPageRetrieved -= HandleLeaderboardRetrieved;

			// Disable all entries
			DisableAllLeaderboardEntries();

			// Re-enable and update required entries
			var entryCount = Mathf.Max(leaderboardEntries.Count, NUMBER_OF_LEADERBOARD_ENTRIES);
			for (int i = 0; i < entryCount; i++)
			{
				_leaderboardEntryUIs[i].gameObject.SetActive(true);
				_leaderboardEntryUIs[i].UpdateEntry(i + 1, leaderboardEntries[i]);
			}
		}

		private void DisableAllLeaderboardEntries()
		{
			for (int i = 0; i < NUMBER_OF_LEADERBOARD_ENTRIES; i++)
			{
				_leaderboardEntryUIs[i].gameObject.SetActive(false);
			}
		}
	}
}
