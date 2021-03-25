using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace project_WAST
{
    [HideInInspector]
    public enum Bodyparts
    {
        headLeft,
        headCenter,
        headRight,
        hipsLeft,
        hipsCenter,
        hipsRight,
        legLeft,
        legCenter,
        legRight,
        footLeft,
        footCenter,
        footRight,
    }

    public class PlayerInteractor : MonoBehaviour
    {
        PlayerManager playerManager;
        PlayerLocomotion playerLocomotion;
        CharacterController cController;
        CameraDetectWalls cameraDetect;
        LayerMask cameraLayerMask;
        InputHandler inputHandler;
        [HideInInspector]
        public GameObject[] interactorObjs;

        public event EventHandler KeyChange;
        public bool canInteract=true;
        public bool isGrab = false;
        private IInteractable interactable;
        private IHold holding;
        [SerializeField]  private IHaveStatus haveStatus;
        [SerializeField] private GameObject cameraHandler;
        [SerializeField] private List<RequirementTypes.RequirementType> requirementList;
        public GameObject pushableObj;
        public Vector3 pushableNormal;

        public List<RequirementTypes.RequirementType> GetKeyList()
        {
            return requirementList;
        }
        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            inputHandler = GetComponent<InputHandler>();
            cController = GetComponent<CharacterController>();
            cameraDetect = cameraHandler.GetComponent<CameraDetectWalls>();

            requirementList = new List<RequirementTypes.RequirementType>();
            requirementList.Add(RequirementTypes.RequirementType.nothing); //karakterin bütün butonlarla etkileþimi iiçin bla bla bu yazýyý düzelt 

        }
        private void Start()
        {
            canInteract = true;
        }
        private void Update()
        {
            if (inputHandler.intRelease)
            {
                canInteract = true;
            }
        }
        public void HandleFixedInteractors()
        {  
            //HandleWallRunCheck(); //askida
        }
        public void GetKey(RequirementTypes.RequirementType reqType)
        {
            if(!ContainsKey(reqType))
            {
                requirementList.Add(reqType);
                KeyChange?.Invoke(this, EventArgs.Empty);
            }           
        }
        public void DeletKey(RequirementTypes.RequirementType reqType)
        {
            requirementList.Remove(reqType);
            KeyChange?.Invoke(this, EventArgs.Empty);
        }
        public bool ContainsKey(RequirementTypes.RequirementType reqType)
        {
            return requirementList.Contains(reqType);
        }        
        private void OnTriggerEnter(Collider other)
        {
            holding = other.GetComponent<IHold>(); //sistemi zorlayabilir kontrol et

            if (holding != null)
            {
                if (ContainsKey(holding.reqType))
                {
                    holding.Holding();
                }
            }
        }              
        private void OnTriggerStay(Collider other)
        {
            RequirementTypes key = other.GetComponent<RequirementTypes>(); //req olarak deðiþtir

            if (key != null)
            {
                GetKey(key.GetKeyType());
                Destroy(key.gameObject);
            }

            if (inputHandler.interactFlag && canInteract)
            {
                interactable = other.GetComponent<IInteractable>();   

                if (interactable != null && ContainsKey(interactable.reqType))
                {
                    Debug.Log(interactable);
                    interactable.Interact();
                    canInteract = false;
                }
            }

            if (inputHandler.intRelease)
            {
                canInteract = true;
                if (interactable != null)
                {
                    if (ContainsKey(interactable.reqType))
                    {                        
                        interactable.StillPress(false);
                       // interactable = null;
                    }
                }       
            }
        }
        private void OnTriggerExit(Collider other)
        {
            holding = other.GetComponent<IHold>();
            if (holding != null)
            {
                if (ContainsKey(holding.reqType))
                {
                    holding.AreaEmpty();
                }
            }
       
            if (interactable != null) //buna daha güzel çözüm bulunabilirse silinmeli
            {               
                haveStatus = other.GetComponent<IHaveStatus>();

                if (ContainsKey(interactable.reqType) && haveStatus !=null && haveStatus.myStatus)
                {                   
                    interactable.StillPress(false);
                }
            }
        }
        void HandleWallRunCheck()
        {
           // playerLocomotion.isWallRight = Physics.Raycast(transform.position, transform.right, 1f, playerLocomotion.walkableWallMask);
           // playerLocomotion.isWallLeft = Physics.Raycast(transform.position, -transform.right, 1f, playerLocomotion.walkableWallMask);
        } //askida

        public bool bodyInteractor(Bodyparts whichPart, LayerMask getMask, float interactionDist, out RaycastHit sendHit)
        {
            int partIndex = (int)whichPart;
            bool bodyInteract = Physics.Raycast(interactorObjs[partIndex].transform.position, transform.forward, out sendHit, interactionDist, getMask);
            Debug.DrawRay(interactorObjs[partIndex].transform.position, transform.forward, Color.magenta);
            return bodyInteract;
        }

        public bool bodyInteractor(Bodyparts whichPart, LayerMask getMask, float interactionDist)
        {
            int partIndex = (int)whichPart;
            bool bodyInteract = Physics.Raycast(interactorObjs[partIndex].transform.position, transform.forward, interactionDist, getMask);
            Debug.DrawRay(interactorObjs[partIndex].transform.position, transform.forward, Color.magenta);
            return bodyInteract;
        }

    }
}