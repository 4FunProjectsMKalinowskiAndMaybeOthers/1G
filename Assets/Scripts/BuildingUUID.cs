using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    class BuildingUUID
    {
        //List<Tuple<bool, bool>> spaceOccupied;
        static bool[,] spaceOccupied = new bool[,] { 
            {true, true, true },
            { false, true, false }, 
            {true, true, true }
        };

        

        public static bool[,] getSpaceOccupied(long uuid)
        {
            
            return spaceOccupied;
        }

        //TODO: This method
        public static Tuple<int, int> getMostTopLeftCorner(long uuid) {
            return new Tuple<int, int>(0, 0);
        }
    }
}
