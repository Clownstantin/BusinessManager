namespace Core
{
    [System.Serializable]
    public struct BusinessSaveData
    {
        public int Index;
        public int Level;
        public float EnhancementsMultiplierSum;
        public int PurchasedEnhancementsMask;
        public float Progress;
        public bool Bought;
    }
}