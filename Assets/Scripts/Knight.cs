using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Knight : ChessPiece
{
    public override List<Vector2Int> GetValidMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int x = currentPosition.x;
        int y = currentPosition.y;

        // Check all eight possible moves for a knight
        AddMoveIfValid(x + 2, y + 1, moves);
        AddMoveIfValid(x + 2, y - 1, moves);
        AddMoveIfValid(x - 2, y + 1, moves);
        AddMoveIfValid(x - 2, y - 1, moves);
        AddMoveIfValid(x + 1, y + 2, moves);
        AddMoveIfValid(x + 1, y - 2, moves);
        AddMoveIfValid(x - 1, y + 2, moves);
        AddMoveIfValid(x - 1, y - 2, moves);

        return moves;
    }

    private void AddMoveIfValid(int x, int y, List<Vector2Int> moves)
    {
        if (boardManager.IsWithinBounds(x, y))
        {
            ChessPiece pieceAtMove = boardManager.ChessPieces[x, y];
            if (pieceAtMove == null || pieceAtMove.color != color)
            {
                moves.Add(new Vector2Int(x, y));
            }
        }
    }
}
