using CanYouCount.ObjectPooling;
using System.Collections.Generic;
using UnityEngine;

namespace CanYouCount
{
	public class GameRenderer : MonoBehaviour
	{
		[SerializeField]
		private GameObject _tileRendererPrefab = null;
		private BasicUnityObjectPool<TileRenderer> _tileRendererObjectPool = null;

		private List<TileRenderer> _visibleTileRenderers;

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

		public void SetGame(Game game)
		{
			// Create visible tiles
			_visibleTileRenderers = new List<TileRenderer>(game.VisibleTileCount);
			for (int i = 0; i < game.VisibleTileCount; i++)
			{
				var tileRenderer = _tileRendererObjectPool.GetObjectFromPool(this.transform);
				tileRenderer.SetTile(game.VisibleTiles[i]);
				_visibleTileRenderers.Add(tileRenderer);
			}

			LayoutTileRenderers();
		}

		private void LayoutTileRenderers()
		{
			for (int i = 0; i < _visibleTileRenderers.Count; i++)
			{
				var renderer = _visibleTileRenderers[i];
				renderer.transform.localPosition = new Vector3(i % 5, i / 5, 0);
			}
		}
	}
}
