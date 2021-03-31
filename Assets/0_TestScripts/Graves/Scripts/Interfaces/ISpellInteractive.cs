using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellInteractive
{
    void SpellInteract(RequirementTypes.SpellElementTypes getSpellElement);

    void PlayerNearBy(bool isNear);

    void PlayerCanInteract(bool canInteract);
}
