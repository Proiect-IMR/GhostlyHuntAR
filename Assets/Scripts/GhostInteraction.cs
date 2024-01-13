using UnityEngine;

public class GhostInteraction : MonoBehaviour
{
    public string ghostName; 

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Ghost"))
                {
                    if (hitInfo.collider.GetComponent<GhostInteraction>().ghostName == ghostName)
                    {
                        // Debug - afișează în consolă numele fantomei corecte
                        Debug.Log("Ai atins fantoma corectă: " + ghostName);
                        // Aici poți adăuga logica pentru a începe dialogul cu această fantomă
                    }
                    else
                    {
                        // Debug - afișează în consolă că ai atins o fantomă greșită
                        Debug.Log("Ai atins o fantomă greșită: " + ghostName);
                    }
                }
            }
        }
    }
}