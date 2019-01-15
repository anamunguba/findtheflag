using UnityEngine;

public class Grid
{
    //building the matrix
    public static int w = 10; //width 0 to 9
    public static int h = 10; //height 0 to 9
    public static Blocks[,] BlocksInMatrix = new Blocks[w, h];

    /// <summary>
    /// Used to reveal how the Board was set at the end of the game
    /// </summary>
    public static void UncoverBoard()
    {
        foreach (Blocks bcks in BlocksInMatrix)
        {
            bcks.LoadTextures();
        }
    }

    /// <summary>
    /// Hides the board so that the other player can't know where the special blocks are
    /// </summary>
    public static void CoverBoard()
    {
        foreach (Blocks bcks in BlocksInMatrix)
        {
            bcks.LoadTextures();
        }
    }

    /// <summary>
    /// Changes the color of SpriteRender on the whole board from gray-ish to white
    /// so the players cab know the game has started
    /// </summary>
    public static void StartBoard()
    {
        foreach (Blocks bcks in BlocksInMatrix)
        {
            bcks.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    /// <summary>
    /// Changes the color of SpriteRender on the whole board from white to gray
    /// so the players know they need to press Start to beging the game
    /// </summary>
    public static void PaintBoard()
    {
        foreach (Blocks bcks in BlocksInMatrix)
        {
            bcks.GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }

    /// <summary>
    /// Remarks the board as empty and reload the textures so the game can beging again.
    /// </summary>
    public static void ReloadBoard()
    {
        foreach (Blocks bcks in BlocksInMatrix)
        {
            bcks.MarkSpecialBlock();
            bcks.LoadTextures();
        }
    }

    /// <summary>
    /// Check if this is a special block. When hiding, so that two special blocks can't be inserted next to each other.
    /// When looking for a special block, so that the player can't look on their own board, i.e, blue player cliking on blue board
    /// </summary>
    /// <param name="x">X position on axis of the blocks that is beeing checked</param>
    /// <param name="y">Y position on axis of the blocks that is beeing checked</param>
    /// <returns></returns>
    public static bool SpecialBlock(int x, int y)
    {
        if (GC.faseGC == EnumFases.Hiding)
        {
            if (GC.turnGC == EnumTurns.BluesTurn)
            {
                if ((x >= 5) && (y >= 0) && (x < w) && (y < h) && (BlocksInMatrix[x, y].IsBlue))
                {
                    return (BlocksInMatrix[x, y].blocktype != EnumBlocks.Empty);
                }
            }
            else
            {
                if (x >= 0 && y >= 0 && x < 5 && y < h && !BlocksInMatrix[x, y].IsBlue)
                {
                    return (BlocksInMatrix[x, y].blocktype != EnumBlocks.Empty);
                }
            }
        }
        else
        {
            if (GC.turnGC == EnumTurns.BluesTurn)
            {
                if ((x >= 0) && (y >= 0) && (x < 5) && (y < h) && (!BlocksInMatrix[x, y].IsBlue))
                {
                    return true;    //will allow to insert block                
                }
                return false; //must not allow to insert                        
            }
            else
            {
                if ((x >= 5) && (y >= 0) && (x < w) && (y < h) && (BlocksInMatrix[x, y].IsBlue))
                {
                    return true; //This clicks on the blue board                
                }
                return false;
            }
        }
        return false; //Nao deixa inserir
    }

    /// <summary>
    /// Checks if this the correct side of the board to insert when Hiding a special block
    /// </summary>
    /// <param name="x">X position on axis of the blocks that is beeing checked</param>
    /// <param name="y">Y position on axis of the blocks that is beeing checked</param>
    /// <returns></returns>
    public static bool CheckSideOfBoard(int x, int y)
    {
        if (GC.faseGC == EnumFases.Hiding)
        {
            if (GC.turnGC == EnumTurns.BluesTurn)
            {
                if ((x >= 0) && (y >= 0) && (x < 5) && (y < h) && (!BlocksInMatrix[x, y].IsBlue))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if ((x >= 5) && (y >= 0) && (x < w) && (y < h) && (BlocksInMatrix[x, y].IsBlue))
                {
                    return true;
                }
                return false;//will not insert here    
            }
        }
        return false;
    }


    public static bool PreviousMove(int x, int y)
    {
        //if ((x >= 0) && (y >= 0) && (x < w) && (y < h) && BlocksInMatrix[x, y].blocktype == EnumBlocks.Pressed)
        if ((x >= 0) && (y >= 0) && (x < w) && (y < h) && BlocksInMatrix[x, y].IsPressed)
        {
            return true;
        }
        return false;
    }

    public static bool CheckPreviousMove(int x, int y)
    {
        if (PreviousMove(x, y + 1) || PreviousMove(x + 1, y) || PreviousMove(x, y - 1) || PreviousMove(x - 1, y))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Verifies that the first movement of the player is on the first line.
    /// </summary>
    /// <param name="x">X position on axis of the block that is beeing checked</param>
    /// <param name="y">Y position on axis of the block that is beeing checked</param>
    /// <returns></returns>
    public static bool CheckIfFirstLine(int x, int y)
    {
       // Debug.Log("FirstLine Fase: " + GC.faseGC);
        if (BlocksInMatrix[x, y].IsFirstLine)
        {
            //Debug.Log("Eh primeira linha");
            return true;
        }

        return false;
    }


    /// <summary>
    /// Checks if any of the adjacent blocks is special and the block itself
    /// </summary>
    ///     /// <param name="x">X position on axis of the block that is beeing checked</param>
    /// <param name="y">Y position on axis of the block that is beeing checked</param>
    /// <returns></returns>
    public static int AdjacentBlocks(int x, int y)
    {
        int count = 0;
        if (SpecialBlock(x, y)) ++count; //self
        if (SpecialBlock(x, y + 1)) ++count; // top       
        if (SpecialBlock(x + 1, y + 1)) ++count; // top-right
        if (SpecialBlock(x + 1, y)) ++count; // right        
        if (SpecialBlock(x + 1, y - 1)) ++count; // bottom-right        
        if (SpecialBlock(x, y - 1)) ++count; // bottom        
        if (SpecialBlock(x - 1, y - 1)) ++count; // bottom-left        
        if (SpecialBlock(x - 1, y)) ++count; // left        
        if (SpecialBlock(x - 1, y + 1)) ++count; // top-left   
        return count;
    }

    /// <summary>
    /// Checks if the block is being inserted on the correct side of the board.
    /// </summary>
    /// <param name="x">X position on axis of the block that is beeing checked</param>
    /// <param name="y">Y position on axis of the block that is beeing checked</param>
    /// <returns></returns>
    public static int SideOfBoard(int x, int y)
    {
        int count = 0;
        if (CheckSideOfBoard(x, y)) ++count; //self
        return count;
    }

    /// <summary>
    /// Checks what type of block was clicked on and do the action that each type of block requires.
    /// </summary>
    /// <param name="x">X position on axis of the block that is beeing checked</param>
    /// <param name="y">Y position on axis of the block that is beeing checked</param>
    /// <returns></returns>
    public static bool CheckIfSpecial(int x, int y)
    {
        if (GC.turnGC == EnumTurns.RedsTurn)
        {
            if (SpecialBlock(x, y))
            {
                if (BlocksInMatrix[x, y].blocktype == EnumBlocks.Flag)
                {
                    GC.faseGC = EnumFases.Ending;
                }
                else
                {
                    if (BlocksInMatrix[x, y].blocktype == EnumBlocks.Blocker)
                    {
                        GC.RedMoves = 0;
                    }
                    else
                    {
                        if (BlocksInMatrix[x, y].blocktype == EnumBlocks.PlusOne)
                        {
                            //GC.RedMoves++;
                            Debug.Log("Nao gasta movimento");
                        }
                        else
                        {
                            GC.RedMoves--;
                            BlocksInMatrix[x, y].blocktype = EnumBlocks.Pressed;
                        }
                    }
                }
                return true;
            }
        }
        else
        {
            if (SpecialBlock(x, y))
            {
                if (BlocksInMatrix[x, y].blocktype == EnumBlocks.Flag)
                {
                    GC.faseGC = EnumFases.Ending;
                }
                else
                {
                    if (BlocksInMatrix[x, y].blocktype == EnumBlocks.Blocker)
                    {
                        GC.BlueMoves = 0;
                    }
                    else
                    {
                        if (BlocksInMatrix[x, y].blocktype == EnumBlocks.PlusOne)
                        {
                            //GC.BlueMoves++;
                            Debug.Log("Nao gasta movimento");
                        }
                        else
                        {
                            GC.BlueMoves--;
                            BlocksInMatrix[x, y].blocktype = EnumBlocks.Pressed;
                            Debug.Log("Menos um movimento");
                        }
                    }
                }
                return true;
            }
        }
        return false;
    }
}