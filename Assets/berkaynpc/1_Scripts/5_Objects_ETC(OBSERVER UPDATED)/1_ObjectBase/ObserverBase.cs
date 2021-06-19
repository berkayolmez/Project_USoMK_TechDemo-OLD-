using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public abstract class ObserverBase : MonoBehaviour, IHaveStatus
    {
        public MyFunctions myFunctions = new MyFunctions(); //Custom functions pool

        public string myID; //Objects ID    //new Idea:ID pool. check it later

        #region Current Object Status //interface (IHaveStatus) implementation

        public bool myCurrentStatus;             
        public bool myStatus => myCurrentStatus;            //Interface implementation

        #endregion      

        #region MyController Objects  //Important: No need to manually add controller objects to myControllerObjList. //List will be ReadOnly

        public bool myControllerStatus;
        public List<GameObject> myControllerObjList = new List<GameObject>();

        #endregion

        protected virtual void Start()                                         
        {
            myControllerObjList.Clear();
            MyGameEvents.current.onImYourController += OnAreYouMyController;      //Add this method to event
            MyGameEvents.current.onSetTarget += OnSettingMe;                      //Add this method to event
            StartCoroutine("SendMyIDtoListenersWithDelay");     //Start coroutine to searching for controller objects
        }

        protected IEnumerator SendMyIDtoListenersWithDelay()
        {
            yield return new WaitForSeconds(0.05f);  
            if (myID != null)       //Protection for null problems
            {
                MyGameEvents.current.SendMyIDtoListeners(myID);     //At the start send myID to listeners (custom event onSendMyIDToListeners)
            }
            yield break;
        }

        /// <summary>
        /// This method triggerred by controller objects.
        /// Controller objects sends their targetID ("getTargetID" in this method) and selves as GameObject ("controllerObj").
        /// After then method, checks if current object's myID matching with getTargetID. 
        /// If these matches, the object from which we get the getTargetID ("controllerObj") is one of controllers this object.
        /// We need keep at controllerObjList that's because logic gates.
        /// </summary>
        /// <param name="getTargetID">
        /// Target ID from Controller Objects </param>      
        protected void OnAreYouMyController(string getTargetID, GameObject controllerObj) 
        {
            if (getTargetID == this.myID)
            {
                if (!myControllerObjList.Contains(controllerObj))       //Protection to repeating objects
                {
                    myControllerObjList.Add(controllerObj);
                }
            }
        }

        /// <summary>
        /// It calls if the button or the controllers are performed
        /// </summary>
        public virtual void OnSettingMe(string getTargetID, bool getbool)
        {

        } 

        protected virtual void OnDestroy() //maybe to ondisable
        {
            MyGameEvents.current.onImYourController -= OnAreYouMyController; 
            MyGameEvents.current.onSetTarget -= OnSettingMe;                
        }
    }
}