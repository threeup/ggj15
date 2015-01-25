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
            StartCoroutine("EnterRoutine");
        }
    }

    IEnumerator EnterRoutine()
    {
        audio.PlayOneShot(pickupSound);
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        if( renderer != null )
            renderer.enabled = false;
        collider2D.enabled = false;

        yield return new WaitForSeconds(1f);

        GameObject.Destroy(transform.parent.gameObject);
    }

    public void HandleProjectile(Vector3 dir)
	{
		if( this.rigidbody2D != null )
        {
        	Debug.Log("dir"+dir);
        	dir.x = -dir.x*0.1f;
        	dir.y = -Mathf.Abs(dir.y);
            rigidbody2D.AddForce(dir*400f);
        }
	}
}
