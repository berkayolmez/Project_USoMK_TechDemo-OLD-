using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{/*
    public class SpeedBoost : MonoBehaviour ///////hýzlanmada sýkýntý var onu çöz bir anda hýzlanýyor ya da yavaþlýyor ************
    {
        private MyFunctions myFunctions = new MyFunctions();

        [Header("Booster Type")]
        [SerializeField] BoosterTypes boosterType;
        public enum BoosterTypes
        {
            SpeedBooster,
            SlowDowner,
        }
        public BoosterTypes GetBoosterType()
        {
            return boosterType;
        }

        [Range(1, 100)]
        [SerializeField] private int inTimerSpeed;
        [Range(1, 100)]
        [SerializeField] private int outTimerSpeed;
        private int timerSpeed;
        [SerializeField] private bool playerInArea;
        [SerializeField] private float currentSpeed;
        [SerializeField] private float maxSpeed;
        public float playerStartSpeed;
        private float enteranceSpeed;      
        private int speedFactor;
        private bool stopIt = false;

        private void Start()
        {
            StartCoroutine("GetRefs");
        }

        IEnumerator GetRefs()
        {
            yield return new WaitForSeconds(0.2f);
            //playerRefs = GameObject.FindGameObjectWithTag("PlayerReferences").GetComponent<PlayerReferences>();
          
            yield break;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                playerController = other.GetComponent<PlayerMovementController>();

                if(playerController!=null)
                {
                    timerSpeed = inTimerSpeed;
                    stopIt = false;
                    playerInArea = true;
                    enteranceSpeed = playerController.speed;
                    currentSpeed = enteranceSpeed;
                    StartCoroutine(NewSpeed());
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            switch(boosterType)
            {
                case BoosterTypes.SpeedBooster:

                    if (playerController != null)
                    {
                        if (playerInArea && playerController.moveControl > 0 && playerController.canMove && !playerController.isJumping)
                        {
                            timerSpeed = inTimerSpeed;
                            speedFactor = 1;
                        }
                        else if (playerInArea && playerController.moveControl <= 0) //canmove?
                        {
                            timerSpeed = outTimerSpeed;
                            speedFactor = -1;
                        }
                    }

                    break;

                case BoosterTypes.SlowDowner:

                    if (playerController != null)
                    {
                        if (playerInArea && playerController.moveControl > 0 && playerController.canMove && !playerController.isJumping)
                        {
                            timerSpeed = outTimerSpeed;
                            speedFactor = -1;
                        }
                    }

                    break;
            }

           
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerController = other.GetComponent<PlayerMovementController>();

                if (playerController != null)
                {
                    timerSpeed = outTimerSpeed;
                    speedFactor = -1;
                    stopIt = true;
                }
            }
        }
       
        IEnumerator NewSpeed()
        {
            while(playerInArea) //farklý bir þey koyulabilir düþün
            {
                currentSpeed += 0.01f* timerSpeed * speedFactor;

                if(currentSpeed>=maxSpeed)
                {
                    currentSpeed = maxSpeed;
                }
                else if(currentSpeed <= playerStartSpeed)
                {                   
                    currentSpeed = playerStartSpeed;

                    if (stopIt)
                    {
                        playerInArea = false;
                        yield break;
                    }
                }
               // playerController.speed = currentSpeed;

                yield return new WaitForFixedUpdate(); //dýþarýda olabilir bunu kontrol et***** //fixed yerine null olabilir kontrol et****
            }
            yield break;        
        }

    }*/
}