using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClass
{

    public class Optional<T>
    {
        T val;
        bool seted = false;

        public Optional()
        {
            seted = false;
        }

        public Optional(T val)
        {
            this.val = val;
            seted = true;
        }

        public T ValueOr(T orValue)
        {
            if (seted)
            {
                return val;
            } else
            {
                return orValue;
            }

        }

    }

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

        public MaterialId() { }

        public MaterialId(int id)
        {
            this.id = id;
        }

        public override string ToString()
        {
            return String.Format("{0}", id);
        }

        public override bool Equals(object obj)
        {
            return obj is MaterialId ? id.Equals((obj as MaterialId).id) : id.Equals(obj);
        }

        public static bool operator ==(MaterialId obj, MaterialId obj2)
        {
            return obj.id.Equals(obj2.id);
        }

        public static bool operator !=(MaterialId obj, MaterialId obj2)
        {
            return !obj.id.Equals(obj2.id);
        }


        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public static implicit operator int(MaterialId d) => d.id;
        public static implicit operator MaterialId(int d) => new MaterialId(d);
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

    public class comparePut : IComparer<MainPut>
    {
        public int Compare(MainPut x, MainPut y)
        {
            if (x.getHoldInfo() == null && y.getHoldInfo() == null)
            {
                return 0;
            }
            else if (x.getHoldInfo() == null && y.getHoldInfo() != null)
            {
                return -1;
            }
            else if (x.getHoldInfo() != null && y.getHoldInfo() == null)
            {
                return 1;
            }
            else
            {
                if( x.getHoldInfo().getCount() > y.getHoldInfo().getCount())
                {
                    return 1;
                } else if (x.getHoldInfo().getCount() == y.getHoldInfo().getCount())
                {
                    return 0;
                } else
                {
                    return -1;
                }
            }
        }
    }

    public abstract class MainPut
    {
        protected int ref_x;
        protected int ref_y;
        protected Output neighbor;
        protected putDirection put;
        protected direction faceDirection;
        protected List<MaterialId> materialsFilter;
        protected MaterialHolder materialHolded;
        protected putState state;

        public MaterialHolder getHoldInfo()
        {
            return materialHolded;
        }

        public MaterialHolder takeMaterial(MaterialHolder holded, int grabCount)
        {
            if(materialHolded != null && (holded == null || holded.getMaterialId() == materialHolded.getMaterialId()))
            {
                int maxPossible = materialHolded.Substract(grabCount);
                MaterialId mid = materialHolded.getMaterialId();
                if (maxPossible != grabCount)
                {
                    materialHolded = null;
                }
                return new MaterialHolder(mid, maxPossible);
            }
            return null;
        }

        public bool addMaterial(MaterialId id, int count)
        {
            if(materialHolded == null)
            {
                materialHolded = new MaterialHolder(id, count);
            } else if (materialHolded.getMaterialId() == id)
            {
                materialHolded.Add(count);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            String desc = String.Empty;
            if (materialHolded != null)
            {
                desc += String.Format("EQ: {0} {1} {2}", materialHolded.getMaterialId(), materialHolded.getCount(), Environment.NewLine);
            } else
            {
                desc += String.Format("EQ: Nothing {0}", Environment.NewLine);
            }

            return desc;
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

        public Input(int ref_x, int ref_y, Output neighbor, direction faceDirection, List<MaterialId> materialsFilter, MaterialHolder materialHolded, putState state, int grabCount)
        {
            this.ref_x = ref_x;
            this.ref_y = ref_y;
            this.neighbor = neighbor;
            put = putDirection.input;
            this.faceDirection = faceDirection;
            this.materialsFilter = materialsFilter;
            this.materialHolded = materialHolded;
            this.state = state;
            this.grabCount = grabCount;
        }

        public bool getFromNeighbour()
        {
            if(neighbor != null)
            {
                MaterialHolder neighbourPresent = neighbor.takeMaterial(materialHolded, grabCount);
                addMaterial(neighbourPresent.getMaterialId(), neighbourPresent.getCount());
                return true;
            }
            return false;
        }
    }

    public class Output : MainPut
    {
        public Output() { }
        public Output(int ref_x, int ref_y, Output neighbor, direction faceDirection, List<MaterialId> materialsFilter, MaterialHolder materialHolded, putState state)
        {
            this.ref_x = ref_x;
            this.ref_y = ref_y;
            this.neighbor = neighbor;
            put = putDirection.output;
            this.faceDirection = faceDirection;
            this.materialsFilter = materialsFilter;
            this.materialHolded = materialHolded;
            this.state = state;
        }
    }


}