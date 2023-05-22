using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    class UIButtonSound : MonoBehaviour
    {
        [Header("Sound Events")]
        public AK.Wwise.Event ButtonSoundEvent;

        public void onClick()
        {
            ButtonSoundEvent.Post(gameObject);
        }
    }
}
