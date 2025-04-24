using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
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
            ftp.transform.position = ftp.waypoints[destination].position;
            ftp.waypointIndex      = destination + 1;
            landed                 = destination;
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
            {
                ftp.transform.position = ftp.waypoints[destination].position;
                ftp.waypointIndex      = destination + 1;
                landed                 = destination;
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
}
