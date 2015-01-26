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
    private int maxHealth = 1;
    private int shellHealth = 2;
    private int wingsHealth = 2;
    public int Health { get { return health; } }
    public bool invincible;

    public NavAgent thisNavAgent;
    public Brain thisBrain;

    private SpriteSequencer activeSpriter;
    public SpriteSequencer[] bodySpriters;
    public SpriteSequencer wingFrontSpriter;
    public SpriteSequencer wingBackSpriter;
    public SpriteSequencer slideSpriter;
    private bool shouldRender = true;

    private bool isSetup = false;

    public bool FacingRight { get { return activeSpriter.baseScale.x > 0f; } }
    public bool HasShell { get { return Health >= shellHealth; } }
    public bool HasWings { get { return Health >= wingsHealth; } }
	
	public override void Awake()
	{
		base.Awake();
        thisNavAgent = GetComponent<NavAgent>();
        thisBrain = GetComponent<Brain>();
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
        //SetupParameters(1, 1f);
        
		this.name = entityName;
	}	

	public void SetupParameters(int health, float walkMultiplier)
	{
		if( isSetup )
		{
			return;
		}
        if( edata.entityType == EntityType.MAINCHARACTER)
        {
            wingsHealth = 3;
            health = Director.Instance.MainStartHealth;
        }
		SetMaxHealth(health);

		thisNavAgent.ModifyWalkSpeed(walkMultiplier);

		isSetup = true;
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

    public void SetMaxHealth(int newMax)
    {
        maxHealth = newMax;
        SetHealth(maxHealth);
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

        if( thisBrain != null )
        {
        	thisBrain.isMoveable = health > 0;
        	thisBrain.isJumpable = HasWings;
        }
    }

    void OnDeath()
    {
        collider2D.enabled = false;
    }

    public void Update()
    {
    	UpdateRender();

        if( this.transform.position.y < -10 )
        {
            Damage();
        }
    }

    public void SetShouldRender(bool isVis)
    {
    	shouldRender = isVis;
    }

    void UpdateRender()
    {
		activeSpriter.SetFacingRight(thisNavAgent.WalkVelocity.x);
        bool sliding = thisNavAgent.IsSliding;
        if( slideSpriter != null )
        {
            activeSpriter.mainRenderer.enabled = !sliding && shouldRender;   
            slideSpriter.mainRenderer.enabled = sliding && shouldRender;
        } 
        else
        {
        	activeSpriter.mainRenderer.enabled = shouldRender;
        }
        if( wingFrontSpriter != null )
        {
            wingFrontSpriter.SetFacingRight(thisNavAgent.WalkVelocity.x);
        }
        if( wingBackSpriter != null )
        {
            wingBackSpriter.SetFacingRight(thisNavAgent.WalkVelocity.x);
        }

        bool flying = !thisNavAgent.isGrounded && !sliding;
        if( HasWings )
        {
            if( flying )
            {
            	if( wingFrontSpriter != null )
            	{
                	wingFrontSpriter.SwitchState(wingFrontSpriter.secondarySet, true);
                }
                if( wingBackSpriter != null)
                {
                	wingBackSpriter.SwitchState(wingBackSpriter.secondarySet, true);
                }
            }
            else
            {
            	if( wingFrontSpriter != null )
            	{
            		wingFrontSpriter.SwitchState(wingFrontSpriter.idleSet, true);	
            	}
                if( wingBackSpriter != null )
                {
                	wingBackSpriter.SwitchState(wingBackSpriter.idleSet, true);
                }
            }
        }
        else
        {
            if( wingFrontSpriter != null )
            {
                wingFrontSpriter.SwitchState(wingFrontSpriter.disabledSet, true);
            }
            if( wingBackSpriter != null )
            {
                wingBackSpriter.SwitchState(wingBackSpriter.disabledSet, true);
            }
        }
        if( flying )
        {
        	activeSpriter.SwitchState(activeSpriter.secondarySet, true);
        }
        else if( health == 0 )
        {
        	activeSpriter.canScale = true;
        	activeSpriter.SwitchState(activeSpriter.disabledSet, false);
        }
        else if ( Mathf.Abs(thisNavAgent.WalkVelocity.x)  < 0.02f )
        {
        	activeSpriter.SwitchState(activeSpriter.idleSet, true);
        }
        else
        {
        	activeSpriter.SwitchState(activeSpriter.primarySet, true);
        }
    }
}
