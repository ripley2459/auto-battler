using UnityEngine;

public class AttackRateBar : MonoBehaviour
{
    #region Fields

    private Attack _attack;

    #endregion Fields

    #region Methods

    private void Awake()
    {
        _attack = GetComponentInParent<Attack>();
        _attack.OnChargingAttack += UpdateBar;
    }

    private void UpdateBar()
    {
        var sX = Mathf.Clamp(_attack.AttackProgression / 1f, 0f, 1f);
        transform.localScale = new Vector3(sX, 1f, 1f);
    }

    #endregion Methods
}