using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_AI : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D boxCol;
    CapsuleCollider2D capCol;
    PolygonCollider2D polyCol;
    Animator ReaperAni;
    [SerializeField] float moveSpeed = 1f;
    public Animation attack;
    private bool DamageDealt = false;
    private int HP = 5;
    private bool HitStun = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        capCol = GetComponent<CapsuleCollider2D>();
        polyCol = GetComponent<PolygonCollider2D>();
        ReaperAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ReaperAni.GetBool("isAttacking") || HitStun) { return;  }

        Walk();
        Attack();

    }

    private void Walk()
    {

        

        ReaperAni.SetBool("isWalking", true);
        rb.velocity = new Vector2(moveSpeed, 0f);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float dist = 10;
        if (player != null)
        {
            dist = Vector2.Distance(transform.position, player.transform.position);
        }
        
            if (dist <= 5)
            {
                if (transform.position.x > player.transform.position.x)
                {
                    moveSpeed = -1;
                    Vector3 s = this.transform.localScale;
                    this.transform.localScale = new Vector3(-1, s.y, s.z);
                }
                else
                {
                    moveSpeed = 1;
                    Vector3 s = this.transform.localScale;
                    this.transform.localScale = new Vector3(1, s.y, s.z);
                }
            }
        
    }

    private void Attack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float dist = 10;
        if (player != null)
        {
            dist = Vector2.Distance(transform.position, player.transform.position);
        }
        if (dist <= .9f)
        {
            Vector3 s = this.transform.localScale;
            if (transform.position.x > player.transform.position.x)
            {  
               this.transform.localScale = new Vector3(-1, s.y, s.z);
            }
            else
            {
                this.transform.localScale = new Vector3(1, s.y, s.z);
            }
            ReaperAni.SetBool("isAttacking", true);
            
            ReaperAni.SetBool("isWalking", false);
            boxCol.enabled = true;
            StartCoroutine(WaitAFrame());
                
        }
        
    }

    IEnumerator WaitAFrame()
    {
        yield return new WaitUntil(() => DamageDealt == true);
        boxCol.enabled = false;
        DamageDealt = false;

        ReaperAni.Play("Attack Enemy", -1, 0f);
        rb.velocity = new Vector2(0, 0);
        
        yield return new WaitForSeconds(0.583f); //Attack animation duration
        Debug.Log("HHHHHHHHHHH");
        ReaperAni.SetBool("isAttacking", false);
        ReaperAni.SetBool("isIdleing", true);

        yield return new WaitForSeconds(.5f);
        ReaperAni.SetBool("isIdleing", false);
    }


    /*private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            return;
        }
        
        Vector3 s = this.transform.localScale;
        this.transform.localScale = new Vector3(-s.x, s.y, s.z);
        moveSpeed *= -1;
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) { return;  }
        moveSpeed *= -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !DamageDealt)
        {
            var script = collision.GetComponent<PlayerMovement>();
            script.TakeDamage(1);
            script.Knockback(new Vector2(transform.localScale.x, 1.5f), 15);
            DamageDealt = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Effects"))
        {
            HP -= 1;
            ReaperAni.Play("Reaper Hit", 0, -1f);
            StartCoroutine(StunTime(.2f));
            if(HP <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator StunTime(float time)
    {
        HitStun = true;
        rb.velocity = Vector2.zero;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player.transform.position.x > transform.position.x)
        {
            rb.AddForce(new Vector2(-1, 0), ForceMode2D.Impulse);
        } else
        {
            rb.AddForce(new Vector2(1, 0), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(time);
        HitStun = false;
    }
}
