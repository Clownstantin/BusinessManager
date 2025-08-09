using UnityEngine;

namespace Core
{
    public struct MonoLink<T> where T : Object
    {
        public T Value;
    }
}