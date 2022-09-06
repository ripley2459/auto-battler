using UnityEngine;

public class NinjaBars : Bars
{
    #region Fields

    [SerializeField] private GameObject _dashBar;

    private Assassin _assassin;

    #endregion Fields

    #region Methods

    protected override void Awake()
    {
        base.Awake();
        _assassin = _teamMember.gameObject.GetComponent<Assassin>();
        _assassin.OnChargingDash += UpdateDashBar;
    }

    private void UpdateDashBar()
    {
        var sX = Mathf.Clamp(_assassin.ActualDashCooldown / _assassin.DashCooldown, 0f, 1f);
        _dashBar.transform.localScale = new Vector3(sX, 1f, 1f);
    }

    #endregion methods
}