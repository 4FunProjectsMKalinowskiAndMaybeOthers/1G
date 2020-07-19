using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BuildingManager.LoadBuildings();

        Building bd = BuildingFactory.Build(1);

        Debug.Log(bd.ToString());

        bd.doProduction(100);
        Debug.Log("After production 1/10");
        Debug.Log(bd.ToString());

        bd.doProduction(900);
        Debug.Log("After production 10/10");
        Debug.Log(bd.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
