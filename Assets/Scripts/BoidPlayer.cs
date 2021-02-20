using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidPlayer : MonoBehaviour
{
    public int team; // 0: Slytherin, 1: Gryffindor
    public Vector3 respawnPosition;

    // Traits
    public float aggressiveness;
    public float maxExhaustion;
    public float maxVelo;
    public float weight;
    public float currentExhaustion;


    private bool isUnconscious = false;
    private bool onHold = false;
    private float unconsciousPenalty;

    private Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        // Update the assigned weight from the game creation
        rigidbody.mass = this.weight;
    }


    void FixedUpdate()
    {
        if (Input.GetKeyUp("u"))
        {
            setUnconscious();
        }

        if (!isUnconscious)
        {
            // Movement
            rigidbody.velocity = (Vector3.zero - this.transform.position) * 0.05f;
        }
        else if (onHold)
        {
            unconsciousPenalty--;
            if (unconsciousPenalty == 0)
                setConscious();
        }
        else 
        { 
            // Must be unconscious but not yet on hold at spawn position. 
            rigidbody.isKinematic = false;
            rigidbody.velocity = Vector3.down*2.5f;
        }

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
            unconsciousPenalty = Game.instance.unconsciousTimeHold;
            rigidbody.isKinematic = true;
            onHold = true;
            if (Game.instance.debug) Debug.Log("Hit ground. Respawning");
        }
        else if (collision.gameObject.CompareTag("Snitch") && !isUnconscious)
        {
            Game.instance.score(this.team);
        }
        else if (collision.gameObject.CompareTag("Player") && !isUnconscious)
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
        rigidbody.isKinematic = true;
        rigidbody.velocity = Vector3.zero;
        if (Game.instance.debug) Debug.Log("Set Unconscious");
    }

    private void setConscious()
    {
        isUnconscious = false;
        onHold = false;
        rigidbody.isKinematic = false;
        if (Game.instance.debug) Debug.Log("Set Conscious");
    }
}
