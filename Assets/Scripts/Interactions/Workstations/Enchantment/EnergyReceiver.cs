using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Interactions.Workstations.Enchantment
{
    class EnergyReceiver : MonoBehaviour, IInteractable
    {
        [BackgroundColor(1.5f, 0f, 0f, 1f)]
        [Header("Transform")]
        [SerializeField] private Transform energyStonePosition;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Transform rightHandPosition;
        private int pickableLayer = 10;
        private int inHandsLayer = 15;
        public GameObject battery;
        public int enchantmentQuality;
        public bool hasBattery;

        [Header("Events")]
        [BackgroundColor(.75f, 0f, 1.5f, 1f)]
        [SerializeField] private go_GameEvent pickObject;
        private Inventory inventory;

        private float useDelay = 5f;
        public string InteractionPrompt => throw new NotImplementedException();

        public Vector3 targetAngle = new Vector3(0f, 360f, 0f);
        private Vector3 currentAngle;

        private int upAndDownTargetIter = 200;
        private int upAndDownIter = 0;
        private bool isUp = true;
        void Start()
        {
            playerInput.onActionTriggered += OnPlayerInputActionTriggered;
            hasBattery = false;

            

        }

        void Update()
        {
            if (hasBattery) { 
            
                currentAngle = new Vector3(0, Mathf.LerpAngle(currentAngle.y, currentAngle.y + 10f, Time.deltaTime), 0);

                battery.transform.eulerAngles = currentAngle;
                if (currentAngle == targetAngle)
                {
                    currentAngle = new Vector3(0f, 0f, 0f);
                }

                if (isUp)
                {
                    battery.transform.position = Vector3.Lerp(battery.transform.position, new Vector3(battery.transform.position.x, battery.transform.position.y + 0.03f, battery.transform.position.z), Time.deltaTime);
                    upAndDownIter++;
                    if(upAndDownIter == upAndDownTargetIter)
                    {
                        isUp = false;
                        
                    }
                }
                else
                {
                    battery.transform.position = Vector3.Lerp(battery.transform.position, new Vector3(battery.transform.position.x, battery.transform.position.y - 0.03f, battery.transform.position.z), Time.deltaTime);
                    upAndDownIter--;
                    if (upAndDownIter == 0)
                    {
                        isUp = true;
                    }
                }
                
            }
        }


        public bool Interact(Interactor interactor)
        {
            inventory = playerTransform.GetComponentInParent<Inventory>();
            if (inventory.hasBattery)
            {              

                battery = inventory.battery;
                SwitchToDefaultLayer(battery.transform);

                inventory.hasBattery = false;
                inventory.battery = null;
                inventory.rightHandFree = true;
                battery.transform.position = energyStonePosition.position;
                battery.transform.rotation = energyStonePosition.rotation;
                battery.transform.SetParent(energyStonePosition);

                battery.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                battery.GetComponent<Collider>().enabled = true;
                enchantmentQuality = (int) battery.GetComponent<EnergyStone>().energy;                

                hasBattery = true;

                currentAngle = battery.transform.eulerAngles;

                return true;
            }

            //hasBattery = false;
            return false;

        }

        public void removeBattery()
        {

        }

        public void destroyBattery()
        {
            Debug.Log("destroyBattery");
            Debug.Log(battery);
            Destroy(battery);
            hasBattery = false;
        }

        private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
        {
            if (hasBattery)
            {
                switch (context.action.name)
                {
                    case "Build":
                        Debug.Log("take back");
                        pickObject.Raise(battery);
                        SwitchToInHandsLayer(battery.transform);
                        inventory.battery.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        battery = null;
                        hasBattery = false;
                        
                        break;

                }
            }

        }

        void SwitchToInHandsLayer(Transform objectInHand)
        {
            objectInHand.gameObject.layer = inHandsLayer;
            foreach (Transform child in objectInHand)
            {
                child.gameObject.layer = inHandsLayer;
                foreach (Transform grandchild in child.transform)
                {
                    grandchild.gameObject.layer = inHandsLayer;
                }
            }
        }

        void SwitchToDefaultLayer(Transform objectInHand)
        {
            objectInHand.gameObject.layer = 0;
            foreach (Transform child in objectInHand)
            {
                child.gameObject.layer = 0;
                foreach (Transform grandchild in child.transform)
                {
                    grandchild.gameObject.layer = 0;
                }
            }
        }

    }
}
