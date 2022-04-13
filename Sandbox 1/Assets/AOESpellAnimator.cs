using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpellAnimator : MonoBehaviour
{
    public bool isActive = false;
    public Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.Rotate(rotation);
        }
        
    }
}
