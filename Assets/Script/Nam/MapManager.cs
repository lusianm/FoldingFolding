﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILE_TYPE
{
    분홍_빈공간,
    파랑_걷는공간,
    검정_변형불가,
    노랑_톱니로부숴
}

public enum Sub_ObjTYPE
{
    가시,
    플레이어,
    깃발,
    톱니
}

public enum Sub_Direction
{
    UP,
    RIGHT,
    DOWN,
    LEFT
}

[System.Serializable]
public struct SubObjData
{
    public Vector2 posIndex;
    public Sub_Direction objDir;
    public Sub_ObjTYPE objType;
}

public class MapManager : MonoBehaviour
{
    [Header("Map Data")]
    /// <summary>
    /// 전체 맵 타일 배열
    /// </summary>
    //public int[,] MapTiles;
    public MapTile[,] MapTiles;

    /// <summary>
    /// 최대 줄 (가로)
    /// </summary>
    public int maxx;
    /// <summary>
    /// 최대 열 (세로)
    /// </summary>
    public int maxy;

    /// <summary>
    /// 미리 생성할 맵 타일 속성값 문자열
    /// </summary>
    [TextArea(3, 7)]
    public string _inputTileData;

    /// <summary>
    /// 맵에 배치할 보조 오브젝트들의 리스트 (가시, 플레이어, ...)
    /// </summary>
    public List<SubObjData> objsData;

    public static MapManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetInitialize_MapTiles();
        DebugLog_MapTiles();
    }

    /////// 임의로 타일을 생성하고자 활성화/비활성화로 처리해두었습니다.   ///////////////////
    /////// 고정된 값으로 생성하기 위해 _inputTileData 필드를 사용해주세요.
    /*
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
    */
    /////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 초기 맵을 생성합니다.
    /// </summary>
    public void SetInitialize_MapTiles()
    {
        List<int[]> tileArr = null;
        float _tilePadValue = GameManager.instance.tilePadValue;
        
        //이동 후 처리를 위한 변수 모음
        Player player = null;
        List<Saw> saws = new List<Saw>();
        List<int[]> sawsIndex = new List<int[]>();

        //비어 있지 않을 경우 입력값으로 고정 생성
        if (!(_inputTileData.Equals("") || _inputTileData == null))
        {
            tileArr = new List<int[]>();
            tileArr = GetParse_InputTileData();

            maxy = tileArr.Count;
            maxx = tileArr[0].Length;
        }

        MapTiles = new MapTile[maxx, maxy];

        GameObject motherTr = new GameObject();

        for (int i = 0; i < maxy; i++)
        {
            for(int j = 0; j < maxx; j++)
            {
                int randTileIndex;

                //임시로 랜덤 타일 생성
                if (tileArr == null)
                {
                    randTileIndex = Random.Range(0, (int)TILE_TYPE.노랑_톱니로부숴 + 1);
                    if (randTileIndex == (int)TILE_TYPE.노랑_톱니로부숴 + 1) randTileIndex = (int)TILE_TYPE.노랑_톱니로부숴;
                }
                //입력한 타일 데이터 호출
                else
                    randTileIndex = tileArr[i][j];

                //오브젝트 생성 및 위치 조정
                GameObject obj = Instantiate(GameManager.instance.tilePrefabList[randTileIndex]);
                obj.transform.Translate(new Vector3(j * _tilePadValue, i * -_tilePadValue, 0), Space.World);
                //obj.name = "tile_" + i + "x" + j;
                obj.transform.parent = motherTr.transform;
                obj.SetActive(true);

                //타일 속성 정의
                MapTile newTile = obj.GetComponent<MapTile>();
                newTile.currX = j;
                newTile.currY = i;
                newTile.myTileType = (TILE_TYPE)System.Enum.ToObject(typeof(TILE_TYPE), randTileIndex); ;

                //배열 저장
                MapTiles[j, i] = newTile;
            }
        }

        #region 서브 오브젝트 배치
        for (int i = 0; i < objsData.Count; i++)
        {
            GameObject target = MapTiles[(int)objsData[i].posIndex.x, (int)objsData[i].posIndex.y].gameObject;

            //오브젝트 생성
            GameObject subObj = Instantiate(GameManager.instance.prefabList[(int)objsData[i].objType].gameObject);
            if (subObj == null) continue;

            subObj.transform.parent = target.transform;
            subObj.transform.localPosition = Vector3.zero;

            switch (objsData[i].objType)
            {
                /*
                //방향성 설정 : 가시만
                case Sub_ObjTYPE.가시:
                    float[,] dirCorr = { { 0, 0.071f }, { 0.054f, 0 }, { 0, -0.03f }, { -0.034f, 0 } };
                    int dirIndex = (int)objsData[i].objDir;
                    subObj.transform.localPosition = new Vector3(dirCorr[dirIndex, 0], dirCorr[dirIndex, 1], 0);
                    subObj.transform.localEulerAngles = new Vector3(0, 0, 180 - 90 * (int)objsData[i].objDir);
                    
                    break;
                */
                case Sub_ObjTYPE.플레이어:
                    player = subObj.GetComponent<Player>();
                    player.SetPlayer((int)objsData[i].posIndex.x, (int)objsData[i].posIndex.y, (int)objsData[i].objDir);
                    break;
                case Sub_ObjTYPE.톱니:
                    saws.Add(subObj.GetComponent<Saw>());
                    sawsIndex.Add(new int[] { (int)objsData[i].posIndex.x, (int)objsData[i].posIndex.y, (int)objsData[i].objDir });
                    //이동 보정 후 초기 값 설정
                    //...
                    break;
                default:
                    subObj.GetComponent<IInteractableObject>().ObjectInit((int)objsData[i].posIndex.x, (int)objsData[i].posIndex.y, (int)objsData[i].objDir);
                    break;
            }
        }
        #endregion

        //생성한 타일들을 중심 좌표로 보정 이동
        float corrX = _tilePadValue * (maxx - 1) * 0.5f;
        float corrY = _tilePadValue * (maxy - 1) * 0.5f;

        motherTr.transform.Translate(new Vector3(-corrX, corrY, 0), Space.World);

        //부모 좌표 초기화 ( 자식 분리 > 부모 좌표 초기화 > 자식 재설정
        Set_MotherVectorZero(motherTr);

        //플레이어의 경우 부모 종속 초기화
        player.transform.parent = null;

        //톱의 경우 이동 후 초기화
        for(int i = 0; i < saws.Count; i++)
        {
            saws[i].transform.parent = null;
            saws[i].GetComponent<IInteractableObject>().ObjectInit(sawsIndex[0][0], sawsIndex[0][1], sawsIndex[0][2]);

            saws[i].isReady = true;
        }
    }

    /// <summary>
    /// 부모 좌표를 0으로 재설정합니다. 자식 오브젝트와 분리했다가 재결합시킵니다.
    /// </summary>
    private void Set_MotherVectorZero(GameObject motherTr)
    {
        MapTile[] childrenTr = motherTr.GetComponentsInChildren<MapTile>();

        for(int i = 0; i < childrenTr.Length; i++) childrenTr[i].transform.parent = null;

        motherTr.transform.position = Vector3.zero;

        for (int i = 0; i < childrenTr.Length; i++) childrenTr[i].transform.parent = motherTr.transform;
    }

    /// <summary>
    /// 문자열로 입력한 타일 속성들을 배열로 반환합니다.
    /// </summary>
    private List<int[]> GetParse_InputTileData()
    {
        List<int[]> result = new List<int[]>();

        string[] lines = _inputTileData.Split('\n');
        for(int i = 0; i < lines.Length; i++)
        {
            if (lines.Equals("") || lines == null) break;

            //Debug.Log("Line " + i + " : " + lines[i]);

            char[] line = lines[i].ToCharArray();
            int[] tempArr = new int[line.Length];

            for (int j = 0; j < line.Length; j++)
                tempArr[j] = int.Parse(line[j].ToString());

            result.Add(tempArr);
        }

        return result;
    }

    /// <summary>
    /// 배치된 맵 타일의 정보를 디버그 로그로 출력합니다.
    /// </summary>
    private void DebugLog_MapTiles()
    {
        string alltemp = "";
        for(int i = 0; i < MapTiles.GetLength(1); i++)
        {
            string temp = "";
            for(int j = 0; j < MapTiles.GetLength(0); j++)
            {
                temp += (int)MapTiles[j, i].myTileType;
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
    /// 입력된 노란색 타일(톱니로 부술 수 있는 타일)을 분홍색 타일(이동할 수 있는 타일)로 변환합니다.
    /// </summary>
    /// <param name="x">좌표 x</param>
    /// <param name="y">좌표 y</param>
    public void SetTile_Passible(int x, int y)
    {
        if (!MapTiles[x, y].myTileType.Equals(TILE_TYPE.노랑_톱니로부숴)) return;

        MapTiles[x, y].originSprite = Resources.Load<Sprite>("PinkTile");
        MapTiles[x, y].GetComponent<SpriteRenderer>().sprite = MapTiles[x, y].originSprite;

        MapTiles[x, y].myTileType = TILE_TYPE.분홍_빈공간;

        //박스 터지는 이펙트 재생
        GameManager.instance.boxEffect.transform.position = MapTiles[x, y].transform.position;
        GameManager.instance.boxEffect.Play();

        SoundManager.instance.Play_SFX(SFX_LIST.DESTROY_BLOCK);
    }

    /// <summary>
    /// 이동할 수 있는 칸인가요?
    /// 입력된 좌표의 타일을 지나갈 수 있는 지 확인합니다.
    /// </summary>
    /// <param name="x">좌표 x</param>
    /// <param name="y">좌표 y</param>
    public bool Get_Passible(int x, int y)
    {
        if (x < 1 || y < 1) return false;
        if (x >= maxx || y >= maxy) return false;

        if (MapTiles[x, y].myTileType.Equals(TILE_TYPE.파랑_걷는공간)) return false;
        if (MapTiles[x, y].myTileType.Equals(TILE_TYPE.노랑_톱니로부숴)) return false;

        return true;
    }

    /// <summary>
    /// 입력된 좌표의 타일 속성 값을 반환합니다. 유효한 타일이 아닌 경우 -1을 반환합니다.
    /// 0 : 분홍_빈공간,
    /// 1 : 파랑_걷는공간,
    /// 2 : 검정_변형불가,
    /// 3 : 노랑_톱니로부숴
    /// </summary>
    /// <param name="x">좌표 x</param>
    /// <param name="y">좌표 y</param>
    public int Get_MapTileType(int x, int y)
    {
        if (x < 0 || y < 0) return -1;
        if (x >= maxx || y >= maxy) return -1;

        return (int)MapTiles[x, y].myTileType;
    }

    /// <summary>
    /// 입력된 좌표의 타일의 World Position 값을 반환합니다. 유효한 타일이 아닌 경우 Vector.Zero를 반환합니다.
    /// </summary>
    /// <param name="x">좌표 x</param>
    /// <param name="y">좌표 y</param>
    public Vector3 Get_MapTilePosition(int x, int y)
    {
        if (x < 0 || y < 0) return Vector3.zero;
        if (x >= maxx || y >= maxy) return Vector3.zero;

        return MapTiles[x, y].transform.position;
    }

}
