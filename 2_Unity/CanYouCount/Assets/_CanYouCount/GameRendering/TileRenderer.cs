using System;
using CanYouCount.ObjectPooling;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CanYouCount
{
	public class TileRenderer : BasicUnityPoolable, IPointerDownHandler
	{
		[SerializeField]
		private TMP_Text _tileValueText = null;

		[SerializeField]
		private SpriteRenderer _tileBorder = null;
		[SerializeField]
		private SpriteRenderer _tileBackground = null;

		public Tile Tile => _tile;
		private Tile _tile;
		private Game _game;
        private const float _showHidinAnimationTime = 0.25f;


		public void SetTile(Game game, Tile tile)
		{
			_tile = tile;
			_game = game;

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

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.K))
				PerformIncorrectTapAnimation();
            if (Input.GetKeyDown(KeyCode.S))
                PerformShowAnimation();
            if (Input.GetKeyDown(KeyCode.H))
                PerformHideAnimation();
		}

		public void PerformIncorrectTapAnimation()
		{
#warning Fix this please :(
			// Reset state
			transform.localRotation = Quaternion.identity;

			const float TotalAnimTime = 0.25f;
			const float TimePerAnim = TotalAnimTime / 5f;
			const float RotationDegree = 20;
			var tweenSeq = LeanTween.sequence();
			tweenSeq.append(transform
				.LeanRotateZ(RotationDegree, TimePerAnim)
				.setEase(LeanTweenType.easeInSine));
			tweenSeq.append(transform
				.LeanRotateZ(-RotationDegree, TimePerAnim)
				.setEase(LeanTweenType.easeInOutSine));
			tweenSeq.append(transform
				.LeanRotateZ(RotationDegree, TimePerAnim)
				.setEase(LeanTweenType.easeInOutSine));
			tweenSeq.append(transform
				.LeanRotateZ(-RotationDegree, TimePerAnim)
				.setEase(LeanTweenType.easeInOutSine));
			tweenSeq.append(transform
				.LeanRotateZ(0, TimePerAnim)
				.setEase(LeanTweenType.easeOutSine));
			tweenSeq.append(() => { transform.localRotation = Quaternion.identity; });


			var colorSeq = LeanTween.sequence();
			var originalColor = _tileBackground.color;
			colorSeq.append(LeanTween.color(_tileBackground.gameObject, Color.red, TotalAnimTime / 2f));
			colorSeq.append(LeanTween.color(_tileBackground.gameObject, originalColor, TotalAnimTime / 2f));
			colorSeq.append(() => { _tileBackground.color = originalColor; });
		}

		public void PerformShowAnimation()
		{
			transform.localScale = Vector3.zero;
			var anim = transform
				.LeanScale(Vector3.one, _showHidinAnimationTime)
				.setEase(LeanTweenType.easeOutBack)
                .setDelay(_showHidinAnimationTime);
		}

		public void PerformHideAnimation(Action callback = null)
		{
            float timeTakesToFade = 0.25f;
            transform.localScale = Vector3.one;
            transform.LeanScale(Vector3.one + new Vector3(0.5f, 0.5f, 0.5f), _showHidinAnimationTime);
            LeanTween.alpha(gameObject, 0f, timeTakesToFade).setOnComplete(callback);
		}
	}
}
