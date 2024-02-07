using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class AchivementsManager : MonoBehaviour
{
    public NPC[] npcs;
    public GameObject[] unlockBadges;
    public GameObject[] activeBadges;
    public TextMeshProUGUI debugText;


    void Start()
    {
        UpdateActiveBadges();
    }


    void UpdateActiveBadges()
    {
        for (int i = 0; i < npcs.Length; i++)
        {
            npcs[i].activeBadge = LoadBadgeState(npcs[i]);
            if (npcs[i].activeBadge)
            {
                activeBadges[i].SetActive(true);
                unlockBadges[i].SetActive(false);
            }
            else
            {
                activeBadges[i].SetActive(false);
            }
        }

    }
    public bool LoadBadgeState(NPC npc)
    {
        string filePath = Path.Combine(Application.persistentDataPath, npc.ghostName + ".json");
        if (File.Exists(filePath))
        {
            debugText.text = "File exists" + " " + filePath;
            string stateJson = File.ReadAllText(filePath);

            NPCBadgeState state = JsonUtility.FromJson<NPCBadgeState>(stateJson);

            return state.activeBadge;
        }
        else
        {
            return false;
        }
    }
}

