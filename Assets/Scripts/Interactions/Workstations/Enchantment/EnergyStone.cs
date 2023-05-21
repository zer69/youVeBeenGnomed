using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interactions.Workstations.Enchantment
{
    class EnergyStone : MonoBehaviour
    {
        public enum Energy
        {
            None,
            S,
            M,
            L,

        }

        [SerializeField] public Energy energy = Energy.None;

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
