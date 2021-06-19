using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace project_usomk
{
    public class ObjectSpellInteractive : ControllerBase, ISpellInteractive
    {
        private Color startColor;
        private Renderer objMats;
        
        #region Block Types
        [Header("Block Type")]                                  
        [SerializeField] BlockTypes blockType;
        public enum BlockTypes 
        {
           SimpleInteract,
           OnOffBlock,
           //withtimer vs vsvsvs
        }
        public BlockTypes GetBlockType() => blockType;
        #endregion
        #region Requirement Types
        [Header("Requirement Element Type")]
        [SerializeField] private RequirementTypes.SpellElementTypes myElementType;
        #endregion

        [Header("For Interact Anim")]
        public GameObject interactMarkObj;

        protected override void Start()
        {
            base.Start();
            objMats = GetComponent<Renderer>();
            if(objMats!=null)
            {
                startColor = objMats.material.GetColor("_EmissionColor");
            }
        }

        /// <summary>
        /// This method calls when objects are hit by spells using interface.
        /// </summary>
        public void SpellInteract(RequirementTypes.SpellElementTypes getSpellElement)
        {
            myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);

            if (myControllerStatus)
            {
                if (ContainsElement(getSpellElement))       //If the element type of the spell matches that of the object.
                {
                    switch(blockType)
                    {
                        case BlockTypes.SimpleInteract:
                            myCurrentStatus=true;
                            if (objMats != null)
                            {
                                objMats.material.SetColor("_EmissionColor", startColor * 5);        //Vfx visualization
                            }
                            InteractedWithMe(myCurrentStatus);
                            break;

                        case BlockTypes.OnOffBlock:
                            myCurrentStatus = !myCurrentStatus;

                            if (objMats != null)
                            {
                                if (myCurrentStatus)
                                {
                                    objMats.material.SetColor("_EmissionColor", startColor * 5);        //Vfx visualization
                                }
                                else
                                {
                                    objMats.material.SetColor("_EmissionColor", startColor * 0.5f);      //Vfx visualization
                                }
                            }                                                 
                            InteractedWithMe(myCurrentStatus);
                            break;
                    }                  
                }
            }
            else
            {
                myCurrentStatus = false;
                if (objMats != null)
                {
                    objMats.material.SetColor("_EmissionColor", startColor * 0.5f);        //Vfx visualization
                }
                InteractedWithMe(myCurrentStatus);
            }
        }

        public override void OnSettingMe(string getID, bool getbool)
        {
            if(getID==this.myID)
            {
                myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);
                if (!myControllerStatus)
                {
                    myControllerStatus = false;
                    myCurrentStatus = false;
                    InteractedWithMe(false);
                }
            }         
        }

        private void InteractedWithMe(bool getBool)
        {
           MyGameEvents.current.SetTarget(targetID, getBool);       //Set target objects
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
            ObjectInteractMark mark = interactMarkObj.GetComponent<ObjectInteractMark>();       //Spell interaction mark

            if(mark!=null)
            {
                mark.ShowMe(canInteract);       //Show mark
            }
        }
    }
}