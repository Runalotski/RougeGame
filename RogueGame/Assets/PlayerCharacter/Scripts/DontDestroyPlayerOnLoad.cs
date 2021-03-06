using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyPlayerOnLoad : MonoBehaviour
{
    public static DontDestroyPlayerOnLoad instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
