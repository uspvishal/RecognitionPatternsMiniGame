using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace USP.MiniGame.recognitionPatterns
{
    /// <summary>
    /// Script by Vishal Lakhani
    /// Email: usp.vishal@gmail.com
    /// Description:
    /// </summary>
    public class ObjectScaleHighLighting : MonoBehaviour
    {
        #region Variables

        [System.Serializable]
        public class Items
        {
            public float delay;
            public GameObject[] Obj;
        }
        public Items[] highLightItems;
        [SerializeField] float firstDelay;
        [SerializeField] float scaleUpSpeed = .2f;

        public UnityEvent OnStart;
        public UnityEvent OnComplete;

        public AudioID clip;
        Coroutine coroutine;
        #endregion

        #region Unity Methods

        public bool willStopOnUserInteraction=true;

        private void Start()
        {
            UtilityEventsManager.OnUserInteracted += OnUserInteracted;
            UtilityEventsManager.OnUserInteractedWrong += OnUserInteractedWrong;
           coroutine =  StartCoroutine(StartExecute());
        }
        
        

        IEnumerator StartExecute()
        {
            CheckAudioAndPlay();
            yield return new WaitForSeconds(firstDelay);
            OnStart?.Invoke();
            foreach (var x in highLightItems)
            {
                yield return new WaitForSeconds(x.delay);
                Vector3[] objsSizes = new Vector3[x.Obj.Length];
                int count = 0;
                foreach (var y in x.Obj)
                {
                    objsSizes[count] = y.transform.localScale;
                    y.transform.DOScale(objsSizes[count] * 1.2f, scaleUpSpeed);
                    y.transform.DOScale(objsSizes[count], scaleUpSpeed).SetDelay(scaleUpSpeed + .1f);
                }


            }
            OnComplete?.Invoke();
        }
        
        void OnUserInteractedWrong(object sender, UtilityEventsManager.UserInteracted data)
        {
            if(willStopOnUserInteraction)
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
            
        }
        
        void OnUserInteracted(object sender, UtilityEventsManager.UserInteracted data)
        {
            if(willStopOnUserInteraction)
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
            
        }


        void CheckAudioAndPlay()
        {
            if (clip != null)
            {
                var a = gameObject.AddComponent<AudioSource>();
                var audioclip = AudioLibrary.instance.GetAudioByEnum(clip);
                if (audioclip)
                {
                    a.PlayOneShot(audioclip);
                    UtilityEventsManager.isControlEnabled = false;
                    DOVirtual.DelayedCall(audioclip.length, () => { UtilityEventsManager.isControlEnabled = true; });
                }
            }
        }

        private void OnDestroy()
        {
            UtilityEventsManager.OnUserInteracted -= OnUserInteracted;
            UtilityEventsManager.OnUserInteractedWrong -= OnUserInteractedWrong;
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}