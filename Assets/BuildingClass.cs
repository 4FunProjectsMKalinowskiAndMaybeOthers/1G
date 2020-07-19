using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameClass;
using System;

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
    
    public BuildingId(int id)
    {
        this.id = id;
    }

    public static bool operator ==(BuildingId obj, BuildingId obj2)
    {
        return obj.id.Equals(obj2.id);
    }

    public static bool operator !=(BuildingId obj, BuildingId obj2)
    {
        return ! obj.id.Equals(obj2.id);
    }


    public override bool Equals(object obj)
    {
        return obj is BuildingId ? id.Equals((obj as BuildingId).id) : id.Equals(obj);
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
    }

    public static implicit operator int(BuildingId d) => d.id;
    public static implicit operator BuildingId(int d) => new BuildingId(d);

}

public class Recipt
{
    System.Tuple<MaterialId, int> result;
    public Dictionary<MaterialId, int> ingredients;

    public MaterialId getResultId() { return result.Item1; }
    public int getResultCount() { return result.Item2; }

    public Recipt(MaterialId materialResultId, int count, Dictionary<MaterialId, int> ingredients)
    {
        this.result = new Tuple<MaterialId, int>(materialResultId, count);
        this.ingredients = ingredients;
    }
}

public enum TimeBase
{
    ms =  1,
    s =   1000,
    min = 600000
}

public class Time
{
    double ms;
    public Time(double time, TimeBase timeBase = TimeBase.ms)
    {
        ms = time * (((double)(timeBase)));
    }
    public double Get(TimeBase timeBase = TimeBase.ms)
    {
        return ms / (((double)(timeBase)));
    }

    public static implicit operator double(Time d) => d.ms;
    public static implicit operator Time(double d) => new Time(d);

}

public class ProductionTime
{
    Time timeNeededMs;
    Time timeSpent = 0;
    bool started = false;

    public ProductionTime(Time timeNeededMs)
    {
        this.timeNeededMs = timeNeededMs;
    }

    public bool AddTimeAndCheck(Time ms)
    {
        if (started != true)
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

public class ProductionClip
{
    public Output output;
    public Recipt recipt;
    public ProductionTime time;

    public ProductionClip(Output output, Recipt recipt, ProductionTime time)
    {
        this.output = output;
        this.recipt = recipt;
        this.time = time;
    }
}

public class Building
{
    BuildingId id;
    ObjectUID uid = ObjectUID.getNewUID();
    List<GameClass.Input> inputs;
    List<ProductionClip> clippers;

    public Building() { }

    public Building(BuildingId id, List<GameClass.Input> inputs, List<ProductionClip> clippers)
    {
        this.id = id;
        this.inputs = inputs;
        this.clippers = clippers;
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
                if(clip.recipt.ingredients != null && clip.recipt.ingredients.Count > 0) { 
                    //fill prodictionCheck
                    foreach (MaterialId material in clip.recipt.ingredients.Keys)
                    {
                        productionCheck[material] = clip.recipt.ingredients[material];
                    }

                    if (inputs != null)
                    {
                        //check avaiable materials in inputs
                        foreach (GameClass.Input input in inputs)
                        {
                            MaterialHolder mh = input.getHoldInfo();
                            if (productionCheck.ContainsKey(mh.getMaterialId()))
                            {
                                productionCheck[mh.getMaterialId()] -= mh.getCount();
                                if (mh.getCount() > 0)
                                {
                                    if ( ! productionInputs.ContainsKey(mh.getMaterialId()))
                                    {
                                        productionInputs[mh.getMaterialId()] = new List<GameClass.Input>();
                                    }
                                    productionInputs[mh.getMaterialId()].Add(input);
                                }
                            }
                        }
                    }

                    //check if evrything materials is avaiable
                    foreach (int count in productionCheck.Values)
                    {
                        if (count > 0)
                        {
                            startProduction = false;
                            break;
                        }
                    }
                }

                if (startProduction)
                {
                    if (clip.recipt.ingredients != null && clip.recipt.ingredients.Count > 0)
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
                                if (inputsCount != 0)
                                {
                                    takeNo = need / inputsCount;
                                }
                            }
                        }
                    }

                    clip.time.startProduction();
                }

            }

            if (clip.time.isStarted() == true) //intentional to start production just after the taking materials or not? to discuss
            {
                if (clip.time.AddTimeAndCheck(addMs) == true)
                {
                    clip.output.addMaterial(clip.recipt.getResultId(), clip.recipt.getResultCount());
                }
            }
        }
    }

    public override string ToString()
    {
        String desc = String.Empty;
        desc += String.Format("Building id: {0}, UID: {1}{2}", id, uid, Environment.NewLine);
        desc += String.Format("Inputs state: {0}", Environment.NewLine);
        int counter = 0;

        if (inputs != null)
        {
            foreach (GameClass.Input buildInput in inputs)
            {
                desc += String.Format("{2}{0} {1}---{1}", buildInput.ToString(), Environment.NewLine, counter++);
            }
        }
        counter = 0;
        desc += String.Format("Outputs state: {0}", Environment.NewLine);
        foreach (ProductionClip pc in clippers)
        {
            if (pc.output != null)
            {
                desc += String.Format("{2}{0} {1}---{1}", pc.output.ToString(), Environment.NewLine, counter++);
            }
        }
        return desc;
    }
}
