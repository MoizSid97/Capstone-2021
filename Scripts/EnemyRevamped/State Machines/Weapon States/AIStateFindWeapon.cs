using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateFindWeapon : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.FindWeapon;
    }
    public void Enter(AIAgent agent)
    {
        WeaponPickUp pickUp = FindClosestWeapon(agent);
        agent.navMeshAgent.destination = pickUp.transform.position;
        agent.navMeshAgent.speed = 5;
    }
    public void Update(AIAgent agent)
    {
        //Check if agent has weapon
        if(agent.weapon.HasWeapon())
        {
            //If it does, call ActivateWepaon function
            agent.weapon.ActivateWeapon();
        }
    }
    public void Exit(AIAgent agent)
    {

    }
    private WeaponPickUp FindClosestWeapon(AIAgent agent)
    {
        //Store an array of weapons and find the nearest gameobject/weapon for AI to pick up
        WeaponPickUp[] weapons = Object.FindObjectsOfType<WeaponPickUp>();
        WeaponPickUp closestWeapon = null;
        float closestDistance = float.MaxValue;
        foreach(var weapon in weapons)
        {
            float distanceToWeapon = Vector3.Distance(agent.transform.position, weapon.transform.position);
            if(distanceToWeapon < closestDistance)
            {
                closestDistance = distanceToWeapon;
                closestWeapon = weapon;
            }
        }
        return closestWeapon;
    }
}
