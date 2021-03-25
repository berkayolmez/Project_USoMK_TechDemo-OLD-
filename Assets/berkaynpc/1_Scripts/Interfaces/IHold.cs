public interface IHold
{
   // bool myStatus { get; }
    RequirementTypes.RequirementType reqType { get; }

    void Holding();

    void AreaEmpty();
}
