using TMPro;
using UnityEngine;

namespace CanYouCount
{
	public class LeaderboardEntryUI : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _placeText = null;

		[SerializeField]
		private TMP_Text _usernameText = null;

		[SerializeField]
		private TMP_Text _scoreText = null;

		public void UpdateEntry(int place, LeaderboardEntry entry)
		{
			_placeText.text = place.ToString();
			_usernameText.text = entry.UserName;
			_scoreText.text = entry.TimeScore.ToString("F2");
		}
	}
}
