using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic component for moving usable by all moving gameObjects
/// </summary>
public class Movement : MonoBehaviour
{
    [Tooltip("The highest speed the object can travel at")]
    [SerializeField]
    private float topSpeed = 100f;
    [Tooltip("The number of seconds it takes the object to reach its top speed")]
    [SerializeField]
    private float accelerationTime = 1f;
    [Tooltip("The number of seconds it takes the object to stop from top speed with no propulsion")]
    [SerializeField]
    private float dampingTime = 1f;
    public Vector2 velocity = Vector2.zero;
    // How much to accelerate and damp per frame to get desired acceleration and damping length
    private float accelerationRate;
    private float dampingRate;
    [Tooltip("The highest speed the object can travel at")]
    public float TopSpeed
    {
        get { return topSpeed; }
        set
        {
            topSpeed = value;
            CalculateAccelerationAndDampingRates();
        }
    }
    [Tooltip("The number of seconds it takes the object to reach its top speed")]
    public float AccelerationTime
    {
        get { return accelerationTime; }
        set
        {
            accelerationTime = value;
            CalculateAccelerationAndDampingRates();
        }
    }
    [Tooltip("The number of seconds it takes the object to stop from top speed with no propulsion")]
    public float DampingTime
    {
        get { return dampingTime; }
        set
        {
            dampingTime = value;
            CalculateAccelerationAndDampingRates();
        }
    }
    public float DampRateX
    {
        get { return -Mathf.Sign(velocity.x) * dampingRate; }
    }
    public float DampRateY
    {
        get { return -Mathf.Sign(velocity.y) * dampingRate; }
    }
    [Tooltip("If the object were to stop moving, this is the position it would end up at after damping")]
    public Vector2 EndPosition
    {
        get
        {
            if (dampingRate > 0f)
            {
                // Found through kinematic equation v_f^2 = v_i^2 + 2a(x_f - x_i)
                return new Vector2(
                    transform.position.x - (Mathf.Pow(velocity.x, 2) / (2 * DampRateX)),
                    transform.position.y - (Mathf.Pow(velocity.y, 2) / (2 * DampRateY))
                    );
            }
            else
            {
                return new Vector2(transform.position.x, transform.position.y);
            }
        }
    }

    private void Awake()
    {
        CalculateAccelerationAndDampingRates();
    }

    /// <summary>
    /// Moves sprite in specified direction
    /// </summary>
    /// <param name="direction">A normalized Vector2 describing movement direction</param>
    public void Move(Vector2 direction)
    {
        if (accelerationTime == 0f)
        {
            velocity = direction * topSpeed;
        }
        else
        {
            // Calculate velocity from provided speed and direction
            velocity += direction * accelerationRate * Time.deltaTime;
        }

        // If there is velocity to damp, and the object is moving away from its
        // direction vector, subtract an amount of damping from the velocity

        // X
        if (velocity.x != 0f && (direction.x == 0f || Mathf.Sign(direction.x) != Mathf.Sign(velocity.x)))
        {
            // If it's set to damp instantly, just set velocity to 0 instantly
            if (dampingTime == 0f)
            {
                velocity.x = 0f;
            }
            else
            {
                // Calculate damping in the opposite direction of current X velocity
                if (Mathf.Abs(DampRateX * Time.deltaTime) >= Mathf.Abs(velocity.x))
                {
                    // If adding damping would cause sign to flip, just set velocity to 0
                    velocity.x = 0f;
                }
                else
                {
                    velocity.x += DampRateX * Time.deltaTime;
                }
            }
        }
        // Y
        if (velocity.y != 0f && (direction.y == 0f || Mathf.Sign(direction.y) != Mathf.Sign(velocity.y)))
        {
            if (dampingTime == 0f)
            {
                velocity.y = 0f;
            }
            else
            { 
                if (Mathf.Abs(DampRateY * Time.deltaTime) >= Mathf.Abs(velocity.y))
                {
                    velocity.y = 0f;
                }
                else
                {
                    velocity.y += DampRateY * Time.deltaTime;
                }
            }
        }

        // If velocity is less than 0.1, just set it to 0
        if (velocity.sqrMagnitude < Mathf.Pow(0.1f, 2))
        {
            velocity = new Vector2(0f, 0f);
        }

        // Clamp velocity to top speed 
        velocity = Vector2.ClampMagnitude(velocity, topSpeed);
        // Calculate transform
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    /// <summary>
    /// Whenever acceleration or damping times update, update their rates accordingly
    /// </summary>
    private void CalculateAccelerationAndDampingRates()
    {
        // Set accelerationRate if wouldn't result in divide-by-zero
        if (accelerationTime > 0f)
        {
            accelerationRate = topSpeed / accelerationTime;
        }
        else
        {
            accelerationRate = 0f;
        }

        // Set dampingRate if wouldn't result in divide-by-zero
        if (dampingTime > 0f)
        {
            dampingRate = topSpeed / dampingTime;
        }
        else
        {
            dampingRate = 0f;
        }
    }

}
