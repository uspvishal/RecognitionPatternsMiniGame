using System;
using UnityEngine;
using UnityEngine.Events;

namespace USP.MiniGame.recognitionPatterns
{
    /// <summary>
    /// Script by Vishal Lakhani
    /// Email: usp.vishal@gmail.com
    /// Description:
    /// </summary>
    public class AudioEvent : MonoBehaviour
    {
     
        public AudioClip audioClip;
        private AudioSource source;
        #region Unity Methods

        private void Start()
        {
            source = gameObject.AddComponent<AudioSource>();
        }

       

        #endregion

        #region Public Methods

        public void PlayAudio()
        {
            if(!source.isPlaying)
                source.PlayOneShot(audioClip);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}