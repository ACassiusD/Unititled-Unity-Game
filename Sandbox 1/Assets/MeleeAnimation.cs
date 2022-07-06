using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MeleeAnimation : MonoBehaviour
{
    public VisualEffect anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool middleMouseClicked = Input.GetMouseButtonDown(2);

        if (middleMouseClicked)
        {
            anim.Play();
        }
    }
}
