using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
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
        IPickable pickableObj;
        InputHandler inputHandler;
        PickableUI pickableUI;      //Pickable object's UI image
        PlayerManager playerManager;
        PlayerInventory playerInventory;
        PlayerAnimatorManager animatorManager;
        public event EventHandler KeyChange;
        public List<IInteractable> intList = new List<IInteractable>();

        [HideInInspector]
        public GameObject[] interactorObjs;

        [Header("Objects")]
        [SerializeField] private GameObject pickableUIObject;

        [Header("Variables")]
        public bool canInteract = true;


        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            animatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            playerManager = GetComponent<PlayerManager>();
            playerInventory = GetComponent<PlayerInventory>();
        }

        private void Start()
        {
            pickableUI = FindObjectOfType<PickableUI>();
            intList.Clear();
        }

        private void Update()
        {
            if (inputHandler.f_Key_Press && canInteract && !playerManager.inAnim)
            {
                HandlePressInteractable(true);
            }

            if (inputHandler.f_Key_Release && !canInteract && !playerManager.inAnim)
            {
                HandlePressInteractable(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleNearByObject(other, true);
        }

        private void OnTriggerStay(Collider other)
        {
            RequirementKeys key = other.GetComponent<RequirementKeys>();

            if (key != null)
            {
                GetKey(key.GetKeyType());
                Destroy(key.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            HandleNearByObject(other, false);
        }

        public void HandleNearByObject(Collider other, bool inArea)
        {
            #region IHold

            IHold holding = other.GetComponent<IHold>();

            if (holding != null)
            {
                if (ContainsKey(holding.reqType))
                {
                    if (inArea)
                    {
                        holding.Holding();
                    }
                    else
                    {
                        holding.AreaEmpty();
                    }
                }
            }

            #endregion

            #region IInteractable 

            IInteractable interactable = other.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (inArea && !intList.Contains(interactable))
                {
                    intList.Add(interactable);
                }

                if (!inArea)
                {
                    interactable.IsStillPressing(false);
                    animatorManager.animator.SetBool("isPressing", false);
                    intList.Remove(interactable);
                    canInteract = true;
                }
            }

            #endregion

            #region IPickable

            IPickable pickableObj = other.GetComponent<IPickable>();

            if (pickableObj != null)
            {
                if (inArea)
                {
                    string pickableText = pickableObj.myPickableText;
                    pickableUI.pickableText.text = "Pick Up (F-Key) " + pickableText;
                    pickableUI.pickableIcon.enabled = true;
                    pickableUI.pickableIcon.sprite = pickableObj.myPickableIcon;
                    //pickableObj.NearByObject();
                    pickableUIObject.SetActive(true);
                    //set UI text to pickable obj
                    //set text pop up to true
                }
                else
                {
                    pickableUIObject.SetActive(false);
                }
            }

            #endregion
        }

        private void HandlePressInteractable(bool isPressed)
        {
            if (isPressed)
            {
                canInteract = false;
            }
            else
            {
                canInteract = true;
            }

            if(intList.Count>0)
            {
                foreach (var a in intList)
                {
                    if (isPressed && ContainsKey(a.reqType))
                    {
                        a.Interact();
                        animatorManager.animator.SetBool("isPressing", true);
                        animatorManager.PlayTargetAnimation("PressEnter", false);
                    }
                    else if (!isPressed && ContainsKey(a.reqType))
                    {
                        a.IsStillPressing(false);
                        animatorManager.animator.SetBool("isPressing", false);
                    }
                }
            }

            #region This will be activated (Pickable OBJ)

            /*
            IPickable pickableObj = other.GetComponent<IPickable>();

            if (pickableObj != null)
            {
                if (isPressed)
                {                 
                    pickableObj.PickInteract(playerManager);
                    pickableUIObject.SetActive(false);

                    //item alınınca farklı bir efett çıksın diye corouine var buna kesin bak
                   // StartCoroutine("PickUpClose", pickUItimer);
                }
            }*/
            #endregion

        }

        #region Body Interactor

        public bool BodyInteractor(Bodyparts whichPart, LayerMask getMask, float interactionDist, out RaycastHit sendHit)
        {
            int partIndex = (int)whichPart;
            bool bodyInteract = Physics.Raycast(interactorObjs[partIndex].transform.position, transform.forward, out sendHit, interactionDist, getMask);
            Debug.DrawRay(interactorObjs[partIndex].transform.position, transform.forward, Color.magenta);
            return bodyInteract;
        }

        public bool BodyInteractor(Bodyparts whichPart, LayerMask getMask, float interactionDist)
        {
            int partIndex = (int)whichPart;
            bool bodyInteract = Physics.Raycast(interactorObjs[partIndex].transform.position, transform.forward, interactionDist, getMask);
            Debug.DrawRay(interactorObjs[partIndex].transform.position, transform.forward, Color.magenta);
            return bodyInteract;
        }

        #endregion

        #region Requirement Get / Delete / Contains
        public void GetKey(RequirementTypes.RequirementType reqType)
        {
            if (!ContainsKey(reqType))
            {
                playerInventory.requirementList.Add(reqType);
                KeyChange?.Invoke(this, EventArgs.Empty);
            }
        }
        public void DeleteKey(RequirementTypes.RequirementType reqType)
        {
            playerInventory.requirementList.Remove(reqType);
            KeyChange?.Invoke(this, EventArgs.Empty);
        }
        public bool ContainsKey(RequirementTypes.RequirementType reqType)
        {
            return playerInventory.requirementList.Contains(reqType);
        }
        #endregion

        #region This will be activated (Pickable OBJ)

        /*private void HandlePressInteractable(Collider other,bool isPressed)
        {
           // IInteractable interactable = other.GetComponent<IInteractable>();

            if (interactable != null && ContainsKey(interactable.reqType))
            {
                if (isPressed)
                {                   
                    canInteract = false;
                    interactable.Interact();
                    animatorManager.animator.SetBool("isPressing", true);
                    animatorManager.PlayTargetAnimation("PressEnter", false);
                }
                else
                {
                    canInteract = true;
                    interactable.StillPress(false);
                    animatorManager.animator.SetBool("isPressing", false);
                }
            }
            
            
            IPickable pickableObj = other.GetComponent<IPickable>();

            if (pickableObj != null)
            {
                if (isPressed)
                {                 
                    pickableObj.PickInteract(playerManager);
                    pickableUIObject.SetActive(false);

                    //item alınınca farklı bir efett çıksın diye corouine var buna kesin bak
                   // StartCoroutine("PickUpClose", pickUItimer);
                }
            }
        } */

        IEnumerator PickUpClose(float getTimer)
        {
            yield return new WaitForSeconds(getTimer);
            pickableUIObject.SetActive(false);
            yield break;
        }

        #endregion

    }
}