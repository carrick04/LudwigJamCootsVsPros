using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get;  set; }
    public GameObject Board;

    public ChessPiece[,] ChessPieces { get; private set; }
    public ChessPiece SelectedPiece { get; private set; }
    public int SelectedPieceX { get; private set; }
    public int SelectedPieceY { get; private set; }
    public int[,] Pieces { get; private set; }

    private const int BoardSize = 8;
    private int tileSize = 1;
    public Color whosTurn;
    public Dictionary<System.Type, GameObject> chessPiecePrefabDict;

    public GameObject pawnPrefab;
    public GameObject rookPrefab;
    public GameObject knightPrefab;
    public GameObject bishopPrefab;
    public GameObject queenPrefab;
    public GameObject kingPrefab;
    
    //public ChessBoardScript boardScript;
    [SerializeField] private Color lightCol;
    [SerializeField] private Color darkCol;
    [SerializeField] private Sprite squareSprite;
    [SerializeField] private const string SQUARE_SORTING_LAYER = "ChessSquare";
    [SerializeField] private const string SQUARE_TAG = "ChessSquare";
    private void Awake()
    {
        Instance = this;

        chessPiecePrefabDict = new Dictionary<System.Type, GameObject>();       
        chessPiecePrefabDict.Add(typeof(Pawn), pawnPrefab);      
        chessPiecePrefabDict.Add(typeof(Rook), rookPrefab);       
        chessPiecePrefabDict.Add(typeof(Knight), knightPrefab);
        chessPiecePrefabDict.Add(typeof(Bishop), bishopPrefab);
        chessPiecePrefabDict.Add(typeof(Queen), queenPrefab);        
        chessPiecePrefabDict.Add(typeof(King), kingPrefab);

        whosTurn = Color.white;
    }

    private void Start()
    {
        ChessPieces = new ChessPiece[BoardSize, BoardSize];
        InitializeBoard(); 
        
        
        
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    Debug.Log(Pieces[i, j]);
                }              
            }
        
    }

    private void Update()
    {
        Debug.Log(SelectedPiece);
        Debug.Log(whosTurn);
        
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
    

    private void InitializeBoard()
    {
        // Create and place the chess pieces on the board
        Pieces = new int[BoardSize, BoardSize]; // initialize the Pieces array with the size of the chess board

        CreateGraphicalBoard(BoardSize);
        

        
        // Initialize and place the pawns
        for (int i = 0; i < BoardSize; i++)
        {
            SpawnPiece<Pawn>(i, 1, Color.white); // spawn a white pawn at (i, 1)
            SpawnPiece<Pawn>(i, 6, Color.black); // spawn a black pawn at (i, 6)
        }
        
        
        // Initialize and place the rooks
        SpawnPiece<Rook>(0, 0, Color.white); // spawn a white rook at (0, 0)
        SpawnPiece<Rook>(7, 7, Color.black); // spawn a black rook at (7, 7)
        
        // Initialize and place the knights
        SpawnPiece<Knight>(1, 0, Color.white); // spawn a white knight at (1, 0)
        SpawnPiece<Knight>(6, 7, Color.black); // spawn a black knight at (6, 7)

        // Initialize and place the bishops
        SpawnPiece<Bishop>(2, 0, Color.white); // spawn a white bishop at (2, 0)
        SpawnPiece<Bishop>(5, 7, Color.black); // spawn a black bishop at (5, 7)

        // Initialize and place the queens
        SpawnPiece<Queen>(3, 0, Color.white); // spawn a white queen at (3, 0)
        
        // Initialize and place the kings
        SpawnPiece<King>(4, 0, Color.white); // spawn a white king at (4, 0)
        SpawnPiece<King>(7, 3, Color.black); // spawn a black king at (4, 7)
    }

    private void SpawnPiece<T>(int file, int rank, Color color) where T : ChessPiece
    {
        try
        {
            // Instantiate the chess piece prefab
            if (!chessPiecePrefabDict.TryGetValue(typeof(T), out GameObject prefab))
            {
                Debug.LogError($"No prefab found for type {typeof(T).Name}");
                return;
            }

            GameObject go = Instantiate(prefab, Board.transform);
            go.transform.localPosition = new Vector3(file * tileSize, rank * tileSize);

            // Set the chess piece's properties
            if (!go.TryGetComponent(out ChessPiece chessPiece))
            {
                Debug.LogError($"No ChessPiece component found on {typeof(T).Name} prefab");
                Destroy(go);
                return;
            }

            chessPiece.file = file;
            chessPiece.rank = rank;
            chessPiece.color = color;
            chessPiece.GetComponent<SpriteRenderer>().sprite = (chessPiece.color == Color.white) ? chessPiece.lightSprite : chessPiece.darkSprite;

            // Update the ChessPieces array
            ChessPieces[file, rank] = chessPiece;
            Pieces[file, rank] = 1;
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while spawning {typeof(T).Name} at ({file}, {rank}): {ex}");
            return;
        }
    }


    public void SelectPiece(ChessPiece piece)
    {
        //SelectedPiece = null;
        SelectedPiece = piece; // set the selected piece to the piece at (x, y) on the chess board
        SelectedPieceX = piece.currentPosition.x; // store the x-coordinate of the selected piece       
        SelectedPieceY = piece.currentPosition.y; // store the y-coordinate of the selected piece
        
    }

    public void MoveSelectedPiece(int x, int y)
    {
        if (SelectedPiece == null)
        {
            Debug.LogError("SelectedPiece is null!");
            return;
        }

        ChessPieces[x, y] = SelectedPiece; // set the piece at (x, y) on the chess board to the selected piece
        ChessPieces[SelectedPieceX, SelectedPieceY] = null; // set the original position of the selected piece to null
        SelectedPiece.SetPosition(x, y); // update the position of the selected piece to (x, y)
        SelectedPiece = null; // clear the selected piece variable
        Debug.Log("Selected Piece Moved");
    }

    public bool IsSquareEmpty(int file, int rank)
    {
        // Get the piece at the given position on the board
        ChessPiece piece = GetPieceAtPosition(file, rank);

        // Return true if the position is empty (i.e. no piece is present)
        return piece == null;
    }

    public bool IsOpponentPiece(int file, int rank, Color color)
    {
        // Get the piece at the given position on the board
        ChessPiece piece = GetPieceAtPosition(file, rank);

        // Return false if the position is empty (i.e. no piece is present)
        if (piece == null)
        {
            return false;
        }

        // Return true if the piece at the position has a different color than the given color
        return piece.color != color;
    }

    private ChessPiece GetPieceAtPosition(int file, int rank)
    {
        // Return null if the given position is out of bounds
        if (file < 0 || file > 7 || rank < 0 || rank > 7)
        {
            return null;
        }

        // Return the chess piece at the given position on the board
        return ChessPieces[file, rank];
    }

    private Vector3 GetTileCenter(int file, int rank)
    {
        // Calculate the position of the center of the tile at the given file and rank
        Vector3 center = Vector3.zero;
        center.x += (file * tileSize) + (tileSize / 2);
        center.z += (rank * tileSize) + (tileSize / 2);
        return center;
    }

    public bool IsWithinBounds(int x, int y)
    {
        // Return true if the given position is within the bounds of the board (i.e. on the 8x8 grid)
        return x >= 0 && x < 8 && y >= 0 && y < 8;
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Vector3 position = new Vector3(x, y, 0);
                Gizmos.DrawWireCube(position, Vector3.one);
            }
        }
        OnDrawGizmosSelected();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Vector3 position = new Vector3(x, y, 0);
                Gizmos.DrawWireCube(position, Vector3.one);
            }
        }
    }
    */
    
}
