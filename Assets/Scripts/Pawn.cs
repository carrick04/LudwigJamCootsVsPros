using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override List<Vector2Int> GetValidMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        int x = file;
        int y = rank;

        // Check moves for white pawns
        if (color == Color.white)
        {
            if (y < 7 && boardManager.IsSquareEmpty(x, y + 1))
            {
                moves.Add(new Vector2Int(x, y + 1));

                if (y == 1 && boardManager.IsSquareEmpty(x, y + 2))
                {
                    moves.Add(new Vector2Int(x, y + 2));
                }
            }

            if (x > 0 && y < 7 && boardManager.IsOpponentPiece(x - 1, y + 1, color))
            {
                moves.Add(new Vector2Int(x - 1, y + 1));
            }

            if (x < 7 && y < 7 && boardManager.IsOpponentPiece(x + 1, y + 1, color))
            {
                moves.Add(new Vector2Int(x + 1, y + 1));
            }
        }
        // Check moves for black pawns
        else
        {
            if (y > 0 && boardManager.IsSquareEmpty(x, y - 1))
            {
                moves.Add(new Vector2Int(x, y - 1));

                if (y == 6 && boardManager.IsSquareEmpty(x, y - 2))
                {
                    moves.Add(new Vector2Int(x, y - 2));
                }
            }

            if (x > 0 && y > 0 && boardManager.IsOpponentPiece(x - 1, y - 1, color))
            {
                moves.Add(new Vector2Int(x - 1, y - 1));
            }

            if (x < 7 && y > 0 && boardManager.IsOpponentPiece(x + 1, y - 1, color))
            {
                moves.Add(new Vector2Int(x + 1, y - 1));
            }
        }

        return moves;
    }
}
