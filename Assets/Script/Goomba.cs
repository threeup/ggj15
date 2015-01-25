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
                thisActor.Damage();
                otherNavAgent.SetVelocityY(0f);
                otherNavAgent.Jump();
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

        transform.localScale = new Vector3(1f, 0.1f);

        while( time < 1f )
        {
            time += Time.deltaTime;
            yield return null;
        }

        LevelManager.Instance.Trash(gameObject);
    }
}
