using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Building
{
    //Class
    public static int test = 0;
    private static int minIdNumber = 0, maxIdNumber = int.MaxValue;
    public static List<bool> freeIds = new List<bool>();

    //Object
    public long id;
    public long uuid;
    public int x, y;
    
    //When checking after construcion of building object then if not placed it must be erasted by Garbage Collector so it is better to check it before construction
    public Building(int x, int y, int cuuid) {
        int? freeId = Building.getFreeId(true);
        if (freeId.HasValue)
        {
            if (Map.compareOccupiedSpaces(x, y, this.uuid, cuuid, Map.OccupyState.Free).Count == 0)
            {
                //Building
                id = (int)freeId;
                uuid = cuuid;
            }else
            {
                //Cannot build action - no space on the map in (x,y)
            }
        }else
        {
            //Cannot build action - no free id for a building
            
            /*string trace = "No free id found in class:" + this.GetType().FullName + 
                " method:" + ((new StackTrace()).GetFrame(0).GetMethod());
            UnityEngine.Debug.Log(trace);
            throw new IndexOutOfRangeException(trace);
            */
        }

    }

    public static int? getFreeId(bool occupy)
    {
        //Checking after last id
        if (freeIds.Count <= maxIdNumber)
        {
            if (occupy == true)
            {
                freeIds.Add(true);
            }
            return freeIds.Count + 1;
        }
        //Check if there is space in the middle of ids
        else
        {
            for (int i = minIdNumber; i < maxIdNumber; i++)
            {
                if (freeIds[i] == false)
                {
                    if (occupy == true)
                    {
                        freeIds[i] = true;
                    }
                    return i;
                }
            }
        }

        //No free id found
        return null;

    }

    public bool[,] getSpaceOccupied() {
        return BuildingUUID.getSpaceOccupied(uuid);
    }
}
