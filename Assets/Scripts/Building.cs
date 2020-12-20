using Assets;
using Assets.Scripts;
using System;
using System.Collections.Generic;
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
    public Building(int y, int x, int cuuid)
    {
        int? freeId = Building.getFreeId(true);
        if (freeId.HasValue)
        {
            //HERETODO: REPAIR a function invocation compareOccupiedSpaces to check for building and map occupation
            UnityEngine.Debug.Log("cuuid: " + cuuid);
            //TODO: WHen comparing with map and building only one uuid is needed
            //if (Map.compareOccupiedSpaces(x, y, x, y, this.uuid, cuuid, Map.OccupyState.Free).Count == 0)

            if (Map.canBuildBuilding(cuuid, y, x, Map.OccupyState.WaitingForConstruction))
            {
                //Building
                id = (long)freeId;
                uuid = cuuid;
                Debug.Log("New b id: " + id);
            }
            else
            {
                if (DebuggingM.BuildingAssert == 2)
                {
                    Debug.Log("No space on the map in(" + x + "," + y + ")");
                }
                //Cannot build action - no space on the map in (x,y)
            }
        }
        else
        {
            //Cannot build action - no free id for a building
            if (DebuggingM.BuildingAssert == 2)
            {
                Debug.Log("No free id found for the building: " + cuuid + " on (" + x + "," + y + ")");
            }

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

    public bool[,] getSpaceOccupied()
    {
        return BuildingUUID.getSpaceOccupied(uuid);
    }
}
