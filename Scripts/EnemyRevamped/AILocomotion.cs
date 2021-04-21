using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    //Floats
    [Header("Health Settings")]
    public float maxHealth;
    public float currentHealth;
    //[Header("AI Settings")]
    //public float dieForce;
    [Header("Material Sensitivity Control")]
    public float blinkIntensity;
    public float blinkDuration;

    //Bools
    [Header("Debugging Information")]
    public bool vel;
    public bool desiredVel;
    public bool path;

    //Scripts
    AIRagdoll ragdoll;
    AIHealthBar AIUI;
    AIAgent aiAgent;

    //Private
    SkinnedMeshRenderer skinMR;
    float blinkTimer;
    Animator anim;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        //Access navmesh agent component
        agent = GetComponent<NavMeshAgent>();
        //Access animator component
        anim = GetComponent<Animator>();
        //Access ragdoll script
        ragdoll = GetComponent<AIRagdoll>();
        //Access the skinmeshrenderer that holds the material
        skinMR = GetComponentInChildren<SkinnedMeshRenderer>();
        //Access AIHealthBar Script
        AIUI = GetComponentInChildren<AIHealthBar>();
        //Access AIAgent Script
        aiAgent = GetComponent<AIAgent>();

        //Set up rigidbodies
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        //For each child component in the gameobject that has a rigidbody
        foreach(var rb in rigidBodies)
        {
            //Apply AIHitBox script
            AIHitbox hitBox = rb.gameObject.AddComponent<AIHitbox>();
            hitBox.healthValue = this;

            //Move all the hitbox objects into hitbox layer
            if(hitBox.gameObject != gameObject)
            {
                //hitBox.gameObject.layer = LayerMask.NameToLayer("Hitbox");
            }
        }

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Old version to move AI
        //timer -= Time.deltaTime;
        //if(timer < 0.0f)
        //{
        //    float sqDistance = (playerPos.position - agent.destination).sqrMagnitude;
        //    if(sqDistance > maxDistance * maxDistance)
        //    {
        //        //Look for player gameobject and follow their position
        //        agent.destination = playerPos.position;
        //    }
        //    timer = maxTime;
        //}

        //anim.SetFloat("Speed", agent.velocity.magnitude);

        //Use the parameter Speed from the AI's animator to play different animation depending on the value
        if (agent.hasPath)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }

        //Change the material colour
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        skinMR.material.color = Color.white * intensity;
    }

    public void TakeDamage(float amount, Vector3 dir)
    {
        //Subtract health
        currentHealth -= amount;
        //Start reducing healthbar width
        AIUI.SetHealthBarPercentage(currentHealth / maxHealth);

        //If enemy AI reaches 0 or less
        if (currentHealth <= 0)
        {
            //Kill gameobject
            Death(dir);
        }

        //Everytime AI gets hit, reset this
        blinkTimer = blinkDuration;
    }

    private void Death(Vector3 dir)
    {
        //When AI is dead, start ragdoll physics
        //ragdoll.ActivateRagdoll();

        //When AI is dead, this gameobject can be pushed by force using physics from the Players bullets
        //dir.y = 1;
        //ragdoll.ApplyForce(dir * dieForce);

        //-------New Method-------//
        //Change to this state when AI is dead
        AIStateDeath deathState = aiAgent.stateMachine.GetState(AIStateID.Death) as AIStateDeath;
        deathState.direction = dir;
        aiAgent.stateMachine.ChangeState(AIStateID.Death);
        
        //Destroy gameobject after 10 seconds
        Destroy(gameObject, 10f);
        
        //Hide HealthBar UI upon death
        AIUI.gameObject.SetActive(false);
    }

    //Debugging info
    private void OnDrawGizmos()
    {
        //Checking for regular velocity
        if(vel)
        {
            //Draw green line for velocity
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
        }

        //Checking for desired velocity
        if (desiredVel)
        {
            //Blue line for desired velocity
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
        }

        //Check and draw a path for the enemy
        if (path)
        {
            //Visualized by a red line
            Gizmos.color = Color.red;
            var agentPath = agent.path;
            Vector3 prevCorner = transform.position;
            foreach(var currentCorner in agentPath.corners)
            {
                Gizmos.DrawLine(prevCorner, currentCorner);
                Gizmos.DrawSphere(currentCorner, 0.1f);

                prevCorner = currentCorner;
            }
        }
    }
}
