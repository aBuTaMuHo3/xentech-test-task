namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IDomainSettingVO
    {
        int DomainId { get; }
        float Value { get; set; }
        string Name { get; }
    }
}