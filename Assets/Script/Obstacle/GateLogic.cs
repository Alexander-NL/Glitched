using UnityEngine;

public class GateController : MonoBehaviour
{
    public int state = 0; // 0 = closed, 1 = open (can be extended to more states if needed)

    private void Start()
    {
        // Set the gate's initial state
        SetGateState(state);
    }

    public void ToggleGate()
    {
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

        // Optional: Change the sprite or animation to reflect the gate's state
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = (newState == 1) ? Color.green : Color.red; // Example: Change color
        }
    }
}