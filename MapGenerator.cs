using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int defaultSize = 100;

    private Color[,] map;

    private void Awake()
    {
        GenerateMap(defaultSize);
    }

    private void GenerateMap(int size)
    {
        Debug.Log("Generating map with size: " + size);

        map = new Color[size, size];
        float step = 1f / size;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                map[x, y] = new Color(x * step, y * step, 0f);

                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tile.transform.position = new Vector3(x, y, 0f);
                tile.transform.localScale = new Vector3(step, step, 1f);
                tile.GetComponent<Renderer>().material.color = map[x, y];
                tile.transform.SetParent(transform);
            }
        }
    }
}
