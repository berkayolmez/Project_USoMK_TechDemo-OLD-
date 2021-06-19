using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace project_usomk
{
    public class PlatformFakeReal : ObjectBase
    {
        Renderer myRenderer;

        #region Platform Types

        [Header("Platform Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] PT_Types platformTriggerType;
        public enum PT_Types                              //BUTTON TYPES
        {
            RealPlatform,
            FakePlatform
        }
        public PT_Types GetPlatformType() => platformTriggerType;

        #endregion

        [Header("Platform Settings")]
        [SerializeField] private float duration;
        [SerializeField] private float strength;
        [SerializeField] private int vibrato;
        [SerializeField] private float randomness;
        [SerializeField] private bool isStarted = false;
        [SerializeField] private float roadShowTime=1;
        private Color newColor;
        private Color startColor;
        private Color startEmission;


        protected override void Start()
        {
            base.Start();
            myRenderer = GetComponent<Renderer>();

            startColor = myRenderer.material.GetColor("_BaseColor");
            startEmission = myRenderer.material.GetColor("_EmissionColor");
        }

        public override void OnSettingMe(string getID, bool getBool)
        {
            if (getID == this.myID)
            {
                if (!isStarted)
                {
                    isStarted = true;
                    switch (platformTriggerType)
                    {
                        case PT_Types.RealPlatform:
                            newColor = new Color(0.1f, 0.8f, 0, 0.5f);
                            break;
                        case PT_Types.FakePlatform:
                            newColor = new Color(0.8f, 0.1f, 0, 0.5f);
                            break;
                    }

                    StartCoroutine("ShowRoad", newColor);  //Show road
                }
            } 
        }

        private void OnTriggerEnter(Collider other)
        {  
            switch(platformTriggerType)
            {
                case PT_Types.FakePlatform:
                    if (other.CompareTag("Player"))
                    {
                        StartCoroutine("FakePlatform");
                    }
                    break;
            }
        }

        /// <summary>
        /// If player on fake ones disable to collider and renderer.
        /// Wait for cooldown timer after then enable them.
        /// </summary>
        IEnumerator FakePlatform()
        {
            transform.DOShakeScale(duration, strength, vibrato, randomness, true);
            newColor = new Color(0.8f, 0.1f, 0, 0.5f);
            StartCoroutine("ShowRoad", newColor);  //Show road

            yield return new WaitForSeconds(duration);

            transform.DOScale(new Vector3(1,1,1),duration);

            yield return new WaitForSeconds(duration);

            this.gameObject.GetComponent<MeshCollider>().enabled = false;
            this.gameObject.GetComponent<Renderer>().enabled = false;

            yield return new WaitForSeconds(2);

            this.gameObject.GetComponent<MeshCollider>().enabled = true;
            this.gameObject.GetComponent<Renderer>().enabled = true;

            yield return null;
        }

        /// <summary>
        /// Show fake and real ones.
        /// </summary>
        IEnumerator ShowRoad(Color getColor)
        {
            myRenderer.material.SetColor("_BaseColor", getColor);
            myRenderer.material.SetColor("_EmissionColor", getColor);

            yield return new WaitForSeconds(roadShowTime);

            myRenderer.material.SetColor("_BaseColor", startColor);
            myRenderer.material.SetColor("_EmissionColor", startEmission);

            isStarted = false;
            yield return null;
        }
    }
}