using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour, IInitializer
{
    #region Fields

    [SerializeField] private string _teamName;
    
    [SerializeField] private Color _teamColor;

    /// <summary>
    /// Choix de l'équipe.
    /// </summary>
    [SerializeField] private TeamList _team = TeamList.No;

    /// <summary>
    /// Limites de la zone de cette équipe.
    /// </summary>
    [SerializeField] private Bounds _bounds;

    /// <summary>
    /// Liste des prefabs qui seront instantiés au début des combats.
    /// </summary>
    [SerializeField] private List<TeamMember> _prefabs = new List<TeamMember>();

    /// <summary>
    /// Liste des combattants.
    /// </summary>
    private List<TeamMember> _members = new List<TeamMember>();

    /// <summary>
    /// Liste des ennemis.
    /// </summary>
    private List<TeamMember> _enemies = new List<TeamMember>();

    #endregion Fields

    #region Properties

    public Color TeamColor => _teamColor;

    public string TeamName => _teamName;

    public TeamList Team => _team;

    public List<TeamMember> Members
    {
        get => _members;
    }

    public List<TeamMember> Enemies => _enemies;

    #endregion Properties

    #region Methods

    public void Awake()
    {
        BattleManager.Instance.OnPreInit += PreInit;
        BattleManager.Instance.OnInit += Init;
        BattleManager.Instance.OnPostInit += PostInit;

        BattleManager.Instance.NotifyDeath += ApplyDeath;
        BattleManager.Instance.OnEndbattle += OnEndBattle;
    }

    private void ApplyDeath(TeamMember member)
    {
        if (member.Team == _team)
        {
            _members.Remove(member);
            if (_members.Count == 0)
            {
                BattleManager.Instance.ApplyLoose(this);
            }
        }
        else
        {
            _enemies.Remove(member);
        }
    }

    public void PreInit()
    {
        foreach (TeamMember prefab in _prefabs)
        {
            Vector3 pos = BattleManager.Instance.GetRandomPositionInBounds(_bounds);

            TeamMember newAgent = Instantiate(prefab, pos, Quaternion.identity);

            newAgent.TeamManager = this;
            newAgent.Team = _team;
            newAgent.Attack.TeamManager = this;
            newAgent.Attack.Team = _team;

            _members.Add(newAgent);
        }
    }

    public void Init()
    {
        _enemies = BattleManager.Instance.GetEnemies(Team);
    }

    public void PostInit()
    {
        // Empty
    }

    public void OnEndBattle()
    {
        foreach (TeamMember member in _members)
        {
            Destroy(member.gameObject);
        }

        foreach (TeamMember enemy in _enemies)
        {
            Destroy(enemy.gameObject);
        }

        _members.Clear();
        _enemies.Clear();
    }

    /// <summary>
    /// Renvoie le combattant ennemis à cette équipe le plus proche d'une position donnée.
    /// </summary>
    /// <param name="member">Position donnée.</param>
    /// <returns>TeamMember du combattant ennemis le plus proche de la position donnée.</returns>
    public TeamMember GetNearestTarget(Transform member)
    {
        TeamMember bestTarget = null;
        float closestDist = Mathf.Infinity;
        Vector3 currentPos = member.position;

        foreach (TeamMember pTarget in _enemies)
        {
            if (pTarget.Life.IsAlive())
            {
                Vector3 directionToTarget = pTarget.transform.position - currentPos;
                float dSqrToTarget = directionToTarget.sqrMagnitude;

                if (dSqrToTarget < closestDist)
                {
                    closestDist = dSqrToTarget;
                    bestTarget = pTarget;
                }
            }
        }

        return bestTarget;
    }

    #endregion Methods

    public enum TeamList
    {
        Blue,
        Red,
        No
    }
}