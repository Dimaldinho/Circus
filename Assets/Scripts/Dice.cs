using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Sprite[] diceSides;    // your 6 real faces
    private Sprite[] animFrames;   // all of your Anim_dice frames
    private SpriteRenderer rend;
    private int whosTurn = 1;
    private bool coroutineAllowed = true;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        // load the six real faces
        diceSides = Resources.LoadAll<Sprite>("DiceSides");
        if (diceSides == null || diceSides.Length < 6)
            Debug.LogError("DiceSides folder must contain 6 sprites named 1–6!");

        // load the animation frames
        animFrames = Resources.LoadAll<Sprite>("Anim_dice");
        if (animFrames == null || animFrames.Length == 0)
            Debug.LogError("No sprites found in Resources/Anim_dice!");

        // show “6” at start
        rend.sprite = diceSides[5];
    }

    private void OnMouseDown()
    {
        if (!GameControl.gameOver && coroutineAllowed)
            StartCoroutine(RollTheDice());
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;

        // shuffle by showing random animFrames
        for (int i = 0; i < 20; i++)
        {
            int rndFrame = Random.Range(0, animFrames.Length);
            rend.sprite = animFrames[rndFrame];
            yield return new WaitForSeconds(0.05f);
        }

        // now pick one of the six real faces
        randomDiceSide = Random.Range(0, 6);
        rend.sprite = diceSides[randomDiceSide];

        // store 1–6
        GameControl.diceSideThrown = randomDiceSide + 1;

        // move the correct player
        if (whosTurn == 1)
            GameControl.MovePlayer(1);
        else
            GameControl.MovePlayer(2);

        whosTurn *= -1;
        coroutineAllowed = true;
    }
}
