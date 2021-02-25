using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * Attached to main camera, controls game scoring, winning, and team generation.
 * 
 */
public class Game : MonoBehaviour
{
    public static Game instance;

    public int teamSize = 5;
    public float unconsciousTimeHold;

    public GameObject playerPrefab;
    public GameObject snitch;
    public Color unconsciousColor;

    public TeamTraits team0;
    public TeamTraits team1;

    public bool debug = true;

    public int team0Score = 0;
    public GameObject textMeshScore0; 
    public GameObject textMeshScore1;
    public int team1Score = 0;
    private int lastTeamScored;

    private Snitch snitchScript;
    /**
     * Initialize game by spawning players
     * 
     */
    void Awake()
    {
        
        // Default team size to 5
        if (! (teamSize >= 5 && teamSize <= 20))
            teamSize = 5;

        team0.players = new List<GameObject>();
        team1.players = new List<GameObject>();
        for (int i = 0; i < teamSize; i++){
            GameObject p1 = createPlayer(isTeam0: false);
            team1.players.Add(p1);
            GameObject p0 = createPlayer(isTeam0: true);
            team0.players.Add(p0);
        }

        snitchScript = snitch.GetComponent<Snitch>();
        instance = this;
        updateUI();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(team0.spawnOrigin, team0.spawnRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(team1.spawnOrigin, team1.spawnRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(Vector3.zero + Vector3.up * 10, 10);
    
    }

    void Update()
    {
        if (team0Score == 100 || team1Score == 100)
        {
            // Game Over, stop game
        }
    }


    /**
     * Implement the scoring scheme. Successive catches are worth 2 points.
     */
    public void score(int team)
    {
        // Respawn the snitch
        snitchScript.respawn();
        // Check if this is a first score, where the value of lastTeam scored is frivolous
        if (team0Score + team1Score == 0)
        {
            if (team == 1) team1Score++;
            else team0Score++;
            updateUI();
            return;
        }

        // Now must check if this is a consecutive score.
        if (team == 1)
        {
            if (lastTeamScored == 1) team1Score += 2;
            else
            {
                team1Score++;
                lastTeamScored = 1;
            }
        }
        else if (team == 0)
        {
            if (lastTeamScored == 0) team0Score += 2;
            else
            {
                team0Score++;
                lastTeamScored = 0;
            }
        }
        updateUI();
    }


    /**
     * Generate players and their traits according to the scriptable object distribution
     * 
     */
    private GameObject createPlayer(bool isTeam0)
    {
        GameObject create;
        if (isTeam0)
        {
            create = Instantiate(playerPrefab, team0.spawnOrigin + Random.onUnitSphere * team0.spawnRadius, Quaternion.identity);
            BoidPlayer boidPlayer = create.GetComponent<BoidPlayer>();
            boidPlayer.setTeamColor(team0.color);
            boidPlayer.team = 0;
            boidPlayer.respawnPosition = team0.spawnOrigin;
            boidPlayer.aggressiveness = sampleGaussian(team0.agressivenessMean, team0.agressivenessSD);
            boidPlayer.maxExhaustion = sampleGaussian(team0.maxExhaustionMean, team0.maxExhaustionSD);
            boidPlayer.maxVelo = sampleGaussian(team0.maxVeloMean, team0.maxVeloSD);
            boidPlayer.weight = sampleGaussian(team0.weightMean, team0.weightSD);
        }
        else
        {
            create = Instantiate(playerPrefab, team1.spawnOrigin + Random.onUnitSphere * team1.spawnRadius, Quaternion.identity);
            BoidPlayer boidPlayer = create.GetComponent<BoidPlayer>();
            boidPlayer.setTeamColor(team1.color);
            boidPlayer.team = 1;
            boidPlayer.respawnPosition = team1.spawnOrigin;
            boidPlayer.aggressiveness = sampleGaussian(team1.agressivenessMean, team1.agressivenessSD);
            boidPlayer.maxExhaustion = sampleGaussian(team1.maxExhaustionMean, team1.maxExhaustionSD);
            boidPlayer.maxVelo = sampleGaussian(team1.maxVeloMean, team1.maxVeloSD);
            boidPlayer.weight = sampleGaussian(team1.weightMean, team1.weightSD);
        }
        return create;        
    }


    /**
     * 
     * Pull a single float from the specified Gaussian Distribution which uses Box-Muller transform.
     * 
     */
    private float sampleGaussian(float mean, float sd)
    {
        // Two uniform random variables
        float u1 = Random.Range(0.00001f, 0.99999f);
        float u2 = Random.Range(0.00001f, 0.99999f);

        // Box-Muller transform
        float z = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Cos(2 * Mathf.PI * u2);

        // Apply paramter scale
        return z * sd + mean;
    }


    /**
     * Called only after a score, update the textmesh pro objects to the new score
     */
    private void updateUI()
    {
        textMeshScore0.GetComponent<TMPro.TextMeshProUGUI>().text = "Team 0 Slytherin Score: " + team0Score.ToString();
        textMeshScore1.GetComponent<TMPro.TextMeshProUGUI>().text = "Team 1 Gryffindor Score: " + team1Score.ToString();
    }
}
