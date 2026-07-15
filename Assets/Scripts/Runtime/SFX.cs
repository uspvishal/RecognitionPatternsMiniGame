using UnityEngine;

namespace USP.MiniGame.recognitionPatterns
{
    /// <summary>
    /// Script by Vishal Lakhani
    /// Email: usp.vishal@gmail.com
    /// Description:
    /// </summary>
    public class SFX : MonoBehaviour
    {
        AudioSource source;
        public AudioClip pick;
        public AudioClip onReturn;
        public AudioClip Wrong;
        public AudioClip correct;
        void Start()
        {
            source = gameObject.AddComponent<AudioSource>();
            UtilityEventsManager.onDraggedObjectAttached += playSuccess;
            UtilityEventsManager.OnUserInteracted += playPick;
            UtilityEventsManager.onDraggedObjectCancelled += playReturn;
        }

        void playPick(object Sender, UtilityEventsManager.UserInteracted itm)
        {
            // if (!source.isPlaying)
            {
                source.PlayOneShot(pick);
            }
        }

        void playReturn(object Sender, UtilityEventsManager.DraggedObjectCancelled itm)
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(onReturn);
            }
        }

        void playSuccess(object Sender, UtilityEventsManager.DraggedObjectAttached itm)
        {
            // if (!source.isPlaying)
            {
                source.PlayOneShot(correct);
            }
        }

        void playWrong(object Sender, UtilityEventsManager.UserInteracted itm)
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(Wrong);
            }
        }

        void OnDisable()
        {
            UtilityEventsManager.onDraggedObjectAttached -= playSuccess;
            UtilityEventsManager.OnUserInteracted -= playPick;
            UtilityEventsManager.onDraggedObjectCancelled -= playReturn;
        }

        void OnDestroy()
        {
            UtilityEventsManager.onDraggedObjectAttached -= playSuccess;
            UtilityEventsManager.OnUserInteracted -= playPick;
            UtilityEventsManager.onDraggedObjectCancelled -= playReturn;
        }
    }
}