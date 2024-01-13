using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public Camera arCamera; // Referința către camera AR
    public DialogueManager dialogueManager;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    NPCModel npcModel = hit.transform.GetComponent<NPCModel>();
                    if (npcModel != null)
                    {
                        //NPC npcData = npcModel.npcData;
                        dialogueManager.AdvanceDialogue();
                    }
                }
            }
        }
    }
}
