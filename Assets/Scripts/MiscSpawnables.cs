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
        public ParticleSystem Particle;
        void Awake()
        {
            instance = this;
        }
        public static void GetParticleSpawnable(Vector3 pos, bool autoDestory = true)
        {
            var particle = instance.Particle;
            ParticleSystem.EmitParams emitParams = new()
            {
                applyShapeToPosition = true,
                position = pos,
            };
            particle.Emit(emitParams, 30);
        }
    }
}