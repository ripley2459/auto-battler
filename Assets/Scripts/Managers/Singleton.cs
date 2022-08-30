﻿using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Fields

    private static T _instance;

    #endregion Fields

    #region Properties

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    throw new Exception(typeof(T) + " Trying to access a nulled instance of a singleton. Exiting.");
                }
            }

            return _instance;
        }
    }

    #endregion Properties

    #region Methods

    #region MonoBehaviour

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
    }

    protected virtual void OnDestroy()
    {
        _instance = null;
    }

    #endregion MonoBehaviour

    #endregion Methods
}