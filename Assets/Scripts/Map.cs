using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    //Class
    static int uuid = 0;

    //Instance
    int mapMaxX = 10, mapMaxY = 10;
    Building[][] map;

    public Map(int cMapMaxX, int cMapMaxY) {
        this.mapMaxX = cMapMaxX;
        this.mapMaxY = cMapMaxY;
    }

    public void LoadTestMap()
    {
        //Load map TMP
        map = new Building[mapMaxY][];
        Building bTmp = null;
        int y;
        for (y = 0; y < mapMaxY; y++) {
            if (map[y] == null)
            {
                map[y] = new Building[mapMaxX];
            }
            int x;
            for (x = 0; x < mapMaxX; x++)
            {
                //Generating buildings every 3 y and 3 x spaces
                if (y % 3 == 0 && x % 3 == 0)
                {
                    for (int yTmp = 0; yTmp < 3; yTmp++)
                    {
                        if ((y+yTmp<mapMaxY) && map[y + yTmp] == null)
                        {
                            map[y+yTmp] = new Building[mapMaxX];
                        }
                        bTmp = new Building(x, y, uuid++);
                        for (int xTmp = 0; xTmp < 3; xTmp++) {
                            //Empty spaces, indentation in building
                            if (yTmp == 1 && (xTmp == 0 || xTmp == 2)) {
                                //Empty space
                            }else
                            {
                                //Building
                                if (y + yTmp < mapMaxY && x + xTmp < mapMaxX)
                                {
                                    Debug.Log((y + yTmp) + " :?: " + (x + xTmp) + " bTmp" + bTmp);
                                    map[y + yTmp][x + xTmp] = bTmp;
                                }
                            }
                        }
                    }
                }

            }
        }

        printOXMap();
    }

    public void printMap() {
        Debug.Log("Dimensions: y=" + map.Length + " x=" + map[0].Length);
        string line = "";
        for (int y = 0; y < mapMaxY; y++) {
            line = "";
            for (int x = 0; x < mapMaxX; x++) {
                line += map[y][x].id + " ";
            }
            Debug.Log(y + ":" + line);
        }
    }

    public void printOXMap() {
        string line = "";
        for (int y = 0; y < mapMaxX; y++) {
            line = "";
            for (int x = 0; x < mapMaxX; x++) {
                line += (map[y][x] != null ? "X" : "O") + " ";
            }
            Debug.Log(y + ":" + line);
        }
    }

    public bool[,] getMapBoolArray() {
        bool[,] mapBoolArray = new bool[mapMaxX, mapMaxY];
        for (int y = 0; y < mapMaxY; y++) {
            for (int x = 0; x < mapMaxX; x++) {
                mapBoolArray[y, x] = map[y][x] != null ? true : false;
            }
        }

        return mapBoolArray; //!!!!!!!!!!!!!!!
    }

    /*
    public List<Tuple<int, int>> combineBuildingOccupingSpace(int x, int y, Building newBuilding) {
        //Returns list of colliding buildings dimensions
        return null; //!!!!!!!!!!!!!!
    }
    */
}
