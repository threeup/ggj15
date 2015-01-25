using UnityEngine;
using System.Collections;

public class InvincibleOnDamage : MonoBehaviour
{
    [SerializeField]
    float invincibleTime;

    ActorEntity thisActor;
    SpriteRenderer thisRenderer;
    float time;

    void Awake()
    {
        thisActor = GetComponent<ActorEntity>();
        if( thisActor == null )
            Debug.LogWarning("InvincibleOnDamage: Missing ActorEntity");

        thisRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void OnDamage()
    {
        if( thisActor.health > 0 )
            StartCoroutine("InvincibleRoutine");
    }

    IEnumerator InvincibleRoutine()
    {
        time = 0f;
        thisActor.invincible = true;

        while( time < invincibleTime )
        {
            time += Time.deltaTime;

            if( thisRenderer != null )
            {
                float dec = Utilities.Decimal(time);
                if( dec < 0.25f || (dec > 0.5f && dec < 0.75f) )
                    thisRenderer.enabled = false;
                else
                    thisRenderer.enabled = true;
            }

            yield return null;
        }

        thisActor.invincible = false;
        if( thisRenderer != null )
        {
            thisRenderer.enabled = true;
        }
    }
}
