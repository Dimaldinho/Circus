using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private int whosTurn = 1;
    private bool coroutineAllowed = true;

    // Use this for initialization
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides");
        // show “6” face at start (index 5)
        rend.sprite = diceSides[5];
    }

    private void OnMouseDown()
    {
        // only roll if the game isn’t over and we’re not already rolling
        if (!GameControl.gameOver && coroutineAllowed)
            StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;

        // “shuffle” the dice faces 20 times
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);           // picks 0–5
            rend.sprite       = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        // store the final result (1–6) for your GameControl
        GameControl.diceSideThrown = randomDiceSide + 1;

        // move the correct player based on turn
        if (whosTurn == 1)
            GameControl.MovePlayer(1);
        else if (whosTurn == -1)
            GameControl.MovePlayer(2);

        // switch turn
        whosTurn *= -1;
        coroutineAllowed = true;
    }
}
