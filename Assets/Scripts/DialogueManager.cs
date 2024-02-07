using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public NPC npc;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI ghostNameText;
    public TMP_InputField inputField;

    public Button nextButton;
    public Button interactButton;


    public GameObject dialoguePanel;
    public GameObject ghostName;
    public GameObject guessButton;
    public GameObject inputFieldObject;
    public TextMeshProUGUI activeGhost;


    public AudioClip[] dialogueAudioClips;

    public AudioSource audioSource;
    public AudioClip wrong_answer;
    public AudioClip correct_answer;

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
                audioSource.Stop(); 
                audioSource.PlayOneShot(dialogueAudioClips[currentDialogueIndex]); 
            }

            HandleInputVisibility();

            currentDialogueIndex++;
        }
    }
    public void InitializeNpc(NPC npc)
    {
        this.npc = npc;
        currentDialogueIndex = 0;
        dialogueAudioClips = npc.audioClips;
    }

    public void CheckAnswer()
    {
        string userInput = inputField.text.Replace(" ", "").Trim();
        string correctAnswer = npc.ghostName.Replace(" ", "").Trim();

        if (userInput.Equals(correctAnswer, System.StringComparison.OrdinalIgnoreCase))
        {
            dialogueText.text = "Corect!";
            audioSource.PlayOneShot(correct_answer);
            npc.guessed = true;
            npc.activeBadge = true;
            SaveBadgeState(npc); 
        }
        else
        {
            dialogueText.text = "Greșit!";
            audioSource.PlayOneShot(wrong_answer);
        }

        inputFieldObject.SetActive(false);
        guessButton.SetActive(false);
    }
    public void SaveBadgeState(NPC npc)
    {
        NPCBadgeState state = new NPCBadgeState { activeBadge = npc.activeBadge };

        string stateJson = JsonUtility.ToJson(state);
        activeGhost.text = "Active badge" + npc.activeBadge + " " + npc.ghostName + ".json";

        string filePath = Path.Combine(Application.persistentDataPath, npc.ghostName + ".json");
        File.WriteAllText(filePath, stateJson);
    }
    private void EndDialogue()
    {
        if(audioSource != null)
        {
            audioSource.Stop();
        }
        currentDialogueIndex = 0;
        dialoguePanel.SetActive(false);
        guessButton.SetActive(false);
        ghostName.SetActive(true);
        if(npc.guessed)
        {
            ghostNameText.text = npc.ghostName;
        }
        inputFieldObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        interactButton.gameObject.SetActive(true);
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
[System.Serializable]
public class NPCBadgeState
{
    public bool activeBadge;
}