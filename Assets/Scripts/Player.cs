using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class Player
    {
        // head, torso, leftArm, rightArm, leftLeg, rightLeg;
        List<PlayerItem> wornItems = new List<PlayerItem>();

        //Object should be replaced with appropiate class or even beter - interface
        public Damage attackWithEverything(Object attackedObject)
        {
            Damage damage = new Damage();

            damage.damageNow += 1;
            /*
            float damage = 0;
            for(PlayerItem pi in wornItems) {
                if(pi.d)
                damage 
            }

            damage.damageNow +=

            //TODO: invokation should be repeated with maybe yield function 
            */

            return damage;
        }


        static bool deleteResourcesForBuilding(List<Tuple<long, int>> resourcesToSubtract)
        {
            return true;
        }

        static bool hasEnoughResources(List<Tuple<long, int>> resourcesNeeded)
        {
            return true;
        }
    }
}
