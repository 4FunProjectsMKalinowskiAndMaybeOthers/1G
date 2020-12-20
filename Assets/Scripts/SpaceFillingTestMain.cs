using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceFillingTestMain : MonoBehaviour
{
    //https://stackoverflow.com/questions/4942113/is-there-a-format-code-shortcut-for-visual-studio
    //See building class on Lukas's branch and adapt map to it
    Map m;

    // Start is called before the first frame update
    void Start()
    {

        m = new Map(10, 10);
        m.LoadTestMap();
    }

// Update is called once per frame
void Update()
    {
        
    }
}
