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
    public SpriteSequencer[] bodySpriters;
    public SpriteSequencer wingFrontSpriter;
    public SpriteSequencer wingBackSpriter;
    public SpriteSequencer slideSpriter;

    public bool FacingRight { get { return activeSpriter.baseScale.x > 0f; } }
    public bool HasShell { get { return Health >= 2; } }
    public bool HasWings { get { return Health >= 3; } }
	
	public override void Awake()
	{
		base.Awake();
        thisNavAgent = GetComponent<NavAgent>();
		entityName = "ACTOR-"+this.gameObject.name+"_"+this.GetHashCode();
        if( bodySpriters.Length > 1 )
        {
            for(int i=1; i<bodySpriters.Length; ++i)
            {
                SpriteSequencer spriter = bodySpriters[i];
                spriter.mainRenderer.enabled = false;
            }
        }
        activeSpriter = bodySpriters[0];
        activeSpriter.mainRenderer.enabled = true;
        if( slideSpriter != null )
        {
            slideSpriter.mainRenderer.enabled = false;
        }
        if( edata.entityType == EntityType.ENEMY )
        {
            maxHealth = 1;
            SetHealth(maxHealth);
        }
        else if( edata.entityType == EntityType.MAINCHARACTER)
        {
            maxHealth = Director.Instance.MainStartHealth;
            SetHealth(maxHealth);
        }
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
        activeSpriter = bodySpriters[Mathf.Clamp(health-1, 0, bodySpriters.Length-1)];
        activeSpriter.mainRenderer.enabled = true;
    }

    void OnDeath()
    {
        collider2D.enabled = false;
    }

    public void Update()
    {
		activeSpriter.SetFacingRight(thisNavAgent.WalkVelocity.x);
        bool sliding = thisNavAgent.IsSliding;
        if( slideSpriter != null )
        {
            activeSpriter.mainRenderer.enabled = !sliding;   
            slideSpriter.mainRenderer.enabled = sliding;
        } 
        if( wingFrontSpriter != null )
        {
            wingFrontSpriter.SetFacingRight(thisNavAgent.WalkVelocity.x);
            wingBackSpriter.SetFacingRight(thisNavAgent.WalkVelocity.x);
        }

        bool flying = !thisNavAgent.isGrounded && !sliding;
        if( HasWings )
        {
            if( flying )
            {
                wingFrontSpriter.SwitchState(wingFrontSpriter.secondarySet, true);
                wingBackSpriter.SwitchState(wingBackSpriter.secondarySet, true);
            }
            else
            {
                wingFrontSpriter.SwitchState(wingFrontSpriter.idleSet, true);
                wingBackSpriter.SwitchState(wingBackSpriter.idleSet, true);
            }
        }
        else
        {
            if( wingFrontSpriter != null )
            {
                wingFrontSpriter.SwitchState(wingFrontSpriter.disabledSet, true);
                wingBackSpriter.SwitchState(wingBackSpriter.disabledSet, true);
            }
        }
        if( flying )
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
