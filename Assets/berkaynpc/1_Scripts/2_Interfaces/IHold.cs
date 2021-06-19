public interface IHold
{
    RequirementTypes.RequirementType reqType { get; }
    void Holding();
    void AreaEmpty();
}
