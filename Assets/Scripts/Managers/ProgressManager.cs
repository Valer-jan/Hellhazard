using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static int Killed, Souls;
    public static int[] LevelUnlocked;

    #region progress
    public static void SaveProgress(LevelCost[] LevelCostArray)
    {
        PlayerPrefs.SetInt("St_Killed", Killed);
        PlayerPrefs.SetInt("St_Souls", Souls);

        // levels
        LevelUnlocked = new int[LevelCostArray.Length];
        for (int i = 0; i < LevelUnlocked.Length; i++)
        {
            if (LevelCostArray[i].IsUnlocked) LevelUnlocked[i] = 1;
            else LevelUnlocked[i] = 0;
            PlayerPrefs.SetInt("Lvl_" + i, LevelUnlocked[i]);
        }
        Debug.Log("Progress saved with levels");
        LoadProgress(LevelCostArray);
    }
    public static void SaveProgress()
    {
        PlayerPrefs.SetInt("St_Killed", Killed);
        PlayerPrefs.SetInt("St_Souls", Souls);

        Debug.LogWarning("Progress saved without levels !");
        LoadProgress();
    }

    public static void LoadProgress(LevelCost[] LevelCostArray)
    {
        if (PlayerPrefs.HasKey("St_Killed"))
        {
            Killed = PlayerPrefs.GetInt("St_Killed");
            Souls = PlayerPrefs.GetInt("St_Souls");

            // levels
            LevelUnlocked = new int[LevelCostArray.Length];
            for (int i = 0; i < LevelUnlocked.Length; i++)
            {
                LevelUnlocked[i] = PlayerPrefs.GetInt("Lvl_" + i);
                if (LevelUnlocked[i] == 1) LevelCostArray[i].IsUnlocked = true;
                else LevelCostArray[i].IsUnlocked = false;
            }
            Debug.Log("Progress loaded with levels");
        }
        else DefoultProgress(LevelCostArray);
    }
    public static void LoadProgress()
    {
        if (PlayerPrefs.HasKey("St_Killed"))
        {
            Killed = PlayerPrefs.GetInt("St_Killed");
            Souls = PlayerPrefs.GetInt("St_Souls");

            Debug.LogWarning("Progress loaded without levels !");
        }
        else Debug.Log("Progress not loaded !");
    }

    public static void DefoultProgress(LevelCost[] LevelCostArray)
    {
        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("St_Killed", 0);
        PlayerPrefs.SetInt("St_Souls", 0);

        // levels
        LevelUnlocked = new int[LevelCostArray.Length];
        for (int i = 0; i < LevelUnlocked.Length; i++)
        {
            LevelUnlocked[i] = 0;
        }

        LevelUnlocked[0] = 1;
        for (int i = 0; i < LevelUnlocked.Length; i++)
        {
            PlayerPrefs.SetInt("Lvl_" + i, LevelUnlocked[i]);
            if (LevelUnlocked[i] == 1) LevelCostArray[i].IsUnlocked = true;
            else LevelCostArray[i].IsUnlocked = false;
        }
        Debug.Log("Defoult progress");
        LoadProgress(LevelCostArray);
    }
    #endregion
}
