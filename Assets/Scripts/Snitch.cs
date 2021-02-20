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
        if (slerpRest == 0 && Random.Range(0.0000f, 0.9999f) < changeProportion)
        {
            // Update direction randomly
            Debug.Log("Snitch Updated velocity vector!");

            newDirection = Random.onUnitSphere * scaledVelocity;
        }
        

        if (slerpRest == 0)
            rigidbody.velocity = Vector3.Slerp(rigidbody.velocity, newDirection, Time.deltaTime);
        else slerpRest--;


        /*
        // Check if nearing boundary
        if (Physics.SphereCast(transform.position, avoidanceRadius, transform.forward, out RaycastHit hit, 1 << 8))
        {
            slerpRest = 500;
            // Update direction directly away from boundary
            rigidbody.velocity = (transform.position - hit.point)* 0.02f;
            Debug.Log("HIT BOUNDARY");
        }
        */
    }





}
