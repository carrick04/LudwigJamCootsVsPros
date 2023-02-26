using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public override List<Vector2Int> GetValidMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        int x = file;
        int y = rank;

        // Check moves along the diagonal in the positive x and y direction
        while (x < 7 && y < 7)
        {
            x++;
            y++;
            if (boardManager.IsSquareEmpty(x, y))
            {
                moves.Add(new Vector2Int(x, y));
            }
            else if (boardManager.IsOpponentPiece(x, y, color))
            {
                moves.Add(new Vector2Int(x, y));
                break;
            }
            else
            {
                break;
            }
        }

        // Check moves along the diagonal in the negative x and y direction
        x = file;
        y = rank;
        while (x > 0 && y > 0)
        {
            x--;
            y--;
            if (boardManager.IsSquareEmpty(x, y))
            {
                moves.Add(new Vector2Int(x, y));
            }
            else if (boardManager.IsOpponentPiece(x, y, color))
            {
                moves.Add(new Vector2Int(x, y));
                break;
            }
            else
            {
                break;
            }
        }

        // Check moves along the diagonal in the positive x and negative y direction
        x = file;
        y = rank;
        while (x < 7 && y > 0)
        {
            x++;
            y--;
            if (boardManager.IsSquareEmpty(x, y))
            {
                moves.Add(new Vector2Int(x, y));
            }
            else if (boardManager.IsOpponentPiece(x, y, color))
            {
                moves.Add(new Vector2Int(x, y));
                break;
            }
            else
            {
                break;
            }
        }

        // Check moves along the diagonal in the negative x and positive y direction
        x = file;
        y = rank;
        while (x > 0 && y < 7)
        {
            x--;
            y++;
            if (boardManager.IsSquareEmpty(x, y))
            {
                moves.Add(new Vector2Int(x, y));
            }
            else if (boardManager.IsOpponentPiece(x, y, color))
            {
                moves.Add(new Vector2Int(x, y));
                break;
            }
            else
            {
                break;
            }
        }

        // Check moves along the file (vertical)
        x = file;
        y = rank;
        while (y < 7)
        {
            y++;
            if (boardManager.IsSquareEmpty(x, y))
            {
                moves.Add(new Vector2Int(x, y));
            }
            else if (boardManager.IsOpponentPiece(x, y, color))
            {
                moves.Add(new Vector2Int(x, y));
                break;
            }
            else
            {
                break;
            }
        }
        x = file;
        y = rank;
        while (y > 0)
        {
            y--;
            if (boardManager.IsSquareEmpty(x, y))
            {
                moves.Add(new Vector2Int(x, y));
            }
            else if (boardManager.IsOpponentPiece(x, y, color))
            {
                moves.Add(new Vector2Int(x, y));
                break;
            }
            else
            {
                break;
            }

            // Check moves along the rank (horizontal)
            x = file;
            y = rank;
            while (x < 7)
            {
                x++;
                if (boardManager.IsSquareEmpty(x, y))
                {
                    moves.Add(new Vector2Int(x, y));
                }
                else if (boardManager.IsOpponentPiece(x, y, color))
                {
                    moves.Add(new Vector2Int(x, y));
                    break;
                }
                else
                {
                    break;
                }
            }
            x = file;
            y = rank;
            while (x > 0)
            {
                x--;
                if (boardManager.IsSquareEmpty(x, y))
                {
                    moves.Add(new Vector2Int(x, y));
                }
                else if (boardManager.IsOpponentPiece(x, y, color))
                {
                    moves.Add(new Vector2Int(x, y));
                    break;
                }
                else
                {
                    break;
                }
            }

            
        }
        return moves;
    }
}

