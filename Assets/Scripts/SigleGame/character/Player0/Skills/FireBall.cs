using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float hitDamage = 10f;
    public float flySpeed = 10f;//�����ٶ�
    public float flyTime = 0f;
    public float angle = 0f;

    public string parentId;

    public Vector3 direction = new(1, 0, 0);

    private Rigidbody2D rb;

    public GameObject fireBallPrefab;



    public void CreatFireBall(Vector3 spawnPosition, Vector3 flyDirection, float speed, float damege, float flyAngle, string id)
    {
        parentId = id;
        direction = flyDirection;
        angle = flyAngle;
        flySpeed = speed;
        hitDamage = damege;
        Instantiate(fireBallPrefab, spawnPosition, Quaternion.identity);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.transform.rotation = Quaternion.Euler(0f, 0f, angle);//�Ƕ�
        rb.transform.position += flySpeed * Time.deltaTime * direction;// - playerMove.moveSpeed * Time.deltaTime * playerMove.direction
        flyTime += Time.deltaTime;

        if (flyTime > 10f)
        {
            Destroy(gameObject);
        }
    }
}
