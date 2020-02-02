using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CanYouCount
{
	public struct LeaderboardEntry
	{
		public string UserName;
		public float TimeScore;
	}

	public class LeaderboardManager : MonoBehaviour
	{
		#region Events

		public event Action OnInitialized;

		public event Action<List<LeaderboardEntry>> OnLeaderboardPageRetrieved;

		#endregion

		[SerializeField]
		private string _leaderboardPathAndID = "CYC2020/Leaderboard-Test01";
		[SerializeField]
		private int _leaderboardEntriesPerPage = 20;
		private string _playerDataPath;

		private ApplicationManager _applicationManager;
		private DatabaseReference _databaseReference;

		private bool _isInitialized = false;

		public IReadOnlyList<LeaderboardEntry> CurrentLeaderboardEntries
			=> _currentLeaderboardEntries.AsReadOnly();
		private List<LeaderboardEntry> _currentLeaderboardEntries;
		private LeaderboardEntry _currentScoreEntry;

		public void Initialize(ApplicationManager applicationManager)
		{
			_applicationManager = applicationManager;
			_currentLeaderboardEntries = new List<LeaderboardEntry>(_leaderboardEntriesPerPage);
			_playerDataPath = SystemInfo.deviceUniqueIdentifier;

			try
			{
#if UNITY_EDITOR
				//if (Application.isEditor)
				//FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(TEST_DATABASE_URL);
#endif

				_databaseReference = FirebaseDatabase.DefaultInstance.GetReference(_leaderboardPathAndID);

				// Test Code to populate database
				//Task.Run(async () =>
				//{
				//	for (int i = 0; i < 30; i++)
				//	{
				//		var userDatabaseEntry = _databaseReference.Child($"TestUser{i:D2}");
				//		var leaderboardEntryJson = JsonUtility.ToJson(new LeaderboardEntry()
				//		{
				//			UserName = $"TestUser{i:D2}",
				//			TimeScore = i * 1 + 40
				//		});
				//		await userDatabaseEntry.SetRawJsonValueAsync(leaderboardEntryJson);
				//	}
				//});

				Task.Run(async () =>
				{
					await GetCurrentLeaderboardEntry();

					Debug.Log($"Firebase Leaderboard initialized: [{FirebaseApp.DefaultInstance.Options.DatabaseUrl}]", this);
					_isInitialized = true;
					OnInitialized?.Invoke();
				});
			}
			catch (Exception ex)
			{
				// Failed to initialize leaderboard manager
				Debug.LogException(ex, this);
				_isInitialized = false;
			}
		}

		public async Task SubmitLeaderboardEntry(LeaderboardEntry leaderboardEntry)
		{
			if (!_isInitialized)
			{
				throw new Exception("Can't submit leaderboard entry until leaderboard service is initialized!");
			}

			if (_currentScoreEntry.Equals(leaderboardEntry))
			{
				return; // Already set; no need to do anything
			}

			if (leaderboardEntry.TimeScore == float.MinValue)
			{
				return; // No score has been obtained yet; no need to do anything
			}

			if (_currentScoreEntry.UserName == leaderboardEntry.UserName && _currentScoreEntry.TimeScore <= leaderboardEntry.TimeScore)
			{
				return; // Current entry is already equal/better; no need to do anything
			}

			// Take better score from current/new
			leaderboardEntry.TimeScore = Mathf.Min(leaderboardEntry.TimeScore, _currentScoreEntry.TimeScore);

			try
			{
				var userDatabaseEntry = _databaseReference.Child(_playerDataPath);
				var leaderboardEntryJson = JsonUtility.ToJson(leaderboardEntry);
				await userDatabaseEntry.SetRawJsonValueAsync(leaderboardEntryJson);
				_currentScoreEntry = leaderboardEntry;
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to submit entry to leaderboard", ex);
			}
		}

		public void GetLeaderboardEntries(int page)
		{
			StartCoroutine(GetLeaderboardEntriesCoroutine(page));
		}

		private IEnumerator GetLeaderboardEntriesCoroutine(int page)
		{
			var task = Task.Run(async () =>
			{
				var newLeaderboardEntries = await GetLeaderboardEntries(page * _leaderboardEntriesPerPage, _leaderboardEntriesPerPage, nameof(LeaderboardEntry.TimeScore));
				_currentLeaderboardEntries = newLeaderboardEntries;
			});

			while (!task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
			{
				yield return null;
			}

			OnLeaderboardPageRetrieved?.Invoke(_currentLeaderboardEntries);
		}

		private async Task GetCurrentLeaderboardEntry()
		{
			try
			{
				var userDatabaseEntry = _databaseReference.Child(_playerDataPath);
				var currentValue = await userDatabaseEntry.GetValueAsync();
				if (!currentValue.Exists)
				{
					_currentScoreEntry = new LeaderboardEntry()
					{
						UserName = "",
						TimeScore = float.MaxValue
					};
				}
				else
				{
					var currentValueJson = currentValue.GetRawJsonValue();
					_currentScoreEntry = JsonUtility.FromJson<LeaderboardEntry>(currentValueJson);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to submit entry to leaderboard", ex);
			}
		}

		private async Task<List<LeaderboardEntry>> GetLeaderboardEntries(int startIndex, int count, string sortingField)
		{
			var leaderboardEntries = new List<LeaderboardEntry>();
			try
			{
				var pageSnapshot = await _databaseReference
					.OrderByChild(sortingField)
					.LimitToFirst(count)
					.GetValueAsync();

				foreach (var databaseEntry in pageSnapshot.Children)
				{
					var entryJson = databaseEntry.GetRawJsonValue();
					var entry = JsonUtility.FromJson<LeaderboardEntry>(entryJson);
					leaderboardEntries.Add(entry);
				}

				return leaderboardEntries;
			}
			catch (Exception ex)
			{
				throw new Exception($"Failed to get leaderboard entries from [{startIndex} to {startIndex + count}], sorted on field [{sortingField}]", ex);
			}
		}
	}
}
