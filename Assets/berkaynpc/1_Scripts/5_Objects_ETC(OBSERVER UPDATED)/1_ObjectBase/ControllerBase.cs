using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    /// <summary>
    /// This class is for objects that both have logic gate and role as a controller. Also it can be triggered by its controller objects.
    /// </summary>
    public class ControllerBase : ObjectBase 
    {
        public List<string> targetID = new List<string>();  

        protected override void Start()
        {
            base.Start();
            MyGameEvents.current.onSendMyIDToListeners += OnListenToGetID;    //Custom Game Event //maybe to onenable
        }

        /// <summary>
        /// Get "getID" from objects thats calling to this method for searching their controllers.
        /// If this "getID" is on the targetID list, shout back to listeners "ImYourController".
        /// Listeners are the objects whose "getID" we get.
        /// </summary>
        protected void OnListenToGetID(string getID) 
        {
            if(targetID!=null)  //Protection for null target problems
            {
                if (this.targetID.Contains(getID)) //Check list for "getID"
                {
                    MyGameEvents.current.ImYourController(getID, this.gameObject);  //Shout back to listeners (listeners = getID). Send to them, myself as gameobject.
                }
            }            
        }

        public virtual void InteractedMe(bool getBool)
        {          
        }

        protected override void OnDestroy() //maybe to ondisable
        {
            MyGameEvents.current.onSendMyIDToListeners -= OnListenToGetID;    
        }  
    }
}