using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public override List<Vector2Int> GetValidMoves()
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();

        
        // Add valid moves along the current rank
    for (int i = 0; i < 8; i++)
        {
            if (i != file)
            {
                Vector2Int move = new Vector2Int(i, rank);
                validMoves.Add(move);
            }
        }

        // Add valid moves along the current file
        for (int i = 0; i < 8; i++)
        {
            if (i != rank)
            {
                Vector2Int move = new Vector2Int(file, i);
                validMoves.Add(move);
            }
        }

        return validMoves;
    }
}