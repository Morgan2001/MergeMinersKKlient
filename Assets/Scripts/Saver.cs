using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SaveNames
{
    Player = 0,
    GameplayField = 1,
    OffersBought = 2,
    BoostsTimeLeft = 3,
    LastActiveDateTime = 4,
    Wheel = 5
}

public static class Saver
{
    public const string LoadingErrorString = "";

    public static void Save<T>(SaveNames saveName, T objToSave)
    {
        var objToSaveJson = JsonUtility.ToJson(objToSave);
        PlayerPrefs.SetString(((int)saveName).ToString(), objToSaveJson);
    }

    public static T Load<T>(SaveNames saveName, T defaultValue = default)
    {
        var objToLoadStr = PlayerPrefs.GetString(((int)saveName).ToString(), LoadingErrorString);

        if (objToLoadStr != LoadingErrorString && objToLoadStr != "{}")
        {
            return JsonUtility.FromJson<T>(objToLoadStr);
        }
        else
        {
            return defaultValue;
        }
    }
}