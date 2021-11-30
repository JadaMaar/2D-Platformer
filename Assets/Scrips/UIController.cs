using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    private Image img1;
    private Image img2;
    private Image img3;
    private Image img4;

    private float MCD;      //Fireball cd
    private float LCD;      //Lightning cd
    private float EarthCD;   //Earthwall cd
    private float RollCD;
    private PlayerMovement script;
    private Lightning Lightning_script;

    void Start()
    {
        img1 = GameObject.FindGameObjectWithTag("CD1").GetComponent<Image>();
        img2 = GameObject.FindGameObjectWithTag("CD2").GetComponent<Image>();
        img3 = GameObject.FindGameObjectWithTag("CD3").GetComponent<Image>();
        img4 = GameObject.FindGameObjectWithTag("CD4").GetComponent<Image>();
        script = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        Lightning_script = GameObject.FindGameObjectWithTag("Lightning").GetComponent<Lightning>();
        MCD = script.GetMagic1CDstart();
        EarthCD = script.GetMagic2CDstart();
        LCD = Lightning_script.GetLightningCDstart();
        RollCD = script.GetDodgeCDstart();
    }


    void Update()
    {
        img1.fillAmount = (script.GetMagic1CD() / MCD);
        img2.fillAmount = (Lightning_script.GetLightningCD() / LCD);
        img3.fillAmount = (script.GetMagic2CD() / EarthCD);
        img4.fillAmount = (script.GetDodgeCD() / RollCD);
    }

    private void OnReset(InputValue value)
    {
        Debug.Log("reset");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
