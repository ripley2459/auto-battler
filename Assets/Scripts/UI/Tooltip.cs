using TMPro;
using UnityEngine;

/// <summary>
/// Affiche un petit encart en bas à gauche incluant des infos sur le personnage survolé avec la souris.
/// </summary>
public class Tooltip : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject _toolTip;
    
    [SerializeField] private TextMeshProUGUI _life;
    
    [SerializeField] private TextMeshProUGUI _damageDealt;
    
    [SerializeField] private TextMeshProUGUI _name;
    
    private TeamMember _teamMember;

    #endregion Fields

    #region MyRegion

    private void Awake()
    {
        _teamMember = GetComponent<TeamMember>();

        _teamMember.Life.OnLifeChanged += UpdateLife;
        _teamMember.Attack.OnDamageDealt += UpdateDamageDealt;
    }

    private void Start()
    {
        _name.text = _teamMember.Name;
        _toolTip.SetActive(false);

        _damageDealt.text = "Damage dealt: 0";
    }

    private void UpdateLife()
    {
        float actualLife = _teamMember.Life.ActualLife;
        float maxLife = _teamMember.Life.MaxLife;
        _life.text = actualLife + "/" + maxLife;
        _life.color = Color.Lerp(Color.red, Color.green, actualLife / maxLife);
    }

    private void UpdateDamageDealt()
    {
        _damageDealt.text = "Damage dealt: " + _teamMember.Attack.DamageDealt.ToString();
    }

    private void OnMouseEnter()
    {
        _toolTip.SetActive(true);
    }

    private void OnMouseExit()
    {
        _toolTip.SetActive(false);
    }

    #endregion
}