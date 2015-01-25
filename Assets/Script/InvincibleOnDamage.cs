using UnityEngine;
using System.Collections;

public class InvincibleOnDamage : MonoBehaviour
{
    [SerializeField]
    float invincibleTime;

    ActorEntity thisActor;
    float time;

    void Awake()
    {
        thisActor = GetComponent<ActorEntity>();
        if( thisActor == null )
            Debug.LogWarning("InvincibleOnDamage: Missing ActorEntity");
    }

    void OnDamage()
    {
        StartCoroutine("InvincibleRoutine");
    }

    IEnumerator InvincibleRoutine()
    {
        time = 0f;
        thisActor.invincible = true;

        while( time < invincibleTime )
        {
            time += Time.deltaTime;
            yield return null;
        }

        thisActor.invincible = false;
    }
}
