using UnityEngine;
using System.Collections;

public class Goal : Item
{
    public AudioClip pickupSound;

    public override void OnEnter(Collider2D other)
    {
        ActorEntity actor = other.GetComponent<ActorEntity>();
        if( actor != null )
        {
            audio.PlayOneShot(pickupSound);
            Director.Instance.ActorHitGoal(actor);
            this.collider2D.enabled = false;
        }
    }

}
