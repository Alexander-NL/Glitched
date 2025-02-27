using UnityEngine;

public class DissapearTile : MonoBehaviour
{
    public GameObject targetObject; // The GameObject to fade and animate
    public float fadeInDuration = 2f; // Duration of the fade in
    public float holdDuration = 2f; // Duration to hold after fading in
    public float fadeOutDuration = 2f; // Duration of the fade out
    public float inactiveHoldDuration = 2f; // Duration to hold after fading out (collider active)

    private Renderer targetRenderer;
    private Material targetMaterial;
    private Animator targetAnimator;
    private BoxCollider2D targetCollider; // Use BoxCollider2D for 2D gameplay
    private float timer;
    private enum FadeState { FadingIn, Holding, FadingOut, InactiveHold }
    private FadeState currentState;

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object is not assigned!");
            return;
        }

        // Get the Renderer, Animator, and BoxCollider2D components
        targetRenderer = targetObject.GetComponent<Renderer>();
        targetAnimator = targetObject.GetComponent<Animator>();
        targetCollider = targetObject.GetComponent<BoxCollider2D>();

        if (targetRenderer == null || targetAnimator == null || targetCollider == null)
        {
            Debug.LogError("Target Object must have a Renderer, Animator, and BoxCollider2D component!");
            return;
        }

        // Create a new material instance to avoid modifying the original material
        targetMaterial = new Material(targetRenderer.material);
        targetRenderer.material = targetMaterial;

        // Initialize variables
        timer = fadeInDuration;
        currentState = FadeState.FadingIn;

        // Start with the object fully transparent and collider disabled
        SetAlpha(0f);
        targetCollider.enabled = false;
        targetObject.SetActive(true);
    }

    void Update()
    {
        if (targetObject == null || targetRenderer == null || targetAnimator == null || targetCollider == null)
            return;

        // Update the timer
        timer -= Time.deltaTime;

        switch (currentState)
        {
            case FadeState.FadingIn:
                HandleFadeIn();
                break;

            case FadeState.Holding:
                HandleHold();
                break;

            case FadeState.FadingOut:
                HandleFadeOut();
                break;

            case FadeState.InactiveHold:
                HandleInactiveHold();
                break;
        }
    }

    private void HandleFadeIn()
    {
        // Fade the object in
        Fade(1f, fadeInDuration);
        targetAnimator.Play("V_Reapppear");

        // Transition to the Hold state once fading in is complete
        if (timer <= 0)
        {
            currentState = FadeState.Holding;
            timer = holdDuration;
        }
    }

    private void HandleHold()
    {
        // Keep the object fully visible
        SetAlpha(1f);
        targetAnimator.Play("V_idle");

        // Transition to the Fade Out state once holding is complete
        if (timer <= 0)
        {
            currentState = FadeState.FadingOut;
            timer = fadeOutDuration;
        }
    }

    private void HandleFadeOut()
    {
        // Fade the object out
        Fade(0f, fadeOutDuration);
        targetAnimator.Play("V_dissapear");

        // Transition to the Inactive Hold state once fading out is complete
        if (timer <= 0)
        {
            currentState = FadeState.InactiveHold;
            timer = inactiveHoldDuration;

            // Enable the collider during the inactive hold phase
            targetCollider.enabled = true;
        }
    }

    private void HandleInactiveHold()
    {
        // Keep the object fully transparent
        SetAlpha(0f);

        // Transition back to the Fade In state once the inactive hold is complete
        if (timer <= 0)
        {
            currentState = FadeState.FadingIn;
            timer = fadeInDuration;

            // Disable the collider during the fade in, hold, and fade out phases
            targetCollider.enabled = false;
        }
    }

    private void Fade(float targetAlpha, float duration)
    {
        // Lerp the alpha value over time
        Color color = targetMaterial.color;
        color.a = Mathf.MoveTowards(color.a, targetAlpha, Time.deltaTime / duration);
        targetMaterial.color = color;
    }

    private void SetAlpha(float alpha)
    {
        // Set the alpha value directly
        Color color = targetMaterial.color;
        color.a = alpha;
        targetMaterial.color = color;
    }
}