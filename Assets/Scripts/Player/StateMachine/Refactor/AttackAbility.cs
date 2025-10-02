using UnityEngine;

public class AttackAbility : IAbility
{
    private Animator animator;

    public AttackAbility(Animator animator)
    {
        this.animator = animator;
    }

    public void Activate()
    {
        animator.SetTrigger("Attack");
        // spawn hitbox, play sound, etc.
    }
}
