using UnityEngine;

public static class DBManager
{
    public static string email;
    public static string name;
    public static string rolln;
    public static int teamID;
    public static string joinCode;
    public static float swim;
    public static float hurdle;
    public static int basket;
    public static int collect1;
    public static int collect2;
    public static int collect3;
    public static int collect4;
    public static int collectStart;

    public static int numberOfLeaderboardDataToFetch { get{return 100;}}
    public static bool isHost;
    public static bool isLoggingIn;
    public static bool isLoggedIn { get{return (email != null);}}
    
    public static bool isCollectCompleted;
    public static bool isCollected1;
    public static bool isCollected2;
    public static bool isCollectStarted;

    public static void Logout(){
        email = null;
    }

}