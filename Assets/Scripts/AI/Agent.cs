using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    #region Fields

    private NavMeshAgent _agent;
    private Vector3 _target = Vector3.zero;

    #endregion

    #region Properties

    public Vector3 Target
    {
        get => _target;
        set => _target = value;
    }

    public float Radius
    {
        get => _agent.radius;
    }

    public NavMeshAgent AgentI => _agent;

    #endregion Properties

    #region Methods

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoRepath = true;
    }

    public void SetDestination()
    {
        _agent.SetDestination(Target);
    }

    public void SetDestination(Vector3 newTarget)
    {
        Target = newTarget;
        _agent.SetDestination(Target);
    }
    /// <summary>
    /// Check if the agent can reach his target.
    /// </summary>
    /// <returns>boolean if or not the agent can reach his target.</returns>
    public bool CanReach()
    {
        return CanReach(Target);
    }

    /// <summary>
    /// Check if the agent can reach his destination;
    /// </summary>
    /// <returns>boolean if or not the agent can reach his destination.</returns>
    public bool CanReach(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(destination, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            return true;
        }
        /* else if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            return false;
        } */

        return false;
    }

    public void StopMoving()
    {
        _agent.SetDestination(transform.position);
    }
    
    #endregion Methods
}