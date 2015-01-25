using UnityEngine;
using System.Collections;

public class NavAgent : MonoBehaviour
{
    [SerializeField]
    Vector3 gravity = new Vector3(0f, -15f, 0f);
    [SerializeField]
	Vector3 velocity;
	public Vector3 Velocity { get { return velocity; } }
    public Vector3 WalkVelocity { get { return walkVelocity; } }
    public bool IsSliding { get { return isSliding; } }
    public bool IsGrounded { get { return isGrounded; } }
    [SerializeField]
    Vector3 acceleration;
    [SerializeField]
    float scalar = 1f;
    [SerializeField]
    float walkSpeed = 4f;
    [SerializeField]
    float jumpSpeed = 14f;
    [SerializeField]
    float jumpButtonTime = 0.1f;
    [SerializeField]
    float slideSpeed = 5f;
    [SerializeField]
    float slideTime = 0.5f;
    [SerializeField]
    float flapSpeed = 10f;
    [SerializeField]
    float terminalVelocity = 20f;
    [SerializeField]
    bool startGrounded = true;

    public AnimationCurve walkSpeedMultiplier = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    public AnimationCurve jumpSpeedMultiplier = new AnimationCurve(new Keyframe(0f, 0.01f), new Keyframe(1f, 0.066f));
    public bool isGrounded;
    public bool canWalk = true;
    public bool isSliding;
    public bool CanSlide { get { return thisActor.Health >= 2; } }
    public bool CanFlap { get { return thisActor.HasWings; } }

    Vector3 walkVelocity;
    ActorEntity thisActor;
    CharacterController2D thisController;
    InputAgent thisInputAgent;

    void Awake()
    {
        thisActor = GetComponent<ActorEntity>();
        if( thisActor == null )
            Debug.LogWarning("NavAgent: Missing ActorEntity");

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
        acceleration = gravity * Time.deltaTime;
        AddVelocity(acceleration);
        SetVelocityX(Mathf.Clamp(velocity.x, -terminalVelocity, terminalVelocity));
        SetVelocityY(Mathf.Clamp(velocity.y, -terminalVelocity, terminalVelocity));

        CharacterController2D.CollisionState collisionState = thisController.Move((velocity + walkVelocity) * Time.deltaTime * scalar);

        if( collisionState.CollideRight || collisionState.CollideLeft )
            SetVelocityX(0f);
        if( collisionState.CollideBelow || collisionState.CollideAbove )
            SetVelocityY(0f);

        isGrounded = collisionState.IsGrounded;
    }

    public void ModifyWalkSpeed(float scale)
    {
        walkSpeed *= scale;
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
        if( canWalk )
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

    public void Slide()
    {
        StopCoroutine("SlideRoutine");
        StartCoroutine("SlideRoutine");
    }

    IEnumerator SlideRoutine()
    {
        float time = 0f;

        canWalk = false;
        isSliding = true;
        float sign = 1f;
        if( !thisActor.FacingRight )
            sign = -1f;
        walkVelocity = Vector3.right * sign * slideSpeed;

        BroadcastMessage("OnSlideStart", SendMessageOptions.DontRequireReceiver);

        while( time < slideTime )
        {
            time += Time.deltaTime;
            yield return null;
        }

        BroadcastMessage("OnSlideStop", SendMessageOptions.DontRequireReceiver);

        canWalk = true;
        isSliding = false;
        walkVelocity = Vector3.zero;
    }

    public void Flap()
    {
        StopCoroutine("FlapRoutine");
        StartCoroutine("FlapRoutine");
    }
    
    IEnumerator FlapRoutine()
    {
        float time = 0f;

        SetVelocityY(0f);
        AddVelocity(Vector3.up * flapSpeed * 0.5f);
        BroadcastMessage("OnFlap", SendMessageOptions.DontRequireReceiver);
        
        yield return null;
        
        while( time < jumpButtonTime )
        {
            if( thisInputAgent.JumpDown && !thisController.State.CollideAbove )
                AddVelocity(Vector3.up * jumpSpeedMultiplier.Evaluate(time / jumpButtonTime) * flapSpeed);
            
            time += Time.deltaTime;
            
            yield return null;
        }
    }
}
