using UnityEngine;
using System.Collections;

public class NavAgent : MonoBehaviour
{
    [SerializeField]
    Vector3 gravity = new Vector3(0f, -9.8f, 0f);
    [SerializeField]
    Vector3 velocity;
    [SerializeField]
    float scalar = 0.256f;
    [SerializeField]
    float jumpVelocity = 100f;
    [SerializeField]
    bool isGrounded;

    CharacterController2D thisController;

    void Awake()
    {
        thisController = GetComponent<CharacterController2D>();
        if( thisController == null )
            Debug.LogWarning("NavAgent: Missing CharacterController2D");

        thisController.WarpToGrounded();
    }

    void FixedUpdate()
    {
        velocity += gravity;

        thisController.Move(velocity * Time.deltaTime * scalar);

        if( thisController.CollideBelow || thisController.CollideAbove )
            velocity.y = 0f;
        if( thisController.CollideRight || thisController.CollideLeft )
            velocity.x = 0f;

        isGrounded = thisController.IsGrounded;
    }

    public void Jump()
    {
        velocity += Vector3.up * jumpVelocity;
    }
}
