using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class King : ChessPiece
{
    public override List<Vector2Int> GetValidMoves()
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();
        Vector2Int[] directions =
        {
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, -1)
        };
        foreach (Vector2Int direction in directions)
        {
            Vector2Int destination = new Vector2Int(file, rank) + direction;
            if (boardManager.IsWithinBounds(destination.x, destination.y))
            {
                if (boardManager.Pieces[destination.x, destination.y] == 0)
                {
                    validMoves.Add(destination);
                }
                else if (boardManager.ChessPieces[destination.x, destination.y].color != color)
                {
                    validMoves.Add(destination);
                }
            }
        }
        return validMoves;
    }
}

