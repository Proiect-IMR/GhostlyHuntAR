using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public NPC npc;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI ghostNameText;
    public TMP_InputField inputField;



    public GameObject dialoguePanel;
    public GameObject ghostName;
    public GameObject guessButton;
    public GameObject inputFieldObject;


    public AudioClip[] dialogueAudioClips;

    public AudioSource audioSource;

    private int currentDialogueIndex = 0;

    private void Start()
    {
        dialoguePanel.SetActive(false);
        ghostName.SetActive(false);
        inputFieldObject.SetActive(false);
        guessButton.SetActive(false);
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void AdvanceDialogue()
    {
        Debug.Log("AdvanceDialogue");
        if (currentDialogueIndex >= npc.dialogue.Length)
        {
            EndDialogue();
        }
        else
        {
            if (!dialoguePanel.activeSelf)
            {
                dialoguePanel.SetActive(true);
            }

            dialogueText.text = npc.dialogue[currentDialogueIndex];

            if (audioSource != null && dialogueAudioClips != null && currentDialogueIndex < dialogueAudioClips.Length)
            {
                audioSource.Stop(); // Stop any currently playing clips
                audioSource.PlayOneShot(dialogueAudioClips[currentDialogueIndex]); // Then play the new clip
            }

            HandleInputVisibility();

            currentDialogueIndex++;
        }
    }
    public void InitializeNpc(NPC npc)
    {
        this.npc = npc;
        currentDialogueIndex = 0;
    }

    public void CheckAnswer()
    {
        string userInput = inputField.text.Replace(" ", "").Trim();
        string correctAnswer = npc.ghostName.Replace(" ", "").Trim();

        if (userInput.Equals(correctAnswer, System.StringComparison.OrdinalIgnoreCase))
        {
            dialogueText.text = "Corect!";
        }
        else
        {
            dialogueText.text = "Greșit!";
        }

        inputFieldObject.SetActive(false);
        guessButton.SetActive(false);
    }
    private void EndDialogue()
    {
        currentDialogueIndex = 0;
        dialoguePanel.SetActive(false);
        ghostName.SetActive(true);
        ghostNameText.text = npc.ghostName;
        inputFieldObject.SetActive(false);
    }
    private void HandleInputVisibility()
    {
        if (currentDialogueIndex == 3)
        {
            inputFieldObject.SetActive(true);
            guessButton.SetActive(true);
            inputField.text = "";
        }
        else
        {
            inputFieldObject.SetActive(false);
            guessButton.SetActive(false);
        }
    }

}
