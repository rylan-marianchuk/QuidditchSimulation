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
    public bool isJoker = false;

    private bool isUnconscious = false;
    private bool onHold = false;
    private float unconsciousPenalty;

    private Rigidbody rigidbody;
    private Color teamColor;

    // For regular player, this will be the snitch. For jokers, its the closest opponent to the snitch.
    private Vector3 goalPosition;

    public GameObject other;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        // Update the assigned weight and color from the game creation
        rigidbody.mass = this.weight;
        GetComponent<Renderer>().material.color = teamColor;
    }

    public void setTeamColor(Color c) { this.teamColor = c; }


    void FixedUpdate()
    {
        if (Input.GetKeyUp("u"))
        {
            setUnconscious();
        }

        if (!isUnconscious)
        {
            if (isJoker) goalPosition = getClosestOpponent();
            else goalPosition = Game.instance.snitch.transform.position;
            // Movement
            /**
             * =========================================================================================================================
             * The following movement was adapted from Omar Addam and his Boid movement found at:
             * https://github.com/omaddam/Boids-Simulation/blob/develop/Assets/Boids/Scripts/Bird.cs
             */
            rigidbody.velocity = (Game.instance.snitch.transform.position - this.transform.position).normalized * aggressiveness/10f;

            // Increase current exhaustion by velocity * weight


            // Update rotation to point in direction of movement.
            transform.forward = rigidbody.velocity.normalized;


            /**
             * End of attributed code ===================================================================================================
             */
            if (maxExhaustion == currentExhaustion)
                setUnconscious();
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
     * Return the correct direction for the player to move so that they are optimally
     */
    private Vector3 avoidCollision()
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
            if (Game.instance.debug) Debug.Log("Collision between same team.");

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
            // Considered Tackled and unconscious
            setUnconscious();
        }
    }


    /**
     * 
     * 
     * 
     */
    public void setUnconscious(){
        if (isUnconscious) return;
        isUnconscious = true;

        // Ignore all forces except gravity
        rigidbody.isKinematic = true;
        rigidbody.velocity = Vector3.zero;
        if (Game.instance.debug) Debug.Log("Set Unconscious");

        // Change color to red to see who becomes unconscious
        this.GetComponent<Renderer>().material.color = Game.instance.unconsciousColor;
    }

    
    
    /**
     * 
     * 
     */
    private void setConscious()
    {
        isUnconscious = false;
        onHold = false;
        rigidbody.isKinematic = false;
        if (Game.instance.debug) Debug.Log("Set Conscious");
        this.GetComponent<Renderer>().material.color = teamColor;
    }


    /**
     * Return the position of the opponent closest to the Snitch
     * 
     * Used for joker goal position.
     */
    private Vector3 getClosestOpponent()
    {
        List<GameObject> opponents;
        if (team == 1) opponents = Game.instance.team0.players;
        else opponents = Game.instance.team1.players;
        Vector3 min = Vector3.positiveInfinity ;
        foreach (GameObject player in opponents)
        {
            if ((player.transform.position - Game.instance.snitch.transform.position).sqrMagnitude < min.sqrMagnitude)
                min = player.transform.position;
        }
        return min;
    }
}
