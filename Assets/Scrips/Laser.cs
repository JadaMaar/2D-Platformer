using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
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
