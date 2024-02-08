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
        StefanLat = 47.1737226938934;
        StefanLong = 27.5749481339473;

        MihaiLat = 47.173067878793;
        MihaiLong = 27.5730231189486;

        IonLat = 47.1736856568739;
        IonLong = 27.5733225091837;

        VeroLat = 47.174563642928;
        VeroLong = 27.5773782228661;

        CreangaLat = 47.1729311618524;
        CreangaLong = 27.5739723843849;

    }

}