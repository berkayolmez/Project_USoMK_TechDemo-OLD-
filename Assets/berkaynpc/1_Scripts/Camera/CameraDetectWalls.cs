using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class CameraDetectWalls : MonoBehaviour
    {
        RaycastHit rayHit;
        public LayerMask layerMask;
        [SerializeField] private GameObject playerObj=null;
        [SerializeField] private GameObject currentWall=null;
        [SerializeField] float lastColor;


        private void Update()
        {
            Debug.DrawLine(transform.position, playerObj.transform.position, Color.red);
        }
        private void FixedUpdate()
        {
           // DetectWalls();
        }

        public void DetectWalls(float getOldTransparency, float getNewTransparency)
        {          
            Vector3 direction = (playerObj.transform.position-transform.position);

            
              if (Physics.Raycast(transform.position, direction, out rayHit, 150f, layerMask))
              {
                    currentWall = rayHit.collider.gameObject;

                   if (currentWall != null)
                   {
                    Debug.Log("içerideyiz"); ////burada sýkýntý var sürekli aramasýný istemiyorum bunu düzelt currentWall !=null vs
                    StartCoroutine(ChangeAlpha(currentWall, getOldTransparency, getNewTransparency));
                   }          
              }  
        }




        private IEnumerator ChangeAlpha(GameObject getGameObj,float oldAlpha, float newAlpha)
        {
            Renderer objRenderer = getGameObj.GetComponent<Renderer>();

            Debug.Log("change alpha");

            float elapsed = 0.0f;
            while (elapsed < 1)
            {
                lastColor = Mathf.Lerp(oldAlpha, newAlpha, elapsed / 1);
                elapsed += Time.deltaTime;
                objRenderer.material.color = new Color(objRenderer.material.color.r, objRenderer.material.color.g, objRenderer.material.color.b, lastColor);
                yield return null;
            }
            lastColor = newAlpha;

            StopCoroutine("ChangeAlpha");
        }

    }
}