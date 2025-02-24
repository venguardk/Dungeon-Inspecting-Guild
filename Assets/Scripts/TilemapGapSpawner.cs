using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGapSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject gapPrefab;
    [SerializeField] private Transform gapsParent;

    void Start()
    {
        SpawnGapsOnTilemap();
    }

    private void SpawnGapsOnTilemap()
    {
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(cellPosition))
                {
                    Vector3 worldPosition = tilemap.GetCellCenterWorld(cellPosition);
                    GameObject newGap = Instantiate(gapPrefab, worldPosition, Quaternion.identity, gapsParent);
                    LevelEditorManager.instance.AddObject(gapPrefab, worldPosition, 0, 0, true);
                }
            }
        }
    }
}
