using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateDeath : AIState
{
    public Vector3 direction;

    public AIStateID GetID()
    {
        return AIStateID.Death;
    }

    public void Enter(AIAgent agent)
    {
        //----THIS REPLACES THE DEATH FUNCTION THAT WAS ORIGINALLY IN AILOCOMOTION SCRIPT----//
        //When AI is dead, start ragdoll physics
        agent.ragdoll.ActivateRagdoll();

        //When AI is dead, this gameobject can be pushed by force using physics from the Players bullets
        direction.y = 1;
        agent.ragdoll.ApplyForce(direction * agent.config.dieForce);

        //Using the AIWeapon script, call this method upon death
        agent.weapon.DropWeapon();

        //Hide HealthBar UI upon death
        //agent.healthUI.gameObject.SetActive(false);

        //agent.meshRenderer.updateWhenOffscreen = true;
    }

    public void Exit(AIAgent agent)
    {
    }

    public void Update(AIAgent agent)
    {
    }
}
