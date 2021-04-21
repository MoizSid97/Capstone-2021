using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent navMeshAgent;
     public AIStateID initialState;
    [HideInInspector] public AIStateMachine stateMachine;
    [HideInInspector] public AIConfig config;
    [HideInInspector] public AIRagdoll ragdoll;
    //public SkinnedMeshRenderer meshRenderer;
    //public AIHealthBar healthUI;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public AIWeapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        //Get the ragdoll script
        ragdoll = GetComponent<AIRagdoll>();
        //Get the meshrenderer to apply glow effect
        //meshRenderer = GetComponent<SkinnedMeshRenderer>();
        //Get the health bar ui script
        //healthUI = GetComponent<AIHealthBar>();
        //Get the navmesh component
        navMeshAgent = GetComponent<NavMeshAgent>();
        //Get the AIWeapon script
        weapon = GetComponent<AIWeapon>();

        //Look for player
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //State machine stuff
        stateMachine = new AIStateMachine(this);
        //Register chase player state script
        stateMachine.RegisterState(new AIStateChasePlayer());
        //Register death state script
        stateMachine.RegisterState(new AIStateDeath());
        //Register idle state script
        stateMachine.RegisterState(new AIStateIdle());
        //Register find weapon state script
        stateMachine.RegisterState(new AIStateFindWeapon());
        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
