using UnityEngine;
using System.Collections;

public class Coin : Item
{
    public override void OnEnter(Collider2D other)
    {
        ActorEntity actor = other.GetComponent<ActorEntity>();
        if( actor != null )
        {
            GameObject.Destroy(transform.parent.gameObject);
        }
    }
}
