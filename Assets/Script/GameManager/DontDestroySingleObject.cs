using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroySingleObject : MonoBehaviour
{
    public static DontDestroySingleObject instance;
    void Awake()
    {
        CheckInstance();
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
