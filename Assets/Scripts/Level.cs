using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;


namespace USP.MiniGame.recognitionPatterns
{
    public class Level : MonoBehaviour
    {
        public GameObject ipadBg, phoneBg;
        public int DragitemsLimit;
        public int currentSuccessfullyDragged;
        public ParticleSystem particle;
        public UnityEvent onLevelStart, onLevelFinish;

        public GameObject[] ObjectsToHighlight;
        public bool SkipIntoAudio;
        public AudioID[] introAudios;


        public AudioID levelIdleVo;

        public List<AudioID> EndingVoS;
        public AudioClip[] WrongVo;
        public AudioClip[] CorrectVO;
        public float initialAudioDelay = .5f;
        public float SecondAudioDelay = .5f;
        AudioSource source;
        public CanvasGroup group;

        public UnityEvent AudioComplete;
        public UnityEvent OnEndingVoPlayed, onEndingVOFinished;

        public float OnSuccessEventDelay = 1;
        public float ChangeLevelAfterEndingVODelay = 1;
        [SerializeField]

        DragHandler dragController;

        public AudioID[] CounterVOs;

        void Awake()
        {
            var tut = FindAnyObjectByType<TutorialManager>();
            if (tut)
                tut.ResetManual();
            dragController = FindObjectOfType<DragHandler>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void OnEnable()
        {
            // group = GetComponentInChildren<CanvasGroup>();

            onLevelStart?.Invoke();
            UtilityEventsManager.isControlEnabled = false;
            source = gameObject.AddComponent<AudioSource>();
            UtilityEventsManager.OnLevelStart?.Invoke();
            UtilityEventsManager.onDraggedObjectAttached += onDraggedObjectSuccess;
            float aspect = GetAspect();
            phoneBg.SetActive(false);
            ipadBg.SetActive(false);
            if (aspect <= 1.4)
            {
                ipadBg.SetActive(true);
                Debug.Log("Mode Tablet");

            }
            else if (aspect > 1.4)
            {
                phoneBg.SetActive(true);
                Debug.Log("Mode Mobile");
            }

            StartCoroutine(AudioPlay());
            InvokeRepeating(nameof(UpdateIfSoundIsPlaying), .1f, .5f);
        }




        public void PlayCounterVO()
        {
            if (!source.isPlaying && currentSuccessfullyDragged > 0)
            {
                StartCoroutine(playCounterVo());
            }
        }
        IEnumerator playCounterVo()
        {

            yield return null;

            if (!source.isPlaying)
            {
                if (currentSuccessfullyDragged == 5)
                {
                    yield break;
                }
                var clip = AudioLibrary.instance.GetAudioByEnum(CounterVOs[currentSuccessfullyDragged - 1]);
                source.PlayOneShot(clip);
                yield return new WaitForSeconds(clip.length);
            }
            /*for (int i = 0; i < currentSuccessfullyDragged; i++)
            {
                var clip = AudioLibrary.instance.GetAudioByEnum(CounterVOs[i]);
                source.PlayOneShot(clip);
                yield return new WaitForSeconds(clip.length + .1f);
            }*/

        }




        public void PlayVOFromPlacement(AudioClip clip)
        {
            source.PlayOneShot(clip);
        }

        public void PlayRandomVOFromWrontPlacement()
        {
            var pieces = GetComponentsInChildren<Piece>();
            bool canPlay = true;

            foreach (var x in pieces)
            {
                var audiosouce = x.GetComponent<AudioSource>();
                if (audiosouce != null && audiosouce.isPlaying)
                {
                    canPlay = false;
                }
            }
            if (canPlay)
            {
                if (!source.isPlaying)
                {
                    source.PlayOneShot(WrongVo[Random.Range(0, WrongVo.Length)]);
                }
            }
        }

        public void PlayRandomVOFromCorrectPlacement()
        {
            var pieces = GetComponentsInChildren<Piece>();
            bool canPlay = true;

            foreach (var x in pieces)
            {
                var audiosouce = x.GetComponent<AudioSource>();
                if (audiosouce != null && audiosouce.isPlaying)
                {
                    canPlay = false;
                }
            }
            if (canPlay)
            {
                if (!source.isPlaying)
                {
                    source.PlayOneShot(CorrectVO[Random.Range(0, CorrectVO.Length)]);
                }
            }
        }

        // void Update()
        // {

        //     dragController.enabled = !source.isPlaying;

        // }

        void UpdateIfSoundIsPlaying()
        {
            if (source.isPlaying)
            {
                if (UtilityEventsManager.CurrentPieceDragged)
                    UtilityEventsManager.CurrentPieceDragged.Release();
            }
            dragController.enabled = !source.isPlaying;
        }



        IEnumerator PlayEndingVos(UnityAction OnComplete)
        {
            yield return new WaitForSeconds(.5f);
            while (source.isPlaying)
            {
                yield return null;
            }

            if (EndingVoS.Count > 0)
            {

                DOVirtual.DelayedCall(.2f, () => OnEndingVoPlayed?.Invoke());
                UtilityEventsManager.OnEndingVOStart?.Invoke();
                foreach (var x in EndingVoS) // use sequence if needed
                {
                    yield return new WaitForSeconds(.5f);

                    if (x != AudioID.none)
                    {
                        var a = AudioLibrary.instance.GetAudioByEnum(x);
                        source.PlayOneShot(a);
                        yield return new WaitForSeconds(a.length + .1f);

                    }
                }
                /*  int index = Random.Range(0, EndingVoS.Count - 1);
                  source.PlayOneShot(EndingVoS[index]);
                  yield return new WaitForSeconds(EndingVoS[index].length);*/
                var exitTransisition = GetComponent<ExitTransistion>();
                if (exitTransisition)
                {
                    DOVirtual.DelayedCall(1.1f, () => exitTransisition.OnExitWipeOff(.2f));
                }

            }
            DOVirtual.DelayedCall(.2f, () => onEndingVOFinished?.Invoke());
            OnComplete?.Invoke();
        }

        public GameObject GetCurrentObjectToHighlight()
        {
            if (currentSuccessfullyDragged < ObjectsToHighlight.Length)
            {
                return ObjectsToHighlight[currentSuccessfullyDragged];
            }
            else
            {
                return null;
            }
        }

        IEnumerator AudioPlay()
        {
#if UNITY_EDITOR
            if (SkipIntoAudio)
            {
                UtilityEventsManager.isControlEnabled = true;
                yield break;
            }
#endif
            foreach (var x in introAudios)
            {
                yield return new WaitForSeconds(initialAudioDelay);
                if (x != AudioID.none)
                {
                    var a = AudioLibrary.instance.GetAudioByEnum(x);
                    source.PlayOneShot(a);
                    yield return new WaitForSeconds(a.length + .1f);
                    /*while (source.isPlaying)
                    {
                        yield return null;
                    }*/
                }
            }
            /* if (startAudio != AudioID.none)
             {
                 yield return new WaitForSeconds(initialAudioDelay);
                 var a = AudioLibrary.instance.GetAudioByEnum(startAudio);
                 source.PlayOneShot(a);
                 while (source.isPlaying)
                 {
                     yield return null;
                 }
             }
             if (SecondAudio != AudioID.none)
             {
                 yield return new WaitForSeconds(SecondAudioDelay);
                 var a = AudioLibrary.instance.GetAudioByEnum(SecondAudio);
                 source.PlayOneShot(a);
                 while (source.isPlaying)
                 {
                     yield return null;
                 }
             }*/
            UtilityEventsManager.isControlEnabled = true;
            AudioComplete?.Invoke();
            // group.DOFade(0,.5f);
            // StartCoroutine(PlaySuccessAndNextAudio(clips.voiceOvers[currentSuccessfullyDragged].complete,clips.voiceOvers[currentSuccessfullyDragged+1].start,clips.voiceOvers[currentSuccessfullyDragged].startDelay));
        }

        void onDraggedObjectSuccess(object sender, UtilityEventsManager.DraggedObjectAttached data)
        {
            currentSuccessfullyDragged++;
            if (currentSuccessfullyDragged >= DragitemsLimit)
            {
                // StartCoroutine(PlaySuccessAndNextAudio(clips.voiceOvers[currentSuccessfullyDragged-1].complete,null,0));
                Debug.Log("Level Finish");
                //FOR CURRENT SUCCESSFULLY DRAGGED
                // var clip = AudioLibrary.instance.GetAudioByEnum(CounterVOs[currentSuccessfullyDragged - 1]);
                // StartCoroutine(PlaySuccessAndNextAudio(clip, null, .5f));
                //-------------------------------------------------------------------------
                DOVirtual.DelayedCall(.5f, () =>
                {
                    DOVirtual.DelayedCall(OnSuccessEventDelay, () => onLevelFinish?.Invoke());
                    StartCoroutine(PlayEndingVos(() =>
                    {
                        DOVirtual.DelayedCall(ChangeLevelAfterEndingVODelay,
                         () => UtilityEventsManager.onLevelFinish?.Invoke());
                    }));
                });
            }
            /*  else
              {
                  var clip = AudioLibrary.instance.GetAudioByEnum(CounterVOs[currentSuccessfullyDragged - 1]);
                  if (clip)
                  {
                      StartCoroutine(PlaySuccessAndNextAudio(clip, null, .3f));

                  }
                  else
                  {
                      Debug.LogError("CLIP NOT FOUND!, SKIPPING");
                  }
                  /* if(clips.voiceOvers!=null && clips.voiceOvers.Length>0){
                       StartCoroutine(PlaySuccessAndNextAudio(clips.voiceOvers[currentSuccessfullyDragged-1].complete,clips.voiceOvers[currentSuccessfullyDragged].start,clips.voiceOvers[currentSuccessfullyDragged].startDelay));
                   }
              }*/
        }

        IEnumerator PlaySuccessAndNextAudio(AudioClip clip, AudioClip next, float delay)
        {
            if (clip != null)
            {
                source.PlayOneShot(clip);
                while (source.isPlaying)
                {
                    yield return null;
                }
            }
            if (next != null)
            {
                yield return new WaitForSeconds(delay);
                source.PlayOneShot(next);
                while (source.isPlaying)
                {
                    yield return null;
                }

            }
        }

        public void LevelFinishEventCall(float delay)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                Debug.Log("Level Finish Event call");
                DOVirtual.DelayedCall(OnSuccessEventDelay, () => onLevelFinish?.Invoke());
                StartCoroutine(PlayEndingVos(() => { DOVirtual.DelayedCall(ChangeLevelAfterEndingVODelay, () => UtilityEventsManager.onLevelFinish?.Invoke()); }));
                // particle.Play();
            });
        }

        void OnDisable()
        {
            UtilityEventsManager.onDraggedObjectAttached -= onDraggedObjectSuccess;
        }
        void OnDestroy()
        {
            UtilityEventsManager.onDraggedObjectAttached -= onDraggedObjectSuccess;
        }

        float GetAspect()
        {
            var camera = Camera.main;
            if (camera == null)
            {
                camera = FindFirstObjectByType<Camera>();
            }
            float aspect = camera.aspect;


            return aspect;
        }

    }
}
