using UnityEngine;

public class AOESpellAnimator : MonoBehaviour
{
    public bool isActive = true;
    public float degreesPerSecond = 50.0f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.Rotate(0, 0, degreesPerSecond * Time.deltaTime); //rotates 50 degrees per second around z axis

        }

    }
}
