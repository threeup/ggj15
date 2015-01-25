using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        OnEnter(other);
    }

    public virtual void OnEnter(Collider2D other) { ; }
}
