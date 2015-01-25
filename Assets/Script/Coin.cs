using UnityEngine;
using System.Collections;

public class Coin : Item
{
    public override void OnEnter(Collider2D other)
    {
        GameObject.Destroy(transform.parent.gameObject);
    }
}
