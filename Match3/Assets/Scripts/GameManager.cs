using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    [SerializeField] private int Height=10;
    [SerializeField] private int Width=10;
    private Transform[,] map;
    private Transform temp;

    int selections = 0;
    int selectedX;
    int selectedY;

    bool isMoving = false;
    bool isStart = true;
    bool isSwapedSuccsesful = true;


    // Start is called before the first frame update
    void Start()
    {
        map = new Transform[Width, Height];
        map=Generator.Instance.GenerateMap(map);
        while (isStart)
        {
            if (!isMoving)
                Match();
            bool firstTime = true;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (Move(map[i, j], i, j, 10000000000f))
                    {
                        if (firstTime)
                        {
                            isMoving = false;
                            firstTime = false;

                        }
                    }
                    else
                        isMoving = true;
                }
            }

        }
    }
    public void UpdateTilesPosition(int x, int y)
    {
        for (int i = y - 1; i >= 0; i--)
        {
            if (map[x, i] != null)
            {
                map[x, i + 1] = map[x, i];
                map[x, i] = null;
            }
        }
        map[x, 0] = Generator.Instance.GenerateMissingTile(map,x, y);
        isMoving = true;
        isSwapedSuccsesful = true;
        UnselectAll();
    }
    public void UnselectAll()
    {

        foreach (Transform item in map)
        {
            item.GetComponent<TileControl>().Unselect();
        }
        selections = 0;
    }
    public void Match()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            int count = 0;
            string name = "-1";
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == null)
                    continue;
                TileControl ctrl = map[i, j].GetComponent<TileControl>();
                if (ctrl.name == name)
                {
                    count++;
                    if (count >= 3 && j + 1 == map.GetLength(1))
                    {
                        for (int k = j - count + 1; k < j + 1; k++)
                        {

                            Destroy(map[i, k].gameObject);
                            UpdateTilesPosition(i, k);
                        }
                        return;
                    }
                }
                else
                {
                    if (count >= 3)
                    {
                        for (int k = j - count; k < j; k++)
                        {

                            Destroy(map[i, k].gameObject);
                            UpdateTilesPosition(i, k);
                        }
                        return;
                    }
                    else
                    {
                        name = ctrl.name;
                        count = 1;
                    }
                }

            }
        }
        if (!isMoving)
            for (int i = 0; i < map.GetLength(1); i++)
            {
                int count = 0;
                string name = "-1";
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    if (map[j, i] == null)
                        continue;
                    TileControl ctrl = map[j, i].GetComponent<TileControl>();
                    if (ctrl.name == name)
                    {
                        count++;
                        if (count >= 3 && j + 1 == map.GetLength(0))
                        {
                            for (int k = j - count + 1; k < j + 1; k++)
                            {

                                Destroy(map[k, i].gameObject);
                                UpdateTilesPosition(k, i);
                            }
                            return;
                        }
                    }
                    else
                    {
                        if (count >= 3)
                        {
                            for (int k = j - count; k < j; k++)
                            {
                                Destroy(map[k, i].gameObject);
                                UpdateTilesPosition(k, i);
                            }
                            return;
                        }
                        else
                        {
                            name = ctrl.name;
                            count = 1;
                        }
                    }
                }
            }
        isStart = false;
    }
    void Swap()
    {
        selections = 0;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j].GetComponent<TileControl>().isSelected)
                {
                    if (selections == 0)
                    {
                        selections++;
                        selectedX = i;
                        selectedY = j;
                        temp = map[i, j];
                    }
                    if (selections > 0 && temp != map[i, j])
                    {

                        if ((selectedY == j || selectedX == i) &&
                            (Mathf.Abs(selectedX - i) == 1 ||
                            Mathf.Abs(selectedY - j) == 1))
                        {
                            if (isSwapedSuccsesful)
                            {

                                Transform buf = map[selectedX, selectedY];
                                map[selectedX, selectedY] = map[i, j];

                                map[i, j] = buf;
                                selectedX = i;
                                selectedY = j;
                                temp = map[i, j];
                                isSwapedSuccsesful = false;
                            }
                            else
                            {

                                Transform buf = map[selectedX, selectedY];
                                map[selectedX, selectedY] = map[i, j];

                                map[i, j] = buf;
                                isSwapedSuccsesful = true;
                                UnselectAll();
                            }

                        }
                        else
                        {
                            UnselectAll();
                        }


                    }
                }
            }
        }
    }
    public bool Move(Transform transform, int x, int y, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(-0.5f * x + (map.GetLength(0)-1)*0.5f/2.0f,
            -0.5f * y + (map.GetLength(1) - 1) * 0.5f / 2.0f,
            0f), 
            speed * Time.deltaTime);
        return transform.position == 
            new Vector3(-0.5f * x + (map.GetLength(0) - 1) * 0.5f / 2.0f,
            -0.5f * y + (map.GetLength(1) - 1) * 0.5f / 2.0f,
            0f);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
            Swap();


        bool firstTime = true;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (Move(map[i, j], i, j, 1.5f))
                {
                    if (firstTime)
                    {
                        isMoving = false;
                        firstTime = false;

                    }
                }
                else
                    isMoving = true;
            }
        }
        if (!isMoving)
            Match();
    }
}
