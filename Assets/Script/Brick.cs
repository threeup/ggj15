using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
    public AudioClip breakSound;
    public GameObject destroyedObject;

    SpriteRenderer thisRenderer;
    Collider2D thisCollider;

    void Awake()
    {
        thisRenderer = GetComponent<SpriteRenderer>();
        thisCollider = GetComponent<Collider2D>();
    }

    public void Activate()
    {
        GameEntity blockEntity = this.gameObject.GetComponent<GameEntity>();
        if( blockEntity.edata.collisionState == CollisionState.DISABL )
        {
            GameObject.Destroy(gameObject);
        }
    }

    void OnCollide(CharacterController2D.CollisionState collisionState)
    {
        NavAgent otherNavAgent = collisionState.gameObject.GetComponent<NavAgent>();
        if( otherNavAgent != null )
        {
            if( collisionState.CollideAbove || (otherNavAgent.IsSliding && (collisionState.CollideRight || collisionState.CollideLeft)) )
            {
                GameEntity blockEntity = this.gameObject.GetComponent<GameEntity>();
                blockEntity.edata.collisionState = CollisionState.DISABL;

                thisRenderer.enabled = false;
                thisCollider.enabled = false;
                destroyedObject.SetActive(true);
                audio.PlayOneShot(breakSound);

                Rigidbody2D[] rigidbodies = destroyedObject.GetComponentsInChildren<Rigidbody2D>();
                for( int i = 0; i < rigidbodies.Length; ++i )
                {
                    rigidbodies[i].gravityScale = 0.75f;
                    rigidbodies[i].AddForce(new Vector2(Random.Range(-100f, 100f), Random.Range(50f, 100f)));
                }
                
                StartCoroutine("CollideRoutine");
            }
        }
    }

    IEnumerator CollideRoutine()
    {
        yield return new WaitForSeconds(2f);
        
        GameObject.Destroy(gameObject);
    }
}
