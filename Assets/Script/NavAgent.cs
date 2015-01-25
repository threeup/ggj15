using UnityEngine;
using System.Collections;

public class NavAgent : MonoBehaviour
{
    [SerializeField]
    Vector3 gravity = new Vector3(0f, -4.9f, 0f);
    [SerializeField]
	Vector3 velocity;
	public Vector3 Velocity { get { return velocity; } }
    public Vector3 WalkVelocity { get { return walkVelocity; } }
    [SerializeField]
    Vector3 acceleration;
    [SerializeField]
    float scalar = 0.256f;
    [SerializeField]
    float walkSpeed = 20f;
    [SerializeField]
    float jumpSpeed = 120f;
    [SerializeField]
    float jumpButtonTime = 0.1f;
    [SerializeField]
    bool startGrounded = true;

    public AnimationCurve walkSpeedMultiplier = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    public AnimationCurve jumpSpeedMultiplier = new AnimationCurve(new Keyframe(0f, 0.01f), new Keyframe(1f, 0.066f));
    public bool isGrounded;

    Vector3 walkVelocity;
    Vector3 jumpVelocity;
    CharacterController2D thisController;
    InputAgent thisInputAgent;

    void Awake()
    {
        thisController = GetComponent<CharacterController2D>();
        if( thisController == null )
            Debug.LogWarning("NavAgent: Missing CharacterController2D");

        thisInputAgent = GetComponent<InputAgent>();
        if( thisInputAgent == null )
            Debug.LogWarning("NavAgent: Missing InputAgent");

        if( startGrounded )
            thisController.WarpToGrounded();
    }

    void FixedUpdate()
    {
        acceleration = gravity;
        AddVelocity(acceleration);

        CharacterController2D.CollisionState collisionState = thisController.Move((velocity + walkVelocity) * Time.deltaTime * scalar);

        if( collisionState.CollideRight || collisionState.CollideLeft )
            SetVelocityX(0f);
        if( collisionState.CollideBelow || collisionState.CollideAbove )
            SetVelocityY(0f);

        isGrounded = collisionState.IsGrounded;
    }

    public void SetVelocityX(float val)
    {
        velocity.x = val;
    }

    public void SetVelocityY(float val)
    {
        velocity.y = val;
    }

    public void AddVelocity(Vector3 vel)
    {
        velocity += vel;
    }

    public void Walk(float moveX)
    {
        walkVelocity = Vector3.right * Mathf.Sign(moveX) * walkSpeedMultiplier.Evaluate(Mathf.Abs(moveX)) * walkSpeed;
    }

    public void Jump()
    {
        StopCoroutine("JumpRoutine");
        StartCoroutine("JumpRoutine");
    }

    IEnumerator JumpRoutine()
    {
        float time = 0f;

        AddVelocity(Vector3.up * jumpSpeed * 0.5f);
        BroadcastMessage("OnJump", SendMessageOptions.DontRequireReceiver);

        yield return null;

        while( time < jumpButtonTime )
        {
            if( thisInputAgent.JumpDown && !thisController.State.CollideAbove )
                AddVelocity(Vector3.up * jumpSpeedMultiplier.Evaluate(time / jumpButtonTime) * jumpSpeed);

            time += Time.deltaTime;

            yield return null;
        }
    }
}
