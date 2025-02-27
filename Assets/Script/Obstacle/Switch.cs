using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public GateController[] connectedGates; // Gates connected to this switch
    private bool isState1 = true; // Tracks the current state of the switch

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the "Hitbox" tag
        if (collision.CompareTag("HitboxBA"))
        {
            Debug.Log("Switch has been turned on");
            ToggleSwitch();
        }
    }

    private void ToggleSwitch()
    {
        // Toggle the state
        isState1 = !isState1;

        // Toggle all connected gates
        foreach (GateController gate in connectedGates)
        {
            gate.ToggleGate();
        }
    }
}