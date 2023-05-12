using UnityEngine;

namespace Extensions
{
    public static class ArrayExtensions
    {
        public static T PickRandom<T>(this T[] array)
        {
            int index = Random.Range(0, array.Length);
            return array[index];
        }
    }
}