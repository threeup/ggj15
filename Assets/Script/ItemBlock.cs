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


                ++hitCount;

                if( hitCount == numOfHits )
                {
                    spriter.SwitchState(spriter.secondarySet, false);
                }
            }
        }
    }
}
