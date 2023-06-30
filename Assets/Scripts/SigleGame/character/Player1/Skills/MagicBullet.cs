using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{
    public float hitDamage = 10f;
    public float flySpeed = 10f;//飞行速度
    public float flyTime = 0f;
    public float angle = 0f;

    public string parentId;

    public Vector3 direction = new(1, 0, 0);

    private Rigidbody2D rb;

    public GameObject magicBulletPrefab;

    public void CreatMagicBullet(Vector3 spawnPosition, Vector3 flyDirection, float speed, float damege, float flyAngle, string id)
    {
        parentId = id;
        direction = flyDirection;
        angle = flyAngle;
        flySpeed = speed;
        hitDamage = damege;
        Instantiate(magicBulletPrefab, spawnPosition, Quaternion.identity);
    }// Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.transform.rotation = Quaternion.Euler(0f, 0f, angle);//角度
        rb.transform.position += flySpeed * Time.deltaTime * direction;// - playerMove.moveSpeed * Time.deltaTime * playerMove.direction
        flyTime += Time.deltaTime;

        if (flyTime > 10f)
        {
            Destroy(gameObject);
        }
    }
}
