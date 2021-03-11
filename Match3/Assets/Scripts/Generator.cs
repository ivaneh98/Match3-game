using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Manager<Generator>
{

    [SerializeField] private List<Transform> objects;
    public Transform[,] GenerateMap(Transform[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Transform obj = Instantiate(objects[Random.Range(0, objects.Count)],
                    new Vector3(-0.5f * i + (map.GetLength(0) - 1) * 0.5f / 2.0f,
                    -0.5f * j + (map.GetLength(1) - 1) * 0.5f / 2.0f,
                    0f),
                    Quaternion.identity);
                map[i, j] = obj;
            }
        }
        return map;
    }

    public Transform GenerateMissingTile(Transform[,] map, int x, int y)
    {
        Transform obj = Instantiate(objects[Random.Range(0, objects.Count)],
                    new Vector3(-0.5f * x + (map.GetLength(0) - 1) * 0.5f / 2.0f,
                    (map.GetLength(1)+3) * 0.5f / 2.0f,
                    0f), 
                                Quaternion.identity);
        return obj;
    }


}
