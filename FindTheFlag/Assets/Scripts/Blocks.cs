using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blocks : MonoBehaviour
{

    //The blocks types of textures
    public Sprite unclickedTexture;
    public Sprite flagTexture;
    public Sprite blockerTexture;
    public Sprite PlusOneTexture;
    public Sprite ClickedTexture;

    //References to Classes
    public UIController UIC;
    public GC GC;

    //Control variable to restrain the positions of the blocks that will be placed
    public bool IsBlue;
    public bool IsFirstLine;
    public bool IsPressed = false;

    public EnumBlocks blocktype;

    public static int Blue1stMove = 0; //0== not made, 1 == had the first move
    public static int Red1stMove = 0;

    //Counts the special blocks inserted
    public static int blocksInserted;

    /// <summary>
    /// Load the textures of each type of block
    /// </summary>
    public void LoadTextures()
    {
        if (GC.faseGC == EnumFases.Hiding && GC.subfaseGC == EnumSubFases.Final)
        {
            GetComponent<SpriteRenderer>().sprite = unclickedTexture;
            IsPressed = false;
        }
        else
        {
            if (this.blocktype == EnumBlocks.Empty)
            {
                GetComponent<SpriteRenderer>().sprite = unclickedTexture;
                IsPressed = false;
            }
            if (this.blocktype == EnumBlocks.Pressed)
            {
                GetComponent<SpriteRenderer>().sprite = ClickedTexture;
                IsPressed = true;
            }
            if (this.blocktype == EnumBlocks.Flag)
            {
                GetComponent<SpriteRenderer>().sprite = flagTexture;
                IsPressed = true;
            }
            if (this.blocktype == EnumBlocks.Blocker)
            {
                GetComponent<SpriteRenderer>().sprite = blockerTexture;
                IsPressed = true;
            }
            if (this.blocktype == EnumBlocks.PlusOne)
            {
                GetComponent<SpriteRenderer>().sprite = PlusOneTexture;
                IsPressed = true;
            }
        }
    }

    /// <summary>
    /// Used to click on the block the player wants to interacts with
    /// </summary>
    public void OnMouseDown()
    {
        if (GC.diceValor == 0 && GC.faseGC != EnumFases.Hiding)
        {
            Debug.Log("Must roll die first.");
        }
        else
        {
            if (GC.faseGC == EnumFases.Playing)
            {
                if (Grid.CheckPreviousMove((int)this.transform.localPosition.x, (int)this.transform.localPosition.y))
                {
                    if (Grid.CheckIfSpecial((int)this.transform.localPosition.x, (int)this.transform.localPosition.y))
                    {
                        LoadTextures();
                    }
                }
                else
                {
                    Debug.Log("Not suposed to click here: " + GC.faseGC);
                }
            }
            else
            {
                if (GC.faseGC == EnumFases.FirstMove)
                {
                    Testing1stLine();
                }
                else
                {
                    if (GC.faseGC == EnumFases.Hiding)
                    {
                        SelectingSpecialPositions();
                    }
                }
            }
            GC.ChangeTurn();
            GC.CheckIfFinished();
        }
    }

    /// <summary>
    /// Controls the fase when the player is hiding the special blocks. Will not allow the player to keep clicking after all five 
    /// special blocks where placed.
    /// </summary>
    /// <param name="x">Position of the current block on the X axis</param>
    /// <param name="y">Position of the current block on the Y axis</param>
    public void HidingBLocks(int x, int y)
    {
        if (blocksInserted >= 5)
        {
            if (GC.turnGC == EnumTurns.BluesTurn)
            {
                Debug.Log("Esperando jogador azul clicar para finalizar");
            }
            else
            {
                Debug.Log("Esperando jogador vermelho clicar para finalizar");
            }
        }
        else
        {
            if (Grid.AdjacentBlocks(x, y) == 0)
            {
                if (Grid.SideOfBoard(x, y) == 0)
                {
                    blocksInserted++;
                    MarkSpecialBlock();
                    AdvanceSubFase();
                }
            }
        }
    }

    /// <summary>
    /// Changes the Enum type of the block depending on the Fase, and the Subfase and calls the method 
    /// that will change the sprite of the block to correspond to the Enum type
    /// </summary>
    public void MarkSpecialBlock()
    {
        if (GC.faseGC == EnumFases.Hiding)
        {
            if (GC.subfaseGC == EnumSubFases.Inicial)
            {
                this.blocktype = EnumBlocks.Empty;
            }
            if (GC.subfaseGC == EnumSubFases.Flag)
            {
                this.blocktype = EnumBlocks.Flag;
            }
            if (GC.subfaseGC == EnumSubFases.Blocker1)
            {
                this.blocktype = EnumBlocks.Blocker;
            }
            if (GC.subfaseGC == EnumSubFases.Blocker2)
            {
                this.blocktype = EnumBlocks.Blocker;
            }
            if (GC.subfaseGC == EnumSubFases.PlusOne1)
            {
                this.blocktype = EnumBlocks.PlusOne;
            }
            if (GC.subfaseGC == EnumSubFases.PlusOne2)
            {
                this.blocktype = EnumBlocks.PlusOne;
            }
            LoadTextures();
        }
    }

    /// <summary>
    /// Controls the progression of the Subfases 
    /// </summary>
    public void AdvanceSubFase()
    {
        if (GC.subfaseGC == EnumSubFases.PlusOne2)
        {
            Debug.Log("Subfase: " + GC.subfaseGC);
        }
        else
        {
            if (GC.subfaseGC == EnumSubFases.PlusOne1)
            {
                GC.subfaseGC = EnumSubFases.PlusOne2;
            }
            else
            {
                if (GC.subfaseGC == EnumSubFases.Blocker2)
                {
                    GC.subfaseGC = EnumSubFases.PlusOne1;
                }
                else
                {

                    if (GC.subfaseGC == EnumSubFases.Blocker1)
                    {
                        GC.subfaseGC = EnumSubFases.Blocker2;
                    }
                    else
                    {
                        if (GC.subfaseGC == EnumSubFases.Flag)
                        {
                            GC.subfaseGC = EnumSubFases.Blocker1;
                        }
                        else
                        {
                            Debug.Log("Press Start.");
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Check what special block was placed, then pass to the next subfase and marks the block as its type (Flag, Blocker, or PlusOne)
    /// </summary>
    public void SelectingSpecialPositions()
    {
        if ((GC.subfaseGC == EnumSubFases.PlusOne2) && (blocksInserted == 4))
        {
            HidingBLocks((int)this.transform.localPosition.x, (int)this.transform.localPosition.y);
        }
        else
        {
            if ((GC.subfaseGC == EnumSubFases.PlusOne1) && (blocksInserted == 3))
            {
                HidingBLocks((int)this.transform.localPosition.x, (int)this.transform.localPosition.y);
            }
            else
            {
                if ((GC.subfaseGC == EnumSubFases.Blocker2) && (blocksInserted == 2))
                {
                    HidingBLocks((int)this.transform.localPosition.x, (int)this.transform.localPosition.y);
                }
                else
                {
                    if ((GC.subfaseGC == EnumSubFases.Blocker1) && (blocksInserted == 1))
                    {
                        HidingBLocks((int)this.transform.localPosition.x, (int)this.transform.localPosition.y);
                    }
                    else
                    {
                        if (GC.subfaseGC == EnumSubFases.Flag)
                        {
                            HidingBLocks((int)this.transform.localPosition.x, (int)this.transform.localPosition.y);
                        }
                    }
                }
            }
        }
    }

    public void Testing1stLine()
    {
        if (GC.turnGC == EnumTurns.BluesTurn && Blue1stMove == 0)
        {
            if (Grid.CheckIfFirstLine((int)this.transform.localPosition.x, (int)this.transform.localPosition.y))
            {
                if (Grid.CheckIfSpecial((int)this.transform.localPosition.x, (int)this.transform.localPosition.y))
                {
                    Blue1stMove++;
                    LoadTextures();
                }
            }
        }
        else if (GC.turnGC == EnumTurns.RedsTurn && Red1stMove == 0)
        {
            if (Grid.CheckIfFirstLine((int)this.transform.localPosition.x, (int)this.transform.localPosition.y))
            {
                if (Grid.CheckIfSpecial((int)this.transform.localPosition.x, (int)this.transform.localPosition.y))
                {
                    Red1stMove++;
                    LoadTextures();
                    Debug.Log("Eita");
                    //GC.faseGC = EnumFases.Playing;
                }
            }
        }
        else
        {
            if (Grid.CheckPreviousMove((int)this.transform.localPosition.x, (int)this.transform.localPosition.y))
            {
                if (Grid.CheckIfSpecial((int)this.transform.localPosition.x, (int)this.transform.localPosition.y))
                {
                    LoadTextures();               
                }
            }            
        }
    }

}