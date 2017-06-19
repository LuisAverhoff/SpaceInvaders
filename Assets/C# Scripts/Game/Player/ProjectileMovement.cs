using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public Vector2 maxVelocity;

    private Rigidbody2D projectileRigidbody2D;

    void Start()
    {
        projectileRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        projectileRigidbody2D.MovePosition(projectileRigidbody2D.position + maxVelocity * Time.fixedDeltaTime);
    }
}
