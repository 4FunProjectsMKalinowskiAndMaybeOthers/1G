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
            {true, true, false },
            { false, true, true },
            {true, true, true }
        };

        //Need to assign null if building's occupied space has changed
        //(y,x) Upper, Down, Left, Right     -Like WSAD
        static Tuple<int, int>[] outermostFields = null;
        static bool? emptySpace = null;


        public static bool[,] getSpaceOccupied(long uuid)
        {
            return spaceOccupied;
        }

        //HERETODO: This method
        //Using singleton pattern?
        /// <summary>
        /// Returns Tuple&lt;Y, X&gt;[4] where 0-UP 1-DOWN 2-LEFT 3-RIGHT 
        /// </summary>
        public static Tuple<int, int>[] getOutermostFields(long uuid)
        {
            //HERETODO: Check if building is not empty

            //Sides
            if (emptySpace == null && outermostFields == null)
            {
                //Upper, Down, Left, Right     -Like WSAD
                emptySpace = true;
                int[] xOF, yOF;
                yOF = new int[4];
                xOF = new int[4];
                yOF[0] = spaceOccupied.GetLength(0);
                xOF[0] = spaceOccupied.GetLength(1);
                yOF[1] = 0;
                xOF[1] = 0;
                yOF[2] = spaceOccupied.GetLength(0);
                xOF[2] = spaceOccupied.GetLength(1);
                yOF[3] = 0;
                xOF[3] = 0;

                for (int y = 0; y < spaceOccupied.GetLength(0); y++)
                {
                    for (int x = 0; x < spaceOccupied.GetLength(1); x++)
                    {
                        if (spaceOccupied[y, x] == true)
                        {
                            emptySpace = false;
                            if (yOF[0] > y)
                            {
                                yOF[0] = y;
                                xOF[0] = x;
                            }
                            if (yOF[1] < y)
                            {
                                yOF[1] = y;
                                xOF[1] = x;
                            }
                            if (xOF[2] > x)
                            {
                                yOF[2] = y;
                                xOF[2] = x;
                            }
                            if (xOF[3] < x)
                            {
                                yOF[3] = y;
                                xOF[3] = x;
                            }
                        }
                    }
                }
                if (emptySpace == false)
                {
                    outermostFields = new Tuple<int, int>[]{
                             new Tuple<int, int>(yOF[0], xOF[0]), new Tuple<int, int>(yOF[1], xOF[1]),
                             new Tuple<int, int>(yOF[2], xOF[2]), new Tuple<int, int>(yOF[3], xOF[3])
                    };
                }
            }

            return outermostFields;
        }
    }
}
