using UnityEngine;

public class MovingTargetMovementComponent : MovementComponent
{
    [Header("Oscillation Settings")]
    public float oscillationDistance = 5f;  // The maximum distance the object can move from its starting position
    public float oscillationSpeed = 1f;    // Speed of the oscillation movement

    [Header("Vertical Oscillation Settings")]
    public float verticalOscillationDistance = 5f;  // The maximum distance the object can move vertically from its starting position
    public float verticalOscillationSpeed = 1f;

    public OscillationDirection currentOscillationDirection = OscillationDirection.Horizontal;

    public enum OscillationDirection
    {
        None,
        Horizontal,
        Vertical
    }

    private Vector3 initialPosition;
    //Declare all entitys avalaible movement states.
    public MovingTargetHorizonalMovementState horizonalMovement;

    void Start()
    {
        //Initialize the states used for this entity.
        horizonalMovement = new MovingTargetHorizonalMovementState(movementStateMachine, this);
        
        //Start the state machine.
        movementStateMachine.Initialize(horizonalMovement);

        initialPosition = transform.position;
    }

    private void OscillateHorizontally()
    {
        float oscillation = Mathf.Sin(Time.time * oscillationSpeed) * oscillationDistance;
        Vector3 newPosition = initialPosition + transform.right * oscillation; // Move horizontally based on the object's right vector
        transform.position = newPosition;
    }

    private void OscillateVertically()
    {
        float verticalOscillation = Mathf.Sin(Time.time * verticalOscillationSpeed) * verticalOscillationDistance;
        Vector3 newVerticalPosition = initialPosition + transform.up * verticalOscillation; // Move vertically based on the object's up vector
        transform.position = newVerticalPosition;
    }

    public void Oscillate()
    {
        switch (currentOscillationDirection)
        {
            case OscillationDirection.Horizontal:
                OscillateHorizontally();
                break;
            case OscillationDirection.Vertical:
                OscillateVertically();
                break;
            default:
                break; // No oscillation for the 'None' option
        }
    }
}
