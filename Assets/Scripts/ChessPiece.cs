using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 mouseOffset;
    public int CurrentX;
    public int CurrentY;

    public Color color;
    public Vector2Int currentPosition;
    public Vector3 currentTransformPos;
    public BoardManager boardManager;
    public Sprite darkSprite;
    public Sprite lightSprite;

    public int file;
    public int rank;

    public bool myTurn;

    private void Awake()
    {
        boardManager = BoardManager.Instance;
        //currentTransformPos = transform.position;
        if (color != null)
        {
            if (color == Color.white)
            {
                GetComponent<SpriteRenderer>().sprite = lightSprite;
            }
            if (color == Color.black)
            {
                GetComponent<SpriteRenderer>().sprite = darkSprite;
            }
        }
    }

    private void Update()
    {
        myTurn = boardManager.whosTurn == color;

        Debug.Log(file + " " + rank);
    }

    private void OnMouseDown()
    {
        if (!myTurn) return;
        isDragging = true;
        boardManager.SelectPiece(this);
        file = boardManager.SelectedPieceX;
        file = boardManager.SelectedPieceY;

        //Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("ChessSquare"));

        /*
        Collider2D[] hits = Physics2D.OverlapCircleAll(boardManager.SelectedPiece.currentPosition, 0.1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.CompareTag("ChessSquare"))
            {
                currentTransformPos = hit.transform.position;
                currentPosition = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.y);
            }

        }
        */
        //currentPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        mouseOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);        
        HighlightLegalSquares();
              
    }

    private void OnMouseUp()
    {
        if (!myTurn) return;

        isDragging = false;

        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.RoundToInt(mousePos.x);
        int y = Mathf.RoundToInt(mousePos.y);

        List<Vector2Int> validMoves = GetValidMoves();
        foreach (Vector2Int move in validMoves)
        {
            if (move.x == x && move.y == y)
            {
                boardManager.MoveSelectedPiece(x, y);
                transform.position = new Vector3(x, 0, y);
                boardManager.whosTurn = GetOponentColor(color);
                return;
            }
        }

        transform.position = new Vector3(file, rank);
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + mouseOffset;
            transform.position = mousePos;

            
            if (!boardManager.IsWithinBounds((int)mousePos.x, (int)mousePos.y))
            {
                // Snap the piece back to its original position if it is dragged outside the bounds of the chessboard
                transform.position = currentTransformPos;
            }
            
        }
    }

    public abstract List<Vector2Int> GetValidMoves();

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
        transform.position = new Vector3(x, 0, y);
    }

    public void HighlightLegalSquares()
    {
        ChessPiece myPiece = this;
        List<Vector2Int> validMoves = myPiece.GetValidMoves();
        foreach (Vector2Int move in validMoves)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(move, 0.1f);
            foreach (Collider2D hit in hits)
            {
                if (hit.tag == "ChessSquare")
                hit.GetComponent<SpriteRenderer>().color = Color.yellow;

            }
        }
    }
    
    public Color GetOponentColor(Color c)
    {
        if (c == Color.white)
        {
            return Color.black;
        }
        else
        {
            return Color.white;
        }
    }
}