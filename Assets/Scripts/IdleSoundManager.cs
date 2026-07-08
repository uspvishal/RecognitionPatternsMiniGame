
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
        void Start()
        {
            currentTimer = firstTime;
            source = gameObject.AddComponent<AudioSource>();

        }

        void OnEnable()
        {
            UtilityEventsManager.OnUserInteracted += UserInteracted;
            UtilityEventsManager.OnAnswerProvided += OnAnswerProvided;
            UtilityEventsManager.onDraggedObjectAttached += UserItemAttached;
            OnInteractManual();
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
        }

        void OnDestroy()
        {
            UtilityEventsManager.OnUserInteracted -= UserInteracted;
            UtilityEventsManager.OnAnswerProvided -= OnAnswerProvided;
            UtilityEventsManager.onDraggedObjectAttached -= UserItemAttached;
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
            if (currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;
            }
            else
            {
                ResetTimer();
                var lvl = FindAnyObjectByType<Level>();
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
                source.PlayOneShot(clip);

            }
        }


    }
}