using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceFillingTestMain : MonoBehaviour
{

    //See building class on Lukas's branch and adapt map to it
    Map m;

    // Start is called before the first frame update
    void Start()
    {
        m = new Map(10, 10);
        m.LoadTestMap();

        List<Tuple<Map.OccupyState, Tuple<int, int>>> collidingFields = Map.compareOccupiedSpaces(0, 0, 0, 0, Map.OccupyState.Occupied);

        Debug.Log("Colliding fields");
        foreach (Tuple<Map.OccupyState, Tuple<int, int>> tMain in collidingFields)
        {
            Debug.Log(tMain.Item2.Item1 + " " + tMain.Item2.Item2 + " " + tMain.Item1.ToString());
        }
    }

// Update is called once per frame
void Update()
    {
        
    }
}
