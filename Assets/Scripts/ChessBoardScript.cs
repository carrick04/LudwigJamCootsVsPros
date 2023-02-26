using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardScript : MonoBehaviour
{
    [SerializeField] private Color lightCol;
    [SerializeField] private Color darkCol;
    [SerializeField] private Sprite squareSprite;
    private const string SQUARE_SORTING_LAYER = "ChessSquare";
    private const string SQUARE_TAG = "ChessSquare";

    [SerializeField] private GameObject blackKingPrefab;
    [SerializeField] private GameObject whiteKingPrefab;

    private void Start()
    {
        //CreateGraphicalBoard();
    }
    public void CreateGraphicalBoard(int boardSize)
    {
        for (int file = 0; file < boardSize; file++)
        {
            for (int rank = 0; rank < boardSize; rank++)
            {
                bool isLightSquare = (file + rank) % 2 != 0;

                var squareColour = (isLightSquare) ? lightCol : darkCol;
                var position = new Vector2(/*-3.5f +*/ file,/* -3.5f + */ rank);

                DrawSquare(squareColour, position, SQUARE_SORTING_LAYER, 0);
            }
        }
        /*
        AddChessPiece(blackKingPrefab, new Vector2(4.5f, 7.5f));
        AddChessPiece(whiteKingPrefab, new Vector2(4.5f, 0.5f));
        */
    }

    void DrawSquare(Color squareColour, Vector2 position, string sortingLayerName, int sortingOrder)
    {
        GameObject square = new GameObject("Square");
        square.transform.parent = transform;
        square.transform.localPosition = position;

        BoxCollider2D boxCollider2D = square.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;

        SpriteRenderer sr = square.AddComponent<SpriteRenderer>();
        sr.sprite = squareSprite;
        sr.color = squareColour;

        // Assign the sorting layer and order + tag
        sr.sortingLayerName = sortingLayerName;
        sr.sortingOrder = sortingOrder;
        sr.tag = SQUARE_TAG;
    }

    private void AddChessPiece(GameObject chessPiecePrefab, Vector2 position)
    {
        GameObject chessPiece = Instantiate(chessPiecePrefab, position, Quaternion.identity);
        chessPiece.transform.SetParent(transform);
    }
}


