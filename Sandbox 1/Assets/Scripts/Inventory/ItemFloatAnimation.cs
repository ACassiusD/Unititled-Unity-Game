using System;
using UnityEngine;

public class ItemFloatAnimation : MonoBehaviour
{
    float originalY;
    public float floatStrength = 1;// change the range of y positions that are possible.
    public float speedStrength = 1;

    void Start()
    {
        this.originalY = this.transform.position.y;
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x,
            originalY + ((float)Math.Sin(Time.time * speedStrength) * floatStrength),
            transform.position.z);
    }
}
