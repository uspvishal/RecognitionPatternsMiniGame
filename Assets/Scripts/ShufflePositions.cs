using System.Collections.Generic;
using UnityEngine;

namespace USP.MiniGame.Addition
{
    public class ShufflePositions : MonoBehaviour
    {
        public GameObject[] Items;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            List<Vector3> positions = new List<Vector3>();

            foreach (var x in Items)
            {
                positions.Add(x.transform.position);

            }
            positions.Shuffle();
            int count = 0;
            foreach (var x in Items)
            {
                x.transform.position = positions[count];
                count++;
            }


        }
    }
}

