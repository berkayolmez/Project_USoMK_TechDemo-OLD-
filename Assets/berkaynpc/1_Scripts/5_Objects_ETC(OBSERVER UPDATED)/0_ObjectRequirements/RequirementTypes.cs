using UnityEngine;

public class RequirementTypes : MonoBehaviour
{
    #region Global Req Types
    public enum RequirementType
    {
        nothing,
        RedKey,
        BlueKey,
        GreenKey,
        Battery,
        Wire,
        CubeNothing,
        CubeRed,
        CubeGreen,
        CubeBlue,
        //Add more
    }
    #endregion

    #region Laser Types

    [SerializeField] LaserReqTypes laserReqType;
    public enum LaserReqTypes
    {
        nothing,
        RedLaser,
        BlueLaser,
        GreenLaser,
    }
    public LaserReqTypes GetLaserReqType()
    {
        return laserReqType;
    }

    #endregion

    #region Spell Element Types

    [SerializeField] SpellElementTypes spellElementType;
    public enum SpellElementTypes
    {
        Electricity,
        Fire,
        Ice,
        Wind,
        Light,
    }
    public SpellElementTypes GetElementType()
    {
        return spellElementType;
    }

    #endregion

    //All Reqiurement types here
}


