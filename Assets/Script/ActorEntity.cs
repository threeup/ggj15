using UnityEngine;
using System.Collections;

public class ActorEntity : GameEntity 
{
    public enum Faction
    {
        None,
        Neutral,
        Player,
        Enemy,
    }

    public Faction faction;
    private int health = 0;
    private int maxHealth = 0;
    public int Health { get { return health; } }
    public bool invincible;

    public NavAgent thisNavAgent;

    private SpriteSequencer activeSpriter;
    public SpriteSequencer[] spriters;
	
	public override void Awake()
	{
		base.Awake();
        thisNavAgent = GetComponent<NavAgent>();
		entityName = "ACTOR-"+this.gameObject.name+"_"+this.GetHashCode();
        spriters = GetComponentsInChildren<SpriteSequencer>();
        if( spriters.Length > 1 )
        {
            for(int i=1; i<spriters.Length; ++i)
            {
                SpriteSequencer spriter = spriters[i];
                spriter.mainRenderer.enabled = false;
            }
        }
        activeSpriter = spriters[0];
        activeSpriter.mainRenderer.enabled = true;
        maxHealth = Director.Instance.MainStartHealth;
        SetHealth(maxHealth);
		this.name = entityName;
	}

    public bool IsAlly(ActorEntity other)
    {
        if( gameObject == other )
            return true;

        if( other.faction == Faction.Neutral )
            return false;

        return faction == other.faction;
    }

    public bool IsEnemy(ActorEntity other)
    {
        if( gameObject == other )
            return false;
        
        if( other.faction == Faction.Neutral )
            return false;
        
        return faction != other.faction;
    }

    public void Damage()
    {
        if( invincible )
            return;

        SetHealth(health - 1);

       

        BroadcastMessage("OnDamage", SendMessageOptions.DontRequireReceiver);

        if( health == 0 )
            BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
    }

    public void SetHealth(int newHealth)
    {
        if( newHealth < 0 )
            newHealth = 0;

        if( newHealth == health )
            return;
        
        Debug.Log("ActorEntity: Damage. Health: " + (health) + " -> " + newHealth);

        health = newHealth;
        activeSpriter.mainRenderer.enabled = false;
        activeSpriter = spriters[Mathf.Clamp(health-1, 0, spriters.Length-1)];
        activeSpriter.mainRenderer.enabled = true;
    }

    void OnDeath()
    {
        collider2D.enabled = false;
    }

    public void Update()
    {
		activeSpriter.SetFacingRight(thisNavAgent.WalkVelocity.x);
        if( !thisNavAgent.isGrounded )
        {
            activeSpriter.SwitchState(activeSpriter.secondarySet, true);
        }
        else if ( Mathf.Abs(thisNavAgent.WalkVelocity.x)  < 0.1f )
        {
            activeSpriter.SwitchState(activeSpriter.idleSet, true);
        }
        else if( health == 0 )
        {
            activeSpriter.SwitchState(activeSpriter.disabledSet, true);
        }
        else
        {
            activeSpriter.SwitchState(activeSpriter.primarySet, true);
        }
    }
}
