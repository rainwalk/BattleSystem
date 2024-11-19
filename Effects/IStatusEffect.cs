public interface IStatusEffect
{
    void ApplyEffect(Ship ship);
    void UpdateEffect(Ship ship, float deltaTime);
}
