using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject Truck;
    private GameObject truck;
    private bool exit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (exit)
        {
            truck.GetComponent<Rigidbody2D>().velocity = new Vector2(-100, 0);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            truck = Instantiate(Truck, transform.position + new Vector3(100, 0, 0), Quaternion.identity);
            exit = true;
        }
    }
}
