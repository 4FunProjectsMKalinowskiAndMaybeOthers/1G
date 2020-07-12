using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    //Class
    static int sx=0, sy=0;

    //Instance
    int mapMaxX = 10, mapMaxY = 10;
    Building[][] map;
    
    public void Load()
    {
        //Load map TMP
        Building lastBuilding = null;
        map = new Building[mapMaxX][];
        for (int x = 0; x < mapMaxX; x++)
        {
            map[x] = new Building[mapMaxY];
            for (int y = 0; y < mapMaxY; y++)
            {
                
                if (x % 3 == 0 && y % 3 == 0)
                {
                    lastBuilding = new Building(x * mapMaxY + y, sx, sy);
                        sx += 3;
                    sy += 3;
                }
                map[x][y] = lastBuilding;
            }
        }
    }

    public bool[][] mapSquaresToString() {
        bool[,] mapBoolArray = new bool[mapMaxX, mapMaxY];
        for (int x = 0, y; x < mapMaxX; x++) {
            for (y = 0; y < mapMaxY; y++) {
                mapBoolArray 
            }
        }

        return null; //!!!!!!!!!!!!!!!
    }

    /*
    public List<Tuple<int, int>> combineBuildingOccupingSpace(int x, int y, Building newBuilding) {
        //Returns list of colliding buildings dimensions
        return null; //!!!!!!!!!!!!!!
    }
    */
}
