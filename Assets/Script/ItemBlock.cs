using UnityEngine;
using System.Collections;

public class ItemBlock : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform spawnPoint;
    public AudioClip spawnSound;
    public int numOfHits;
    public SpriteSequencer spriter;

    int hitCount;

    public void Activate()
    {
        GameEntity blockEntity = this.gameObject.GetComponent<GameEntity>();
        if( blockEntity.edata.collisionState == CollisionState.DISABL )
        {
            hitCount = numOfHits;
            spriter.SwitchState(spriter.disabledSet, false);
        }
    }

    void OnCollide(CharacterController2D.CollisionState collisionState)
    {
        NavAgent otherNavAgent = collisionState.gameObject.GetComponent<NavAgent>();
        if( otherNavAgent != null )
        {
            if( collisionState.CollideAbove || (otherNavAgent.IsSliding && (collisionState.CollideRight || collisionState.CollideLeft)) )
            {
                if( hitCount < numOfHits )
                {
                    GameObject go = GameObject.Instantiate(itemPrefab) as GameObject;
                    go.transform.position = spawnPoint.position;


                    Rigidbody2D rigidbody = go.GetComponentInChildren<Rigidbody2D>();
                    if( rigidbody != null )
                    {
                        rigidbody.gravityScale = 0.5f;
                        rigidbody.AddForce(new Vector2(Random.Range(-150f, 150f), Random.Range(100f, 200f)));
                    }
                    go.collider2D.isTrigger = false;

                    audio.PlayOneShot(spawnSound);

                    ++hitCount;

                    if( hitCount == numOfHits )
                    {
                        spriter.SwitchState(spriter.secondarySet, false);
                        GameEntity blockEntity = this.gameObject.GetComponent<GameEntity>();
                        GameEntity otherEntity = collisionState.gameObject.GetComponent<GameEntity>();
                        blockEntity.edata.collisionState = CollisionState.DISABL;
                        blockEntity.edata.hitByHuman = otherEntity.edata.entityType == EntityType.MAINCHARACTER;
                    }
                }
            }
        }
    }
}
