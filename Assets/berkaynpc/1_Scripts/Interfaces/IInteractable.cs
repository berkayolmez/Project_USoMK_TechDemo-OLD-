public interface IInteractable
{
    //bool myStatus{ get;}
    RequirementTypes.RequirementType reqType { get;}
    void Interact();
    void StillPress(bool buttonStatus);

}
