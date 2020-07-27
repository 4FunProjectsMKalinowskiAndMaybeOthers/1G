using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Building
{
    public static List<bool> freeIds = new List<bool>();
    private static int minIdNumber=0, maxIdNumber = int.MaxValue;

    public long id;
    public long uuid;
    public int x, y;
    public static int test= 0;

    public Building(int x, int y, int cuuid) {
        int? freeId = getFreeId(true);
        if (freeId.HasValue)
        {
            id = (int)freeId;
            freeIds.Add(true);
            uuid = cuuid;
        }else
        {
            string trace = "No free id found in class:" + this.GetType().FullName + 
                " method:" + ((new StackTrace()).GetFrame(0).GetMethod());
            UnityEngine.Debug.Log(trace);
            throw new IndexOutOfRangeException(trace);
        }
    }

    public bool[,] getSpaceOccupied() {
        return BuildingUUID.getSpaceOccupied(uuid);
    }

    public static int? getFreeId(bool occupy) {
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
        else {
            for (int i=minIdNumber; i<maxIdNumber; i++ ) {
                if (freeIds[i] == false) {
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

}
