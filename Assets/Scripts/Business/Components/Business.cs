namespace BusinessManager.Core
{
    public struct Business
    {
        public int Index;
        public int Level;

        public float BaseCost;
        public float BaseIncome;
        public float IncomeDelay;

        // 0..1 progress of current income cycle
        public float Progress01;

        // Sum of purchased enhancements multipliers in [0..] (e.g. 0.2f for +20%)
        public float EnhancementsMultiplierSum;

        // Bit mask of purchased enhancements (bit i -> enhancement i purchased)
        public int PurchasedEnhancementsMask;
    }
}