/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ɵ�ͼ�������߼����ǣ��������ߵ���ǰ��ͼ�ı߽�
//��hasEnemy=false ������ԭ��ͼ���������Խ�����������һ�ŵ�ͼ

//����ͼ�ĳ�ʼ״̬����Ϊһ��Ԥ���壨Prefab����������ͼ�е����ж�������ԡ�
//Ȼ������Ҫ�������ɵ�ͼʱ��ͨ��ʵ������Ԥ����������һ����ȫ��ͬ�ĵ�ͼ��

//����һ���յ�GameObject��Ϊ��ͼ�ĸ��ڵ㣬�ڸýڵ��·��õ�ͼ�е����ж��󣬰������Ρ�������ߵȡ�
//�����GameObject����Ϊһ��Ԥ���壬�Ա�����Ҫʱ����ʵ������

//����Ҫ�������ɵ�ͼʱ��ʹ��Instantiate����ʵ������Ԥ����


//��¼��ͼ�ĸ�������
public class MapState
{
    //����(����ȫ��
    public int terrainType;
    
    private bool hasEnemy;

    //���ó�ʼ״̬
    public MapState(bool enemy, int terrain)
    {
        terrainType = terrain;
        hasEnemy = enemy;
    }
}

public class CreatMap : MonoBehaviour
{
    public GameObject mapPrefab; // ��ͼԤ����
    private MapState[,] initialMapState; // �����ʼ��ͼ״̬�Ķ�ά����

    private void InitializeMap()
    {
        // ���������ɵ�ͼ�ĳ�ʼ״̬���������Ρ������
        // ��ͼ�����߼�����...
        // ��ʼ����ͼ����
        int width = initialMapState.GetLength(0);
        int height = initialMapState.GetLength(1);
        initialMapState = new MapState[width, height];

        // ������ͼ��ÿ�����ӣ����ɵ���
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y <height; y++)
            {
                // �ڵ�ǰ����λ�����ɵ���
                Vector3 position = new Vector3(x, 0, y);
                GameObject terrain = Instantiate(terrainPrefab, position, Quaternion.identity);

                // ���õ��ε�����
                int terrainType = Random.Range(0, 3); // ���ѡ���������
                terrain.GetComponent<Terrain>().terrainType = terrainType;

                // �ڵ�ͼ�����б����������
                initialMapState[x, y] = terrainType;
            }
        }
    }

    private void SaveInitialMapState()
    {
        int width = initialMapState.GetLength(0);
        int height = initialMapState.GetLength(1);

        initialMapState = new MapState[width, height];

        // ������ͼ��ÿ�����ӣ������ʼ״̬
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject mapObject = /* ��ȡ��ͼ���� 

                // ��ȡ��ͼ��������ԣ����������Զ���
                int terrainType = mapObject.GetComponent<Terrain>().terrainType;
                bool hasEnemy = mapObject.GetComponent<Enemy>().IsActive();

                // �����ͼ����ĳ�ʼ״̬
                initialMapState[x, y] = new MapState(hasEnemy,terrainType );
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // ��ʼ����ͼ�������ʼ״̬
        InitializeMap();
        SaveInitialMapState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/
