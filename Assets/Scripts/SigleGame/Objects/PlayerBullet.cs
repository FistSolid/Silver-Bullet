using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    public float flySpeed = 18f;
    public float flyTime = 0f;
    public float hitDamage = 4f;

    public string parentId;

    public Vector3 direction = new (1, 0, 0);

    private Rigidbody2D rb;

    
    public Move playerMove;
    public Quaternion rotation;
    float angle = 0f;
    
    public GameObject bulletPrefab;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
    }

    public void CreatePlayerBullet(Vector3 spawnPosition, Vector3 flyDirection, float speed, float damege, float flyAngle, string id)
    {
        parentId = id;
        direction = flyDirection;
        angle = flyAngle;
        flySpeed = speed;
        hitDamage = damege;

        Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
    }


    // Update is called once per frame
    void Update()
    {


        rb.transform.rotation = Quaternion.Euler(0f, 0f, angle);//½Ç¶È
        rb.transform.position += flySpeed * Time.deltaTime * direction;// - playerMove.moveSpeed * Time.deltaTime * playerMove.direction
        flyTime += Time.deltaTime;

        if (flyTime > 10f)
        {
            Destroy(gameObject);
        }
    }
}
