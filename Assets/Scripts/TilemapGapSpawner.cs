using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGapSpawner : MonoBehaviour
{
    // This script spawns the Gap prefab at areas where on the tilemap there are gaps
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject gapPrefab;
    [SerializeField] private Transform gapsParent;

    void Start()
    {
        // Spawn gaps at the start of the scene
        SpawnGapsOnTilemap();
    }

    private void SpawnGapsOnTilemap()
    {
        BoundsInt bounds = tilemap.cellBounds; // Get the bounds of the tilemap
        for (int x = bounds.xMin; x < bounds.xMax; x++) // Iterate through every tile of the tilemap
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0); // Acquiring tile (x, y) position
                if (tilemap.HasTile(cellPosition))
                {
                    // If there is a gap tile at this position of the tilemap, spawn a gap prefab
                    Vector3 worldPosition = tilemap.GetCellCenterWorld(cellPosition);
                    GameObject newGap = Instantiate(gapPrefab, worldPosition, Quaternion.identity, gapsParent);
                    LevelEditorManager.instance.AddObject(gapPrefab, worldPosition, 0, 0, true);
                }
            }
        }
    }
}
