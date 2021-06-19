using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    /// <summary>
    /// Find target interactive objects for spells.
    /// </summary>
    public class PlayerTargetInteractor : MonoBehaviour
    {
        [Header("Targets")]
        public Transform currentTargetTransform;    //Closest object
        public Transform lastTargetTransform;       //The last object the player moves away from is kept here.
        public List<Transform> spellTargetTransforms = new List<Transform>();

        private void OnTriggerEnter(Collider other)         //The target must have a rigidbody. Otherwise OnTriggerEnter doesn't find it.
        {
            ISpellInteractive spellInteract = other.GetComponent<ISpellInteractive>();
            if (spellInteract != null)
            {
                spellTargetTransforms.Add(other.transform);
                spellInteract.PlayerNearBy(true);
            }
        }

        private void Update()
        {           
            if(spellTargetTransforms.Count>0)
            {
                float minDist = 100;        //This will be public variable

                foreach (var target in spellTargetTransforms)
                {
                    ISpellInteractive spellInteractive = target.GetComponent<ISpellInteractive>();

                    if(spellInteractive!=null)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position,target.position);

                        if (distanceToTarget < minDist)
                        {
                            if(currentTargetTransform != null)
                            {
                                currentTargetTransform.GetComponent<ISpellInteractive>().PlayerCanInteract(false);
                            }

                            currentTargetTransform = target;                                             
                            minDist = distanceToTarget;
                        }
                    }
                }

                if(currentTargetTransform != null)
                {
                    currentTargetTransform.GetComponent<ISpellInteractive>().PlayerCanInteract(true);
                }
            }
            else
            {
                if (currentTargetTransform != null)
                {
                    currentTargetTransform.GetComponent<ISpellInteractive>().PlayerCanInteract(false);
                    currentTargetTransform = null;
                }
            }
        }

        private void OnTriggerExit(Collider other)      //The target must have a rigidbody. Otherwise OnTriggerEnter doesn't find it.
        {
            ISpellInteractive spellInteract = other.GetComponent<ISpellInteractive>();
            if (spellInteract != null)
            {
                lastTargetTransform = other.transform;
                spellTargetTransforms.Remove(other.transform);             
                spellInteract.PlayerCanInteract(false);
                spellInteract.PlayerNearBy(false);
            }
        }

    }
}