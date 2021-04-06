using UnityEngine;

public class RequirementTypes : MonoBehaviour
{
    // [Flags]  //þuan çalýþmýyor çare bulunca eklenecek. eþleþtirme red,blue arýyor. ayrý ayrý görmüyor split ile belki çözülebilir

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
        //daha fazla þey eklenebilir
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
}


