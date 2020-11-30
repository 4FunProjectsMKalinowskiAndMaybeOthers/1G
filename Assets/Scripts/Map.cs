using Assets;
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
    int mapMaxX = 10, mapMaxY = 10;


    public Map(int cMapMaxX, int cMapMaxY) {
        this.mapMaxX = cMapMaxX;
        this.mapMaxY = cMapMaxY;
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
        bool[,] mapBoolArray = new bool[mapMaxY, mapMaxX];
        for (int y = 0; y < mapMaxY; y++) {
            for (int x = 0; x < mapMaxX; x++) {
                mapBoolArray[y, x] = map[y][x] != null ? true : false;
            }
        }

        return mapBoolArray;
    }

    public static List<Tuple<Map.OccupyState, Tuple<int, int>>> compareOccupiedSpaces(int yTranslation0, int xTranslation0, int yTranslation1, int xTranslation1, long uuid0, long uuid1, Map.OccupyState os) {
        //HERETODO: Mode:   0 Building-Building   1 Map-Building   2-Map-Building-Building
        //AND: what type of occupying states buildings are trying to achieve

        //Values error checking
        //UPPER, DOWN, LEFT, RIGHT
        Tuple<int, int>[,] bOF0 = BuildingUUID.getOutermostFields(uuid0);
        Tuple<int, int>[,] bOF1 = BuildingUUID.getOutermostFields(uuid1);
        if (checkIfBuildingIsOutsideTheMap(bOF0, yTranslation0, xTranslation0) 
            || checkIfBuildingIsOutsideTheMap(bOF1, yTranslation1, xTranslation1)) {
            return null;
        }

        //b[0].y = b[1].y - yTranslation1
        List<Tuple<Map.OccupyState, Tuple<int, int>>> collidingFields = new List<Tuple<Map.OccupyState, Tuple<int, int>>>();

        //Get max size of fields
        //[y, x]
        Tuple<int, int> testMapSize = getTestMapSizeForCompareOccupiedSpaces(yTranslation1, xTranslation1, uuid0, uuid1);
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

    bool checkIfBuildingIsOutsideTheMap(Tuple<int,int>[,] bOutermostFields, yTranslation, xTranslation) {
        if()
    }

    public static Tuple<int, int> getTestMapSizeForCompareOccupiedSpaces(int yTranslation1, int xTranslation1, long uuid0, long uuid1){
        //y, x
        Tuple<int, int> size = new Tuple<int, int>(
            (BuildingUUID.getSpaceOccupied(uuid0).GetLength(0) + yTranslation1 > BuildingUUID.getSpaceOccupied(uuid1).GetLength(0) ?  BuildingUUID.getSpaceOccupied(uuid0).GetLength(0) + yTranslation1 : BuildingUUID.getSpaceOccupied(uuid1).GetLength(0))
            , (BuildingUUID.getSpaceOccupied(uuid0).GetLength(1) + xTranslation1 > BuildingUUID.getSpaceOccupied(uuid1).GetLength(1) ? BuildingUUID.getSpaceOccupied(uuid0).GetLength(1) + yTranslation1 : BuildingUUID.getSpaceOccupied(uuid1).GetLength(1))
            );
        return size;
    }

    public bool canBuildUnderField(OccupyState os1, OccupyState os2) {
        if (os1 == OccupyState.Free || os2 == OccupyState.Free)
        {//One of buildings has free space
            return true;
        }
        return false;
    }

    public bool canPutBlueprintUnderField() {
        throw new NotImplementedException();
        return false;
    }
}
