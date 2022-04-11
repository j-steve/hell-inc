using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// General-purpose utilities methodsd relating to randomization.
    /// </summary>
    static public class RandomUtilities
    {
        /// <summary>
        /// Returns a random element from the array.
        /// </summary>
        static public T GetRandom<T>(this T[] list)
        {
            int randomIndex = UnityEngine.Random.Range(0, list.Length);
            return list[randomIndex];
        }

        /// <summary>
        /// Returns a random element from the array.
        /// </summary>
        static public T GetRandom<T>(this IEnumerable<T> list)
        {
            int size = list.Count();
            int randomIndex = UnityEngine.Random.Range(0, size);
            return list.ElementAt(randomIndex);
        }

        /// <summary>
        /// Returns a random element from the array.
        /// </summary>
        static public T GetRandom<T>(this T[,] list)
        {
            int randomIndex1 = UnityEngine.Random.Range(0, list.GetLength(0));
            int randomIndex2 = UnityEngine.Random.Range(0, list.GetLength(1));
            return list[randomIndex1, randomIndex2];
        }
    }
}
