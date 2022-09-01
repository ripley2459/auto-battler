using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject _lifeBar;
    [SerializeField] private GameObject _attackRateBar;
    private TeamMember _teamMember;
    private Image _lifeImage;

    #endregion Fields

    #region Methods

    private void Awake()
    {
        _teamMember = GetComponentInParent<TeamMember>();
        _teamMember.Life.OnLifeChanged += UpdateLifeBar;
        _teamMember.Attack.OnChargingAttack += UpdateAttackRateBar;

        _lifeImage = _lifeBar.GetComponent<Image>();
    }

    private void UpdateLifeBar()
    {
        float actualLife = _teamMember.Life.ActualLife;
        float maxLife = _teamMember.Life.MaxLife;

        var sX = Mathf.Clamp(actualLife / maxLife, 0f, 1f);
        _lifeBar.transform.localScale = new Vector3(sX, 1f, 1f);
        
        _lifeImage.color = Color.Lerp(Color.red, Color.green, actualLife / maxLife);
    }

    private void UpdateAttackRateBar()
    {
        var sX = Mathf.Clamp(_teamMember.Attack.AttackProgression / 1f, 0f, 1f);
        _attackRateBar.transform.localScale = new Vector3(sX, 1f, 1f);
    }

    #endregion Methods
}