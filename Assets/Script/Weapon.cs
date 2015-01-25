using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    private float launchSpeed = 200f;
    public GameObject launchNode;
    public GameObject projectilePrefab;
    public ActorEntity owner;

    void Awake()
    {
        owner = GetComponent<ActorEntity>();
    }


    public void Fire(float moveX)
    {
        Vector2 launchVector = Vector2.zero;
        launchVector.x = moveX;
        launchVector.y = 1f;
        GameObject go = GameObject.Instantiate(projectilePrefab, launchNode.transform.position, Quaternion.identity) as GameObject;
        WeaponProjectile projectile = go.GetComponent<WeaponProjectile>();
        projectile.owner = this.owner;

        Rigidbody2D rigidbody = go.GetComponentInChildren<Rigidbody2D>();
        if( rigidbody != null )
        {
            rigidbody.AddForce(launchVector*launchSpeed);
        }
    }

}
