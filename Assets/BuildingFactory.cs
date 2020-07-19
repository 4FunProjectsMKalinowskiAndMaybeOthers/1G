using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuildingFactory
{

    public static Building Build(BuildingId id)
    {
        return BuildingManager.GetBuilding(id);
    }

}
