using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIVersion3 : MonoBehaviour
{
    //Health specific fields
    [SerializeField] 
    private float health;
    [SerializeField]
    private float lowHealthThreshold;
    [SerializeField]
    private float healthRestoreRate;

    //Searching for target/player range
    [SerializeField]
    private float chasingRange;
    [SerializeField]
    private float shootingRange;

    [SerializeField]
    private Transform playerTrans;
    [SerializeField]
    private CoverBT[] availableCovers;

    private Material mtl;
    private Transform bestCoverSpot;
    private NavMeshAgent agent;
    private NodeBT topNode;

    private float _currentHealth;

    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, health); }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mtl = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        _currentHealth = health;

        ConstructBehaviourTree();
    }
    
    private void ConstructBehaviourTree()
    {
        IsCoverAvailableNode coverAvailableNode = new IsCoverAvailableNode(availableCovers, playerTrans, this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, lowHealthThreshold);
        IsCoveredNode isCoveredNode = new IsCoveredNode(playerTrans, transform);
        ChaseNode chaseNode = new ChaseNode(playerTrans, agent, this);
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTrans, transform);
        RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTrans, transform);
        ShootNode shootNode = new ShootNode(agent, this);

        SequenceBT chaseSequence = new SequenceBT(new List<NodeBT> { chasingRangeNode, chaseNode });
        SequenceBT shootSequence = new SequenceBT(new List<NodeBT> { shootingRangeNode, shootNode });
        SequenceBT goToCoverSequence = new SequenceBT(new List<NodeBT> { coverAvailableNode, goToCoverNode });

        SelectorBT findCoverSelector = new SelectorBT(new List<NodeBT> { goToCoverSequence, chaseSequence });
        SelectorBT tryToTakeCoverSelector = new SelectorBT(new List<NodeBT> { isCoveredNode, findCoverSelector });

        SequenceBT mainCoverSequence = new SequenceBT(new List<NodeBT> { healthNode, tryToTakeCoverSelector });

        topNode = new SelectorBT(new List<NodeBT> { mainCoverSequence, shootSequence, chaseSequence });
    }

    private void Update()
    {
        topNode.Evaluate();

        if(topNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
            agent.isStopped = true;
        }

        currentHealth += Time.deltaTime * healthRestoreRate;
    }

    private void OnMouseDown()
    {
        currentHealth -= 30f;
    }

    public void SetColor(Color color)
    {
        mtl.color = color;
    }

    public void SetBestCoverSpot(Transform bestCoverSpot)
    {
        this.bestCoverSpot = bestCoverSpot;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }
}
