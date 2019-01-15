using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    //Texts that will be modified
    public Text txtRedPoints;
    public Text txtBluePoints;
    public Text txtWinner;
    public Text txtRestart;
    public Text txtDieValor;
    public Text txtBlueMoves;
    public Text txtRedMoves;
    public Text txtTutorial;

    //Counter for the point earned by each player at the end of the game
    public static int bluePoints;
    public static int redPoints;

    //Canvas GameObjects to activate and deactivate depending on the Fase or Subfase
    public GameObject RestartButton;
    public GameObject StartBtn;
    public GameObject DiceBtn;
    public GameObject ScoreMovetxt;
    public GameObject BlueTurnText;
    public GameObject RedTurnText;
    public GameObject WinnerText;
    public GameObject TutorialButton;
    public GameObject TutorialMSG;

    //Class references
    public GC GC;

    private int TutorialIsActive = 0;

    /// <summary>
    /// Sums one point on the score of the winning player at the end of the game
    /// </summary>
    public void AddPoints()
    {
        if (GC.IsOver != 0)
        {
            Debug.Log("Will not add anymore points.");
        }
        else
        {            
            if (GC.turnGC == EnumTurns.BluesTurn)
            {
                bluePoints++;
            }
            else
            {
                redPoints++;
            }            
        }
        GC.IsOver = 1;
        txtBluePoints.text = "Blue Points: " + bluePoints.ToString(); //passa a info dos pontos da Unity
        txtRedPoints.text = "Red Points: " + redPoints.ToString(); //passa a info dos pontos da Unity
    }

    /// <summary>
    /// changes the Canvas from inactive to active when the game is over, and show the color of the winner
    /// </summary>
    public void ShowWinner()
    {
        if (GC.faseGC == EnumFases.Ending)
        {
            WinnerText.SetActive(true);
            if (GC.turnGC == EnumTurns.BluesTurn)
            {
                txtWinner.text = "Blue player wins!";
            }
            else
            {
                txtWinner.text = "Red player wins!";
            }
        }
        else
        {
            WinnerText.SetActive(false);
        }
    }

    /// <summary>
    /// Show whose turn it is. 
    /// </summary>
    public void WhosTurn()
    {
        if (GC.faseGC != EnumFases.Ending && GC.subfaseGC != EnumSubFases.Inicial)
        {
            if (GC.turnGC == EnumTurns.BluesTurn)
            {
                BlueTurnText.SetActive(true);
                RedTurnText.SetActive(false);
            }
            else
            {
                BlueTurnText.SetActive(false);
                RedTurnText.SetActive(true);
            }
        }
        else
        {
            BlueTurnText.SetActive(false);
            RedTurnText.SetActive(false);
        }
    }

    /// <summary>
    /// Display or Hide the gameobject StartButton with the FinishHiding and StartGame buttons
    /// </summary>
    /// <param name="fase"></param>
    public void DisplayHidingButtons()
    {
        if (GC.faseGC == EnumFases.Hiding)
        {
            StartBtn.SetActive(true);
        }
        else
        {
            StartBtn.SetActive(false);
        }
    }

    /// <summary>
    /// Activates/Deactivates the button to Roll the Die.
    /// </summary>
    public void DisplayDice()
    {
        // txtDieValor.text = "Result: " + GC.diceValor.ToString();
        txtBlueMoves.text = "Moves left: " + GC.BlueMoves.ToString();
        txtRedMoves.text = "Moves left: " + GC.RedMoves.ToString();
        if ((GC.faseGC == EnumFases.FirstMove || GC.faseGC == EnumFases.Playing) && (GC.RolledDice == 1))
        {
            DiceBtn.SetActive(true);
        }
        else
        {
            DiceBtn.SetActive(false);
        }
    }

    /// <summary>
    /// Activates/Deactivates the text the shows the total of Scores and Movements a player has
    /// </summary>
    public void DisplayScoreNMoves()
    {
        txtBluePoints.text = "Score: " + bluePoints.ToString();
        txtRedPoints.text = "Score: " + redPoints.ToString();
        if (GC.faseGC == EnumFases.Hiding)
        {
            ScoreMovetxt.SetActive(false);
        }
        else
        {
            ScoreMovetxt.SetActive(true);
        }
    }

    /// <summary>
    /// Method for the Button FinishHiding that, when all 5 special blocks were placed will 
    /// change the Fase of the game, or the turn of the player.
    /// </summary>
    public void CLickFinishHide()
    {
        if (Blocks.blocksInserted != 5)
        {
            Debug.Log("Can't touch this!");
        }
        else
        {
            GC.subfaseGC = EnumSubFases.Final;
            Grid.CoverBoard();
            if (GC.faseGC == EnumFases.Hiding)
            {
                if (GC.turnGC == EnumTurns.BluesTurn)
                {
                    Blocks.blocksInserted = 0;
                    GC.subfaseGC = EnumSubFases.Flag;
                    GC.turnGC = EnumTurns.RedsTurn;
                }
                else
                {
                    GC.faseGC = EnumFases.FirstMove;
                    GC.turnGC = EnumTurns.BluesTurn;
                    Debug.Log("Fase = " + GC.faseGC);
                }
            }
            else
            {
                Debug.Log("Nao esta na fase correta.");
            }
        }
    }

    /// <summary>
    /// Method of the button that will Restart the game completly zeroing the scores
    /// </summary>
    public void ClickOnRestart()
    {
        GC.ReloadGame();
    }

    /// <summary>
    /// Allows the game to begging changing the Subfase and painting the board white.
    /// </summary>
    public void ClickedOnStart()
    {
        Grid.StartBoard();
        GC.subfaseGC = EnumSubFases.Flag;
    }

    /// <summary>
    /// Controls the display of these methods: DisplayHidingButtons, DisplayScoreNMoves, 
    /// DisplayDice, WhosTurn and ShowWinner
    /// </summary>
    public void UpdateDisplay()
    {
        DisplayHidingButtons();
        DisplayScoreNMoves();
        DisplayDice();
        WhosTurn();
        ShowWinner();
    }

    public void ClickedTutorial()
    {
        if (TutorialIsActive == 0)
        {
            TutorialMSG.SetActive(true);
            TutorialIsActive = 1;
        }
        else
        {
            TutorialMSG.SetActive(false);
            TutorialIsActive = 0;
        }

             
    }
}