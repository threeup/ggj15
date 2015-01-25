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
    public int health = 2;
    public bool invincible;

	public override void Awake()
	{
		base.Awake();
		entityName = "ACTOR-"+this.gameObject.name+"_"+this.GetHashCode();
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

        if( health > 0 )
            --health;

        Debug.Log("ActorEntity: Damage. Health: " + (health + 1) + " -> " + health);

        BroadcastMessage("OnDamage", SendMessageOptions.DontRequireReceiver);

        if( health == 0 )
            BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
    }

    void OnDeath()
    {
        collider2D.enabled = false;
    }
}
