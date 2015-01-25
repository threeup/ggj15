using UnityEngine;
using System.Collections;

public class WeaponProjectile : MonoBehaviour
{
    public ActorEntity owner;
    public SpriteSequencer spriter;
    public BasicTimer ttlTimer = new BasicTimer(5f);
    private bool canCollide = true;

    void Awake()
    {

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if( !canCollide || collider.gameObject == owner.gameObject )
        {
            return;
        }

        canCollide = false;
        Vector3 direction = (collider.transform.position - this.transform.position).normalized;
        collider.SendMessage("HandleProjectile", direction, SendMessageOptions.DontRequireReceiver);
        spriter.SwitchState(spriter.secondarySet, false);
        rigidbody2D.velocity = Vector3.zero;
        collider2D.enabled = false;
        ttlTimer.SetMin(0.5f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if( !canCollide )
        {
            return;
        }
        canCollide = false;
        Vector3 direction = (collision.collider.transform.position - this.transform.position).normalized;
        collision.collider.SendMessage("HandleProjectile", direction, SendMessageOptions.DontRequireReceiver);
        spriter.SwitchState(spriter.secondarySet, false);
        rigidbody2D.velocity = Vector3.zero;
        collider2D.enabled = false;
        ttlTimer.SetMin(0.5f);
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        if( ttlTimer.Tick(deltaTime) )
        {
            Destroy(this.gameObject);
        }
    }

}
