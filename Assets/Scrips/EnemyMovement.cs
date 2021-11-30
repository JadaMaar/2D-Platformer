using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D boxCol;
    CapsuleCollider2D capCol;
    [SerializeField] float moveSpeed = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        capCol = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(moveSpeed, 0f);
        /*if (capCol.IsTouchingLayers(LayerMask.GetMask("Effects")))
        {
            Debug.Log("ded");
            Destroy(this.gameObject);
        }*/
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Vector3 s = this.transform.localScale;
        this.transform.localScale = new Vector3(-s.x, s.y, s.z);
        moveSpeed *= -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Effects"))
        {
            Debug.Log("ded");
            Destroy(this.gameObject);
        }
    }

}
