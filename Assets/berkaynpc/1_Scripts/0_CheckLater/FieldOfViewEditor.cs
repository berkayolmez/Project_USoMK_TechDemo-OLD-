using UnityEngine;
using UnityEditor;
using System.Collections;

namespace project_usomk
{
  // [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : MonoBehaviour
    {/*
        void OnSceneGUI()
        {
            {
                FieldOfView fov = (FieldOfView)target;
                Handles.color = Color.white;
                Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);

                Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
                Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);

                Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
                Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);               
            }
        }*/
    }
}