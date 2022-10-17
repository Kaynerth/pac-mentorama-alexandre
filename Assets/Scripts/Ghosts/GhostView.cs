using UnityEngine;

public enum GhostType
{
    Blinky,
    Pinky,
    Inky,
    Clyde
}

public class GhostView : MonoBehaviour
{
    public CharacterMotor CharacterMotor;
    public GhostAI GhostAI;
    public Animator Animator;
    public GhostType GhostType;

    void Start()
    {
        Animator.SetInteger("GhostType", (int)GhostType);
        CharacterMotor.OnDirectionChange += CharacterMotor_OnDirectionChange;
        GhostAI.OnGhostStateChange += GhostAI_OnGhostStateChange;
    }

    private void GhostAI_OnGhostStateChange(GhostState ghostState)
    {
        Animator.SetInteger("State", (int)ghostState);
    }

    private void CharacterMotor_OnDirectionChange(Direction direction)
    {
        Animator.SetInteger("Direction", (int)direction - 1);
    }
}
