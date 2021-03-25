using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReflectable 
{
    RequirementTypes.LaserReqTypes laserReqType { get; }
    void Reflect(bool getBool,float getLaserLength);

}
