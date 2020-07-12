using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceHierarchy : MonoBehaviour
{
    public enum putDirection {
        input,
        output
    }

    public enum direction
    {
        up,
        right,
        down,
        left
    }

    public enum putState
    {
        notConnected,
        connected
    }

    public class MaterialId
    {
        public int id;
    }

    public class MaterialHolder
    {
        MaterialId materialId;
        int count;

        public MaterialId getMaterialId()
        {
            return materialId;
        }
        public int getCount()
        {
            return count;
        }

        public MaterialHolder() { }
        public MaterialHolder(MaterialId materialId, int count)
        {
            this.materialId = materialId;
            this.count = count;
        }

        public int Substract(int grabCount)
        {
            count -= grabCount;

            return count > -1 ? grabCount : grabCount + count;
        }

        internal void Add(int neighbourPresentCount)
        {
            count += neighbourPresentCount;
        }
    }

    public abstract class MainPut
    {
        protected uint ref_x;
        protected uint ref_y;
        protected Output neighbor;
        protected putDirection put;
        protected direction faceDirection;
        protected List<MaterialId> materialsFilter;
        protected MaterialHolder materialHolded;
        protected putState state;

        public int takeMaterial(MaterialHolder holded, int grabCount)
        {
            if(holded == null || holded.getMaterialId() == materialHolded.getMaterialId())
            {
                int maxPossible = materialHolded.Substract(grabCount);
                if(maxPossible != grabCount)
                {
                    materialHolded = null;
                }
                return maxPossible;
            }
            return 0;
        }
    }

    public class Input : MainPut
    {
        protected int grabCount;
        public Input()
        {
            put = putDirection.input;
            grabCount = 1;
        }

        public bool getFromNeighbour()
        {
            if(neighbor != null)
            {
                int neighbourPresentCount = neighbor.takeMaterial(materialHolded, grabCount);
                materialHolded.Add(neighbourPresentCount);
                return true;
            }
            return false;
        }
    }

    public class Output : MainPut
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
