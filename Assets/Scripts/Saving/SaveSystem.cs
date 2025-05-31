using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class SaveSystem
{
    private static SaveFile saveFile;

    private const string SAVE_FILE_KEY = "SaveFile";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadFile() 
    {
        if (PlayerPrefs.HasKey(SAVE_FILE_KEY) is false)
        {
            saveFile = new();
            return;
        }

        string json = PlayerPrefs.GetString(SAVE_FILE_KEY);
        saveFile = JsonConvert.DeserializeObject<SaveFile>(json);
    }

    public static string LoadFrom (ILoadable loadable) 
    {
        if (saveFile == null) return null;

        return saveFile.loadables.Find(l => l.ID == loadable.GetID())?.info;
    }

    public static void Save (ILoadable loadable) 
    {
        var loadableInfo = saveFile.loadables.Find(l => l.ID == loadable.GetID());

        if (loadableInfo == null) 
        {
            loadableInfo = new LoadableInfo()
            {
                ID = loadable.GetID()
            };

            saveFile.loadables.Add(loadableInfo);
        }

        loadableInfo.info = loadable.GetSaveInfo();

        SaveOnPrefs();
    }

    private static void SaveOnPrefs()
    {
        PlayerPrefs.SetString(SAVE_FILE_KEY, JsonConvert.SerializeObject(saveFile));
    }

    [Serializable]
    public class SaveFile 
    {
        public List<LoadableInfo> loadables;

        public SaveFile()
        {
            this.loadables = new();
        }
    }

    [Serializable]
    public class LoadableInfo 
    {
        public string ID;
        public string info;
    }
}

