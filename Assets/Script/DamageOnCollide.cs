using UnityEngine;
using System.Collections;

public class DamageOnCollide : MonoBehaviour
{
    ActorEntity thisActor;

    void Awake()
    {
        thisActor = GetComponentInParent<ActorEntity>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ActorEntity otherActor = other.GetComponent<ActorEntity>();
        if( otherActor != null )
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
