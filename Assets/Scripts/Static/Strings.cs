using System.IO;
using UnityEngine;

namespace Core
{
    public partial class Strings
    {
        #region Save
        public static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
        public static string StoredPath => Path.Combine(Application.persistentDataPath, "storedSave.json");
        #endregion

        #region Configs
        public const string ModulePath = "Modules/";
        public const string BusinessPath = "Business/";
        #endregion
    }
}