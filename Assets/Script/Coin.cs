using UnityEngine;
using System.Collections;

public class Coin : Item
{
    public override void OnEnter(Collider2D other)
    {
        ActorEntity actor = other.GetComponent<ActorEntity>();
        if( actor != null )
        {
        	Director.Instance.AddCoin(actor, 1);
            GameObject.Destroy(this.gameObject);
        }
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
