using UnityEngine;

using DG.Tweening;

namespace USP.MiniGame.recognitionPatterns
{
      public class Transition : MonoBehaviour
      {

            private static Transition instance;
            [SerializeField] private Transform[] capsules;
            Vector3[] CapsuleStartPos;
            [SerializeField] private float verticalOffset = 14F, stepDelay = 0.1F;

            void Awake()
            {
                  instance = this;
                  int count = 0;
                  CapsuleStartPos = new Vector3[capsules.Length];
                  foreach (var x in capsules)
                  {
                        CapsuleStartPos[count] = x.position;
                        count++;
                  }
            }


            private Sequence seq;

            [ContextMenu("PLAY-TESTING")]
            public void PlayEditor()
            {
                  if (instance)
                  {

                        Play(null);
                  }
                  else
                  {
                        instance = this;

                        Play(null);
                  }
            }

            public static void Play(TweenCallback onCovered, TweenCallback onComplete = null)
            {

                  var sequence = instance.seq;
                  if (sequence != null && sequence.IsPlaying()) return;

                  sequence = DOTween.Sequence();
                  for (int i = 0; i < instance.capsules.Length; i++) sequence.Append(instance.capsules[i].DOMoveY(0F, instance.stepDelay).From(instance.CapsuleStartPos[i]));
                  sequence.AppendCallback(onCovered);
                  for (int i = 0; i < instance.capsules.Length; i++) sequence.Append(instance.capsules[i].DOMoveY(instance.verticalOffset, instance.stepDelay));
                  sequence.OnComplete(onComplete);
                  sequence.OnKill(() => sequence = null);
                  sequence.Play();
            }
      }
}