using UnityEngine;

public class GateController : MonoBehaviour
{
    public int state = 0; // 0 = closed, 1 = open (can be extended to more states if needed)
    public Sprite closedSprite; // Sprite for the closed state
    public Sprite openSprite; // Sprite for the open state

    public AudioSource src;
    public AudioClip GateAudio;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    private void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the gate's initial state
        SetGateState(state);
    }

    public void ToggleGate()
    {
        src.clip = GateAudio;
        src.Play();
        // Increment state and use modulo to keep it binary
        state = (state + 1) % 2; // Modulo 2 ensures state is always 0 or 1
        SetGateState(state);
    }

    private void SetGateState(int newState)
    {
        // Enable/disable the collider to block or allow passage
        Collider2D gateCollider = GetComponent<Collider2D>();
        if (gateCollider != null)
        {
            gateCollider.enabled = (newState == 0); // Collider enabled when closed (state 0)
        }

        // Change the sprite to reflect the gate's state
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = (newState == 0) ? closedSprite : openSprite;
        }
    }
}