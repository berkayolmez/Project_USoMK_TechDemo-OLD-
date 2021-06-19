using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class JumpPads : MonoBehaviour
    {
        [Range(10,100)]
        [SerializeField] private float force;       //Jump pad force
        [Range(0, 60)]
        [SerializeField] private float coolDown = 1;        //Cooldown
        [SerializeField] private Transform launchWay;       //For launch direction 
        private bool canLaunch=true;                        //Can jump pad launch?
        private Color startColor;                           //For canLaunch visualization
        private Renderer objMats;                           //For canLaunch visualization

        private void Start()
        {            
            objMats = GetComponent<Renderer>();
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
                        if(coolDown!=0)
                        {
                            objMats.material.SetColor("_EmissionColor", startColor * 0);        //Set material emmission to zero
                        }

                        playerController.HandleAddImpact(launchWay.right,force);        //Add a force to player

                        canLaunch = false;
                        StartCoroutine(WaitFor());
                    }
                }

                if(other.CompareTag("MoveObj"))
                {
                    Rigidbody rigid = other.GetComponent<Rigidbody>();

                    if(rigid!=null && launchWay != null)
                    {
                        if (coolDown != 0)
                        {
                            objMats.material.SetColor("_EmissionColor", startColor * 0);        //Set material emmission to zero
                        }
                        rigid.velocity = launchWay.right * force;       //Add a force to object
                        StartCoroutine(WaitFor());
                    }
                }
            }
        }

        IEnumerator WaitFor()
        {
            yield return new WaitForSeconds(coolDown);      //wait for cooldown
            objMats.material.SetColor("_EmissionColor", startColor);        //Set back material emmission to startColor.
            canLaunch = true;
        }
    }
}