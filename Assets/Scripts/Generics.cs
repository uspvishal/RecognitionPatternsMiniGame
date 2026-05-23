using System.Collections.Generic;
namespace USP.MiniGame.Addition
{
    public static class Generics
    {

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                // Pick a new index between the current index (inclusive) and the end of the list (exclusive)
                int randomIndex = UnityEngine.Random.Range(i, n);

                // Swap elements
                T temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }
    }
}