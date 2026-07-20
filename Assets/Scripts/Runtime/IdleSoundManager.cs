
using System.Collections.Generic;
using UnityEngine;

namespace USP.MiniGame.recognitionPatterns
{
    /// <summary>
    /// Script by Vishal Lakhani
    /// Email: usp.vishal@gmail.com
    /// Description:
    /// </summary>
    public class IdleSoundManager : MonoBehaviour
    {
        AudioSource source;

        public AudioClip clip;

        float currentTimer;
        public float firstTime;
        public float SecondTime;
        public List<AudioSource> AudiosToCheck;

        public static IdleSoundManager instance;

        void Awake()
        {
            instance = this;
        }
        void Start()
        {
            currentTimer = firstTime;
            source = gameObject.AddComponent<AudioSource>();
            UtilityEventsManager.resetTimers += ResetTimer;
            InvokeRepeating(nameof(KeepCheckingIfAnyAudiosPlaying), 1, 1);
        }

        public void AddAudioSourceToCheckList(AudioSource source)
        {
            if (!AudiosToCheck.Contains(source))
            {
                AudiosToCheck.Add(source);
            }
        }

        void OnEnable()
        {
            UtilityEventsManager.OnUserInteracted += UserInteracted;
            UtilityEventsManager.OnAnswerProvided += OnAnswerProvided;
            UtilityEventsManager.onDraggedObjectAttached += UserItemAttached;
            OnInteractManual();
        }

        void KeepCheckingIfAnyAudiosPlaying()
        {
            if (AudiosToCheck.Count == 0)
            {
                return;
            }
            AudiosToCheck.RemoveAll(x => x == null);
            foreach (var x in AudiosToCheck)
            {
                if (x.isPlaying)
                {
                    source.Stop();
                    ResetTimer();
                    break;
                }
            }
        }

        void UserInteracted(object sender, UtilityEventsManager.UserInteracted e)
        {
            ResetTimer();
            if (source.isPlaying)
            {
                source.Stop();
            }
        }

        void UserItemAttached(object sender, UtilityEventsManager.DraggedObjectAttached e)
        {
            ResetTimer();
            if (source.isPlaying)
            {
                source.Stop();
            }
        }

        void OnAnswerProvided(string a)
        {
            ResetTimer();
            if (source.isPlaying)
            {
                source.Stop();
            }
        }

        void OnDisable()
        {
            UtilityEventsManager.OnUserInteracted -= UserInteracted;
            UtilityEventsManager.OnAnswerProvided -= OnAnswerProvided;
            UtilityEventsManager.onDraggedObjectAttached -= UserItemAttached;
            UtilityEventsManager.resetTimers -= ResetTimer;
            CancelInvoke(nameof(KeepCheckingIfAnyAudiosPlaying));
        }

        void OnDestroy()
        {
            UtilityEventsManager.OnUserInteracted -= UserInteracted;
            UtilityEventsManager.OnAnswerProvided -= OnAnswerProvided;
            UtilityEventsManager.onDraggedObjectAttached -= UserItemAttached;
            UtilityEventsManager.resetTimers -= ResetTimer;
            CancelInvoke(nameof(KeepCheckingIfAnyAudiosPlaying));
        }

        public void OnInteractManual()
        {
            ResetTimer();
            if (source && source.isPlaying)
            {
                source.Stop();
            }

        }

        public void ResetTimer()
        {
            currentTimer = SecondTime;

        }

        void Update()
        {
            if (!UtilityEventsManager.isControlEnabled)
            {
                return;
            }
            if (currentTimer > 0)
            {

                currentTimer -= Time.deltaTime;
            }
            else
            {
                ResetTimer();
                var lvl = FindAnyObjectByType<Level>();

                if (lvl)
                {
                    var individualAudios = FindObjectsByType<IndependentAudioPlay>(sortMode: FindObjectsSortMode.None);
                    if (lvl.GetComponent<AudioSource>().isPlaying)
                    {

                        return;
                    }
                    var objectScales = FindObjectsByType<ObjectScaleHighLighting>(sortMode: FindObjectsSortMode.None);
                    foreach (var x in objectScales)
                    {
                        if (x.GetComponent<AudioSource>().isPlaying)
                        {
                            return;
                        }
                    }
                    foreach (var x in individualAudios)
                    {
                        if (x.GetComponent<AudioSource>().isPlaying)
                        {
                            return;
                        }
                    }
                }
                source.PlayOneShot(clip);

            }
        }


    }
}