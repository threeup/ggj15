using UnityEngine;
using System.Collections;

public class ItemBlock : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform spawnPoint;
    public int numOfHits;
    public GameObject normalState;
    public GameObject emptyState;

    int hitCount;

    void OnCollide(CharacterController2D.CollisionState collisionState)
    {
        if( collisionState.CollideAbove )
        {
            if( hitCount < numOfHits )
            {
                GameObject go = GameObject.Instantiate(itemPrefab) as GameObject;
                go.transform.position = spawnPoint.position;
                SpriteRenderer renderer = go.GetComponentInChildren<SpriteRenderer>();
                if( renderer != null )
                {
                    renderer.enabled = true;
                }

                ++hitCount;

                if( hitCount == numOfHits )
                {
                    normalState.SetActive(false);
                    emptyState.SetActive(true);
                }
            }
        }
    }
}
