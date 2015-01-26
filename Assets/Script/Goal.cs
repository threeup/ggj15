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
            bool success = Director.Instance.ActorHitGoal(actor);
            if( success )
            {
                audio.PlayOneShot(pickupSound);
                this.collider2D.enabled = false;
            }
            Debug.Log("Goal Hit" +success);
        }
    }

}
