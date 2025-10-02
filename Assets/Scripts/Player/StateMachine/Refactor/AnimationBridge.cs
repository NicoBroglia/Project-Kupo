using UnityEngine;

public class AnimationBridge : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on " + gameObject.name);
            }
        }
    }
    public void SetMoveSpeed(float value)
    {
        animator.SetFloat("MoveSpeed", value);
    }

}
