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
        if( thisActor.Health > 0 )
            StartCoroutine("InvincibleRoutine");
    }

    IEnumerator InvincibleRoutine()
    {
        time = 0f;
        thisActor.invincible = true;

        while( time < invincibleTime )
        {
            time += Time.deltaTime;

            float dec = Utilities.Decimal(time);
            if( dec < 0.25f || (dec > 0.5f && dec < 0.75f) )
                thisActor.SetShouldRender(false);
            else
                thisActor.SetShouldRender(true);
        
            yield return null;
        }

        thisActor.invincible = false;
        thisActor.SetShouldRender(true);
    }
}
