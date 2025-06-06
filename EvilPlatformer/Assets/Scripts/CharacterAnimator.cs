using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator2D : MonoBehaviour
{
    public enum State
    {
        Idle,
        Run,
        Jump
    }

    [Header("Sprite Renderer")]
    [Tooltip("The SpriteRenderer component on your character.")]
    public SpriteRenderer spriteRenderer;

    [Header("Idle Animation")]
    [Tooltip("List of sprites for the idle animation (looped).")]
    public List<Sprite> idleSprites = new List<Sprite>();
    [Tooltip("Frames per second for the idle animation.")]
    public float idleFrameRate = 6f;

    [Header("Run Animation")]
    [Tooltip("List of sprites for the run animation (looped).")]
    public List<Sprite> runSprites = new List<Sprite>();
    [Tooltip("Frames per second for the run animation.")]
    public float runFrameRate = 12f;

    [Header("Jump Animation")]
    [Tooltip("Single-frame sprite to display while jumping.")]
    public Sprite jumpSprite;

    // Current animation state
    private State currentState = State.Idle;
    // Frame‐timer and index for cycling through idle/run lists
    private float frameTimer = 0f;
    private int frameIndex = 0;

    private void ResetAnimationTimer()
    {
        frameTimer = 0f;
        frameIndex = 0;
    }

    /// <summary>
    /// Call this to change the animation state. 
    /// If you switch between Idle, Run, or Jump, the timer and frameIndex reset.
    /// </summary>
    public void SetState(State newState)
    {
        if (newState == currentState) return;
        currentState = newState;
        ResetAnimationTimer();

        // Immediately apply the first sprite of the new state:
        switch (currentState)
        {
            case State.Idle:
                if (idleSprites.Count > 0)
                    spriteRenderer.sprite = idleSprites[0];
                break;
            case State.Run:
                if (runSprites.Count > 0)
                    spriteRenderer.sprite = runSprites[0];
                break;
            case State.Jump:
                if (jumpSprite != null)
                    spriteRenderer.sprite = jumpSprite;
                break;
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                AnimateLoop(idleSprites, idleFrameRate);
                break;
            case State.Run:
                AnimateLoop(runSprites, runFrameRate);
                break;
            case State.Jump:
                // Jump is a single‐frame animation; we just ensure the jumpSprite is showing
                if (jumpSprite != null && spriteRenderer.sprite != jumpSprite)
                    spriteRenderer.sprite = jumpSprite;
                break;
        }
    }

    /// <summary>
    /// Cycles through the given sprite list at the specified frameRate (frames per second).
    /// Loops back to the start when it reaches the end.
    /// </summary>
    private void AnimateLoop(List<Sprite> sprites, float frameRate)
    {
        if (sprites == null || sprites.Count == 0 || frameRate <= 0f)
            return;

        frameTimer += Time.deltaTime;
        float frameDuration = 1f / frameRate;

        if (frameTimer >= frameDuration)
        {
            frameTimer -= frameDuration;
            frameIndex = (frameIndex + 1) % sprites.Count;
            spriteRenderer.sprite = sprites[frameIndex];
        }
    }
}
