using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animBodyAvatar : MonoBehaviour
{
    [SerializeField]
    private float _amplitude = 0.01f;
    [SerializeField]
    private float _frequency = 0.0001f;

    [SerializeField]
    private float height_offset = 2f;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = transform.position.x;
        float y = Mathf.Sin(Time.time * _frequency) * _amplitude;
        float z = transform.position.z;

        transform.position = new Vector3(x, y+height_offset, z);
    }


}
