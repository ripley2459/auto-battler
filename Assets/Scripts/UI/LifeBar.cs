using UnityEngine;

public class LifeBar : MonoBehaviour
{
    #region Fields

    private Life _life;

    #endregion Fields

    #region Methods

    private void Awake()
    {
        _life = GetComponentInParent<Life>();
        _life.OnLifeChanged += UpdateBar;
    }

    private void UpdateBar()
    {
        var sX = Mathf.Clamp(_life.ActualLife / _life.MaxLife, 0f, 1f);
        transform.localScale = new Vector3(sX, 1f, 1f);
    }

    #endregion Methods
}