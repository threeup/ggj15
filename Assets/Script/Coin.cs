using UnityEngine;
using System.Collections;

public class Coin : Item
{
    public AudioClip pickupSound;

    public override void OnEnter(Collider2D other)
    {
        ActorEntity actor = other.GetComponent<ActorEntity>();
        if( actor != null )
        {
            Director.Instance.AddCoin(actor, 1);
            audio.PlayOneShot(pickupSound);
            SpriteSequencer sequencer = GetComponentInChildren<SpriteSequencer>();
            if( sequencer != null )
                sequencer.mainRenderer.enabled = false;
            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
            for( int i = 0; i < colliders.Length; ++i )
            {
                colliders[i].enabled = false;
            }
            bool hitByHuman = actor.edata.entityType == EntityType.MAINCHARACTER;
            Debug.Log("hit by hitByHuman"+hitByHuman);
            StartCoroutine(EnterRoutine(hitByHuman));
        }
    }

    IEnumerator EnterRoutine(bool hitByHuman)
    {
        yield return new WaitForSeconds(1f);

        LevelManager.Instance.Trash(this.gameObject, hitByHuman);
    }

    public void HandleProjectile(Vector3 dir)
	{
		if( this.rigidbody2D != null )
        {
        	dir.x = -dir.x*0.1f;
        	dir.y = -Mathf.Abs(dir.y);
            rigidbody2D.AddForce(dir*400f);
            collider2D.isTrigger = false;
        }
	}
}
