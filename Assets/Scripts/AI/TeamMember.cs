using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TeamMember : MonoBehaviour
{
    #region Fields

    [SerializeField] private TeamManager.TeamList _team = TeamManager.TeamList.No;
    private TeamManager _teamManager;
    private Agent _agent;
    private Attack _attack;
    private Life _life;
    private String _name;

    // https://www.fantasynamegenerators.com/pirate-names.php
    public static String[] RandomNames = new[]
    {
        "Stanwick 'The Cook' Norman",
        "Goldsmith 'Naive' Talon",
        "Bray 'Deserter' Darth",
        "Ashby 'Daring' Cidolfus",
        "Blake 'Shifty' Appleton",
        "Kody 'Haunted' Ward",
        "Vail 'Grisly' Clayton",
        "Langdon 'Devious' Livingstone",
        "Rugby 'Speechless' Sutherland",
        "Tedmund 'Double-Crossed' Roscoe",
        "Brand 'Cruelty' Bonney",
        "Chad 'Soft Heart' Drakkar",
        "Orman 'Rigger' Puck",
        "Hartley 'Landlubber' Lincoln",
        "Orman 'Treasure' Middleton",
        "Strong 'Treason' Crompton",
        "Wadham 'Jagged' Keic",
        "Gifford 'Furious' Loki",
        "Rudd 'Ghostly' Artemis",
        "Oswin 'Haunted' Sidney"
    };

    #endregion Fields

    #region Properties

    public string Name => _name;

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

        _name = RandomNames[Random.Range(0, RandomNames.Length)];
        gameObject.name = _name;
    }

    private void Start()
    {
        foreach (var r in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            r.material.color = _teamManager.TeamColor;
        }
    }

    #endregion Methods
}