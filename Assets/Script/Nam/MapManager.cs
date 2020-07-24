using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILE_TYPE
{
    분홍_빈공간,
    파랑_걷는공간,
    검정_변형불가,
    노랑_톱니로부숴
}

public class MapManager : MonoBehaviour
{
    /// <summary>
    /// 전체 맵 타일 배열
    /// </summary>
    //public int[,] MapTiles;
    public MapTile[,] MapTiles;
    public MapTile clickTile;

    /// <summary>
    /// 최대 줄 (가로)
    /// </summary>
    public int maxx;
    /// <summary>
    /// 최대 열 (세로)
    /// </summary>
    public int maxy;

    [SerializeField] float _tilePadValue = 0.33f;

    public MapTile[] tilePrefab;

    public static MapManager instance;
    private void Awake()
    {
        instance = this;
    }

    /////// 임의로 타일을 생성하고자 활성화/비활성화로 처리해두었습니다.   ///////////////////
    private void OnEnable()
    {
        SetInitialize_MapTiles();

        DebugLog_MapTiles();
    }

    private void OnDisable()
    {
        GameObject obj = GameObject.Find("New Game Object");
        if (obj == null) return;
        Destroy(obj);
    }
    /////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 초기 맵을 생성합니다.
    /// </summary>
    private void SetInitialize_MapTiles()
    {
        //MapTiles = new int[maxx, maxy];
        MapTiles = new MapTile[maxx, maxy];

        GameObject motherTr = new GameObject();

        for (int i = 0; i < maxx; i++)
        {
            for(int j = 0; j < maxy; j++)
            {
                //임시로 랜덤 타일 생성
                int randTileIndex = Random.Range(0, (int)TILE_TYPE.노랑_톱니로부숴 + 1);
                if (randTileIndex == (int)TILE_TYPE.노랑_톱니로부숴 + 1) randTileIndex = (int)TILE_TYPE.노랑_톱니로부숴;

                //오브젝트 생성 및 위치 조정
                GameObject obj = Instantiate(tilePrefab[randTileIndex].gameObject);
                obj.transform.Translate(new Vector3(j * _tilePadValue, i * -_tilePadValue, 0), Space.World);
                //obj.name = "tile_" + i + "x" + j;
                obj.transform.parent = motherTr.transform;
                obj.SetActive(true);

                //타일 속성 정의
                MapTile newTile = obj.GetComponent<MapTile>();
                newTile.currX = i;
                newTile.currY = j;
                newTile.myTileType = (TILE_TYPE)System.Enum.ToObject(typeof(TILE_TYPE), randTileIndex); ;

                //배열 저장
                MapTiles[i, j] = newTile;
            }
        }

        //생성한 타일들을 중심 좌표로 보정 이동
        float corrX = _tilePadValue * (maxy - 1) * 0.5f;
        float corrY = _tilePadValue * (maxx - 1) * 0.5f;

        motherTr.transform.Translate(new Vector3(-corrX, corrY, 0), Space.World);

    }

    private void DebugLog_MapTiles()
    {
        string alltemp = "";
        for(int i = 0; i < MapTiles.GetLength(0); i++)
        {
            string temp = "";
            for(int j = 0; j < MapTiles.GetLength(1); j++)
            {
                temp += (int)MapTiles[i, j].myTileType;
            }
            alltemp += temp + "\n";
        }
        Debug.Log(alltemp);
    }

    /// <summary>
    /// 클릭한 타일의 인덱스 값을 반환합니다.
    /// </summary>
    /// <param name="clickTile">클릭한 타일 오브젝트입니다.</param>
    public int[] Get_TileIndex(MapTile clickTile)
    {
        int[] tileXY = { clickTile.currX, clickTile.currY };

        Debug.Log(tileXY[0] + ", " + tileXY[1] + " [" + clickTile.myTileType.ToString() + "]");

        return tileXY;
    }

    /// <summary>
    /// 이동할 수 있는 칸인가요? 
    /// </summary>
    public bool Get_Passible(int x, int y)
    {
        if (x < 1 || y < 1) return false;
        if (x >= maxx || y >= maxy) return false;

        if (MapTiles[x, y].myTileType.Equals(TILE_TYPE.파랑_걷는공간)) return false;
        if (MapTiles[x, y].myTileType.Equals(TILE_TYPE.노랑_톱니로부숴)) return false;

        return true;
    }

}
