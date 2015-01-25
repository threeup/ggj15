using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public AudioClip jumpSound;
    public AudioClip hurtSound;
    public AudioClip deathSound;

    void OnJump()
    {
        audio.PlayOneShot(jumpSound);
    }

    void OnDamage()
    {
        audio.PlayOneShot(hurtSound);
    }

    void OnDeath()
    {
        StartCoroutine("DeathRoutine");
    }
    
    IEnumerator DeathRoutine()
    {
        // GameOver
        audio.PlayOneShot(deathSound);
        GameObject.Destroy(gameObject);
        yield break;
    }
}
