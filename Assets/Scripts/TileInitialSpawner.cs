using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInitialSpawner : MonoBehaviour
{
    // This script is for spawning initial uneditable object onto the screen
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
                    Vector3 gridPosition = new Vector3(Mathf.Ceil(tilemap.GetCellCenterWorld(cellPosition).x - 0.75f) + 0.5f, Mathf.Ceil(tilemap.GetCellCenterWorld(cellPosition).y - 0.75f) + 0.5f, 0);
                    Matrix4x4 tileTransform = tilemap.GetTransformMatrix(cellPosition);
                    Quaternion rotation = Quaternion.LookRotation(tileTransform.GetColumn(2), tileTransform.GetColumn(1));
                    GameObject newGap = Instantiate(initialPrefab, gridPosition, rotation, initialParent);
                    LevelEditorManager.instance.AddObject(initialPrefab, gridPosition, rotation.eulerAngles.z, 0, true);
                }
            }
        }
    }
}
