using UnityEngine;
using System.Runtime.InteropServices;

// All Objective-C exposed methods should be bound here
public class FlurryrBinding
{
    [DllImport("__Internal")]
    private static extern void _FlurryLogEvent(string flurryEvent);
    [DllImport("__Internal")]
    private static extern void _FlurryLogIn(string key);
    [DllImport("__Internal")]
    private static extern void _FlurryLogUncaughtException();
    [DllImport("__Internal")]
    private static extern void _FlurryLogStartGame( string gameType, string difficulty);
    [DllImport("__Internal")]
    private static extern void _FlurryLogPerformedCombo( string comboName);
    [DllImport("__Internal")]
    private static extern void _FlurryLogDeath( string levelAndZone);
    [DllImport("__Internal")]
    private static extern void _FlurryLogEndOfMission(  string mission);
    [DllImport("__Internal")]
    private static extern void _FlurryShowAppcircle();

    public static void FlurryLogIn(string key)
    {
        // Call plugin only when running on real device
        if (Application.platform != RuntimePlatform.OSXEditor)
            _FlurryLogIn(key);
    }

    public static void FlurryLogEvent(string flurryEvent)
    {
        // Call plugin only when running on real device
        if (Application.platform != RuntimePlatform.OSXEditor)
            _FlurryLogEvent(flurryEvent);
    }


    public static void FlurryLogStartGame( E_GameType gameType, E_GameDifficulty diff)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
            _FlurryLogStartGame(gameType.ToString(), diff.ToString());
    }
    
    public static void FlurryLogPerformedCombo( string comboName)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
            _FlurryLogPerformedCombo(comboName);
    }
    
    public  static void FlurryLogDeath( int level, int zone)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
            _FlurryLogDeath(level.ToString() + "-" + zone.ToString());
    }

    public static void FlurryLogEndOfMission(  E_GameType gameType, E_GameDifficulty difficulty, int level)
    {
        if (Application.platform != RuntimePlatform.OSXEditor)
            _FlurryLogEndOfMission(gameType.ToString() + "-" + difficulty.ToString() + "-"  + level);
    }


    public static void FlurryLogUncaughtException()
    {
        // Call plugin only when running on real device
        if (Application.platform != RuntimePlatform.OSXEditor)
            _FlurryLogUncaughtException();
    }
    
    public static void FlurryShowAppcircle()
    {
        // Call plugin only when running on real device
        if (Application.platform != RuntimePlatform.OSXEditor)
            _FlurryShowAppcircle();
    }
}
