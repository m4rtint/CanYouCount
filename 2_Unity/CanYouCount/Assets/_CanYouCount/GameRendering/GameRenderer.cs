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

		private Game _game;

		public void Initialize()
		{
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
		}

		private void UnsubscribeFromGameEvents()
		{
			_game.OnWrongTileTapped -= HandleWrongTileTapped;
			_game.OnCorrectTileTapped -= HandleSwapTile;
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
			for (int i = 0; i < _visibleTileRenderers.Count; i++)
			{
				var tileRenderer = _visibleTileRenderers[i];
				if (tileRenderer.Tile.Equals(tile))
				{
					return tileRenderer;
				}
			}

			return null;
		}

		private void HandleSwapTile(Tile arg1, Tile arg2)
		{
		}

		private void HandleWrongTileTapped(Tile wrongTile)
		{
			// Find the correct renderer
			var wrongTileRenderer = GetVisibleTileRendererForTile(wrongTile);
			wrongTileRenderer?.PerformIncorrectTapAnimation();
		}
	}
}
