using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class mapgen : MonoBehaviour {
    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    //2d Array to hold the map
    int[,] map;

    //call gen map first
    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }

    //initialize map, then call random fill
    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        //smooth the map over 5 times, can change this
        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }


        Meshgen meshGen = GetComponent<Meshgen>();
        meshGen.GenerateMesh(map, 1);
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            //use the time to set the seed
            seed = Time.time.ToString();
        }

        //use hashcode of seed value to generate pseudoRandom value
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        //go through the map
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //if on an edge, make the block 1 ie. filled
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    //non edge, use pseudoRandom to fill or not
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;

            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    //color
    void OnDrawGizmos()
    {
        //if (map != null)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
        //            Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
        //            Gizmos.DrawCube(pos, Vector3.one);
        //        }
        //    }
        //}
    }

}
