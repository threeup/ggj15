using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public AudioClip jumpSound;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    public AudioClip flapSound;

    void OnJump()
    {
        audio.PlayOneShot(jumpSound);
    }

    void OnFlap()
    {
        audio.PlayOneShot(flapSound);
    }

    void OnDamage()
    {
        audio.PlayOneShot(hurtSound);
    }

    void OnDeath()
    {
        audio.PlayOneShot(deathSound);
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        if( renderer != null )
            renderer.enabled = false;
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        for( int i = 0; i < colliders.Length; ++i )
        {
            colliders[i].enabled = false;
        }

        StartCoroutine("DeathRoutine");
    }
    
    IEnumerator DeathRoutine()
    {
        // GameOver
        yield return new WaitForSeconds(2f);
        GameEntity ge = GetComponent<GameEntity>();
        Director.Instance.ActorDied(ge);
        LevelManager.Instance.Trash(gameObject);
        
    }
}
