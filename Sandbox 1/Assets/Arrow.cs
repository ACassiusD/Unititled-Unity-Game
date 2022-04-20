using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody rb;
    public bool enableRotation = false;
    public Collider[] attackHitboxes;
    public bool destoryOnHit = false;
    public bool collided = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (enableRotation)
        {
           if(rb.velocity.magnitude >= 0.1f)
                transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
        if (!collided)
        {
            hitCheck();
        }
    }

    public void hitCheck()
    {
        string debugMsg = "";
        int hitCount = 0;
        var hitboxCollider = attackHitboxes[0];
        var cols = Physics.OverlapBox(hitboxCollider.bounds.center, hitboxCollider.bounds.extents, hitboxCollider.transform.rotation, LayerMask.GetMask("Interactive"));
        foreach (Collider c in cols)
        {
            Debug.Log(c.name);
            if (c.tag == "Player")
            {
                return;
            }
            if (c.tag == "Enemy")
            {
                var enemyScript = c.gameObject.GetComponent<Enemy>();

                int[] dmgValues;
                var attackValues = new Dictionary<string, int>();
                attackValues.Add("damage", 10);
                attackValues.Add("knockback", 3000);
                hitCount++;
                enemyScript.receiveDamage(10, 3000, rb.velocity);
                debugMsg += ("|Hit " + c.name);
                Debug.Log(this.name + " Hit (" + hitCount + ") " + debugMsg);
                collided = true;
                if (destoryOnHit)
                {
                    Destroy(this);
                }
            }
        }
    }
}
