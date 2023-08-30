using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public GameObject indestructibleTilePrefab;
    public GameObject ironOrePrefab;
    public GameObject copperOrePrefab;
    public GameObject victoriumOrePrefab;
    public GameObject invisibleTilePrefab;
    public int mapSizeX = 100;
    public int mapSizeY = 100;
    public int indestructibleBorderSize = 10;
    public int depositSize = 5;
    public int ironOreDepositCount = 10;
    public int copperOreDepositCount = 8;
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        int depthLevel = tilePrefabs.Length;

        int halfSizeX = mapSizeX / 2;

        int victoriumPosition=Random.Range(-halfSizeX+1,halfSizeX-1);

        for (int x = -halfSizeX; x < halfSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                Vector3 position = new Vector3(x, -y, 0f);

                GameObject tilePrefab;

                // Check if the tile is on the left, right, or bottom border
                if (x < -halfSizeX + indestructibleBorderSize || x >= halfSizeX - indestructibleBorderSize || y >= mapSizeY - indestructibleBorderSize || (y < indestructibleBorderSize && x != 0))
                {
                    tilePrefab = indestructibleTilePrefab;
                }
                else if (x==victoriumPosition && y== mapSizeY - indestructibleBorderSize-1)
                {
                    tilePrefab = victoriumOrePrefab;
                }
                else
                {
                    int tileIndex = Mathf.FloorToInt((float)y / mapSizeY * depthLevel);
                    tilePrefab = tilePrefabs[tileIndex];
                }

                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                tile.transform.SetParent(transform);

                TileHealth tileHealth = tile.GetComponent<TileHealth>();
                if (tileHealth != null)
                {
                    // Set the tile's hit points based on its depth level
                    tileHealth.SetHitPoints(Mathf.FloorToInt((float)y / mapSizeY * depthLevel)+2);
                }
                if (IsSurroundedByTiles(x, y))
                {
                    GameObject invisibleTile = Instantiate(invisibleTilePrefab, position, Quaternion.identity);
                    invisibleTile.transform.SetParent(transform);
                }
            }
        }

        // Generate ore deposits
        GenerateOreDeposits(ironOrePrefab, ironOreDepositCount, depositSize);
        GenerateOreDeposits(copperOrePrefab, copperOreDepositCount, depositSize);
    }

    private void GenerateOreDeposits(GameObject orePrefab, int depositCount, int depositSize)
    {
        for (int i = 0; i < depositCount; i++)
        {
            int depositX = Random.Range(-mapSizeX / 2, mapSizeX / 2);
            int depositY = Random.Range(indestructibleBorderSize, mapSizeY);
            // Declare a HashSet to store the occupied positions

            for (int j = 0; j < depositSize; j++)
            {
                int offsetX = Random.Range(-depositSize / 2, depositSize / 2);
                int offsetY = Random.Range(-depositSize / 2, depositSize / 2);

                int x = depositX + offsetX;
                int y = depositY + offsetY;

                if (IsValidPosition(x, y))
                {
                    Vector3 position = new Vector3(x, -y, 0f);

                    // Check if the position is already occupied
                    if (occupiedPositions.Contains(position))
                    {
                        // Skip this position and try another one
                        continue;
                    }

                    // Add the position to the HashSet
                    occupiedPositions.Add(position);

                    // Destroy the existing tile at the position
                    DestroyTileAtPosition(position);

                    // Instantiate the ore tile
                    GameObject oreTile = Instantiate(orePrefab, position, Quaternion.identity);
                    oreTile.transform.SetParent(transform);

                    TileHealth tileHealth = oreTile.GetComponent<TileHealth>();
                    if (tileHealth != null)
                    {
                        // Set the ore tile's hit points
                        tileHealth.SetHitPoints(1); // Assuming ore tiles have 1 hit point
                    }
                    if (IsSurroundedByTiles(x, y))
                    {
                        GameObject invisibleTile = Instantiate(invisibleTilePrefab, position, Quaternion.identity);
                        invisibleTile.transform.SetParent(transform);
                    }
                }
            }

        }
    }

    private bool IsValidPosition(int x, int y)
    {
        int halfSizeX = mapSizeX / 2;

        // Check if position is within map bounds and not in the indestructible border
        return x >= -halfSizeX + indestructibleBorderSize &&
               x < halfSizeX - indestructibleBorderSize &&
               y >= indestructibleBorderSize &&
               y < mapSizeY- indestructibleBorderSize;
    }

    private void DestroyTileAtPosition(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(position);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Tile") && collider.gameObject!=victoriumOrePrefab)
            {
                Destroy(collider.gameObject);
            }
        }
    }
    private bool IsSurroundedByTiles(int x, int y)
    {
        int leftX = x - 1;
        int rightX = x + 1;
        int topY = y + 1;
        int bottomY = y - 1;

        return TileExists(leftX, y) && TileExists(rightX, y) && TileExists(x, topY) && TileExists(x, bottomY);
    }

    private bool TileExists(int x, int y)
    {
        int halfSizeX = mapSizeX / 2;

        // Check if position is within map bounds and not in the indestructible border
        return x >= -halfSizeX &&
               x < halfSizeX &&
               y >= 0 &&
               y < mapSizeY;
    }
}
