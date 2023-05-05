using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : FiniteStateMachine
{
    public Bounds bounds;
    public float viewRadius;
    public Transform player;
    public EnemyIdleState idleState;
    public EnemyWanderState wanderState;
    public EnemyChaseState chaseState;

    public NavMeshAgent Agent { get; private set; }
//    public Animator Anim {  get; private set; }

    public AudioSource AudioSource { get; private set; }

    protected override void Awake()
    {
        idleState = new EnemyIdleState(this, idleState);
        wanderState = new EnemyWanderState(this, wanderState);
        chaseState = new EnemyChaseState(this, chaseState);
        entryState = idleState;
        if (TryGetComponent(out NavMeshAgent agent) == true)
        {
            Agent = agent;
        }
        if (TryGetComponent(out AudioSource aSrc) == true)
        {
            AudioSource = aSrc;
        }
 //       if (transform.GetChild(0).TryGetComponent(out Animator anim) == true)
//        {
 //           Anim = anim;
 //       }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); 
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}

public abstract class EnemyBehaviourState : IState
{
    protected Enemy Instance { get; private set; }


    public EnemyBehaviourState(Enemy instance)
    {
        Instance = instance;

    }

    public abstract void OnStateEnter();

    public abstract void OnStateExit();

    public abstract void OnStateUpdate();

    public virtual void DrawStateGizmos() { }
}

[System.Serializable]

public class EnemyIdleState : EnemyBehaviourState
{
    [SerializeField]
    private Vector2 idleTimeRange = new Vector2(3, 10);
    [SerializeField]
    private AudioClip idleClip;

    private float timer = -1;
    private float idleTime = 0;

    public EnemyIdleState(Enemy instance, EnemyIdleState idle) : base(instance) 
    {
        idleTimeRange = idle.idleTimeRange;
        idleClip = idle.idleClip;
    }

    public override void OnStateEnter() 
    { 
        Instance.Agent.isStopped = true;
        idleTime = Random.Range(idleTimeRange.x, idleTimeRange.y);
        timer = 0;
    //    Instance.Anim.SetBool("Moving", false);
        Debug.Log("idle waiting for " + idleTime + " seconds");
    }

    public override void OnStateExit() 
    { 
        timer = -1;
        idleTime = 0;
        //Debug.Log("Exiting idle state");
    }

    public override void OnStateUpdate() 
    {
        if (Vector3.Distance(Instance.transform.position, Instance.player.position) <= Instance.viewRadius)
        {
            if (Instance.CurrentState.GetType() != typeof(EnemyChaseState))
            {
                Instance.SetState(Instance.chaseState);
            }
        }

        if (timer >= 0) 
        {
            timer += Time.deltaTime;
            if(timer >= idleTime) 
            {
                Debug.Log("Exiting Idle State after " + idleTime + " seconds");
                Instance.SetState(Instance.wanderState);
            }
        }
    }
}

[System.Serializable]
public class EnemyWanderState : EnemyBehaviourState
{
    private Vector3 targetPosition;

    [SerializeField]
    private float wanderSpeed = 3.5f;
    [SerializeField]
    private AudioClip wanderClip;

    public EnemyWanderState(Enemy instance, EnemyWanderState wander) : base(instance) 
    {
        wanderSpeed = wander.wanderSpeed;
        wanderClip = wander.wanderClip;
    }

    public override void OnStateEnter()
    {
        Instance.Agent.speed = wanderSpeed;
        Instance.Agent.isStopped = false;
        Vector3 randomPosInBounds = new Vector3(
            Random.Range(-Instance.bounds.extents.x, Instance.bounds.extents.x),
            Instance.transform.position.y,
            Random.Range(-Instance.bounds.extents.z, Instance.bounds.extents.z));
        targetPosition = randomPosInBounds;
        Instance.Agent.SetDestination(targetPosition);
    //    Instance.Anim.SetBool("Moving", true);
    //    Instance.Anim.SetBool("Chasing", false);
        Debug.Log("target pos " + targetPosition);
    }

    public override void OnStateExit()
    {
    }

    public override void OnStateUpdate()
    {
        Vector3 t = targetPosition;
        t.y = 0;
        if(Vector3.Distance(Instance.transform.position, targetPosition) <= Instance.Agent.stoppingDistance) 
        {
            Instance.SetState(Instance.idleState);
        }

        if (Vector3.Distance(Instance.transform.position, Instance.player.position) <= Instance.viewRadius)
        {
            Instance.SetState(Instance.chaseState);
        }

    }

    public override void DrawStateGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(targetPosition, 0.5f);
    }
}

[System.Serializable]
public class EnemyChaseState : EnemyBehaviourState
{
    [SerializeField]
    private float chaseSpeed = 5f;
    [SerializeField]
    private AudioClip chaseClip;

    public EnemyChaseState(Enemy instance, EnemyChaseState chase) : base(instance) 
    {
        chaseSpeed = chase.chaseSpeed;
        chaseClip = chase.chaseClip;
    }

    public override void OnStateEnter()
    {
        Instance.Agent.isStopped = false;
        Instance.Agent.speed = chaseSpeed;
//        Instance.Anim.SetBool("Moving", true);
//        Instance.Anim.SetBool("Chasing", true);
        Debug.Log("chase on");
    }

    public override void OnStateExit()
    {
        Debug.Log("chase off");
    }

    public override void OnStateUpdate()
    {
        Instance.Agent.SetDestination(Instance.player.position);

        if(Vector3.Distance(Instance.transform.position, Instance.player.position) > Instance.viewRadius) 
        {
            Instance.SetState(Instance.wanderState);
        }
    }
}
