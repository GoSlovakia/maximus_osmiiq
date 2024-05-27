using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPrototype : MonoBehaviour
{
    public string username;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        
    }
}
