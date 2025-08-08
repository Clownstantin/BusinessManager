using UnityEngine;

namespace BusinessManager.Core
{
    [System.Serializable]
    public struct BusinessConfigData
    {
        [field: SerializeField] public BusinessNamesData NamesData { get; private set; }
        [field: SerializeField] public float BaseCost { get; private set; }
        [field: SerializeField] public float BaseIncome { get; private set; }
        [field: SerializeField] public float IncomeDelay { get; private set; }
        [field: SerializeField] public EnhancementConfigData[] Enhancements { get; private set; }

#if UNITY_EDITOR
        public void Validate()
        {
            if (NamesData == null || Enhancements == null)
                return;

            int namesLength = NamesData.Enhancements.Length;
            int enhancmentsLength = Enhancements.Length;

            if (namesLength != enhancmentsLength)
                Debug.LogError($"Business '{NamesData.Name}' has {namesLength} names but {enhancmentsLength} enhancements. They must match in count.");
        }
#endif
    }
}