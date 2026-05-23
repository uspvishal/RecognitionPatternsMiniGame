//Author : Vishal Lakhani
//Description usage from repo rbsv
//email: lahsivki@gmail.com

using System.Collections;
using DG.Tweening;
using UnityEngine;
namespace USP.MiniGame.Addition
{

    public class SequenceTween : MonoBehaviour
    {
        public GameObject[] ObjectList;
        public float duration, interval, initialDelay;
        Vector3[] ogSize;
        public Ease ease;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            int count = 0;
            ogSize = new Vector3[ObjectList.Length];
            foreach (var x in ObjectList)
            {
                ogSize[count] = x.transform.localScale;
                x.transform.localScale = Vector3.zero;
                count++;
            }
            StartCoroutine(StartAnimations());
            //UtilityEventsManager.isControlEnabled = false;
            // DOVirtual.DelayedCall(3,()=>{UtilityEventsManager.isControlEnabled = true;});

        }

        IEnumerator StartAnimations()
        {
            if (initialDelay > 0)
            {
                yield return new WaitForSeconds(initialDelay);
            }
            else
            {
                yield return null;
            }
            int count = 0;

            foreach (var x in ObjectList)
            {
                x.transform.DOScale(ogSize[count], duration).SetEase(ease);
                count++;
                yield return new WaitForSeconds(interval);
            }
        }


        IEnumerator EndAnimations()
        {

            yield return null;

            int count = 0;

            foreach (var x in ObjectList)
            {
                x.transform.DOScale(ogSize[count] * 1.1f, .5f);
                x.transform.DOScale(Vector3.zero, .4f).SetDelay(.5f);
                count++;
                yield return null;
                //yield return new WaitForSeconds(interval);
            }
        }

        public void ExitTransition() => StartCoroutine(EndAnimations());





    }
}
