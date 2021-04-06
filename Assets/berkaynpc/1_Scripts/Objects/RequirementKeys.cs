using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace project_WAST
{
    public class RequirementKeys : MonoBehaviour
    {
        [SerializeField] RequirementTypes.RequirementType reqKeyType;    

        public RequirementTypes.RequirementType GetKeyType()
        {
            return reqKeyType;
        }

        [Header("DOTween")]
        [SerializeField] private bool canRotate;
        [SerializeField] private float coinRotateSpeed;
        // [SerializeField] private float jumpPower;
        // [SerializeField] private int numJumps;
        // [SerializeField] private float duration;
        // [SerializeField] private bool snapping;
        // [SerializeField] private float rotateDuration;
        // [SerializeField] private int vibrato;
        // [SerializeField] private float elasticity;

        private void Awake()
        {
            if (coinRotateSpeed <= 0)
            {
                coinRotateSpeed = 1f;
            }
            canRotate = true;
        }

        private void Update()
        {
            if (canRotate)
            {
                transform.Rotate(Vector3.up, Space.World);
            }
        }

        /*
        public void CoinFeedBack()
        {
            canRotate = false;
            Vector3 newPos = new Vector3(transform.localPosition.x, transform.localPosition.y + 3, transform.localPosition.z + 12);
            this.gameObject.transform.DOLocalJump(newPos, jumpPower, numJumps, duration, snapping);
            StartCoroutine(WaitFor());
        }

        IEnumerator WaitFor()
        {
            yield return new WaitForSeconds(0.25f);

            Vector3 newRot = new Vector3(transform.rotation.x, transform.rotation.y + 360, transform.rotation.z);
            this.gameObject.transform.DOPunchRotation(newRot, rotateDuration, vibrato, elasticity);

            yield return new WaitForSeconds(0.30f);

            //myParticles.Play();

            yield return new WaitForSeconds(0.75f);

            this.gameObject.transform.DOScale(0, 0.2f);

            yield return new WaitForSeconds(0.5f);

            Destroy(gameObject);

            StopCoroutine(WaitFor());

        }*/

    }
}