using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace USP.MiniGame.Addition
{
    /// <summary>
    /// Script by Vishal Lakhani
    /// Email: usp.vishal@gmail.com
    /// Description:
    /// </summary>
    public class IndependentAudioPlay : MonoBehaviour
    {
        public AudioID audioToPlay;
        public UnityEvent OnCompleteCallback;
        IEnumerator Start()
        {
            if (audioToPlay != AudioID.none)
            {
                UtilityEventsManager.isControlEnabled = false;
            }
            var x = gameObject.AddComponent<AudioSource>();
            var clip = AudioLibrary.instance.GetAudioByEnum(audioToPlay);
            x.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length + .1f);
            OnCompleteCallback?.Invoke();
            if (audioToPlay != AudioID.none)
            {
                UtilityEventsManager.isControlEnabled = true;
            }
        }
    }
}