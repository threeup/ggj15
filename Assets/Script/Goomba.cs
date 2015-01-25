using UnityEngine;
using System.Collections;

public class Goomba : MonoBehaviour
{
    void OnDeath()
    {
        StartCoroutine("DeathRoutine");
    }

    IEnumerator DeathRoutine()
    {
        float time = 0f;

        transform.localScale = new Vector3(1f, 0.1f);

        while( time < 1f )
        {
            time += Time.deltaTime;
            yield return null;
        }

        GameObject.Destroy(gameObject);
    }
}
