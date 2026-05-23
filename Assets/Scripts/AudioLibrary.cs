using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace USP.MiniGame.Addition
{
    /// <summary>
    /// Script by Vishal Lakhani
    /// Email: usp.vishal@gmail.com
    /// Description:
    /// </summary>
    public class AudioLibrary : MonoBehaviour
    {
        #region Variables

        public static AudioLibrary instance;
        public List<AudioClipsC> audioLib;
        [System.Serializable]
        public class AudioClipsC
        {
            public string name;
            public AudioClip female, male;
        }

        public List<AudioClip> femaleAudio;
        public List<AudioClip> maleAudio;

        bool isMale;

        #endregion

        #region Unity Methods

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }

        public AudioClip GetAudioByClip(AudioClip clip)
        {
            var audioC = audioLib.Find(x => x.name == clip.name);
            return isMale ? audioC.male : audioC.female;
        }

        public AudioClip GetAudioByName(string namee)
        {
            var audioC = audioLib.Find(x => x.name == namee);
            return isMale ? audioC.male : audioC.female;
        }

        public AudioClip GetAudioByEnum(AudioID id)
        {
            if (id == 0)
            {
                return null;
            }
            Debug.Log((int)id);
            var audioC = audioLib[(int)id - 1];
            return isMale ? audioC.male : audioC.female;
        }

        public AudioClip GetAudioByIndex(int index)
        {
            var audioC = audioLib[index];
            return isMale ? audioC.male : audioC.female;
        }


        #endregion

        #region Public Methods



        #endregion

        #region Private Methods
#if UNITY_EDITOR
        void OnValidate()
        {

            /* foreach (var x in femaleAudio)
             {
                 if (x.name.Contains("."))
                 {
                     x.name = x.name.Replace(".", "");
                 }
                 string useName = x.name.ToUpper().Replace("FEMALE_", "");
                 var y = audioLib.Find(y => useName == y.name);
                 if (y != null && y.female == null)
                 {
                     y.female = x;
                 }
                 else
                 {
                     audioLib.Add(new AudioClipsC() { name = useName, female = x });
                 }
             }

             foreach (var x in maleAudio)
             {
                 if (x.name.Contains("."))
                 {
                     x.name = x.name.Replace(".", "");
                 }
                 string useName = x.name.ToUpper().Replace("MALE_", "");
                 var y = audioLib.Find(y => useName == y.name);
                 if (y != null && y.male == null)
                 {
                     y.male = x;
                 }
                 else
                 {
                     audioLib.Add(new AudioClipsC() { name = useName, male = x });
                 }
             }
             //------------------------------------

             foreach (var x in audioLib)
             {
                 if (string.IsNullOrEmpty(x.name))
                 {
                     if (x.female != null)
                     {
                         x.name = x.female.name;
                     }
                     else
                         if (x.male != null)
                         {
                             x.name = x.male.name;
                         }
                 }
             }*/

        }
#endif


        #endregion
    }
}