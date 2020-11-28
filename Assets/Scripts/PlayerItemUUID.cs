using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    class PlayerItemUUID
    {
        //WholeBody is for wearing a power armor
        public enum PlaceOfWearing {UnWearable, Hand, TwoHands, Leg, TwoLegs, Torso, Head, WholeBody};
        List<PlaceOfWearing> placesOfWearing;

        float attackDamage = 1.0f;
        float attackPerSecond = 1.5f;
        //List<Tuple<bool, bool>> spaceOccupied;
        bool[,] spaceOccupied = new bool[,] { 
            {true, true, true },
            { false, true, false }, 
            {true, true, true }
        };

        public bool[,] getSpaceOccupied(long uuid)
        {
            return spaceOccupied;
        }

        
    }
}
