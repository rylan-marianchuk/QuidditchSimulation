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

    public GameObject playerPrefab;
    public List<BoidPlayer> players;

    public TeamTraits team0;
    public TeamTraits team1;
    

    public int team0Score = 0;
    public int team1Score = 0;



    private int lastTeamScored;

    /**
     * Generate players and their traits according to the scriptable object distribution
     * 
     */
    void Awake()
    {
        // Default team size to 5
        if (! (teamSize >= 5 && teamSize <= 20))
            teamSize = 5;

        for (int i = 0; i < teamSize; i++){
            GameObject p1 = createPlayer(isTeam0: false);
            GameObject p2 = createPlayer(isTeam0: true);
        }

    }

    void Start()
    {
    
    
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
        // Check if this is a first score, where the value of lastTeam scored is frivolous
        if (team0Score + team1Score == 0)
        {
            if (team == 1) team1Score++;
            else team0Score++;
            return;
        }

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
    }


    /**
     * 
     * 
     * 
     */
    private GameObject createPlayer(bool isTeam0)
    {
        GameObject create;
        if (isTeam0)
        {
            create = Instantiate(playerPrefab, team0.spawnOrigin + Random.onUnitSphere * team0.spaweenRadius, Quaternion.identity);
        }
        else
        {
            create = Instantiate(playerPrefab, team1.spawnOrigin + Random.onUnitSphere * team1.spaweenRadius, Quaternion.identity);
        }
        return create;        
    }
}
