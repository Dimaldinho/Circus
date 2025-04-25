using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class GameControl : MonoBehaviour
{
    private static GameObject player1MoveText, player2MoveText;
    private static TextMeshProUGUI whoWinsText;
    private static GameObject player1, player2;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    public static bool gameOver = false;
    static readonly Dictionary<int,int> jumps = new Dictionary<int,int> {
    { 2, 10 },   // ladder
    { 15, 22 },  
    { 17, 5 },  
    { 18, 11 },  
    { 19, 24 },  
    { 26,  14 },
    { 27,  34 },
    };

    // Use this for initialization
    void Start()
    {   
        
        diceSideThrown       = 0;
        player1StartWaypoint = 0;
        player2StartWaypoint = 0;
        gameOver             = false;
        // Grab the UI text objects
        whoWinsText     = GameObject.Find("WhoWinsText").GetComponent<TextMeshProUGUI>();
        player1MoveText   = GameObject.Find("Player1MoveText");
        player2MoveText   = GameObject.Find("Player2MoveText");

        // Grab the player tokens
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        // Initially no one is moving
        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;

        // Show/hide the move prompts
        whoWinsText.gameObject.SetActive(false);
        player1MoveText   .SetActive(true);
        player2MoveText   .SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if Player 1 has finished moving this turn

    if (player1.GetComponent<FollowThePath>().waypointIndex
        > player1StartWaypoint + diceSideThrown)
    {
        var ftp = player1.GetComponent<FollowThePath>();
        ftp.moveAllowed = false;
        player1MoveText.SetActive(false);
        player2MoveText.SetActive(true);

        // where they _actually_ landed
        int landed = ftp.waypointIndex - 1;

        // --- LADDER CHECK: if they landed on 6, jump to 24 ---
        if (jumps.TryGetValue(landed, out int destination))
        {
            int jumpDistance = destination - landed;

            // reset the FollowThePath index back to this tile,
            // so it will walk from [landed] → [landed+1] → … → [destination]
            ftp.waypointIndex      = landed + 1;
            player1StartWaypoint   = landed;

            // temporarily override diceSideThrown so your Update()
            // will re-enter “moving” logic until we've walked jumpDistance steps
            diceSideThrown = jumpDistance;
            
            // and resume walking
            ftp.moveAllowed = true;

            // bail out now; the normal “> startWaypoint + diceSideThrown”
            // check will fire again once the walk-to-jump completes
            return;
        }

        // now store the start index for next turn
        player1StartWaypoint = landed;
    }

        // Check if Player 2 has finished moving this turn
        if (player2.GetComponent<FollowThePath>().waypointIndex
            > player2StartWaypoint + diceSideThrown)
        {
            var ftp = player2.GetComponent<FollowThePath>();
            ftp.moveAllowed = false;
            player2MoveText.SetActive(false);
            player1MoveText.SetActive(true);

            // where they _actually_ landed
            int landed = ftp.waypointIndex - 1;

            // --- LADDER CHECK: if they landed on 6, jump to 24 ---
            if (jumps.TryGetValue(landed, out int destination))
        {   new WaitForSeconds(4f);
            int jumpDistance = destination - landed;

            ftp.waypointIndex      = landed + 1;
            player2StartWaypoint   = landed;
            diceSideThrown         = jumpDistance;
            
            ftp.moveAllowed        = true;
            return;
        }

            // now store the start index for next turn
            player2StartWaypoint = landed;
        }
        

        // Check for win: P1 reaches the final waypoint
        if (player1.GetComponent<FollowThePath>().waypointIndex
            == player1.GetComponent<FollowThePath>().waypoints.Length)
        {
            whoWinsText.gameObject.SetActive(true);
            whoWinsText.text = "Player 1 Wins";
            gameOver = true;
        }

        // Check for win: P2 reaches the final waypoint
        if (player2.GetComponent<FollowThePath>().waypointIndex
            == player2.GetComponent<FollowThePath>().waypoints.Length)
        {
            whoWinsText.gameObject.SetActive(true);
            // hide any move prompts
            player1MoveText.SetActive(false);
            player2MoveText.SetActive(false);
            whoWinsText.text = "Player 2 Wins";
            gameOver = true;
        }
    }

    // Called from Dice.RollTheDice to let a player start moving
    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove)
        {
            case 1:
                player1.GetComponent<FollowThePath>().moveAllowed = true;
                break;
            case 2:
                player2.GetComponent<FollowThePath>().moveAllowed = true;
                break;
        }
    }
    public static void ResetGame()
    {
        diceSideThrown       = 0;
        player1StartWaypoint = 0;
        player2StartWaypoint = 0;
        gameOver             = false;
    }
}
