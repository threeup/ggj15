using UnityEngine;
using System.Collections;

public class ItemBlock : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform spawnPoint;
    public int numOfHits;
    public SpriteSequencer spriter;

    int hitCount;

    void OnCollide(CharacterController2D.CollisionState collisionState)
    {
        if( collisionState.CollideAbove )
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

                ++hitCount;

                if( hitCount == numOfHits )
                {
                    spriter.SwitchState(spriter.secondarySet, false);
                }
            }
        }
    }
}
