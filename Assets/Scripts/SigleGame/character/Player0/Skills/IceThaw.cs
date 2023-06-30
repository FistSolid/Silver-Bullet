using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceThaw : MonoBehaviour
{
    public float hitDamage = 10f;
    //技能半径
    public float radius = 5f;
    //技能持续时间
    public float continueTime = 0f;
    public string parentId;

    private Rigidbody2D rb;

    public GameObject iceThaw;

    public void CreatIceThaw(Vector3 spawnPosition, float damage , float rad, string id)
    {
        hitDamage = damage;
        radius = rad;
        parentId = id;
     
        Instantiate(iceThaw, spawnPosition, Quaternion.identity);

    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        continueTime += Time.deltaTime;
        if(continueTime > 6f) {
        Destroy(iceThaw);
        }
    }
}
