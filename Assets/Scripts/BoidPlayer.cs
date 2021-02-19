using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidPlayer : MonoBehaviour
{
    public int team; // 0: Gryffindor, 1: Slytherin
    public Vector3 respawnPosition;

    // Traits
    private float aggressiveness;
    private float maxExhaustion;
    private float maxVelo;
    private float currentExhaustion;

    //[HideInInspector] public GameObject this_instance;

    private bool isUnconscious;
    private float unconsciousPenalty;


    void Start()
    {
        // Assign color
       
    }


    void Update()
    {
        
    }


    /**
     * 
     * Handle four types of collisions:
     *   1. Arena environment
     *   2. Opponent
     *   3. Proponent
     *   4. Snitch
     * 
     */
    void OnCollisionEnter(Collision collision)
    {
        // Create a collision listener to get the two instances of the collision

        if (collision.gameObject.CompareTag("Ground") && this.isUnconscious)
        {
            // Teleport to spawn point and hold for penalty
            gameObject.transform.position = respawnPosition;
        }
        else if (collision.gameObject.CompareTag("Snitch"))
        {
            Game.instance.score(this.team);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Do nothing if collides with other teammate and randomness fails past the 5% threshold
            BoidPlayer other = collision.gameObject.GetComponent<BoidPlayer>();
            if (other.team == this.team && Random.Range(0.0f, 1.0f) < 0.95) return;

            float myval = this.aggressiveness * (Random.Range(0, 1) * (1.2f - 0.8f) + 0.8f) * (1 - (this.currentExhaustion / this.maxExhaustion));
            float otherval = other.aggressiveness * (Random.Range(0, 1) * (1.2f - 0.8f) + 0.8f) * (1 - (other.currentExhaustion / other.maxExhaustion));

            float low = Mathf.Min(myval, otherval);
            if (low == myval)
                setUnconscious();
            else
                other.setUnconscious();
        }
        else if (collision.gameObject.CompareTag("Terrain"))
        {
            // Considered Tackled
        }
    }


    public void setUnconscious(){
        if (isUnconscious) return;
        isUnconscious = true;

        // Ignore all forces except gravity

        
    }
}
