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

public class Recipt
{
    System.Tuple<MaterialId, int> result;
    public Dictionary<MaterialId, int> ingredients;

    public MaterialId getResultId() { return result.Item1; }
    public int getResultCount() { return result.Item2; }
}

public enum TimeBase
{
    ms = 0,
    s = 1,
    min = 60
}

public class Time
{
    double ms;
    public Time(double time, TimeBase timeBase = TimeBase.ms)
    {
        ms = time * (((double)(timeBase)) * 1000.0);
    }
    public double Get(TimeBase timeBase = TimeBase.ms)
    {
        return ms / (((double)(timeBase)) *1000.0);
    }

    public static implicit operator double(Time d) => d.ms;
    public static implicit operator Time(double d) => new Time(d);

}

public class Building
{
    BuildingId id;
    ObjectUID uid = ObjectUID.getNewUID();
    List<GameClass.Input> inputs;

    class ProductionClip
    {
        public Output output;
        public Recipt recipt;
        public ProductionTime time;
    }

    List<ProductionClip> clippers;

    class ProductionTime
    {
        Time timeNeededMs;
        Time timeSpent = 0;
        bool started = false;

        public bool AddTimeAndCheck(Time ms)
        {
            if(started != true)
            {
                return false;
            }
            timeSpent += ms;
            if (timeSpent >= timeNeededMs)
            {
                timeSpent = 0;
                started = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isStarted()
        {
            return started;
        }
        
        public void startProduction()
        {
            timeSpent = 0;
            started = true;
        }
    }

    public void doProduction(int addMs)
    {
        foreach(ProductionClip clip in clippers)
        {
            if (clip.time.isStarted() == false)
            {
                bool startProduction = true;

                Dictionary<MaterialId, int> productionCheck = new Dictionary<MaterialId, int>();
                Dictionary<MaterialId, List<GameClass.Input>> productionInputs = new Dictionary<MaterialId, List<GameClass.Input>>();

                //fill prodictionCheck
                foreach ( MaterialId material in clip.recipt.ingredients.Keys)
                {
                    productionCheck[material] = clip.recipt.ingredients[material];
                }
                
                //check avaiable materials in inputs
                foreach (GameClass.Input input in inputs)
                {
                    MaterialHolder mh = input.getHoldInfo();
                    if (productionCheck.ContainsKey(mh.getMaterialId()))
                    {
                        productionCheck[mh.getMaterialId()] -= mh.getCount();
                        if (mh.getCount() > 0)
                        {
                            productionInputs[mh.getMaterialId()].Add(input);
                        }
                    }
                }

                //check if evrything materials is avaiable
                foreach (int count in productionCheck.Values)
                {
                    if(count > 0)
                    {
                        startProduction = false;
                        break;
                    }
                }

                if (startProduction)
                {
                    foreach (MaterialId material in clip.recipt.ingredients.Keys)
                    {
                        int need = clip.recipt.ingredients[material];
                        int inputsCount = productionInputs[material].Count;
                        int takeNo = need / inputsCount;

                        productionInputs[material].Sort(new comparePut());
                        //if there would be a problem with a not enough taken items its propably here
                        foreach (GameClass.Input input in productionInputs[material])
                        {
                            MaterialHolder tookMH = input.takeMaterial(new MaterialHolder(material, 0), takeNo);
                            need -= tookMH.getCount();

                            --inputsCount; //yes, here
                            takeNo = need / inputsCount;
                        }
                    }
                    clip.time.startProduction();
                }

            }
            else
            {
                if (clip.time.AddTimeAndCheck(addMs) == true)
                {
                    clip.output.addMaterial(clip.recipt.getResultId(), clip.recipt.getResultCount());
                }
            }
        }
    }

}
