using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SkillCombiner : MonoBehaviour
{
    [SerializeField] Image FirstPick;
    [SerializeField] Image SecondPick;
    [SerializeField] Image Combination;
    [Header ("Base spells")]
    public Sprite Fire;
    public Sprite Lightning;
    public Sprite Earth;
    public Sprite Water;
    [Header("Combinations")]
    public Sprite Lava;
    public Sprite Laser;
    [Header("Stuff")]
    public Sprite White;
    [SerializeField] CanvasGroup canvGroup;

    private bool menuOpen = false;


    private void Start()
    {
        canvGroup.interactable = false;
        canvGroup.alpha = 0;
    }

    public void ResetPick()
    {
        FirstPick.sprite = White;
        SecondPick.sprite = White;
        Combination.sprite = White;
    }

    public void PickFire1()
    {
        if (!FirstPick.sprite == White) { return; }
        FirstPick.sprite = Fire;
    }

    public void PickFire2()
    {
        if (!SecondPick.sprite == White) { return; }
        SecondPick.sprite = Fire;
    }

    public void PickLightning1()
    {
        if (!FirstPick.sprite == White) { return; }
        FirstPick.sprite = Lightning;
    }

    public void PickLightning2()
    {
        if (!SecondPick.sprite == White) { return; }
        SecondPick.sprite = Lightning;
    }

    public void PickEarth1()
    {
        if (!FirstPick.sprite == White) { return; }
        FirstPick.sprite = Earth;
    }

    public void PickEarth2()
    {
        if (!SecondPick.sprite == White) { return; }
        SecondPick.sprite = Earth;
    }

    public void PickWater1()
    {
        if (!FirstPick.sprite == White) { return; }
        FirstPick.sprite = Water;
    }

    public void PickWater2()
    {
        if (!SecondPick.sprite == White) { return; }
        SecondPick.sprite = Water;
    }

    public void Combine()
    {
        if (SecondPick.sprite == White || FirstPick.sprite == White) { return; }
        //Lava Pillar
        if ((FirstPick.sprite == Fire && SecondPick.sprite == Earth) || (FirstPick.sprite == Earth && SecondPick.sprite == Fire))
        {
            Combination.sprite = Lava;
            PlayerMovement.GetPlayer().LavaUnlocked = true;
        } 
        else if ((FirstPick.sprite == Fire && SecondPick.sprite == Lightning) || (FirstPick.sprite == Lightning && SecondPick.sprite == Fire))
        {
            Combination.sprite = Laser;
        }
    }

    public void Open()
    {
        LeanTween.alphaCanvas(canvGroup, 1, 0.2f);
        canvGroup.interactable = true;
        menuOpen = true;
    }

    public void Exit()
    {
        LeanTween.alphaCanvas(canvGroup, 0, 0.2f);
        canvGroup.interactable = false;
        menuOpen = false;
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);
        ResetPick();
    }

    private void OnCombine(InputValue value)
    {
        Debug.Log("C");
        if (!menuOpen)
        {
            Open();        
        }
        else
        {
            Exit();
        }
    }
}
