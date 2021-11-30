using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthwall : MonoBehaviour
{
    private Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(3f);
        ani.Play("Earthwall destruction");
        yield return new WaitForSeconds(.667f);
        Destroy(this.gameObject);
    }
}
