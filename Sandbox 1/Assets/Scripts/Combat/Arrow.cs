using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody rb;
    public bool enableRotation = true;
    public Collider[] attackHitboxes;
    public bool destoryOnHit = true;
    public bool collided = false;
    public float despawnTimer = 10f;
    public float damage = 50;
    public int knockbackForce = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (enableRotation)
        {
            if (rb.velocity.magnitude >= 0.1f)
                transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
        if (!collided)
        {
            hitCheck();
            updateDespawnTimer();
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
            if (c.tag == "Player")
                return;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                

            if (c.tag == "Enemy")
            {
                var enemyScript = c.gameObject.GetComponent<Enemy>();
                var knockbackDirection = rb.velocity;
                hitCount++;

                enemyScript.receiveDamage(damage, knockbackForce, knockbackDirection);
                debugMsg += ("|Hit " + c.name);
                Debug.Log(this.name + " Hit (" + hitCount + ") " + debugMsg);
                collided = true;
                if (destoryOnHit)
                {
                    Destroy(this.gameObject);
                    break;
                }
            }
        }
    }

    public void updateDespawnTimer()
    {
        despawnTimer -= Time.deltaTime;
        if (despawnTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
