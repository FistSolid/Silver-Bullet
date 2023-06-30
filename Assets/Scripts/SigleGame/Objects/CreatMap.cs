/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//生成地图的整体逻辑就是，当人物走到当前地图的边界
//且hasEnemy=false 就利用原地图的所有属性进而重新生成一张地图

//将地图的初始状态构建为一个预制体（Prefab），包含地图中的所有对象和属性。
//然后，在需要重新生成地图时，通过实例化该预制体来创建一个完全相同的地图。

//创建一个空的GameObject作为地图的根节点，在该节点下放置地图中的所有对象，包括地形、怪物、道具等。
//将这个GameObject保存为一个预制体，以便在需要时进行实例化。

//当需要重新生成地图时，使用Instantiate函数实例化该预制体


//记录地图的各种属性
public class MapState
{
    //属性(不完全）
    public int terrainType;
    
    private bool hasEnemy;

    //设置初始状态
    public MapState(bool enemy, int terrain)
    {
        terrainType = terrain;
        hasEnemy = enemy;
    }
}

public class CreatMap : MonoBehaviour
{
    public GameObject mapPrefab; // 地图预制体
    private MapState[,] initialMapState; // 保存初始地图状态的二维数组

    private void InitializeMap()
    {
        // 在这里生成地图的初始状态，包括地形、怪物等
        // 地图生成逻辑代码...
        // 初始化地图数据
        int width = initialMapState.GetLength(0);
        int height = initialMapState.GetLength(1);
        initialMapState = new MapState[width, height];

        // 遍历地图的每个格子，生成地形
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y <height; y++)
            {
                // 在当前格子位置生成地形
                Vector3 position = new Vector3(x, 0, y);
                GameObject terrain = Instantiate(terrainPrefab, position, Quaternion.identity);

                // 设置地形的属性
                int terrainType = Random.Range(0, 3); // 随机选择地形类型
                terrain.GetComponent<Terrain>().terrainType = terrainType;

                // 在地图数据中保存地形类型
                initialMapState[x, y] = terrainType;
            }
        }
    }

    private void SaveInitialMapState()
    {
        int width = initialMapState.GetLength(0);
        int height = initialMapState.GetLength(1);

        initialMapState = new MapState[width, height];

        // 遍历地图的每个格子，保存初始状态
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject mapObject = /* 获取地图对象 

                // 获取地图对象的属性，根据需求自定义
                int terrainType = mapObject.GetComponent<Terrain>().terrainType;
                bool hasEnemy = mapObject.GetComponent<Enemy>().IsActive();

                // 保存地图对象的初始状态
                initialMapState[x, y] = new MapState(hasEnemy,terrainType );
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // 初始化地图，保存初始状态
        InitializeMap();
        SaveInitialMapState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/
