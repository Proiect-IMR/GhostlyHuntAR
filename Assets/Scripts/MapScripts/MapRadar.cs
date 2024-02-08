using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class MapRadar : MonoBehaviour
{
    public string apiKey;
    public float lat;
    public float lon;
    public int zoom = 12;
    public enum resolution { low = 1, high = 2 };
    public resolution mapResolution = resolution.low;
    public enum type { roadmap, satellite, gybrid, terrain };
    public type mapType = type.roadmap;
    private string url = "";
    private int mapWidth = 640;
    private int mapHeight = 640;
    private bool mapIsLoading = false; //not used. Can be used to know that the map is loading 
    private Rect rect;

    private string apiKeyLast;
    private float latLast = -33.85660618894087f;
    private float lonLast = 151.21500701957325f;
    private int zoomLast = 12;
    private resolution mapResolutionLast = resolution.low;
    private type mapTypeLast = type.roadmap;
    private bool updateMap = true;


    // Start is called before the first frame update
    void Start()
    {
        float latitude = PlayerLocationData.Latitude;
        float longitude = PlayerLocationData.Longitude;
        lat = latitude;
        lon = longitude;

        StartCoroutine(GetGoogleMap());
        rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
        mapWidth = (int)Math.Round(rect.width);
        mapHeight = (int)Math.Round(rect.height);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateMap && (apiKeyLast != apiKey || !Mathf.Approximately(latLast, lat) || !Mathf.Approximately(lonLast, lon) || zoomLast != zoom || mapResolutionLast != mapResolution || mapTypeLast != mapType))
        {
            rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
            mapWidth = (int)Math.Round(rect.width);
            mapHeight = (int)Math.Round(rect.height);
            StartCoroutine(GetGoogleMap());
            updateMap = false;
        }
    }

    private string GenerateCirclePath(double centerLat, double centerLon, double radiusInMeters)
    {
        int points = 36; // Number of points to define the circle
        double radiusInDegrees = radiusInMeters / 111000f; // Roughly convert meters to degrees
        string path = "path=color:0xff0000ff|weight:2|fillcolor:0xffff0033";

        for (int i = 0; i < points; i++)
        {
            double theta = 2.0 * Math.PI * i / points;
            double lat = centerLat + (radiusInDegrees * Math.Cos(theta));
            double lon = centerLon + (radiusInDegrees * Math.Sin(theta)) / Math.Cos(centerLat * Math.PI / 180);
            path += "|" + lat + "," + lon;
        }

        // Close the circle by connecting the last point to the first
        path += "|" + (centerLat + (radiusInDegrees * Math.Cos(0))) + "," +
                       (centerLon + (radiusInDegrees * Math.Sin(0)) / Math.Cos(centerLat * Math.PI / 180));

        return path;
    }
    IEnumerator GetGoogleMap()
    {


        string stefansCircle = GenerateCirclePath(PlayerLocationData.StefanLat, PlayerLocationData.StefanLong, 50); // Radius of 50 meters
        string mihaisCircle = GenerateCirclePath(PlayerLocationData.MihaiLat, PlayerLocationData.MihaiLong, 50); // Radius of 50 meters
        string ionsCircle = GenerateCirclePath(PlayerLocationData.IonLat, PlayerLocationData.IonLong, 50); // Radius of 50 meters
        string verosCircle = GenerateCirclePath(PlayerLocationData.VeroLat, PlayerLocationData.VeroLong, 50); // Radius of 50 meters
        string creangasCircle = GenerateCirclePath(PlayerLocationData.CreangaLat, PlayerLocationData.CreangaLong, 50); // Radius of 50 meters




        string markers = "&markers=icon:URL_TO_YOUR_ICON_IMAGE%7C" + lat + "," + lon;
        url = "https://maps.googleapis.com/maps/api/staticmap?center=" + lat + "," + lon + "&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight + "&scale=" + mapResolution + "&maptype=" + mapType + "&" + stefansCircle +
           "&" + mihaisCircle + "&" + ionsCircle + "&" + verosCircle + "&" + creangasCircle + markers +
          "&key=" + apiKey;
        mapIsLoading = true;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("WWW ERROR: " + www.error);
        }
        else
        {
            mapIsLoading = false;
            gameObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            apiKeyLast = apiKey;
            latLast = lat;
            lonLast = lon;
            zoomLast = zoom;
            mapResolutionLast = mapResolution;
            mapTypeLast = mapType;
            updateMap = true;
        }
    }

}