using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellInteractive
{
    void SpellInteract(RequirementTypes.SpellElementTypes getSpellElement);
    /// <summary>
    /// To show UI or do something when player comes near the object.
    /// </summary>
    void PlayerNearBy(bool isNear);     
    void PlayerCanInteract(bool canInteract);       //spell interactive objects
}
