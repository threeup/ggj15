using UnityEngine;
using System.Collections;

public class NavAgent : MonoBehaviour
{
    [SerializeField]
    Vector3 gravity = new Vector3(0f, -9.8f, 0f);
    [SerializeField]
    Vector3 velocity;
    [SerializeField]
    Vector3 acceleration;
    [SerializeField]
    float scalar = 0.256f;
    [SerializeField]
    float jumpVelocity = 100f;
    [SerializeField]
    float walkSpeed = 10f;
    [SerializeField]
    bool startGrounded = true;

    public AnimationCurve walkSpeedMultiplier = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    public bool isGrounded;

    Vector3 walkVelocity;
    CharacterController2D thisController;

    void Awake()
    {
        thisController = GetComponent<CharacterController2D>();
        if( thisController == null )
            Debug.LogWarning("NavAgent: Missing CharacterController2D");

        if( startGrounded )
            thisController.WarpToGrounded();
    }

    void FixedUpdate()
    {
        acceleration = gravity;
        velocity += acceleration;

        CharacterController2D.CollisionState collisionState = thisController.Move((velocity + walkVelocity) * Time.deltaTime * scalar);

        if( collisionState.CollideBelow || collisionState.CollideAbove )
            velocity.y = 0f;
        if( collisionState.CollideRight || collisionState.CollideLeft )
            velocity.x = 0f;

        isGrounded = collisionState.IsGrounded;
    }

    public void Walk(float moveX)
    {
        walkVelocity = Vector3.right * Mathf.Sign(moveX) * walkSpeedMultiplier.Evaluate(Mathf.Abs(moveX)) * walkSpeed;
    }

    public void Jump()
    {
        velocity += Vector3.up * jumpVelocity;
        BroadcastMessage("OnJump", SendMessageOptions.DontRequireReceiver);
    }
}
