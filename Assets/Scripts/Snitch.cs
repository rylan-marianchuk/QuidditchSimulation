using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * Simple class to move the snitch randomly
 * 
 */
public class Snitch : MonoBehaviour
{
    // The speed of snitch
    public float scaledVelocity;
        
    // The proportion of frames the snitch will choose a new direction
    public float changeProportion;

    public float avoidanceRadius;

    private Vector3 newDirection;
    private Rigidbody rigidbody;

    // This variable needed to not slerp when nearing an arena boundary
    private int slerpRest;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Random.onUnitSphere * scaledVelocity;
    }


    /**
     * With probability changeProportion, update the Snitch's velocity vector using Slerp to resolve the jerkyness
     */
    void FixedUpdate()
    {
        if (Game.instance.gameOver) return;
        if (slerpRest == 0 && Random.Range(0.0000f, 0.9999f) < changeProportion)
        {
            newDirection = Random.onUnitSphere * scaledVelocity;
        }
        

        if (slerpRest == 0)
            rigidbody.velocity = Vector3.Slerp(rigidbody.velocity, newDirection, Time.deltaTime);
        else slerpRest--;
    }

    public void respawn()
    {
        transform.position = new Vector3(Random.Range(-48f, 48f), Random.Range(0f, 24f), Random.Range(-48f, 48f));
    }

}
