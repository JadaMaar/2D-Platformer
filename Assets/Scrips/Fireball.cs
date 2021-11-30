using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private float direc;
    private float lifetime = 5f;
    private Rigidbody2D rb;
    private CircleCollider2D circleCol;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCol = GetComponent<CircleCollider2D>();

        FlipSprite();
    }

    void Update()
    {
        
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(this.gameObject);
        }

        /*if (circleCol.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            Destroy(this.gameObject);
        }

        if (circleCol.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            Destroy(this.gameObject);
        }*/
    }

    public void SetDirection(float direction)
    {
        Debug.Log(direction);
        direc = direction;
        rb.AddForce(new Vector2(8f, 0f) * direc);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            //Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
