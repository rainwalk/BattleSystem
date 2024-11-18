using UnityEngine;

public abstract class EffectBase : IEffect
{
    public string EffectName { get; protected set; }
    public virtual void ApplyEffect(Ship origin, Ship target) { }
}
