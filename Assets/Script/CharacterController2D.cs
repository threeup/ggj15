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

    public class CollisionState
    {
        public GameObject gameObject;
        public List<RaycastHit2D> collisions = new List<RaycastHit2D>();

        int collisionFlags;

        public void Reset()
        {
            collisionFlags = 0;
            collisions.Clear();
        }
        
        public bool CheckFlag(CollisionFlags flag)
        {
            int f = (int)flag;
            return ((collisionFlags & f) == f);
        }
        
        public void SetFlag(CollisionFlags flag)
        {
            int f = (int)flag;
            collisionFlags |= f;
        }
        
        public void ClearFlag(CollisionFlags flag)
        {
            int f = (int)flag;
            collisionFlags &= ~f;
        }

        public bool CollideRight { get { return CheckFlag(CollisionFlags.Right); } }
        public bool CollideLeft { get { return CheckFlag(CollisionFlags.Left); } }
        public bool CollideBelow { get { return CheckFlag(CollisionFlags.Below); } }
        public bool CollideAbove { get { return CheckFlag(CollisionFlags.Above); } }
        public bool HasCollision { get { return collisions.Count > 0; } }
        public bool WasGrounded { get { return CheckFlag(CollisionFlags.WasGrounded); } }
        public bool IsGrounded { get { return CollideBelow; } }
        public bool BecameGrounded { get { return !WasGrounded && IsGrounded; } }
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

    GameObject thisGameObject;
    Transform thisTransform;
    BoxCollider2D thisCollider;
    //Rigidbody2D thisRigidbody;
    CollisionState collisionState = new CollisionState();


    void Awake()
    {
        thisGameObject = gameObject;
        thisTransform = transform;
        thisCollider = (BoxCollider2D)collider2D;
        //thisRigidbody = rigidbody2D;

        platformMask |= oneWayPlatformMask;

        collisionState.gameObject = thisGameObject;
    }

    public CollisionState Move(Vector3 delta)
    {
        bool wasGrounded = collisionState.CollideBelow;

        collisionState.Reset();

        if( wasGrounded )
            collisionState.SetFlag(CollisionFlags.WasGrounded);

        Vector2 origin2D = Utilities.Vector3ToVector2(thisTransform.position);

        if( delta.y < 0f && collisionState.WasGrounded )
            HandleVerticalSlope(ref delta, ref origin2D);

        if( delta.x != 0f )
            HorizontalMove(ref delta, ref origin2D);

        if( delta.y != 0f )
            VerticalMove(ref delta, ref origin2D);

        if( !collisionState.HasCollision && delta.x != 0f && delta.y != 0f )
            DiagonalMove(ref delta, ref origin2D);

        thisTransform.Translate(delta, Space.World);

        for( int i = 0; i < collisionState.collisions.Count; ++i )
        {
            collisionState.collisions[i].collider.SendMessage("OnCollide", collisionState, SendMessageOptions.DontRequireReceiver);
        }
        BroadcastMessage("OnCollide", collisionState, SendMessageOptions.DontRequireReceiver);

        return collisionState;
    }

    void HorizontalMove(ref Vector3 delta, ref Vector2 origin2D)
    {
        bool goingRight = delta.x > 0f;
        LayerMask mask = platformMask;
        mask &= ~oneWayPlatformMask;
        RaycastHit2D hit = Physics2D.BoxCast(origin2D, thisCollider.size, thisTransform.rotation.z, new Vector2(delta.normalized.x, 0), Mathf.Abs(delta.x), mask.value);
        if( hit )
        {
            if( !HandleHorizontalSlope(ref delta, Vector2.Angle(hit.normal, Vector2.up)) )
            {
                if( goingRight )
                {
                    delta.x = (hit.point.x - thisCollider.size.x * 0.5f - skin) - origin2D.x;
                    collisionState.SetFlag(CollisionFlags.Right);
                }
                else
                {
                    delta.x = (hit.point.x + thisCollider.size.x * 0.5f + skin) - origin2D.x;
                    collisionState.SetFlag(CollisionFlags.Left);
                }
            }
            collisionState.collisions.Add(hit);
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
                collisionState.SetFlag(CollisionFlags.Below);
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

        RaycastHit2D hit = Physics2D.BoxCast(origin2D, thisCollider.size, thisTransform.rotation.z, new Vector2(0, delta.normalized.y), Mathf.Abs(delta.y), mask.value);
        if( hit )
        {
            if( goingUp )
            {
                delta.y = (hit.point.y - thisCollider.size.y * 0.5f - skin) - origin2D.y;
                collisionState.SetFlag(CollisionFlags.Above);
            }
            else
            {
                delta.y = (hit.point.y + thisCollider.size.y * 0.5f + skin) - origin2D.y;
                collisionState.SetFlag(CollisionFlags.Below);
            }
            collisionState.collisions.Add(hit);
        }
    }

    void HandleVerticalSlope(ref Vector3 delta, ref Vector2 origin2D)
    {
        RaycastHit2D hit = Physics2D.BoxCast(origin2D, thisCollider.size, thisTransform.rotation.z, new Vector2(0, delta.normalized.y), delta.y, platformMask.value);
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

    void DiagonalMove(ref Vector3 delta, ref Vector2 origin2D)
    {
        LayerMask mask = platformMask;
        mask &= ~oneWayPlatformMask;

        RaycastHit2D hit = Physics2D.BoxCast(origin2D, thisCollider.size, thisTransform.rotation.z, delta.normalized, delta.magnitude, mask.value);
        if( hit )
        {
            if( hit.normal.x < 0f )
            {
                delta.x = (hit.point.x - thisCollider.size.x * 0.5f - skin) - origin2D.x;
                collisionState.SetFlag(CollisionFlags.Right);
            }
            else if( hit.normal.x > 0f )
            {
                delta.x = (hit.point.x + thisCollider.size.x * 0.5f + skin) - origin2D.x;
                collisionState.SetFlag(CollisionFlags.Left);
            }
            else if( hit.normal.y < 0f )
            {
                delta.y = (hit.point.y - thisCollider.size.y * 0.5f - skin) - origin2D.y;
                collisionState.SetFlag(CollisionFlags.Above);
            }
            else if( hit.normal.y > 0f )
            {
                delta.y = (hit.point.y + thisCollider.size.y * 0.5f + skin) - origin2D.y;
                collisionState.SetFlag(CollisionFlags.Below);
            }

            collisionState.collisions.Add(hit);
        }
    }

    public void WarpToGrounded()
    {
        do
        {
            Move(Vector3.down);
        } while( !collisionState.IsGrounded );
    }
}
