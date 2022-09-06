using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Script de base des agent/combattants.
/// </summary>
public class Agent : MonoBehaviour
{
    #region Fields

    private NavMeshAgent _agent;
    
    /// <summary>
    /// Destination de l'agent
    /// </summary>
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

    /// <summary>
    /// Actualise la destination de l'agent.
    /// </summary>
    public void SetDestination()
    {
        _agent.SetDestination(Target);
    }

    /// <summary>
    /// Change et actualise la destination de l'agent.
    /// </summary>
    /// <param name="newTarget">Nouvelle destination</param>
    public void SetDestination(Vector3 newTarget)
    {
        Target = newTarget;
        _agent.SetDestination(Target);
    }
    /// <summary>
    /// Vérifie si l'agent peut atteindre sa destination actuelle.
    /// </summary>
    /// <returns>Vrai si la destination est accéssible.</returns>
    public bool CanReach()
    {
        return CanReach(Target);
    }

    /// <summary>
    /// Vérifie si l'agent peut atteindre une destination donnée.
    /// </summary>
    /// <param name="destination">Destination à vérifier</param>
    /// <returns>Vrai si la destination est accéssible.</returns>
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

    /// <summary>
    /// Immobilise l'agent.
    /// </summary>
    public void StopMoving()
    {
        _agent.SetDestination(transform.position);
    }
    
    #endregion Methods
}