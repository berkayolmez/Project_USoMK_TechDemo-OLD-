using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class JumpPads : MonoBehaviour
    {
        [Range(10,100)]
        [SerializeField] private float force;
        [Range(0, 60)]
        [SerializeField] private float duration = 1;
        [SerializeField] private Transform launchWay;
        private bool canLaunch=true;
        private Color startColor;
        private Renderer objMats;

        private void Start()
        {            
            objMats =GetComponent<Renderer>();
            startColor = objMats.material.GetColor("_EmissionColor");
        }

        private void OnTriggerEnter(Collider other)
        {
            if(canLaunch)
            {
                if (other.CompareTag("Player"))
                {
                    PlayerLocomotion playerController = other.GetComponent<PlayerLocomotion>();
                    if (playerController != null && launchWay != null)
                    {
                        if(duration!=0)
                        {
                            objMats.material.SetColor("_EmissionColor", startColor * 0);
                        }

                        playerController.HandleAddImpact(launchWay.right,force);
                 
                        // playerController.velocity = launchWay.right * force;
                        //playerController.isJumping = true;
                        //cController.Move(launchWay.right * force);
                        canLaunch = false;
                        StartCoroutine(WaitFor());
                    }
                }

                if(other.CompareTag("MoveObj"))
                {
                    Rigidbody rigid = other.GetComponent<Rigidbody>();

                    if(rigid!=null && launchWay != null)
                    {
                        if (duration != 0)
                        {
                            objMats.material.SetColor("_EmissionColor", startColor * 0);
                        }
                        //rigid.velocity(launchWay.right*force,ForceMode.Impulse);
                        rigid.velocity = launchWay.right * force;
                        StartCoroutine(WaitFor());
                    }
                }
            }
        }

        IEnumerator WaitFor()
        {
            yield return new WaitForSeconds(duration);
            objMats.material.SetColor("_EmissionColor", startColor);
            canLaunch = true;
        }
    }
}