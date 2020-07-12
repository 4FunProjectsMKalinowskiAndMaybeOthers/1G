using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceFillingTestMain : MonoBehaviour
{
    Map m;

    // Start is called before the first frame update
    void Start()
    {
        m = new Map();
        m.Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
