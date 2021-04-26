using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
   
    public class SetScale : MonoBehaviour,IHaveButton
    {
        [SerializeField] private float newScale = 1;
        [SerializeField] private float duration=1;
        [SerializeField] bool canSet;

        public void PressedButton(bool isButtonOn)
        {
            if(canSet)
            {
                canSet = false;
                if (isButtonOn)
                {
                    StartCoroutine(SetNewScale(newScale));
                }
                else
                {
                    StartCoroutine(SetNewScale2(0));
                }
            } 
        }

        IEnumerator SetNewScale(float getScale)
        {
            float elapsedTime = 0;
            while (transform.localScale.y <= getScale)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, getScale, transform.localScale.z), elapsedTime/duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Debug.Log("deneme1");
            transform.localScale = new Vector3(transform.localScale.x, getScale, transform.localScale.z);
            canSet = true;
            yield break;
        }

        IEnumerator SetNewScale2(float getScale)
        {
            float elapsedTime = 0;
            while (transform.localScale.y >= getScale)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, getScale, transform.localScale.z), elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Debug.Log("deneme2");
            transform.localScale = new Vector3(transform.localScale.x, getScale, transform.localScale.z);
            canSet = true;
            yield break;
        }
    }
}