using Assets;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{

    //Class
    //NFU = Not For Use in game, just for coparing.
    public enum OccupyState { Null, Free, NFUBeforeCanBuild, Blueprint, NFUCanDeleteWithoutFurtherConsequences, WaitingForResources, NFUBeforeDeleteResourcesNeedToBeGathered, WaitingForConstruction, Occupied };
    public enum CompareSpaceType { BuildingToMap, BuildingToBuilding, BuildingToBuildingToMap };
    //These two are for handling buildings on the map
    static Building[][] map;
    static OccupyState[][] mapOccupyState;//Write updating of this state

    //Instance
    public static int mapMaxX = 10, mapMaxY = 10;


    public Map(int cMapMaxX, int cMapMaxY)
    {
        //TODO: Uncomment on final steps
        //this.mapMaxX = cMapMaxX;
        //this.mapMaxY = cMapMaxY;
    }

    public static Building[][] getMap()
    {
        return map;
    }

    public static Map.OccupyState[][] getMapOccupyStates() {
        return mapOccupyState;
    }

    public void LoadTestMap()
    {
        int newBuildingUUID = 0;

        //Load map TMP
        map = new Building[mapMaxY][];
        mapOccupyState = new Map.OccupyState[mapMaxY][];
        int y, x;
        for (y = 0; y < mapMaxY; y++)
        {
            if (map[y] == null)
            {
                map[y] = new Building[mapMaxX];
                mapOccupyState[y] = new Map.OccupyState[mapMaxX];
            }
        }

        for (y = 0; y < mapMaxY; y++)
        {
            for (x = 0; x < mapMaxX; x++)
            {
                //Generating buildings every 3 y and 3 x spaces
                if (y % 3 == 0 && x % 3 == 0)
                {
                    putBuilding(newBuildingUUID, y, x, Map.OccupyState.Occupied);
                }

            }
        }

        //printOXMap();
        printMap();
    }

    public void printMap()
    {
        Debug.Log("Dimensions: y=" + map.Length + " x=" + map[0].Length);
        string line = "";
        for (int y = 0; y < mapMaxY; y++)
        {
            line = "";
            for (int x = 0; x < mapMaxX; x++)
            {
                if (map[y][x] != null)
                {
                    line += map[y][x].id + "\t";
                }
                else
                {
                    line += " \t";
                }
            }
            Debug.Log(y + ":" + line);
        }
    }

    public void printOXMap()
    {
        string line = "";
        for (int y = 0; y < mapMaxX; y++)
        {
            line = "";
            for (int x = 0; x < mapMaxX; x++)
            {
                line += (map[y][x] != null ? "X" : "O") + " ";
            }
            Debug.Log(y + ":" + line);
        }
    }

    public bool[,] getMapBoolArray()
    {
        bool[,] mapBoolArray = new bool[mapMaxY, mapMaxX];
        for (int y = 0; y < mapMaxY; y++)
        {
            for (int x = 0; x < mapMaxX; x++)
            {
                mapBoolArray[y, x] = map[y][x] != null ? true : false;
            }
        }

        return mapBoolArray;
    }

    ///<summary>
    /// Argument OccupyState is compared to buildings - if building is lower than argument then 
    /// building occupies place with that given state. <para />
    /// Returns list of colliding spaces. First building should also have translation.<para/>
    /// If lists are null or their count == 0 then returns null. 
    ///</summary>
    public static List<Tuple<Map.OccupyState, Tuple<int, int>>> compareOccupiedSpaces(int y, int x, bool withMap, List<int> yTranslations, List<int> xTranslations, List<long> uuids, Map.OccupyState os)
    {
        //HERETODO: Mode:   0 Map-Building   1 Building-Building   2-Map-Building-Building
        //AND: what type of occupying states buildings are trying to achieve

        //Values error checking
        //UPPER, DOWN, LEFT, RIGHT

        List<Tuple<int, int>[]> buildingsOutermostFields = new List<Tuple<int, int>[]>();
        List<List<Tuple<int, int>>> outsideOfMapBuildingsDims = new List<List<Tuple<int, int>>>();

        //TODO: Check if values are not null
        if (yTranslations == null || xTranslations == null || uuids == null)
        {
            return null;
        }

        //Check if lengths do not match each other
        if (!(yTranslations.Count == xTranslations.Count && xTranslations.Count == uuids.Count))
        {
            //Maybe: return null
            ;
        }

        //Get the min size of lists
        int argumentsListsMinLength = yTranslations.Count < xTranslations.Count ? yTranslations.Count : xTranslations.Count;
        argumentsListsMinLength = uuids.Count < argumentsListsMinLength ? uuids.Count : argumentsListsMinLength;

        for (int i = 0; i < argumentsListsMinLength; i++)
        {
            buildingsOutermostFields.Add(BuildingUUID.getOutermostFields(uuids[i]));
            if (checkIfBuildingIsOutsideTheMap(Map.mapMaxY, Map.mapMaxX, buildingsOutermostFields[i], yTranslations[i], xTranslations[i]))
            {
                if (DebuggingM.MapAssert == 2)
                {
                    Debug.Log("Building: " + i + " is outside of map");
                }
                buildingsOutermostFields[i] = null;
            }
        }

        //If there is nothing to compare on lists
        if (argumentsListsMinLength == 0)
        {
            return null;
        }
        //TODO: Pass map sizes when everything is done
        //b[0].y = b[1].y - yTranslation1
        List<Tuple<Map.OccupyState, Tuple<int, int>>> collidingFields = new List<Tuple<Map.OccupyState, Tuple<int, int>>>();

        //Get max size of fields
        //[y, x]
        Tuple<int, int> testMapSize = getTestMapSizeForCompareOccupiedSpaces(yTranslations, xTranslations, buildingsOutermostFields);
        Map.OccupyState[,] testMap = new Map.OccupyState[testMapSize.Item1, testMapSize.Item2];
        Debug.Log("TestMapSize Y X:" + testMapSize.Item1 + " " + testMapSize.Item2);



        List<bool[,]> b = new List<bool[,]>();
        foreach (long uuid in uuids)
        {
            b.Add(BuildingUUID.getSpaceOccupied(uuid));
        }

        byte startB = 0;
        if (withMap)
        {
            for (int yIt = y, xIt; yIt < y + testMapSize.Item1; yIt++)
            {
                for (xIt = x; xIt < x + testMapSize.Item2; xIt++)
                {
                    if (map[yIt][xIt] != null)
                    {
                        testMap[yIt - y, xIt - x] = mapOccupyState[yIt][xIt];
                    }
                }
            }
        }
        else
        {

            //TODO: Check this for's
            for (int ySB = yTranslations[startB], xSB; ySB < yTranslations[startB] + b[startB].GetLength(0); ySB++)
            {
                for (xSB = xTranslations[startB]; xSB < xTranslations[startB] + b[startB].GetLength(1); xSB++)
                {
                    Debug.Log(ySB + " " + xSB + " startB" + startB + " tran:" + yTranslations[startB] + " " + 
                        xTranslations[startB]);
                    testMap[ySB, xSB] = ((b[startB])[ySB - yTranslations[startB], xSB - xTranslations[startB]]
                        == true ? OccupyState.Occupied : OccupyState.Free);
                }
            }

            startB = 1;
        }



        //HERETODO: check if before and after map buildings do not raise errors
        for (int actualBIt = startB, yNB, xNB; actualBIt < argumentsListsMinLength; actualBIt++)
        {
            for (yNB = yTranslations[actualBIt]; yNB < b[actualBIt].GetLength(0) + yTranslations[actualBIt] && yNB < testMapSize.Item1; yNB++)
            {
                for (xNB = xTranslations[actualBIt]; xNB < b[actualBIt].GetLength(1) + xTranslations[actualBIt] && xNB < testMapSize.Item2; xNB++)
                {
                    if ((b[actualBIt])[yNB - yTranslations[actualBIt], xNB - xTranslations[actualBIt]] == true)
                    {
                        if ((int)testMap[yNB, xNB] < (int)os)
                        {
                            testMap[yNB, xNB] = os;
                        }
                        else
                        {
                            //Adds second building state to the colliding fields List
                            collidingFields.Add(new Tuple<OccupyState, Tuple<int, int>>(testMap[yNB, xNB], new Tuple<int, int>(yNB, xNB)));
                        }
                    }
                }
            }
        }

        return collidingFields;
    }

    //UPPER, DOWN, LEFT, RIGHT - bOuterMostFields(y,x) - Like WSAD
    static bool checkIfBuildingIsOutsideTheMap(int mapMaxY, int mapMaxX, Tuple<int, int>[] bOutermostFields, int yTranslation, int xTranslation)
    {
        bool outsideOfMap = false;

        if (bOutermostFields[0].Item1 + yTranslation < 0)
        {
            outsideOfMap = true;
        }
        else if (bOutermostFields[1].Item1 + yTranslation > mapMaxY)
        {
            outsideOfMap = true;
        }
        else if (bOutermostFields[2].Item2 + xTranslation < 0)
        {
            outsideOfMap = true;
        }
        else if (bOutermostFields[3].Item2 + xTranslation > mapMaxX)
        {
            outsideOfMap = true;
        }
        return outsideOfMap;
    }

    public static Tuple<int, int> getTestMapSizeForCompareOccupiedSpaces(List<int> yTranslations, List<int> xTranslations, List<Tuple<int, int>[]> buildingsOutermostFields)
    {
        int minY = int.MaxValue, maxY = 0, minX = int.MaxValue, maxX = 0;

        //UPPER, DOWN, LEFT, RIGHT - bOuterMostFields(y,x) - Like WSAD
        for (int i = 0; i < buildingsOutermostFields.Count; i++)
        {
            //Y-UP
            if ((buildingsOutermostFields[i])[0].Item1 + yTranslations[i] < minY)
            {
                minY = (buildingsOutermostFields[i])[0].Item1 + yTranslations[i];
            }
            //Y-DOWN
            if ((buildingsOutermostFields[i])[1].Item1 + yTranslations[i] > maxY)
            {
                maxY = (buildingsOutermostFields[i])[1].Item1 + yTranslations[i];
            }
            //X-LEFT
            if ((buildingsOutermostFields[i])[2].Item2 + xTranslations[i] < minX)
            {
                minX = (buildingsOutermostFields[i])[2].Item2 + xTranslations[i];
            }
            //X-RIGHT
            if ((buildingsOutermostFields[i])[3].Item2 + xTranslations[i] > maxX)
            {
                maxX = (buildingsOutermostFields[i])[3].Item2 + xTranslations[i];
            }
        }

        Debug.Log("YBOF: " + (buildingsOutermostFields[0])[0].Item1 + (buildingsOutermostFields[0])[1].Item1);
        Debug.Log("maxY" + maxY + " minY" + minY);

        Tuple<int, int> size = new Tuple<int, int>(maxY - minY, maxX - minX);

        return size;
    }

    static bool canBuildUnderField(OccupyState os1, OccupyState os2)

    {
        if (os1 == OccupyState.Free || os2 == OccupyState.Free)
        {//One of buildings has free space
            return true;
        }
        return false;
    }

    static bool canPutBlueprintUnderField()
    {
        throw new NotImplementedException();
        return false;
    }

    public static bool canBuildBuilding(int fuuid, int y, int x, Map.OccupyState bOccupyState)
    {
        //HERETODO: One line below
        List<Tuple<Map.OccupyState, Tuple<int, int>>> collidingFields = compareOccupiedSpaces(y, x, true, new List<int>(), new List<int>(), new List<long> { (long)fuuid }, bOccupyState);

        if ((collidingFields == null || collidingFields.Count == 0) && !checkIfBuildingIsOutsideTheMap(10, 10, BuildingUUID.getOutermostFields(fuuid), y, x))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static byte putBuilding(int fuuid, int y, int x, Map.OccupyState bOccupyState)
    {
        if (canBuildBuilding(fuuid, y, x, bOccupyState))
        {
            Building bTmp = new Building(x, y, fuuid);
            bool[,] spaceOccupied = bTmp.getSpaceOccupied();
            for (int yIt = y; yIt < y + spaceOccupied.GetLength(0); yIt++)
            {
                for (int xIt = x; xIt < x + spaceOccupied.GetLength(1); xIt++)
                {
                    if (spaceOccupied[yIt - y, xIt - x] == true)
                    {
                        map[yIt][xIt] = bTmp;
                        mapOccupyState[yIt][xIt] = bOccupyState;
                    }
                }
            }
        }
        else
        {
            if (DebuggingM.MapAssert == 1)
            {
                Debug.Log("Error putBuilding");
            }
            return 1;
        }

        return 0;
    }
}
