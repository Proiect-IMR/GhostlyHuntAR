using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;

[Serializable]
public struct GhostLocation
{
    public string name;
    public double latitude;
    public double longitude;
}

public class GhostManager : MonoBehaviour
{
    public GhostLocation[] ghostLocations;
    public DialogueManager dialogueManager;
    public NPC[] npcs;
    public Button interactButton;
    public GameObject[] ghosts;
    public TextMeshProUGUI textMeshPro;
    private GameObject currentActiveGhost;
    private GameObject nearestGhost = null;
    private IEnumerator Start()
    {

        if (!Input.location.isEnabledByUser)
            yield break;

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            Debug.Log("Working");
        }
    }
    void Update()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // Get the player's current location
            LocationInfo playerLocation = Input.location.lastData;
            UpdateNearestGhost(playerLocation);

            Debug.Log("Update");
        }
    }
    public float maximumDistanceToActivate;

    void UpdateNearestGhost(LocationInfo playerLocation)
    {
        double nearestDistance = double.MaxValue;
        int nearestGhostIndex = -1;

        for (int i = 0; i < ghostLocations.Length; i++)
        {
            double distance = HaversineDistance(
                playerLocation.latitude, playerLocation.longitude,
                ghostLocations[i].latitude, ghostLocations[i].longitude
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestGhost = ghosts[i];
                nearestGhostIndex = i;
            }
        }

        if (nearestGhostIndex != -1 && nearestDistance <= maximumDistanceToActivate)
        {
            if (currentActiveGhost != nearestGhost)
            {
                if (currentActiveGhost != null)
                {
                    currentActiveGhost.SetActive(false);
                }

                nearestGhost.SetActive(true);
                currentActiveGhost = nearestGhost;
                textMeshPro.text = ghostLocations[nearestGhostIndex].name;
                interactButton.gameObject.SetActive(true);

                dialogueManager.npc = npcs[nearestGhostIndex];
                dialogueManager.InitializeNpc(npcs[nearestGhostIndex]);
                dialogueManager.AdvanceDialogue();
            }
        }
        else
        {
            if (currentActiveGhost != null)
            {
                currentActiveGhost.SetActive(false);
                currentActiveGhost = null; // Clear the current active ghost
            }

            DeactivateAllGhostsAndShowMessage();
        }
    }



    void DeactivateAllGhostsAndShowMessage()
    {
        foreach (GameObject ghost in ghosts)
        {
            ghost.SetActive(false);
        }
        interactButton.gameObject.SetActive(false);
        textMeshPro.text = "No ghosts within range.";
        currentActiveGhost = null;
    }


    public static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double earthRadiusKm = 6371.0;
        double dLat = ToRadians(lat2 - lat1);
        double dLon = ToRadians(lon2 - lon1);
        lat1 = ToRadians(lat1);
        lat2 = ToRadians(lat2);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return earthRadiusKm * c;
    }
    public void SetCurrentGhost()
    {
        if (currentActiveGhost != null)
        {
            textMeshPro.text = currentActiveGhost.name;
        }
        else
        {
            textMeshPro.text = "No active ghost.";
        }
    }

    public static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }


}
