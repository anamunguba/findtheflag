using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Até esta versão ainda não defini que o jogador deve caminha em linha reta (frente ou trás) no trabuleiro.
//Nem que ele deve iniciar pelos blocos superiores.
public class GC : MonoBehaviour
{
    //public int diceBlue;
    //public int diceRed;
    //public static int diceSum; //Will use this if I decide to have to dice on the game
    public static int diceValor;
    public static int RolledDice; // 0 == deactivate die button, 1 == activate die button

    //Amount of Moves still avaiable of each player
    public static int BlueMoves;
    public static int RedMoves;

    //References to the Enums
    public static EnumTurns turnGC;
    public static EnumFases faseGC;
    public static EnumSubFases subfaseGC;

    //To control the additon of scores
    public int IsOver; //0 == not over, 0!= over

    //Class reference
    public UIController UIC;

    // Use this for initialization
    void Start()
    {

        //This is creating the Matrix and initiating the game of the 1st time
        GameObject[] arblocks = GameObject.FindGameObjectsWithTag("block");
        foreach (GameObject item in arblocks)
        {
            int x = Mathf.Abs((int)item.transform.localPosition.x);
            int y = Mathf.Abs((int)item.transform.localPosition.y);
            Grid.BlocksInMatrix[x, y] = item.GetComponent<Blocks>();
        }
        ReloadGame();
    }

    // Update is called once per frame
    void Update()
    {
        UIC.UpdateDisplay();
    }

    /// <summary>
    /// Used when the die button is pressed to randomly get the number of moves from 1 to 5
    /// </summary>
    public void RollDie()
    {
        RedMoves = 0;
        BlueMoves = 0;
        diceValor = Random.Range(1, 6);
        RedMoves = diceValor;
        BlueMoves = diceValor;
        RolledDice = 0;
    }

    /// <summary>
    /// Checks if the game is over, if so, calls the method to Add the Points to the winner, reval the board and allow a new game.
    /// </summary>
    public void CheckIfFinished()
    {        
            if (GC.faseGC == EnumFases.Ending)
            {                
                Debug.Log("CheckIfFinished");
                UIC.AddPoints();
                Grid.UncoverBoard();
                UIC.txtRestart.text = "New Game";
            }                  
    }

    /// <summary>
    /// Allows the game to be played again, without loosing the information about the scores.
    /// </summary>
    public void ReloadGame()
    {
        IsOver = 0;
        UIC.txtRestart.text = "Restart Game";
        Blocks.Blue1stMove = 0;
        Blocks.Red1stMove = 0;
        Grid.PaintBoard();
        subfaseGC = EnumSubFases.Inicial;
        faseGC = EnumFases.Hiding;
        turnGC = EnumTurns.BluesTurn;
        diceValor = 0;
        BlueMoves = 0;
        RedMoves = 0;
        RolledDice = 1;
        Blocks.blocksInserted = 0;
        Grid.ReloadBoard();
        UIC.TutorialMSG.SetActive(false);
        /*if (faseGC != EnumFases.Ending)
        {
            UIController.bluePoints = 0;
            UIController.redPoints = 0;
        }*/
    }

    /// <summary>
    /// Checks whos turn it is and pass to the next.
    /// </summary>
    public void ChangeTurn()
    {
        if (faseGC != EnumFases.Hiding)
        {
            if (turnGC == EnumTurns.BluesTurn)
            {
                if (BlueMoves <= 0)
                {
                    BlueMoves = 0;
                    turnGC = EnumTurns.RedsTurn;
                }
            }
            else
            {
                if (RedMoves <= 0)
                {
                    RedMoves = 0;
                    diceValor = 0;
                    RolledDice = 1;
                    turnGC = EnumTurns.BluesTurn;
                }
            }
        }
    }
}