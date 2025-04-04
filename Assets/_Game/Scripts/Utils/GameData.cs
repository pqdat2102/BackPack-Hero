using UnityEngine;

public class GameData
{
    public static int SoundSetting
    {
        get { return PlayerPrefs.GetInt("SoundSetting", 1); }
        set { PlayerPrefs.SetInt("SoundSetting", value); }
    }
    public static int BgMusicSetting
    {
        get { return PlayerPrefs.GetInt("BgMusicSetting", 1); }
        set { PlayerPrefs.SetInt("BgMusicSetting", value); }
    }

    public static int CheckDoneTut
    {
        get { return PlayerPrefs.GetInt("check_done_tut", 0); }
        set { PlayerPrefs.SetInt("check_done_tut", value); }
    }
    public static int CheckTutMain
    {
        get { return PlayerPrefs.GetInt("check_tut_main", 0); }
        set { PlayerPrefs.SetInt("check_tut_main", value); }
    }
}