using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;


public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink Jason")]
    [SerializeField] private TextAsset inkJason;

    [SerializeField] private TextMeshProUGUI text;

    private bool playerInRange;

    static Story story;
    static Choice choiceSelected;

    //private bool interactPressed = false;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        story = new Story(inkJason.text);
        text.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);
            /*if (interactPressed)
            {
                PlayerMovement.GetPlayer().FireballUnlocked = true;
                Debug.Log(inkJason);
            }*/
        } else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            ExitDialogueMode();
        }
    }

    private void OnInteract(InputValue value)
    {
        if (playerInRange)
        {
            //interactPressed = true;
            //PlayerMovement.GetPlayer().FireballUnlocked = true;
            //text.text = "Fireball unlocked!";
            //StartDialogue();
            text.transform.parent.gameObject.SetActive(true);
            if (story.canContinue)
            {
                text.text = story.Continue();
                if (story.currentChoices.Count != 0)
                {

                }
            }
            else
            {
                ExitDialogueMode();
            }
        }
    }

    private void StartDialogue()
    {
       
    }

    private void ExitDialogueMode()
    {
        text.text = "";
        text.transform.parent.gameObject.SetActive(false);
    }
}
