using UnityEngine;

namespace CanYouCount
{
	/// <summary>This class adjusts the size of the RectTransform it is attached to stay within the Screen's safe area (no notches/cutouts)</summary>
	[RequireComponent(typeof(RectTransform))]
	public class UISafeArea : MonoBehaviour
	{
		[SerializeField]
		private int _screenBottomMarginPixels = 0;

		private RectTransform _rectTransform;
		private Rect _appliedSafeArea;

		private Rect CurrentSafeArea => Screen.safeArea;

		private void OnEnable()
		{
			_rectTransform = GetComponent<RectTransform>();

			Refresh();
		}

		private void Update()
		{
			Refresh();
		}

		private void Refresh()
		{
			if (CurrentSafeArea != _appliedSafeArea)
				ApplySafeArea(CurrentSafeArea);
		}

		private void ApplySafeArea(Rect safeArea)
		{
			_appliedSafeArea = safeArea;

			// Convert safe area rectangle from absolute pixels to normalised anchor coordinates
			Vector2 anchorMin = safeArea.position + new Vector2(0, _screenBottomMarginPixels);
			Vector2 anchorMax = safeArea.position + safeArea.size - new Vector2(0, _screenBottomMarginPixels);
			anchorMin.x /= Screen.width;
			anchorMin.y /= Screen.height;
			anchorMax.x /= Screen.width;
			anchorMax.y /= Screen.height;

			_rectTransform.anchorMin = anchorMin;
			_rectTransform.anchorMax = anchorMax;

			Debug.Log($"Safe Area Applied to [{name}]: [x={safeArea.x}, y={safeArea.y}, w={safeArea.width}, h={safeArea.height}]");
		}
	}
}
