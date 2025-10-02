using UnityEngine;

public class AttackAbility : IAbility
{
    private readonly AnimationBridge animationBridge;

    public AttackAbility(AnimationBridge animationBridge)
    {
        this.animationBridge = animationBridge;
    }

    public void Activate()
    {
        // spawn hitbox, play sound, etc.
    }
}
