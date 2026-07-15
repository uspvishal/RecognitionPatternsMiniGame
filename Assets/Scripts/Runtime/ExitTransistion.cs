using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
namespace USP.MiniGame.recognitionPatterns
{
    public class ExitTransistion : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public float maxScale = 1.3f;
        public float transisitionTime = .3f;
        public GameObject[] items;
        Vector3[] currentScale;
        public
        bool LocalScale;

        void Start()
        {
            DOVirtual.DelayedCall(3f, () =>
            {
                currentScale = new Vector3[items.Length];
                int count = 0;
                foreach (var x in items)
                {
                    currentScale[count] = x.transform.localScale;
                    count++;
                }
            });
        }

        public void OnExitWipeOff(float delay)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                Debug.Log("EXIT WIPE OFFFF-------------");
                int count = 0;
                foreach (var x in items)
                {
                    x.transform.DOScale(currentScale[count] * maxScale, transisitionTime);
                    x.transform.DOScale(Vector3.zero, transisitionTime).SetDelay(transisitionTime);
                    count++;
                }
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
