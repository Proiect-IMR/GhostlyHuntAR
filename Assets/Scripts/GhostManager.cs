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
    public Button nextButton;
    public GameObject[] ghosts;

    public TextMeshProUGUI textMeshPro;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI activeGhost;

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
            PlayerLocationData.Latitude = playerLocation.latitude;
            PlayerLocationData.Longitude = playerLocation.longitude;

        }
    }
    private float maximumDistanceToActivateInMeters = 20f;

    void UpdateNearestGhost(LocationInfo playerLocation)
    {
        double maximumDistanceToActivateInKilometers = maximumDistanceToActivateInMeters / 1000.0; 
        distanceText.text = "Distance to activate: " + maximumDistanceToActivateInKilometers;
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
            activeGhost.text = "Active Ghost: " + nearestGhost.name + " " + nearestGhostIndex + " " + nearestDistance;
        }

        if (nearestGhostIndex != -1 && nearestDistance <= maximumDistanceToActivateInKilometers)
        {
            //distanceText.text = "Distance to ghost : " + nearestDistance;
            distanceText.text = "Active badge" + npcs[nearestGhostIndex].activeBadge;
            if (currentActiveGhost != nearestGhost)
            {
                if (currentActiveGhost != null)
                {
                    currentActiveGhost.SetActive(false);
                }
                activeGhost.text = "Active Ghost: " + nearestGhost.name;

                nearestGhost.SetActive(true);
                currentActiveGhost = nearestGhost;
                textMeshPro.text = ghostLocations[nearestGhostIndex].name;
                interactButton.gameObject.SetActive(true);

                dialogueManager.npc = npcs[nearestGhostIndex];
                dialogueManager.InitializeNpc(npcs[nearestGhostIndex]);
            }
        }
        else
        {
            if (currentActiveGhost != null)
            {
                currentActiveGhost.SetActive(false);
                currentActiveGhost = null; // Clear the current active ghost
                interactButton.gameObject.SetActive(false);
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
        const double earthRadiusKm = 6378.0;
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
    public void InteractWithGhost()
    {
        interactButton.gameObject.SetActive(false);
        dialogueManager.AdvanceDialogue();
        nextButton.gameObject.SetActive(true);
    }

    public static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }


}
