using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class Damage
    {
        //DamageAll is auto incremented when player gets even single damage in DamageNow setter
        public enum DamageTypePrimary { Acid, Beam, Electric, Energy, Explosion, Fire, Ice, Laser, Magical, Physical, Spirit, Water};
        //public enum DamageTypeSecondary { };
        DamageTypePrimary damageTypePrimary; //Each damage type should have it's own damage value

        public float damageNow = 0;
        public float DamageNow {
            get { return damageNow; }
            set { damageNow = value;
                damageAll += value;
            }
        }

        float damageAll = 0;
        public float DamageAll {
            get { return damageAll; }
            set { damageAll += value; }
        }

        public Damage() {
        }

    }
}
