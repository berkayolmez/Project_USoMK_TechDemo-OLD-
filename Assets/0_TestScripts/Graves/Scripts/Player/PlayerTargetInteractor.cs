using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class PlayerTargetInteractor : MonoBehaviour
    {
        [Header("Targets")]
        public Transform currentTargetTransform;
        public List<Transform> spellTargetTransforms = new List<Transform>();


        private void OnTriggerEnter(Collider other)
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
                float minDist = 100;

                foreach (var target in spellTargetTransforms)
                {
                    ISpellInteractive test = target.GetComponent<ISpellInteractive>();

                    if(test!=null)
                    {
                        float dstToTarget = Vector3.Distance(transform.position,target.position);

                        if (dstToTarget < minDist)
                        {
                            if(currentTargetTransform != null)
                            {
                                currentTargetTransform.GetComponent<ISpellInteractive>().PlayerCanInteract(false);
                            }

                            currentTargetTransform = target;                                             
                            minDist = dstToTarget;
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

        private void OnTriggerExit(Collider other)
        {
            ISpellInteractive spellInteract = other.GetComponent<ISpellInteractive>();
            if (spellInteract != null)
            {
                spellTargetTransforms.Remove(other.transform);
                spellInteract.PlayerCanInteract(false);
                spellInteract.PlayerNearBy(false);
            }
        }

    }
}