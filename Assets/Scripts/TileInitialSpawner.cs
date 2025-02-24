using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInitialSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject initialPrefab;
    [SerializeField] private Transform initialParent;

    void Start()
    {
        SpawnInitialOnTilemap();
    }

    private void SpawnInitialOnTilemap()
    {
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(cellPosition))
                {
                    Vector3 gridPosition = new Vector3(Mathf.Ceil((tilemap.GetCellCenterWorld(cellPosition).x - 0.5f) / 0.96f) * 0.96f + 0.06f, Mathf.Ceil((tilemap.GetCellCenterWorld(cellPosition).y - 0.5f) / 0.96f) * 0.96f + 0.34f, 0);
                    GameObject newGap = Instantiate(initialPrefab, gridPosition, Quaternion.identity, initialParent);
                    LevelEditorManager.instance.AddObject(initialPrefab, gridPosition, 0, 0, true);
                }
            }
        }
    }
}
