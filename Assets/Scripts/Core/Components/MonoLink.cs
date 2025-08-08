using UnityEngine;

namespace BusinessManager.Core
{
    public struct MonoLink<T> where T : Object
    {
        public T Value;
    }
}