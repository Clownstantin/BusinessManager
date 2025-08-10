using System;

namespace Core
{
    [Serializable]
    public class SaveData
    {
        public float Balance;
        public BusinessSaveData[] Businesses;
    }
}