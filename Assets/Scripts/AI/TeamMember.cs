using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamMember : MonoBehaviour
{
    #region Fields

    [SerializeField] private TeamManager.TeamList _team = TeamManager.TeamList.No;
    private TeamManager _teamManager;
    private Agent _agent;
    private Attack _attack;
    private Life _life;

    #endregion Fields

    #region Properties

    public TeamManager.TeamList Team
    {
        get => _team;
        set => _team = value;
    }

    public TeamManager TeamManager
    {
        get => _teamManager;
        set => _teamManager = value;
    }

    public Agent Agent => _agent;

    public Attack Attack => _attack;

    public Life Life => _life;

    #endregion Properties

    #region Methods

    private void Awake()
    {
        _agent = GetComponent<Agent>();
        _attack = GetComponent<Attack>();
        _life = GetComponent<Life>();
    }

    private void Start()
    {
        foreach (var r in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            Debug.Log(r.materials[0].color);
            r.material.color = _teamManager.TeamColor;
            Debug.Log(r.materials[0].color);
        }
    }

    #endregion Methods
}