using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    public enum CollisionFlags
    {
        None = 0,
        Right = 1,
        Left = 2,
        Below = 4,
        Above = 8,
        WasGrounded = 16,
    }

    const int NumLayers = 32;

    [Range(0.001f, 0.3f)]
    [SerializeField]
    float skin = 0.02f;
    [SerializeField]
    LayerMask platformMask;
    [SerializeField]
    LayerMask oneWayPlatformMask;
    [SerializeField]
    LayerMask triggerMask;
    [Range(0f, 90f)]
    [SerializeField]
    float slopeLimit = 30f;
    public AnimationCurve slopeSpeedMultiplier = new AnimationCurve(new Keyframe(-90, 1.5f), new Keyframe(0, 1), new Keyframe(90, 0));

    public event Action<RaycastHit2D> onControllerCollidedEvent;
    public event Action<Collider2D> onTriggerEnterEvent;
    public event Action<Collider2D> onTriggerStayEvent;
    public event Action<Collider2D> onTriggerExitEvent;

    GameObject thisGameObject;
    Transform thisTransform;
    BoxCollider2D thisCollider;
    //Rigidbody2D thisRigidbody;
    int collisionFlags;

    public void ResetFlags()
    {
        collisionFlags = 0;
    }

    bool CheckFlag(CollisionFlags flag)
    {
        int f = (int)flag;
        return ((collisionFlags & f) == f);
    }

    void SetFlag(CollisionFlags flag)
    {
        int f = (int)flag;
        collisionFlags |= f;
    }

    void ClearFlag(CollisionFlags flag)
    {
        int f = (int)flag;
        collisionFlags &= ~f;
    }

    public bool CollideRight { get { return CheckFlag(CollisionFlags.Right); } }
    public bool CollideLeft { get { return CheckFlag(CollisionFlags.Left); } }
    public bool CollideBelow { get { return CheckFlag(CollisionFlags.Below); } }
    public bool CollideAbove { get { return CheckFlag(CollisionFlags.Above); } }
    public bool WasGrounded { get { return CheckFlag(CollisionFlags.WasGrounded); } }
    public bool BecameGrounded { get { return !WasGrounded && IsGrounded; } }
    public bool IsGrounded { get { return CollideBelow; } }

    void Awake()
    {
        thisGameObject = gameObject;
        thisTransform = transform;
        thisCollider = (BoxCollider2D)collider2D;
        //thisRigidbody = rigidbody2D;

        platformMask |= oneWayPlatformMask;

        // Have CC2D ignore all layers except for triggers
        for( int i = 0; i < NumLayers; ++i )
        {
            if( (triggerMask.value & 1 << i) == 0 )
                Physics2D.IgnoreLayerCollision(thisGameObject.layer, i);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if( onTriggerEnterEvent != null )
            onTriggerEnterEvent(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if( onTriggerStayEvent != null )
            onTriggerStayEvent(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if( onTriggerExitEvent != null )
            onTriggerExitEvent(other);
    }

    public void Move(Vector3 delta)
    {
        bool wasGrounded = CollideBelow;

        ResetFlags();

        if( wasGrounded )
            SetFlag(CollisionFlags.WasGrounded);

        Vector2 origin2D = Utilities.Vector3ToVector2(thisTransform.position);

        if( delta.y < 0f && WasGrounded )
            HandleVerticalSlope(ref delta, ref origin2D);

        // Horizontal component
        if( delta.x != 0f )
            HorizontalMove(ref delta, ref origin2D);

        // Vertical component
        if( delta.y != 0f )
            VerticalMove(ref delta, ref origin2D);

        thisTransform.Translate(delta, Space.World);
    }

    void HorizontalMove(ref Vector3 delta, ref Vector2 origin2D)
    {
        bool goingRight = delta.x > 0f;
        LayerMask mask = platformMask;
        mask &= ~oneWayPlatformMask;
        RaycastHit2D hit = Physics2D.BoxCast(origin2D, thisCollider.size, 0f, new Vector2(delta.normalized.x, 0), Mathf.Abs(delta.x), mask.value);
        if( hit )
        {
            if( !HandleHorizontalSlope(ref delta, Vector2.Angle(hit.normal, Vector2.up)) )
            {
                if( goingRight )
                {
                    delta.x = (hit.point.x - thisCollider.size.x * 0.5f - skin) - origin2D.x;
                    SetFlag(CollisionFlags.Right);
                }
                else
                {
                    delta.x = (hit.point.x + thisCollider.size.x * 0.5f + skin) - origin2D.x;
                    SetFlag(CollisionFlags.Left);
                }
            }
        }
    }

    bool HandleHorizontalSlope(ref Vector3 delta, float angle)
    {
        // Ignore walls
        if( Mathf.RoundToInt(angle) == 90 )
            return false;

        if( angle < slopeLimit )
        {
            if( delta.y < 0f )
            {
                float slopeMultiplier = slopeSpeedMultiplier.Evaluate(angle);
                delta.x *= slopeMultiplier;
                delta.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * delta.x);
                SetFlag(CollisionFlags.Below);
            }
        }
        else
            delta.x = 0f;

        return true;
    }

    void VerticalMove(ref Vector3 delta, ref Vector2 origin2D)
    {
        bool goingUp = delta.y > 0f;
        LayerMask mask = platformMask;
        if( goingUp )
            mask &= ~oneWayPlatformMask;

        RaycastHit2D hit = Physics2D.BoxCast(origin2D, thisCollider.size, 0f, new Vector2(0, delta.normalized.y), Mathf.Abs(delta.y), mask.value);
        if( hit )
        {
            if( goingUp )
            {
                delta.y = (hit.point.y - thisCollider.size.y * 0.5f - skin) - origin2D.y;
                SetFlag(CollisionFlags.Above);
            }
            else
            {
                delta.y = (hit.point.y + thisCollider.size.y * 0.5f + skin) - origin2D.y;
                SetFlag(CollisionFlags.Below);
            }
        }
    }

    void HandleVerticalSlope(ref Vector3 delta, ref Vector2 origin2D)
    {
        RaycastHit2D hit = Physics2D.BoxCast(origin2D, thisCollider.size, 0f, new Vector2(0, delta.normalized.y), delta.y, platformMask.value);
        if( hit )
        {
            float angle = Vector2.Angle(hit.normal, Vector2.up);
            if( angle == 0 )
                return;

            bool isMovingDownSlope = Mathf.Sign(hit.normal.x) == Mathf.Sign(delta.x);
            if( isMovingDownSlope )
            {
                float slopeMultiplier = slopeSpeedMultiplier.Evaluate(-angle);
                delta.y = (hit.point.y + thisCollider.size.y * 0.5f + skin) - origin2D.y;
                delta.x *= slopeMultiplier;
            }
        }
    }

    public void WarpToGrounded()
    {
        do
        {
            Move(Vector3.down);
        } while( !IsGrounded );
    }
}
