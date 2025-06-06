using UnityEngine;

public class AnimationStateTester : MonoBehaviour
{
    [Tooltip("Reference to the CharacterAnimator2D component")]
    public CharacterAnimator2D animator2D;

    private void Update()
    {
        if (animator2D == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator2D.SetState(CharacterAnimator2D.State.Idle);
            Debug.Log("State set to: Idle");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator2D.SetState(CharacterAnimator2D.State.Run);
            Debug.Log("State set to: Run");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator2D.SetState(CharacterAnimator2D.State.Jump);
            Debug.Log("State set to: Jump");
        }
    }
}
