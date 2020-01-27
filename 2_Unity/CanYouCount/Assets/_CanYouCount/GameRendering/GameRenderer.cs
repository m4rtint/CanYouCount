using CanYouCount.ObjectPooling;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CanYouCount
{
	public class GameRenderer : MonoBehaviour
	{
		[Header("Object References")]
		[SerializeField]
		private Camera _camera = null;

		[SerializeField]
		private GameObject _tileRendererPrefab = null;

		[Header("Rendering Variables")]
		[SerializeField]
		private int _gridWidth = 5;
		[SerializeField]
		private float _tilePadding = 0.5f;
		[SerializeField]
		private float _tileSize = 1;

		private BasicUnityObjectPool<TileRenderer> _tileRendererObjectPool = null;
		private List<TileRenderer> _visibleTileRenderers;

		private ApplicationManager _applicationManager;
		private Game _game;

		public void Initialize(ApplicationManager applicationManager)
		{
			_applicationManager = applicationManager;
			// Subscribe to events
			_applicationManager.OnAppStateChanged += HandleAppStateChanged;

			_tileRendererObjectPool = new BasicUnityObjectPool<TileRenderer>(
				allocationFunction: () =>
				{
					var gObj = Instantiate<GameObject>(_tileRendererPrefab);
					var tileRenderer = gObj.GetComponent<TileRenderer>();
					return tileRenderer;
				},
				parent: this.transform);
			_tileRendererObjectPool.PreallocateObjects(10);
		}

		public void Cleanup()
		{
			if(_applicationManager == null || _visibleTileRenderers == null)
			{
				return;
			}

			_applicationManager.OnAppStateChanged -= HandleAppStateChanged;

			// Return all renderers to their object pools
			for (int i = 0; i < _visibleTileRenderers.Count; i++)
			{
				_tileRendererObjectPool.ReturnObjectToPool(_visibleTileRenderers[i]);
			}

			// Dispose of all object pools
			_tileRendererObjectPool.Dispose();
		}

		public void SetGame(Game game)
		{
			if (_game == game)
			{
				return; // Game already set
			}

			if (_game != null)
			{
				// Return all objects to pool
				for (int i = 0; i < _visibleTileRenderers.Count; i++)
				{
					var tileRenderer = _visibleTileRenderers[i];
					tileRenderer.SetTile(null, Tile.BlankTile);
					_tileRendererObjectPool.ReturnObjectToPool(tileRenderer);
				}

				// Unsubscribe old events
				UnsubscribeFromGameEvents();
			}

			// Set the new game
			_game = game;

			// Create visible tiles
			_visibleTileRenderers = new List<TileRenderer>(game.VisibleTileCount);
			for (int i = 0; i < game.VisibleTileCount; i++)
			{
				var tileRenderer = _tileRendererObjectPool.GetObjectFromPool(this.transform);
				tileRenderer.SetTile(game, game.VisibleTiles[i]);
				tileRenderer.PerformShowAnimation();
				_visibleTileRenderers.Add(tileRenderer);
			}

			// Move camera to right spot
			float camX = (_gridWidth / 2f) * (_tileSize + _tilePadding * 2) - _tileSize / 2f;
			float camY = -((_gridWidth / 2f) * (_tileSize + _tilePadding * 2) - _tileSize / 2f);
			float camSize = _gridWidth * (_tileSize + _tilePadding * 2);

			_camera.transform.localPosition = new Vector3(camX, camY, -10);
			_camera.orthographicSize = camSize;

			// Subscribe to game events
			SubscribeToGameEvents();

			// Layout Tiles
			LayoutTileRenderers();
		}

		private void SubscribeToGameEvents()
		{
			_game.OnWrongTileTapped += HandleWrongTileTapped;
			_game.OnCorrectTileTapped += HandleSwapTile;
			_game.OnShowHintOnTile += HandleShowHintOnTile;
		}

		private void UnsubscribeFromGameEvents()
		{
			_game.OnWrongTileTapped -= HandleWrongTileTapped;
			_game.OnCorrectTileTapped -= HandleSwapTile;
			_game.OnShowHintOnTile -= HandleShowHintOnTile;
		}

		private void LayoutTileRenderers()
		{
			for (int i = 0; i < _visibleTileRenderers.Count; i++)
			{
				var renderer = _visibleTileRenderers[i];

				float x = (i % _gridWidth) * (_tileSize + _tilePadding * 2) + _tilePadding;
				float y = -(i / _gridWidth) * (_tileSize + _tilePadding * 2) + _tilePadding;

				renderer.transform.localPosition = new Vector3(x, y, 0);
			}
		}

		private TileRenderer GetVisibleTileRendererForTile(Tile tile)
		{
			var index = GetVisibleTileRendererIndexForTile(tile);
			return index == -1 ? null : _visibleTileRenderers[index];
		}

		private int GetVisibleTileRendererIndexForTile(Tile tile)
		{
			for (int i = 0; i < _visibleTileRenderers.Count; i++)
			{
				var tileRenderer = _visibleTileRenderers[i];
				if (tileRenderer.Tile.Equals(tile))
				{
					return i;
				}
			}

			return -1;
		}

		private void HandleAppStateChanged(AppStates newState)
		{
			bool shouldBeActive = newState == AppStates.Pregame || newState == AppStates.Ingame || newState == AppStates.GameOverAnimation;
			gameObject.SetActive(shouldBeActive);
		}

		private void HandleSwapTile(Tile oldTile, Tile newTile)
		{
			// Get the old tile renderer
			var tileRendererIndex = GetVisibleTileRendererIndexForTile(oldTile);
			var oldTileRenderer = _visibleTileRenderers[tileRendererIndex];
			if (oldTileRenderer == null)
			{
				// Act confused, but ultimately do nothing
				Debug.LogError($"Was expecting to have an active {nameof(TileRenderer)} for tile [{oldTile.TileValue}] but didn't find any");
			}

			// Get a new tile renderer
			var newTileRenderer = _tileRendererObjectPool.GetObjectFromPool();
			newTileRenderer.SetTile(_game, newTile);

			// Perform swap
			_visibleTileRenderers[tileRendererIndex] = newTileRenderer;
			LayoutTileRenderers();

			// Perfom animations
			newTileRenderer.PerformShowAnimation();
			oldTileRenderer.PerformHideAnimation(() =>
			{
				_tileRendererObjectPool.ReturnObjectToPool(oldTileRenderer);
			});
		}

		private void HandleWrongTileTapped(Tile wrongTile)
		{
			// Find the correct renderer
			var wrongTileRenderer = GetVisibleTileRendererForTile(wrongTile);
			wrongTileRenderer?.PerformIncorrectTapAnimation();
		}

		private void HandleShowHintOnTile(Tile hintTile, bool shouldShow)
		{
			var tileRenderer = GetVisibleTileRendererForTile(hintTile);
			if (tileRenderer != null)
			{
				tileRenderer.ShowHint(shouldShow);
			}
		}
	}
}
