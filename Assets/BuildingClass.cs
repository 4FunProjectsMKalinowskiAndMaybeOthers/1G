using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameClass;

public class ObjectUID
{
    long id;
    static long counter = 0;
    public static ObjectUID getNewUID()
    {
        ++counter;
        return new ObjectUID(counter);
    }

    private ObjectUID() { }
    private ObjectUID(long id) { this.id = id; }
}
    
public class BuildingId
{
    int id;
}

public class Building
{
    BuildingId id;
    ObjectUID uid = ObjectUID.getNewUID();
    List<GameClass.Input> inputs;
    List<Output> outputs;
}
