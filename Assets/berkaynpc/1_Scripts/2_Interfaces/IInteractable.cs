public interface IInteractable
{
    RequirementTypes.RequirementType reqType { get;}

    /// <summary>
    /// Player can interact with some objects. This method is called when player interacts.
    /// </summary>
    void Interact();

    /// <summary>
    /// This method checks if the player is holding down the interaction key. Triggered when interaction key is released.
    /// </summary>
    void IsStillPressing(bool buttonStatus);
}
