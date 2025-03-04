using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public GateController[] connectedGates; // Gates connected to this switch
    public Sprite state1Sprite; // Sprite for state 1 (e.g., "off")
    public Sprite state2Sprite; // Sprite for state 2 (e.g., "on")
    public SpriteRenderer buttonSpriteRenderer; // Reference to the SpriteRenderer component

    public AudioSource src;
    public AudioClip SwitchAudio;

    private bool isState1 = true; // Tracks the current state of the switch

    private void Start()
    {
        // Ensure the sprite is set to the initial state
        if (buttonSpriteRenderer != null && state1Sprite != null)
        {
            buttonSpriteRenderer.sprite = state1Sprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the "HitboxBA" tag
        if (collision.CompareTag("HitboxBA"))
        {
            Debug.Log("Switch has been toggled");
            ToggleSwitch();
        }
    }

    private void ToggleSwitch()
    {
        // Toggle the state
        isState1 = !isState1;

        // Update the button sprite based on the new state
        if (buttonSpriteRenderer != null)
        {
            buttonSpriteRenderer.sprite = isState1 ? state1Sprite : state2Sprite;
        }

        // Toggle all connected gates
        foreach (GateController gate in connectedGates)
        {
            src.clip = SwitchAudio;
            src.Play();
            gate.ToggleGate();
        }
    }
}