using UnityEngine;

public class OrbitAroundSphere3D : MonoBehaviour
{
    [Header("Orbit Center")]
    public Transform sphereCenter;

    [Header("Orbit Settings")]
    [Tooltip("Distance from the center of the sphere")]
    public float orbitRadius = 10f;

    [Tooltip("Number of complete revolutions")]
    public float revolutions = 3f;

    [Tooltip("Orbit speed in degrees per second")]
    public float speed = 90f;

    [Header("Orbit Plane")]
    [Tooltip("Tilt of the circular orbit around the X axis")]
    public float tiltX = 30f;

    [Tooltip("Tilt of the circular orbit around the Z axis")]
    public float tiltZ = 0f;

    [Header("Object Rotation")]
    [Tooltip("If enabled, the object rotates to face the direction it is moving")]
    public bool faceDirection = false;

    private float currentAngle = 0f;
    private float totalAngleTravelled = 0f;

    private Vector3 previousPosition;

    // Object does NOT orbit until Orbit() is called
    private bool isOrbiting = false;


    void Start()
    {
        // Place object at starting position
        Vector3 center = Vector3.zero;

        if (sphereCenter != null)
        {
            center = sphereCenter.position;
        }

        transform.position =
            center + new Vector3(orbitRadius, 0f, 0f);

        previousPosition = transform.position;
    }


    void Update()
    {
        // Do nothing until button calls Orbit()
        if (isOrbiting == false)
            return;


        float targetAngle = revolutions * 360f;

        // Stop after desired number of revolutions
        if (totalAngleTravelled >= targetAngle)
        {
            isOrbiting = false;
            return;
        }


        float angleStep = speed * Time.deltaTime;


        // Prevent overshooting final revolution
        if (totalAngleTravelled + angleStep > targetAngle)
        {
            angleStep =
                targetAngle - totalAngleTravelled;
        }


        currentAngle += angleStep;

        totalAngleTravelled += angleStep;


        // Convert degrees to radians
        float radians =
            currentAngle * Mathf.Deg2Rad;


        // Circular orbit in XZ plane
        Vector3 localOrbitPosition =
            new Vector3(
                Mathf.Cos(radians) * orbitRadius,
                0f,
                Mathf.Sin(radians) * orbitRadius
            );


        // Tilt orbit plane
        Quaternion orbitTilt =
            Quaternion.Euler(
                tiltX,
                0f,
                tiltZ
            );


        Vector3 tiltedPosition =
            orbitTilt * localOrbitPosition;


        // Find sphere center
        Vector3 center =
            Vector3.zero;

        if (sphereCenter != null)
        {
            center =
                sphereCenter.position;
        }


        previousPosition =
            transform.position;


        // Move object
        transform.position =
            center + tiltedPosition;


        // Optional: face direction of movement
        if (faceDirection)
        {
            Vector3 movementDirection =
                transform.position -
                previousPosition;


            if (movementDirection.sqrMagnitude > 0.0001f)
            {
                transform.rotation =
                    Quaternion.LookRotation(
                        movementDirection
                    );
            }
        }
    }


    // CALL THIS FROM YOUR BUTTON
    public void Orbit()
    {
        totalAngleTravelled = 0f;
        isOrbiting = true;
    }
}