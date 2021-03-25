using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class PlayerShaderTrack : MonoBehaviour
    {
        public static int PosID = Shader.PropertyToID("_Position");
        public static int SizeID = Shader.PropertyToID("_Size");

        public float setSize=1;
        public Material wallMat;
        private Camera cameraHandler;
        public LayerMask layerMask;

        private void Start()
        {
            cameraHandler = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        private void Update()
        {
            var dir = cameraHandler.transform.position - transform.position;
            var ray = new Ray(transform.position, dir.normalized);

            if(Physics.Raycast(ray,3000,layerMask))
            {
                wallMat.SetFloat(SizeID, setSize);
            }
            else
            {
                wallMat.SetFloat(SizeID, 0);
            }

            var view = cameraHandler.WorldToViewportPoint(transform.position);
            wallMat.SetVector(PosID, view);
        }

    }
}