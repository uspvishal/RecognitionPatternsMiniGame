using Unity.Mathematics;
using UnityEngine;

namespace USP.MiniGame.recognitionPatterns
{
    /// <summary>
    /// Script by Vishal Lakhani
    /// Email: usp.vishal@gmail.com
    /// Description:
    /// </summary>
    public class MiscSpawnables : MonoBehaviour
    {
        public static MiscSpawnables instance;
        public GameObject Particle;
        void Awake()
        {
            instance = this;
        }
        public static GameObject GetParticleSpawnable(Vector3 pos, bool autoDestory = true)
        {
            pos.z -= 1;
            var p = Instantiate(instance.Particle, pos, quaternion.identity);
            if (autoDestory)
            {
                Destroy(p, 1);
            }
            return p;
        }
    }
}