using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lightning : MonoBehaviour
{
    private LineRenderer lr;
    private EdgeCollider2D edgeCol;
    private Animator myAnimator;
    const float LightningCDstart = 1.7f;
    private float LightningCD = LightningCDstart;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        edgeCol = GetComponent<EdgeCollider2D>();
        myAnimator = transform.parent.GetComponent<Animator>();
    } 

    void Update()
    {
        LightningCD -= Time.deltaTime;
    }

    private void OnFire(InputValue value)
    {
        if(PlayerMovement.GetPlayer().FireballUnlocked == false) { return; }
        if (myAnimator.GetBool("isRolling") || myAnimator.GetBool("isClimbing") || LightningCD > 0) { return;  }


        lr.enabled = true;
        edgeCol.enabled = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 pos = transform.position;
        Vector3 direc = mousePos - pos;
        Vector3 offset = Vector3.Cross(direc, new Vector3(0f, 0f, 1f)).normalized;

        
        lr.SetPosition(0, pos);

        float rand = Random.Range(.8f, -.8f);
        float randx = Random.Range(.1f, -.1f);
        lr.SetPosition(1, Vector3.Lerp(pos, mousePos, 0.25f + randx) + offset * rand);

        rand = Random.Range(.8f, -.8f);
        randx = Random.Range(.1f, -.1f);
        lr.SetPosition(2, Vector3.Lerp(pos, mousePos, 0.5f + randx) + offset * rand);

        rand = Random.Range(.8f, -.8f);
        randx = Random.Range(.1f, -.1f);
        lr.SetPosition(3, Vector3.Lerp(pos, mousePos, 0.75f + randx) + offset * rand);

        lr.SetPosition(4, mousePos);

        //edgecollider tracing the linerenderer
        Vector2[] colliderpoints;
        //Debug.Log(pos);
        colliderpoints = edgeCol.points;
        colliderpoints[0] = new Vector2(0,0);
        colliderpoints[1] = transform.InverseTransformPoint(lr.GetPosition(1));
        colliderpoints[2] = transform.InverseTransformPoint(lr.GetPosition(2));
        colliderpoints[3] = transform.InverseTransformPoint(lr.GetPosition(3));
        colliderpoints[4] = transform.InverseTransformPoint(lr.GetPosition(4));
        edgeCol.points = colliderpoints;


        StartCoroutine(wait());

        LightningCD = LightningCDstart;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(.3f);
        lr.enabled = false;
        edgeCol.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Destroy(collision.gameObject);
        }
    }

    public float GetLightningCDstart()
    {
        return LightningCDstart;
    }

    public float GetLightningCD()
    {
        return LightningCD;
    }


}
