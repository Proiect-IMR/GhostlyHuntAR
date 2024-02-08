public static class PlayerLocationData
{
    public static float Latitude { get; set; }
    public static float Longitude { get; set; }

    public static double StefanLat { get; set; }
    public static double StefanLong { get; set; }

    public static double MihaiLat { get; set; }
    public static double MihaiLong { get; set; }

    public static double IonLat { get; set; }
    public static double IonLong { get; set; }

    public static double VeroLat { get; set; }
    public static double VeroLong { get; set; }

    public static double CreangaLat { get; set; }
    public static double CreangaLong { get; set; }

    static PlayerLocationData()
    {
        StefanLat = 47.1490096043921;
        StefanLong = 27.5939775719424;

        MihaiLat = 47.1492788195143;
        MihaiLong = 27.5941104750433;

        IonLat = 47.1488000244202;
        IonLong = 27.5936504592582;

        VeroLat = 47.1495065669653;
        VeroLong = 27.594558234129;

        CreangaLat = 47.148412794584;
        CreangaLong = 27.5931382394774;

    }

}