using UnityEngine;

namespace BusinessManager.Core.UI
{
    public sealed class BusinessWindowView : MonoBehaviour
    {
        [SerializeField] private Transform _listContainer;

        public Transform ListContainer => _listContainer;
    }
}