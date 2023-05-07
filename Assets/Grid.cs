using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject gridTile,piece;
    public XAxis gridClass;
    public XAxis newPiece = null;
    public int xCount, yCount;
    public Vector2 changeInPosition;
    
    public float level;
    private void Start()
    {
        CreateGrid();
        StartCoroutine(ChangePositonByTime());
    }
   
    [ContextMenu("Random")]
    public void Instantiate()
    {


        Vector2 newGridPositon = new Vector2(Random.Range(0, xCount),yCount - 1);
        Tile tile = new Tile();
        


        tile.gameObject = Instantiate(piece,tile.postion,Quaternion.identity);
        gridClass.SetTilePosition(  tile,newGridPositon);



        XAxis grid = new XAxis(1,1);
        grid.SetTileGO(tile.gameObject, new Vector2(0, 0),newGridPositon);
        newPiece = (grid);
        
    }
   
    IEnumerator ChangePositonByTime()
    {
        while (true)
        {

            yield return new WaitForSeconds(level);
            if (newPiece != null)
            {
                bool success = gridClass.SetDown(newPiece);
                Debug.Log(success);
                if (!success)
                {
                    newPiece = null;
                }
            }
            
            

        }

    }
    
    public void CreateGrid()
    {
        float width = gridTile.transform.localScale.x,height = gridTile.transform.localScale.y;
        changeInPosition = new Vector2(width,height);
        
        gridClass = new(xCount,yCount);
        for (int i = 0; i < gridClass.x.Length; i++)
        {
            for (int j = 0; j < gridClass.x[i].y.Length; j++)
            {

                Vector2 position = new Vector2((i * width -
                    gridClass.x.Length * width / 2) + width / 2,



                    (j * height - gridClass.x[i].y.Length * height / 2) + height / 2);

                Instantiate(gridTile,position
                    
                    
                    , Quaternion.identity,transform);
                gridClass.x[i].y[j].postion = position;
                gridClass.x[i].y[j].gridPosition = new Vector2(i,j);
                
            }
        }

    }
}

[System.Serializable]
public class XAxis
{
        public Vector2 GetPosition(Vector2 gridPosition)
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
    public void SetTile(Vector2 prevGridPos,Vector2 change)
    {
        SetTilePosition(GetTile(prevGridPos), prevGridPos + change);
        GetTile(prevGridPos).gameObject = null;
    
    }
    public void SetTileGO(GameObject go, Vector2 GridPos,Vector2 mainGridPosition)
    {
        GetTile(GridPos).gameObject = go;

        GetTile(GridPos).gridPosition = mainGridPosition;

    }
    public void SetTilePosition(Tile tile,Vector2 newGridPos)
    {

        tile.gridPosition = newGridPos;
        tile.postion = GetPosition(newGridPos);
        tile.gameObject.transform.position = tile.postion;
        tile.gameObject.transform.Translate(Vector3.back * 1);

        GetTile(newGridPos).gameObject = tile.gameObject;

    }
    public bool SetDown(XAxis piece)
    {
        foreach (var item in piece.x)
        {
            foreach (var item2 in item.y)
            {

                
                if (item2.gridPosition.y ==0 || !IsTileNull(item2.gridPosition + Vector2.down*1))
                {
                    return false;
                }
            }
        }
        foreach (var item in piece.x)
        {
            foreach (var item2 in item.y)
            {
                SetTile(item2.gridPosition, Vector2.down * 1);
                item2.gridPosition += Vector2.down * 1; 
            }
        }
        return true;
    }
   
    public YAxis[] x;
    public XAxis(int xCount,int yCount)
    {
        x = new YAxis[xCount];
        for (int i = 0; i < x.Length; i++)
        {
            x[i] = new(yCount);
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
    public GameObject gameObject;
    public Vector2 postion;
    public Vector2 gridPosition;
   
}
[System.Serializable]
public class Piece
{
     
    public List<Tile> tiles = new();
}
