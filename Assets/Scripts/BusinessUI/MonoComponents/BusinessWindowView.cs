using TMPro;
using UnityEngine;

namespace Core.UI
{
    public sealed class BusinessWindowView : MonoBehaviour
    {
        [field: SerializeField] public Transform ListContainer { get; private set; }
        [field: SerializeField] public TextMeshProUGUI BalanceText { get; private set; }
    }
}