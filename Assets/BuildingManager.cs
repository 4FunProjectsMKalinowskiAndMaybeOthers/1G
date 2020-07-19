using GameClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuildingManager
{
    static Dictionary<BuildingId, Building> BuildingTemplates = new Dictionary<BuildingId, Building>();
    public static void LoadBuildings()
    {
        BuildingId buildingId = 1;
        MaterialId materialId = 1;
        MaterialId ingredientId = 2;
        Time time = new Time(1, TimeBase.s);


        Dictionary<MaterialId, int> ingredients = new Dictionary<MaterialId, int>();
        ingredients.Add(ingredientId, 2);

        List<MaterialId> materialsFilter = new List<MaterialId>();
        materialsFilter.Add(materialId);

        MaterialHolder mh = new MaterialHolder(materialId, 0);

        ProductionClip pc = new ProductionClip(new GameClass.Output(0, 0, null, GameClass.direction.right, materialsFilter, mh, putState.notConnected),
                                            new Recipt(materialId, 1, ingredients),
                                            new ProductionTime(time));

        List<ProductionClip> lpc = new List<ProductionClip>();
        lpc.Add(pc);

        List<GameClass.Input> inputs = new List<GameClass.Input>();

        List<MaterialId> materialsFilterIn = new List<MaterialId>();
        materialsFilter.Add(ingredientId);

        MaterialHolder mhin = new MaterialHolder(ingredientId, 2);

        inputs.Add(new GameClass.Input(0, 0, null, direction.left, materialsFilterIn, mhin, putState.notConnected, 1));

        BuildingTemplates[buildingId] = new Building(buildingId, inputs, lpc);
    }

    public static Building GetBuilding(BuildingId id)
    {
        try
        {
            return BuildingTemplates[id];
        } catch
        {
            return null;
        }
    }
}
