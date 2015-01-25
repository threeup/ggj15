using UnityEngine;
using System.Collections;

public class DamageOnCollide : MonoBehaviour
{
    public bool right = true;
    public bool left = true;
    public bool above = true;
    public bool below = true;

    ActorEntity thisActor;
    Transform thisTransform;

    void Awake()
    {
        thisActor = GetComponent<ActorEntity>();
        thisTransform = transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ActorEntity otherActor = other.GetComponent<ActorEntity>();
        if( otherActor != null )
        {
            bool doDamage = false;
            BoxCollider2D box = (BoxCollider2D)other;
            Vector2 otherOrigin2D = Utilities.Vector3ToVector2(box.transform.position);
            Vector3 otherExtents = box.bounds.extents;

            Vector2[] rightPoints = new Vector2[] { otherOrigin2D + Vector2.right * otherExtents.x,
                otherOrigin2D + Vector2.right * otherExtents.x + Vector2.up * otherExtents.y,
                otherOrigin2D + Vector2.right * otherExtents.x + Vector2.up * -otherExtents.y, };
            Vector2[] leftPoints = new Vector2[] { otherOrigin2D + Vector2.right * -otherExtents.x,
                otherOrigin2D + Vector2.right * -otherExtents.x + Vector2.up * otherExtents.y,
                otherOrigin2D + Vector2.right * -otherExtents.x + Vector2.up * -otherExtents.y, };
            Vector2[] abovePoints = new Vector2[] { otherOrigin2D + Vector2.up * otherExtents.y,
                otherOrigin2D + Vector2.right * otherExtents.x + Vector2.up * otherExtents.y,
                otherOrigin2D + Vector2.right * -otherExtents.x + Vector2.up * otherExtents.y, };
            Vector2[] belowPoints = new Vector2[] { otherOrigin2D + Vector2.up * -otherExtents.y,
                otherOrigin2D + Vector2.right * otherExtents.x + Vector2.up * -otherExtents.y,
                otherOrigin2D + Vector2.right * -otherExtents.x + Vector2.up * -otherExtents.y, };

            int numRight = 0;
            int numLeft = 0;
            int numAbove = 0;
            int numBelow = 0;

            if( thisTransform.position.x < otherOrigin2D.x - otherExtents.x )
            {
                for( int i = 0; i < rightPoints.Length; ++i )
                {
                    if( box.OverlapPoint(rightPoints[i]) )
                        ++numRight;
                }
            }

            if( thisTransform.position.x > otherOrigin2D.x + otherExtents.x )
            {
                for( int i = 0; i < leftPoints.Length; ++i )
                {
                    if( box.OverlapPoint(leftPoints[i]) )
                        ++numLeft;
                }
            }

            if( thisTransform.position.y < otherOrigin2D.y - otherExtents.y )
            {
                for( int i = 0; i < abovePoints.Length; ++i )
                {
                    if( box.OverlapPoint(abovePoints[i]) )
                        ++numAbove;
                }
            }

            if( thisTransform.position.y > otherOrigin2D.y + otherExtents.y )
            {
                for( int i = 0; i < belowPoints.Length; ++i )
                {
                    if( box.OverlapPoint(belowPoints[i]) )
                        ++numBelow;
                }
            }

            if( right && numRight >= numAbove && numRight >= numBelow )
                doDamage = true;
            else if( left && numLeft >= numAbove && numLeft >= numBelow )
                doDamage = true;
            else if( above && numAbove >= numRight && numAbove >= numLeft )
                doDamage = true;
            else if( below && numBelow >= numRight && numBelow >= numLeft )
                doDamage = true;

            if( doDamage )
            {
                if( thisActor != null )
                {
                    if( thisActor.IsEnemy(otherActor) )
                        otherActor.Damage();
                }
                else
                {
                    otherActor.Damage();
                }
            }
        }
    }
}
