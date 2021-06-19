using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    [CreateAssetMenu(fileName ="New Object Type",menuName ="Objects/New PushCube Type")]
    public class PushCubes : ScriptableObject
    {
        [SerializeField] private new string name = "New PushCube Type";
        [SerializeField] private Texture2D cubeTexture;

        [Header("Select a Loader Type")]
        [SerializeField] ObjectPushType pushType;
        public enum ObjectPushType
        {
            simpleMovementable,
            canControllable,
        }
        public ObjectPushType PushType => pushType;

        [Header("Requirement Type")]                                          //REQUIREMENT TYPE
        [SerializeField] private RequirementTypes.RequirementType reqType;    //    


        public string Name => name;
        public Texture2D CubeTexture => cubeTexture;
    }
}