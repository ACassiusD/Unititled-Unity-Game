using UnityEngine;

public class HomingProjectile : Projectile
{
    public Transform target; // Target that the projectile will home in on
    public float turnSpeed = 5f; // Turning speed
    public float homingSpeed = 10f; // Speed at which the projectile homes towards the target

    protected override void Start()
    {
        base.Start(); // This ensures the rb is set from the base class

        // Find the MovingTarget (2) GameObject in the scene
        GameObject targetObject = GameObject.Find("MovingTarget (2)");
        if (targetObject)
        {
            target = targetObject.transform;
        }
        else
        {
            Debug.LogWarning("MovingTarget (2) not found in the scene!");
        }
    }

    protected override void Update()
    {
        // Check if a target has been set
        if (target != null)
        {
            // Calculate the direction from the projectile to the target
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Calculate the new direction the projectile should face
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, directionToTarget, turnSpeed * Time.deltaTime, 0.0f);

            // Set the projectile's rotation to face the new direction
            transform.rotation = Quaternion.LookRotation(newDirection);

            // Move the projectile towards the target
            transform.position += directionToTarget * homingSpeed * Time.deltaTime;
        }

        // Call the base Projectile Update
        base.Update();
    }
}
