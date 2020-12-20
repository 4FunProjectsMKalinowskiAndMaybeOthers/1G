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
    //Code tags: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/recommended-tags-for-documentation-comments

    //When checking after construcion of building object then if not placed it must be erasted by Garbage Collector so it is better to check it before construction
    public Building(int y, int x, int cuuid)
    {
        if (Map.canBuildBuilding(cuuid, y, x, Map.OccupyState.WaitingForConstruction))
        {
            int? freeId = Building.getFreeId(true);
            if (freeId.HasValue)
            {
                //Building
                id = (long)freeId;
                uuid = cuuid;
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
        else
        {
            if (DebuggingM.BuildingAssert == 2)
            {
                Debug.Log("No space on the map in(" + x + "," + y + ")");
            }
            //Cannot build action - no space on the map in (x,y)
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
            return freeIds.Count - 1;
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
