using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{

    public GameObject gridTile, piece;
    public XAxis gridClass;
    public XAxis newPiece;
    public int xCount, yCount;
    public Vector2 changeInPosition;

    public float level;
    public List<Color> colors;
    public InputSystem inputSystem;
    public InputSystem.IPlayerActionMapActions playerAction;
    private void Awake()
    {
        inputSystem = new();

    }
    private void Start()
    {
        CreateGrid();
        newPiece = null;
        Instantiate();
        StartCoroutine(ChangePositonByTime());
    }
    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.PlayerActionMap.Left.started += OnInputLeft;
        inputSystem.PlayerActionMap.Right.started += OnInputRight;
        inputSystem.PlayerActionMap.Down.started += OnInputDown;
        inputSystem.PlayerActionMap.Up.started += OnInputUp;
        inputSystem.PlayerActionMap.Space.started += OnInputSpace;
    }
       private void OnInputSpace(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        bool deadEnd = gridClass.ChangePiecePositionBy1(newPiece, Vector2.down, true);
        if (deadEnd)
        {
            newPiece = null;
            IsLayerCompleted();
            Instantiate();
        }
    }
    private void OnInputUp(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        RotatePiece();
    }

    private void OnInputRight(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        gridClass.ChangePiecePositionBy1(newPiece, Vector2.right);
    }

    private void OnInputLeft(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        gridClass.ChangePiecePositionBy1(newPiece, Vector2.left);
    }

    private void OnInputDown(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        bool deadEnd = gridClass.ChangePiecePositionBy1(newPiece, Vector2.down);
        if (deadEnd)
        {
            newPiece = null;
            IsLayerCompleted();

            Instantiate();
        }
    }
    IEnumerator ChangePositonByTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(level);

            if (newPiece != null)
            {
                bool deadEnd = gridClass.ChangePiecePositionBy1(newPiece, Vector2.down);
                if (deadEnd)
                {
                    newPiece = null;
                    IsLayerCompleted();
                    Instantiate();
                }
            }
        }
    }

    public void CreateGrid()
    {
        float width = gridTile.transform.localScale.x, height = gridTile.transform.localScale.y;
        changeInPosition = new Vector2(width, height);

        gridClass = new(xCount, yCount);
        for (int i = 0; i < gridClass.x.Length; i++)
        {
            for (int j = gridClass.x[i].y.Length - 1; j >= 0; j--)
            {

                Vector2 position = new Vector2((i * width -
                    gridClass.x.Length * width / 2) + width / 2,



                    (j * height - gridClass.x[i].y.Length * height / 2) + height / 2);

                Instantiate(gridTile, position, Quaternion.identity, transform);
                gridClass.x[i].y[j].postion = position;
                gridClass.x[i].y[j].GridPosition = new Vector2(i, j);

            }
        }
       
    }


    public void Instantiate()
    {
        XAxis grid;
        Vector2 newGridPositon;
        int prob = Random.Range(0, 11);
        Piece piece;
        if (prob <= 1)
        {
            piece = Piece.t;
        }
        else if (prob <= 3)
        {
            piece = Piece.an1;

        }
        else if (prob <= 5)
        {
            piece = Piece.an2;

        }
        else if (prob < 7)
        {
            piece = Piece.l;
        }
        else if (prob < 9)
        {
            piece = Piece.square;
        }
        else
        {
            piece = Piece.zeroByThree;
        }


        GetPieceInGrid(piece, out grid, out newGridPositon);

        gridClass.OnlyAssignGridPosition(grid, newGridPositon);
        if (gridClass.IsSpaceAvailable(grid))
        {
            newPiece = grid;
            gridClass.AddGrid(grid, newGridPositon);
            grid.SetColor(colors[Random.Range(0,colors.Count)]);
        }
        else
        {
            print(piece);
            print(newGridPositon);
            print(grid.xCount + " " + xCount);
            print(grid.yCount + " " + yCount);
            print("game over");
        }
    }

    public void RotatePiece()
    {
        if (newPiece != null)
        {
            XAxis rot = new XAxis(newPiece.x[0].y.Length, newPiece.x.Length);

            gridClass.RemovePieceGo(newPiece);
            for (int x = 0, xx = newPiece.x.Length - 1; x < newPiece.x.Length; x++)
            {
                for (int y = 0, yy = newPiece.x[0].y.Length - 1; y < newPiece.x[0].y.Length; y++)
                {

                    rot.x[y].y[xx].gameObject = newPiece.x[x].y[y].gameObject;
                    yy--;
                }
                xx--;
            }
            gridClass.OnlyAssignGridPosition(rot, newPiece.x[0].y[0].gridPosition);
            if (gridClass.IsSpaceAvailable(rot))
            {
                gridClass.AddGrid(rot, newPiece.x[0].y[0].gridPosition);
                newPiece = rot;
            }

            else
            {

                gridClass.AddGrid(newPiece, newPiece.x[0].y[0].gridPosition);
            }

        }

    }
    private void GetPieceInGrid(Piece pieceType, out XAxis grid, out Vector2 newGridPositon)
    {
        switch (pieceType)
        {

            case Piece.zeroByThree:
                grid = new(3, 1);
                newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(2, 0), Instantiate(piece), newGridPositon);
                grid.piece = pieceType;
                break;
            case Piece.t:
                grid = new(3, 2);
                newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(2, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 1), Instantiate(piece), newGridPositon);
                grid.piece = pieceType;

                break;
            case Piece.square:
                grid = new(2, 2);
                newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 1), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 1), Instantiate(piece), newGridPositon);
                grid.piece = pieceType;

                break;
            case Piece.l:
                grid = new(2, 3);
                newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 1), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 2), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 0), Instantiate(piece), newGridPositon);
                grid.piece = pieceType;


                break;
            case Piece.an1:
                grid = new(2, 3);
                newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 1), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 1), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 2), Instantiate(piece), newGridPositon);
                grid.piece = pieceType;


                break;
            case Piece.an2:
                grid = new(2, 3);
                newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 1), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 1), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 2), Instantiate(piece), newGridPositon);
                grid.piece = pieceType;


                break;
            default:
                grid = new(2, 2);
                newGridPositon = new Vector2(Random.Range(0, xCount - grid.x.Length + 1), yCount - grid.x[0].y.Length);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 0), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(0, 1), Instantiate(piece), newGridPositon);
                grid.SetGOAndGridPositionByIndex(new Vector2(1, 1), Instantiate(piece), newGridPositon);
                grid.piece = pieceType;

                break;
        }

    }


    List<int> GetFilledRow()
    {
        List<int> rowsIndex = new();
        for (int y = 0; y < yCount; y++)
        {
            for (int x = 0; x < xCount; x++)
            {

                if (gridClass.GetTile(new Vector2(x, y)).gameObject == null)
                {
                    if (y<3)
                    {
                        Debug.Log($"break bc of {x}, {y}");
                    }
                    
                    break;
                }

                if (x == xCount - 1)
                {

                    rowsIndex.Add(y);
                }
            }
        }
        return rowsIndex;
    }
    void IsLayerCompleted()
    {


        List<int> y = GetFilledRow();
        foreach (var item in y)
        {
            Debug.Log( $"filled at {item}");
        }
        foreach (var row in y)
        {
            for (int xx = 0; xx < gridClass.x.Length; xx++)
            {

                Destroy(gridClass.GetTile(new Vector2(xx, row)).gameObject.gameObject);

                gridClass.GetTile(new Vector2(xx, row)).gameObject = null;

                for (int yy= row; yy+1< yCount; yy++)
                {
                    gridClass.GetTile(new Vector2(xx, yy)).gameObject = gridClass.GetTile(new Vector2(xx, yy + 1)).gameObject;

                    gridClass.SetPosition(gridClass.GetTile(new Vector2(xx, yy)));

                }

            }
            
        }
       /* if (y == -1)
        {
            return;
        }

        for (int xx = 0; xx < gridClass.x.Length; xx++)
        {

            Destroy(gridClass.GetTile(new Vector2(xx, y)).gameObject.gameObject);

            gridClass.GetTile(new Vector2(xx, y)).gameObject = null;
            for (int yy = y + 1; yy < yCount; yy++)
            {
                gridClass.GetTile(new Vector2(xx, yy - 1)).gameObject = gridClass.GetTile(new Vector2(xx, yy)).gameObject;

                gridClass.SetPosition(gridClass.GetTile(new Vector2(xx, yy - 1)));

            }

        }*/
        //IsLayerCompleted();




    }


}

   


[System.Serializable]
public class XAxis
{
    public bool IsSpaceAvailable(XAxis piece)
    {

        foreach (var item in piece.x)
        {
            foreach (var item2 in item.y)
            {
                if (item2.gridPosition.x >= xCount || item2.gridPosition.y >= yCount)
                {
                    Debug.Log("count is bigger");
                    return false;
                }
                if (GetTile(item2.gridPosition).gameObject != null)
                {
                    Debug.Log($"place not available = {item2.gridPosition}");

                    return false;
                }

            }
        }
        return true;
    }
    public void RemovePieceGo(XAxis piece)
    {

        for (int i = 0; i < piece.x.Length; i++)
        {
            for (int j = 0; j < piece.x[i].y.Length; j++)
            {
                if (piece.x[i].y[j].gameObject != null)
                {
                    GetTile(piece.x[i].y[j].gridPosition).gameObject = null;
                }
            }
        }
    }
    public Vector2 GetWorldPosition(Vector2 gridPosition)
    {
        return x[(int)gridPosition.x].y[(int)gridPosition.y].postion;
    }
    public Tile GetTile(Vector2 gridPosition)

    {
        return x[(int)gridPosition.x].y[(int)gridPosition.y];
    }
    public bool IsTileNull(Vector2 gridPosition)
    {



        return GetTile(gridPosition).gameObject == null;
    }
    public void OnlyAssignGridPosition(XAxis newGrid, Vector2 newGridPosition)
    {
        for (int i = newGrid.x.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < newGrid.x[i].y.Length; j++)
            {

                newGrid.x[i].y[j].gridPosition = newGridPosition + Vector2.right * i - Vector2.down * j;

            }
        }

    }
    public void AddGrid(XAxis newGrid, Vector2 newGridPosition, bool isRight = false)
    {
        if (isRight)
        {
            for (int i = newGrid.x.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < newGrid.x[i].y.Length; j++)
                {
                    if (GetTile(newGrid.x[i].y[j].GridPosition).gameObject != null)
                    {
                        Debug.Log("Game Over");

                        return;
                    }
                    newGrid.x[i].y[j].gridPosition = newGridPosition + Vector2.right * i - Vector2.down * j;
                    GetTile(newGrid.x[i].y[j].GridPosition).gameObject = newGrid.x[i].y[j].gameObject;

                    SetPosition(GetTile(newGrid.x[i].y[j].GridPosition));
                }
            }

        }
        else
        {
            for (int i = 0; i < newGrid.x.Length; i++)
            {
                for (int j = 0; j < newGrid.x[i].y.Length; j++)
                {
                    if (GetTile(newGrid.x[i].y[j].GridPosition).gameObject != null)
                    {
                        Debug.Log("Game Over");

                        return;
                    }
                    newGrid.x[i].y[j].gridPosition = newGridPosition + Vector2.right * i - Vector2.down * j;
                    GetTile(newGrid.x[i].y[j].GridPosition).gameObject = newGrid.x[i].y[j].gameObject;

                    SetPosition(GetTile(newGrid.x[i].y[j].GridPosition));
                }
            }
        }

    }
    public void SetGOAndGridPositionByIndex(Vector2 index, GameObject go, Vector2 gridPositon)
    {
        x[(int)index.x].y[(int)index.y].gameObject = go;
        x[(int)index.x].y[(int)index.y].GridPosition = gridPositon + index;
    }
   /* public void SetGOAndGridPosition(Tile tile)
    {
        GetTile(tile.gridPosition).gameObject = tile.gameObject;
        SetTilePosition(tile);
    }*/
    public void ChangePiecePosition(Vector2 oldPosition, Vector2 change)
    {


        Tile oldTile = GetTile(oldPosition);
        Tile newTile = GetTile(oldPosition + change);

        newTile.gameObject = oldTile.gameObject;
        oldTile.gameObject = null;

        SetPosition(newTile);
    }
    public void SetPosition(Tile tile)
    {
        if (tile.gameObject == null)
        {
            return;
        }
        var obj = GetTile(tile.GridPosition).gameObject;
        obj.transform.position = GetWorldPosition(tile.GridPosition);
        obj.transform.Translate(Vector3.back * 1);
        tile.gameObject.name = tile.GridPosition.ToString();
        /* if (tile.gridPosition.y == 0)
         {

         tile.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
         }
         else if (tile.gridPosition.y == 1){
         tile.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

         }
         else if (tile.gridPosition.y == 2){
         tile.gameObject.GetComponent<SpriteRenderer>().color = Color.green;

         }else if (tile.gridPosition.y == 3){
         tile.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;

         }else if (tile.gridPosition.y == 4){
         tile.gameObject.GetComponent<SpriteRenderer>().color = Color.grey;

         }*/
    }

   

    /// <summary>
    /// 
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="direction"></param>
    /// <param name="infinity"></param>
    /// <returns>if deadend return true</returns>
    public bool ChangePiecePositionBy1(XAxis piece, Vector2 direction, bool infinity = false)
    {
        
        RemovePieceGo(piece);

        // Checking if empty space is available
        foreach (var item in piece.x)
        {
            foreach (var item2 in item.y)
            {

                if (item2.gameObject == null)
                {
                    continue;
                }
                if (direction == Vector2.down)
                {
                    if (item2.GridPosition.y == 0 || !IsTileNull(item2.GridPosition + Vector2.down))

                    {
                        AddGrid(piece, piece.x[0].y[0].gridPosition);


                        return true;

                    }
                }
                if (direction == Vector2.right)
                {
                    if (item2.GridPosition.x == x.Length - 1 || !IsTileNull(item2.gridPosition + Vector2.right))
                    {
                        AddGrid(piece, piece.x[0].y[0].gridPosition, isRight: true);
                        return true;
                    }
                }
                if (direction == Vector2.left)
                {
                    if (item2.GridPosition.x == 0 || !IsTileNull(item2.gridPosition + Vector2.left))
                    {
                        AddGrid(piece, piece.x[0].y[0].gridPosition);

                        return true;
                    }
                }

            }
        }
        
        
        if (direction == Vector2.right)
        {

            AddGrid(piece, piece.x[0].y[0].gridPosition + direction, isRight: true);
        }
        else
        {
            AddGrid(piece, piece.x[0].y[0].gridPosition + direction);
            if (infinity)
            {
                bool deadEnd= ChangePiecePositionBy1(piece, direction, infinity: true);
                return deadEnd;               /* if (!success)
                {
                }*/
            }
        }
        return false;
    }

    public Piece piece;
    public YAxis[] x;
    public int xCount, yCount;
    public XAxis(int xCount, int yCount)
    {
        this.xCount = xCount;
        this.yCount = yCount;
        x = new YAxis[xCount];
        for (int i = 0; i < x.Length; i++)
        {
            x[i] = new(yCount);
        }
    }
    public void SetColor(Color color)
    {
        foreach (var item in x)
        {
            foreach (var item1 in item.y)
            {
                if (item1.gameObject!=null)
                {
                item1.gameObject.GetComponent<SpriteRenderer>().color = color;

                }
            }
        }


    }
}

[System.Serializable]
public class YAxis
{
    public Tile[] y;
    public YAxis(int yCount)
    {
        y = new Tile[yCount];
        for (int i = 0; i < y.Length; i++)
        {
            y[i] = new();
        }
    }
}

[System.Serializable]
public class Tile
{
    public Tile()
    {

    }
    public Tile(GameObject gameObject, Vector2 gridPosition)
    {
        this.gameObject = gameObject;
        this.GridPosition = gridPosition;
    }
    public GameObject gameObject;
    public Vector2 postion;
    public Vector2 gridPosition;

    public Vector2 GridPosition { get => gridPosition; set { gridPosition = value; } }
}
public enum Direction
{
    down, right, left, up
}
public enum Piece
{
    none, zeroByThree, t, square, an1, an2, l
}
