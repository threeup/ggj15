using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    void OnDeath()
    {
        StartCoroutine("DeathRoutine");
    }
    
    IEnumerator DeathRoutine()
    {
        // GameOver
        GameObject.Destroy(gameObject);
        yield break;
    }
}
