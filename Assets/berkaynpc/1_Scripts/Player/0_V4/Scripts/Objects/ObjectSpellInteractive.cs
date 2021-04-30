using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace project_WAST
{
    public class ObjectSpellInteractive : MonoBehaviour, ISpellInteractive,IHaveStatus,IHaveButton
    {
        private Color startColor;
        private Renderer objMats;
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("My Status (True/False)")]
        [SerializeField] private bool blockStatus = true;
        public bool myStatus => blockStatus;

        [Header("Requirement Element Type")] 
        [SerializeField] private RequirementTypes.SpellElementTypes myElementType;

        #region Block Types
        [Header("Block Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] BlockTypes blockType;
        public enum BlockTypes                                //BUTTON TYPES
        {
           SimpleInteract,
           OnOffBlock,
           //withtimer vs vsvsvs
        }
        public BlockTypes GetBlockType() => blockType;
        #endregion

        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [Header("For Interact Anim")]
        public GameObject interactMarkObj;


        [Header("Connected And Controller Objects")]
        [SerializeField] private GameObject[] connectedGameObjs;  // ALL CONNECTED OBJECTS TO THIS BUTTON
        [SerializeField] private GameObject[] controllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;           // CHECK ALL Controller BUTTONS

        private void Start()
        {        

            objMats = GetComponent<Renderer>();
            if(objMats!=null)
            {
                startColor = objMats.material.GetColor("_EmissionColor");
            }
        }

        public void SpellInteract(RequirementTypes.SpellElementTypes getSpellElement)
        {
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);

            if (controllerStatus)
            {
                if (ContainsElement(getSpellElement))
                {
                    switch(blockType)
                    {
                        case BlockTypes.SimpleInteract:
                            blockStatus=true;
                            if (objMats != null)
                            {
                                objMats.material.SetColor("_EmissionColor", startColor * 5);
                            }
                            InteractedWithMe(blockStatus);
                            break;

                        case BlockTypes.OnOffBlock:
                            blockStatus = !blockStatus;

                            if (objMats != null)
                            {
                                if (blockStatus)
                                {

                                    objMats.material.SetColor("_EmissionColor", startColor * 5);
                                }
                                else
                                {
                                    objMats.material.SetColor("_EmissionColor", startColor * 0.5f);
                                }
                            }
                           
                       
                            InteractedWithMe(blockStatus);

                            break;
                    }                  
                }
            }
            else
            {
                blockStatus = false;
                if (objMats != null)
                {
                    objMats.material.SetColor("_EmissionColor", startColor * 0.5f);
                }
                InteractedWithMe(blockStatus);
            }
        }

        public void PressedButton(bool isButtonOn) //controller objects send this
        {
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);
            if (!controllerStatus)
            {
                controllerStatus = false;
                blockStatus = false;
                InteractedWithMe(false);
            }
        }

        private void InteractedWithMe(bool getBool)
        {
            myFunctions.SetMyConnectedObjects(connectedGameObjs, getBool);
        }

        public bool ContainsElement(RequirementTypes.SpellElementTypes getSpellElement)
        {
            return myElementType == getSpellElement;
        }

        public void PlayerNearBy(bool isNear)
        {
            interactMarkObj.SetActive(isNear);
        }

        public void PlayerCanInteract(bool canInteract)
        {
            ObjectInteractMark mark = interactMarkObj.GetComponent<ObjectInteractMark>();

            if(mark!=null)
            {
                mark.ShowMe(canInteract);
            }
            //set anim
        }
    }
}