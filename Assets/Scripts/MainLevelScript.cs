using System.Collections;
using UnityEngine;

namespace USP.MiniGame.recognitionPatterns
{
    /// <summary>
    /// Script by Vishal Lakhani
    /// Email: usp.vishal@gmail.com
    /// Description:
    /// </summary>
    public class MainLevelScript : MonoBehaviour
    {
        #region Variables
        public static MainLevelScript instance;
        public GameObject mainMenu;
        public LevelData[] Levels;
        private LevelData currentLevel;
        private GameObject SpawnedLevel;

        public AudioID[] audios;
        AudioSource source;


        #endregion

        #region Unity Methods

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            source = gameObject.AddComponent<AudioSource>();
            mainMenu.SetActive(true);
            int index = 0;
            foreach (var x in Levels)
            {
                x.index = index;
                index++;
            }
            StartCoroutine(PlayStartingVO());

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// dont call it with transition it already handles transition
        /// </summary>
        /// <param name="index">Provide index of level</param>
        public void GenerateLevel(int index = 0)
        {
            Transition.Play(() =>
            {
                mainMenu.SetActive(false);
                if (SpawnedLevel != null)
                {
                    Destroy(SpawnedLevel);
                }
                currentLevel = Levels[index];
                SpawnedLevel = Instantiate(Levels[index].level);
            });

        }

        public void StartNextLevel()
        {
            foreach (var x in Levels)
            {
                if (!x.isComplete)
                {
                    GenerateLevel(x.index);
                    return;
                }
            }
            mainMenu.SetActive(true);
        }


        public void PlayNextLevel()
        {
            if (currentLevel != null)
            {
                int index = currentLevel.index;
                if (index < Levels.Length - 1)
                {
                    index++;
                    GenerateLevel(index);
                }
            }
        }

        public void ShowMainMenu()
        {
            Transition.Play(() =>
            {
                currentLevel = null;
                if (SpawnedLevel != null)
                {
                    Destroy(SpawnedLevel);
                }
                mainMenu.SetActive(true);
            });
        }

        public void MarkedLevelComplete(int index = -1)
        {
            if (index == -1)
            {
                if (currentLevel != null)
                {
                    currentLevel.isComplete = true;
                }
            }
            else
            {
                Levels[index].isComplete = true;
            }
        }

        #endregion

        #region Private Methods
        IEnumerator PlayStartingVO()
        {

            UtilityEventsManager.isControlEnabled = false;
            yield return new WaitForSeconds(.5f);
            foreach (var x in audios) // use sequence if needed
            {
                yield return new WaitForSeconds(.5f);

                if (x != AudioID.none)
                {
                    var a = AudioLibrary.instance.GetAudioByEnum(x);
                    source.PlayOneShot(a);
                    yield return new WaitForSeconds(a.length + .1f);

                }
            }
            UtilityEventsManager.isControlEnabled = true;
        }


        #endregion
    }
    [System.Serializable]
    public class LevelData
    {
        public int index;
        public GameObject level;
        public bool isComplete;
    }
}