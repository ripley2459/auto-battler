using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Oriente constament le widget face à la caméra.
/// </summary>
public class Orientation : MonoBehaviour
{
    #region Fields

    private Camera _camera;

    #endregion Fields

    #region Methods

    void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward,
            _camera.transform.rotation * Vector3.up);
    }

    #endregion Methods
}