using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class Platforms : ObjectBase
    {
        CharacterController characterController;
        private Vector3 prevPos;
        private Vector3 transformVelocity;
        private Vector3 transformRotVelo;
        private bool isPlayerHere;
        public bool resetMe=false;

        #region Platform Types
        [Header("Platform Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] PlatformTypes platformType;
        public enum PlatformTypes                              //BUTTON TYPES
        {
            PlatformMove,
            PlatformMoveLoop,
        }
        public PlatformTypes GetPlatformType() => platformType;
        #endregion

        [Header("Platform Settings")]
        [SerializeField] private string resetID;        //ID used to reset pressure plate. Its not necessary. Just an options
        public Transform playerTransform;
        [SerializeField] private float platformSpeed = 1;
        [SerializeField] private bool isStarted = false;        //Is platform start to move.

        [Header("Target Positions")]
        [SerializeField] private Transform startPos;        //Platform start position
        [SerializeField] private Transform endPos;          //Platform end position
        private Vector3 oldStartPos;
        private Vector3 oldEndPos;

        protected override void Start()
        {
            base.Start();
            resetMe = false;
            if (startPos!=null && endPos!=null)
            {
                oldStartPos = startPos.position;
                oldEndPos = endPos.position;
            }
            StartCoroutine("WaitForStart");          
        }

        IEnumerator WaitForStart()
        {
            yield return new WaitForSeconds(0.1f);
            switch (platformType)
            {
                case PlatformTypes.PlatformMove:
                case PlatformTypes.PlatformMoveLoop:
                    if (myControllerObjList.Count <= 0)     //If there is no controller object start auto.
                    {
                        myControllerStatus = true;
                        myCurrentStatus = true;
                        StartCoroutine(MoveObject()); 
                    }
                    break;
            }
        }

        private void FixedUpdate()
        {
            transformVelocity = (transform.position - prevPos) / Time.deltaTime;        //Platform velocity.
            prevPos = transform.position;                                               //Previous platform position.

            if (isPlayerHere)                                                           //If player on platform.
            {
                if(characterController!=null)
                {
                    characterController.Move(transformVelocity * Time.deltaTime);       //Move player with platform.          
                }
            }
        }

        public override void OnSettingMe(string getID, bool getBool)
        {
            if (getID == this.myID)
            {
                myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);

                if (myControllerStatus)
                {
                    myCurrentStatus = myControllerStatus;
                    switch (myLogicGateType) 
                    {
                        case MyFunctions.LogicGateType.DontHaveGate:
                            myCurrentStatus = getBool;
                            break;
                        case MyFunctions.LogicGateType.Not:
                            myCurrentStatus = !getBool;
                            break;
                    }

                    if (myCurrentStatus && !isStarted)
                    {
                        resetMe = false;
                        StartCoroutine(MoveObject());
                    }

                }
                else if (!myControllerStatus)
                {
                    myCurrentStatus = false;
                    myControllerStatus = false;
                    StopCoroutine(MoveObject());
                }
            } 
            
            if(getID==this.resetID)
            {
                ResetValues();
            }
        }

        public void ResetValues()
        {
            resetMe = true;
            isStarted = false;
            myCurrentStatus = false;
            StopCoroutine(MoveObject());          
            startPos.position = oldStartPos;
            endPos.position = oldEndPos;
            transform.position = oldStartPos;     
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.CompareTag("Player"))
            {              
                characterController = other.GetComponent<CharacterController>();
                if(characterController != null)
                {                   
                    isPlayerHere = true;
                    playerTransform = other.transform;
                }
            }

            if(other.transform.CompareTag("MoveObj"))
            {
                other.transform.SetParent(transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.transform.CompareTag("Player"))
            {              
                isPlayerHere = false;
                playerTransform = null;
                characterController = null;
            }

            if (other.transform.CompareTag("MoveObj"))
            {
                other.transform.SetParent(null);
              
            }
        } 

        IEnumerator MoveObject() //Find another way
        {
            float elapsedTime = 0;
            bool isFin = false;
            bool getBool = false;

            switch (platformType)
            {
                case PlatformTypes.PlatformMove:
                    isStarted = true;
                    getBool = isStarted;
                    break;

                case PlatformTypes.PlatformMoveLoop:
                    getBool = myCurrentStatus;
                    break;
            }

            while (getBool && !resetMe)
            {               
                transform.position = Vector3.MoveTowards(startPos.position, endPos.position, (elapsedTime * platformSpeed)); 
                elapsedTime += Time.deltaTime;

                if (transform.position == endPos.position)
                {
                    if (endPos.position == oldStartPos)     
                    {
                        endPos.position = oldEndPos;         //swith to new end pos which is oldEndPos
                        startPos.position = transform.position;      //new start position is current transform position
                        isFin = true;
                        elapsedTime = 0;
                    }
                    else if (endPos.position == oldEndPos)
                    {
                        endPos.position = oldStartPos;      //swith to new end pos which is oldStartPos
                        startPos.position = transform.position;     //new start position is current transform position
                        isFin = true;
                        elapsedTime = 0;
                    }

                }

                switch(platformType)
                {
                    case PlatformTypes.PlatformMove:                                             

                        if (isFin)
                        {
                            getBool = false;
                            isStarted = false;
                            yield break;                           
                        }
                        else
                        {
                            yield return new WaitForFixedUpdate();
                        }

                        break;

                    case PlatformTypes.PlatformMoveLoop:

                        if (myCurrentStatus)
                        {
                            yield return new WaitForFixedUpdate();
                        }
                        else
                        {
                            isStarted = false;
                            startPos.position = transform.position;
                            yield break;
                        }

                        break;
                }             
            }

            startPos.position = transform.position;

            yield break;
        }
    }
}