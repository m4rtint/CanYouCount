using System;
using CanYouCount.ObjectPooling;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CanYouCount
{
	public class TileRenderer : BasicUnityPoolable, IPointerDownHandler
	{
		private const float _showHidinAnimationTime = 0.25f;
		private Vector3 scaleOutSize = new Vector3(1.5f, 1.5f, 1.5f);

		[SerializeField]
		private TMP_Text _tileValueText = null;

		[SerializeField]
		private SpriteRenderer _tileBorder = null;
		[SerializeField]
		private SpriteRenderer _tileBackground = null;

		public Tile Tile => _tile;
		private Tile _tile;
		private Game _game;
		private Color _originalColor = Color.white;

		private bool _showingHint;

		private void OnEnable()
		{
			_originalColor = _tileBackground.color;
		}

		public void SetTile(Game game, Tile tile)
		{
			_tile = tile;
			_game = game;
			_showingHint = false;
			ResetColor();

			if (!tile.TileValue.HasValue)
			{
				// Tile is Blank Tile
				SetEnabled(false);
			}
			else
			{
				// Tile has value
				SetEnabled(true);

				_tileValueText.text = tile.TileValue.Value.ToString();
			}
		}

		private void SetEnabled(bool state)
		{
			_tileBackground.enabled = state;
			_tileBorder.enabled = state;
			_tileValueText.enabled = state;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			_game.OnTileTapped(_tile);
		}

		public void PerformIncorrectTapAnimation()
		{
			// Reset state
			ResetColor();
			ResetRotation();

			const float TotalAnimTime = 0.25f;
			const float TimePerAnim = TotalAnimTime / 5f;
			const float RotationDegree = 20;

			transform
			.LeanRotateZ(RotationDegree, TimePerAnim)
			.setLoopPingPong(2)
			.setOnComplete(() => ResetRotation());


			var colorSeq = LeanTween.sequence();
			colorSeq.append(LeanTween.color(_tileBackground.gameObject, Color.red, TotalAnimTime / 2f));
			colorSeq.append(LeanTween.color(_tileBackground.gameObject, _originalColor, TotalAnimTime / 2f));
			colorSeq.append(() => { ResetColor(); });
		}

		private void ResetRotation()
		{
			transform.localRotation = Quaternion.identity;
		}

		public void PerformShowAnimation()
		{
			ResetColor();
			transform.localScale = Vector3.zero;
			transform
				.LeanScale(Vector3.one, _showHidinAnimationTime)
				.setEase(LeanTweenType.easeOutBack)
				.setDelay(_showHidinAnimationTime);
			LeanTween.alpha(gameObject, 1f, _showHidinAnimationTime);
		}

		public void PerformHideAnimation(Action callback = null)
		{
			transform.localScale = Vector3.one;
			transform.LeanScale(scaleOutSize, _showHidinAnimationTime);
			LeanTween.alpha(gameObject, 0f, _showHidinAnimationTime).setOnComplete(callback);
		}

		public void ShowHint(bool shouldShow)
		{
			_showingHint = shouldShow;
			Hint();
		}

		private void Hint()
		{
			if (!_showingHint)
			{
				return;
			}

			const float TotalAnimTime = 0.25f;

			var colorSeq = LeanTween.sequence();
			colorSeq.append(LeanTween.color(_tileBackground.gameObject, new Color(1, 0.8196079f, 0.1568628f), TotalAnimTime / 2f));
			colorSeq.append(LeanTween.color(_tileBackground.gameObject, _originalColor, TotalAnimTime / 2f));
			colorSeq.append(TotalAnimTime / 2f);
			colorSeq.append(() =>
			{
				ResetColor();
				Hint();
			});
		}

		private void ResetColor()
		{
			_tileBackground.color = _originalColor;
		}
	}
}
