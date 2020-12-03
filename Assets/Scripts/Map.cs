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
    static Building[][] map;

    //Instance
    public static int mapMaxX = 10, mapMaxY = 10;


    public Map(int cMapMaxX, int cMapMaxY) {
        //TODO: Uncomment on final steps
        //this.mapMaxX = cMapMaxX;
        //this.mapMaxY = cMapMaxY;
    }

    public static Building[][] getMap() {
        return map;
    }

    public void LoadTestMap()
    {
        //!!!!!!!!! Perfectly wrong uuid
        int newBuildingUUID = 0;

        //Load map TMP
        map = new Building[mapMaxY][];
        Building bTmp = null;
        int y, x;
        for (y = 0; y < mapMaxY; y++) {
            if (map[y] == null)
            {
                map[y] = new Building[mapMaxX];
            }
            for (x = 0; x < mapMaxX; x++)
            {
                Debug.Log("y" + y + " x" + x);
                //Generating buildings every 3 y and 3 x spaces
                if (y % 3 == 0 && x % 3 == 0)
                {
                    putBuilding(0, y, x);
                    /*
                    for (int yTmp = 0; yTmp < 3; yTmp++)
                    {
                        if ((y + yTmp < mapMaxY) && map[y + yTmp] == null)
                        {
                            map[y + yTmp] = new Building[mapMaxX];
                        }

                        bTmp = new Building(x, y, newBuildingUUID);
                        for (int xTmp = 0; xTmp < 3; xTmp++) {
                            //Empty spaces, indentation in building
                            if (yTmp == 1 && (xTmp == 0 || xTmp == 2)) {
                                //Empty space
                            } else
                            {
                                //Building
                                if (y + yTmp < mapMaxY && x + xTmp < mapMaxX)
                                {
                                    //Debug.Log((y + yTmp) + " :?: " + (x + xTmp) + " bTmp" + bTmp);
                                    map[y + yTmp][x + xTmp] = bTmp;
                                }
                            }
                        }
                    }*/
                }

            }
        }

        //printOXMap();
        printMap();
    }

    public void printMap() {
        Debug.Log("Dimensions: y=" + map.Length + " x=" + map[0].Length);
        string line = "";
        for (int y = 0; y < mapMaxY; y++) {
            line = "";
            for (int x = 0; x < mapMaxX; x++) {
                if (map[y][x] != null)
                {
                    line += map[y][x].id + "_";
                }
                else {
                    line += " _"; //Two spaces instead of number
                }
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
        bool[,] mapBoolArray = new bool[mapMaxY, mapMaxX];
        for (int y = 0; y < mapMaxY; y++) {
            for (int x = 0; x < mapMaxX; x++) {
                mapBoolArray[y, x] = map[y][x] != null ? true : false;
            }
        }

        return mapBoolArray;
    }


    //Returns null if building is outside the map
    public static List<Tuple<Map.OccupyState, Tuple<int, int>>> compareOccupiedSpaces(List<int> yTranslations, List<int> xTranslations, List<long> uuids, Map.OccupyState os) {
        //HERETODO: Mode:   0 Map-Building   1 Building-Building   2-Map-Building-Building
        //AND: what type of occupying states buildings are trying to achieve

        //Values error checking
        //UPPER, DOWN, LEFT, RIGHT

        List<Tuple<int, int>[]> bOFs = new List<Tuple<int, int>[]>();
        List<List<Tuple<int, int>>> outsideOfMapBuildingsDims = new List<List<Tuple<int, int>>>();

        //TODO: Check if sizes are equal in input lists
        {
            for (int i = 0; i < uuids.Count; i++) {
                bOFs.Add(BuildingUUID.getOutermostFields(uuids[i]));
                //HEREHEREHERETODO:
                outsideOfMapBuildingsDims = 
                if (checkIfBuildingIsOutsideTheMap(Map.mapMaxY, Map.mapMaxX, bOFs[i], yTranslations[i], xTranslations[i])) ;
                {
                    if (DebuggingM.MapAssert == 2)
                    {
                        Debug.Log("Building: " + i + " is outside of map");
                    }
                    return null;
                }
            }
        }
        //TODO: Pass map sizes when everything is done
        //b[0].y = b[1].y - yTranslation1
        List<Tuple<Map.OccupyState, Tuple<int, int>>> collidingFields = new List<Tuple<Map.OccupyState, Tuple<int, int>>>();

        //Get max size of fields
        //[y, x]
        Tuple<int, int> testMapSize = getTestMapSizeForCompareOccupiedSpaces(yTranslations, xTranslations, uuids, bOFs);
        Map.OccupyState[,] testMap = new Map.OccupyState[testMapSize.Item1, testMapSize.Item2];
        Debug.Log("TestMapSize:" + testMapSize.Item1 + " " + testMapSize.Item2);

       

        List<bool[,]> b = new List<bool[,]>();
        b.Add(BuildingUUID.getSpaceOccupied(uuid0));
        b.Add(BuildingUUID.getSpaceOccupied(uuid1));
        byte startB = 0;
        byte endB = 1;//(byte)(startB == 1 ? 0 : 1);

        for (int ySB = 0; ySB < b[startB].GetLength(0); ySB++) {
            for (int xSB = 0; xSB < b[startB].GetLength(1); xSB++) {
                testMap[ySB, xSB] = ((b[startB])[ySB, xSB] == true ? OccupyState.Occupied : OccupyState.Free);
            }
        }

        //HERETODO: check if before and after map buildings do not raise errors
        for (int yEB = yTranslation1; yEB < b[endB].GetLength(0) + yTranslation1 && yEB < testMapSize.Item1 ; yEB++)
        {
            for (int xEB = xTranslation1; xEB < b[endB].GetLength(1) + xTranslation1 && xEB < testMapSize.Item2; xEB++)
            {
                if ((b[endB])[yEB - yTranslation1, xEB - xTranslation1] == true) {
                    //if ((int)testMap[yEB, xEB] < (int)os)
                    if((int)testMap[yEB, xEB] < (int)os)
                    {
                        testMap[yEB, xEB] = os;
                    }
                    else {
                        //Adds second building state to the colliding fields List
                        collidingFields.Add(new Tuple<OccupyState, Tuple<int, int>> (testMap[yEB, xEB], new Tuple<int, int>(yEB, xEB)));
                    }
                }
            }
        }

        return collidingFields;
    }

    //UPPER, DOWN, LEFT, RIGHT - bOuterMostFields(y,x) - Like WSAD
    static bool checkIfBuildingIsOutsideTheMap(int mapMaxY, int mapMaxX, Tuple<int,int>[] bOutermostFields, int yTranslation, int xTranslation) {
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
        else if (bOutermostFields[3].Item2 + xTranslation > mapMaxX) {
            outsideOfMap = true;
        }
        Debug.Log(bOutermostFields + " " + outsideOfMap);
        return outsideOfMap;
    }

    public static Tuple<int, int> getTestMapSizeForCompareOccupiedSpaces(List<int> yTranslations, List<int> xTranslations, List<long> uuids, List<Tuple<int, int>[]> bOFs){
        //y, x
        int y = 0;
        int x = 0;

        //UPPER, DOWN, LEFT, RIGHT - bOuterMostFields(y,x) - Like WSAD
        for (int i = 0; i < uuids.Count; i++) {
            if((bOFs[i])[0] + yTranslations[i]
                }

        Tuple<int, int> size = new Tuple<int, int>(y, x);
        /*
            ()
            , (BuildingUUID.getSpaceOccupied(uuid0).GetLength(1) + xTranslation1 > BuildingUUID.getSpaceOccupied(uuid1).GetLength(1) ? BuildingUUID.getSpaceOccupied(uuid0).GetLength(1) + yTranslation1 : BuildingUUID.getSpaceOccupied(uuid1).GetLength(1))
            );
            */
        return size;
    }

    static bool canBuildUnderField(OccupyState os1, OccupyState os2) {
        if (os1 == OccupyState.Free || os2 == OccupyState.Free)
        {//One of buildings has free space
            return true;
        }
        return false;
    }

    static bool canPutBlueprintUnderField() {
        throw new NotImplementedException();
        return false;
    }

    public static bool canBuildBuilding(Building b, int y, int x) {
        List<Tuple<Map.OccupyState, Tuple<int, int>>> collidingFields = compareOccupiedSpaces(0, 0, 0, 0, b.uuid, 0, OccupyState.Occupied);

        if (collidingFields == null || collidingFields.Count == 0)
        {
            return true;
        }
        else {
            return false;
        }
    }

    public static byte putBuilding(int fuuid, int y, int x) {
        List<Tuple<OccupyState, Tuple<int, int>>> collidingFields = compareOccupiedSpaces(0, 0, 0, 0, fuuid, 0, OccupyState.Occupied);
        if (collidingFields == null || collidingFields.Count == 0)
        {
            Building bTmp = new Building(x, y, fuuid);
            bool[,] spaceOccupied = bTmp.getSpaceOccupied();
            for (int yIt = y; yIt < y + spaceOccupied.GetLength(0); yIt++) {
                for (int xIt = x; xIt < x + spaceOccupied.GetLength(1); xIt++) {
                    if (spaceOccupied[yIt, xIt] == true) {
                        map[yIt][xIt] = bTmp;
                    }
                }
            }
        }
        else {
            if (DebuggingM.MapAssert == 1) {
                Debug.Log("Error putBuilding");
            }
            return 1;
        }

        return 0;
    }
}
