using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    Arrow,
    Spear
}

public class Projectile : MonoBehaviour
{
    // SCENE COMPONENTS
    private CollisionManager collisionManager;

    // GAMEOBJECT COMPONENTS
    private Movement movement;
    private CollisionHandler collisionHandler;
    private List<Hitbox> hitboxes;

    // MOVEMENT INFO
    public Vector2 direction = Vector2.zero;
    public float speed = 100f;

    private void Awake()
    {
        // Get collision manager from scene
        collisionManager = GameObject.FindGameObjectWithTag("CollisionManager").GetComponent<CollisionManager>();

        // Get own relevant components
        movement = gameObject.GetComponent<Movement>();
        collisionHandler = gameObject.GetComponent<CollisionHandler>();
        hitboxes = collisionHandler.hitboxes;
    }

    public void SetMotion(Vector3 position, Vector2 direction, float accelerationTime, float dampingTime)
    {
        transform.position = position;
        this.direction = direction;
        movement.AccelerationTime = accelerationTime;
        movement.DampingTime = dampingTime;
        movement.TopSpeed = speed;

        if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        collisionManager.AddToColliderSets(collisionHandler);
    }

    // Update is called once per frame
    void Update()
    {
        movement.Move(direction);

        // If any Physics Box is too far offscreen, destroy this projectile
        foreach(PhysicsBox phys in collisionHandler.physicsBoxes)
        {
            Vector3 distance = phys.GetDistanceOffscreen();

            if (Mathf.Abs(distance.x) > 100 || Mathf.Abs(distance.y) > 100)
            {
                collisionManager.RemoveFromColliderSets(collisionHandler);
                Destroy(gameObject);
            }
        }
    }

    public void OnHit()
    {
        collisionManager.RemoveFromColliderSets(collisionHandler);
        Destroy(gameObject);
    }
}
