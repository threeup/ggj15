using UnityEngine;
using System.Collections;

public class Goomba : MonoBehaviour
{
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
            NavAgent otherNavAgent = other.GetComponent<NavAgent>();
            BoxCollider2D box = (BoxCollider2D)other;

            if( box.bounds.min.y > thisTransform.position.y )
            {
                if( otherActor.IsEnemy(thisActor) )
                    thisActor.Damage();
                otherNavAgent.SetVelocityY(0f);
                otherNavAgent.Jump();
            }
            else if( otherNavAgent.isSliding )
            {
                thisActor.Damage();
            }
            else
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

    void OnDeath()
    {
        StartCoroutine("DeathRoutine");
    }

    IEnumerator DeathRoutine()
    {
        float time = 0f;

        while( time < 1f )
        {
            time += Time.deltaTime;
            yield return null;
        }

        LevelManager.Instance.Trash(gameObject, false);
    }
}
