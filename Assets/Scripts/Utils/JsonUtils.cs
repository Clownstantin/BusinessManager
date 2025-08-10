using System.IO;
using UnityEngine;

namespace Core
{
    public static class JsonUtils
    {
        public static void ToFile<T>(string path, T data)
        {
            string json = JsonUtility.ToJson(data, prettyPrint: false);
            File.WriteAllText(path, json);
        }

        public static bool FromFile<T>(string path, out T data)
        {
            data = default;
            if (!File.Exists(path))
                return false;

            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<T>(json);
            return true;
        }
    }
}