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
    public class MainLevelScript : MonoBehaviour
    {
        #region Variables
        public static MainLevelScript instance;
        public GameObject mainMenu;
        public GameObject IpadBg, MobileBG;
        public LevelData[] Levels;
        private LevelData currentLevel;
        private GameObject SpawnedLevel;

        public AudioID[] audios, endingVOs;
        AudioSource source;

        public UnityEvent allComplete;

        Coroutine startingAudio;
        #endregion

        #region Unity Methods

        private void Awake()
        {
            instance = this;
        }

        void OnEnable()
        {
            SetControls();
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            UtilityEventsManager.OnUserInteracted += UserInteracted;
            IpadBg.SetActive(!CameraAutoFit.IsWideAspect);
            MobileBG.SetActive(CameraAutoFit.IsWideAspect);
            source = gameObject.AddComponent<AudioSource>();
            IdleSoundManager.instance.AddAudioSourceToCheckList(source);
            mainMenu.SetActive(true);
            int index = 0;
            foreach (var x in Levels)
            {
                x.index = index;
                index++;
            }
            startingAudio = StartCoroutine(PlayStartingVO());

        }


        void SetControls()
        {
            UtilityEventsManager.isControlEnabled = false;
            DOVirtual.DelayedCall(1, () => { UtilityEventsManager.isControlEnabled = true; });
        }

        void UserInteracted(object sender, UtilityEventsManager.UserInteracted data)
        {
            if (startingAudio != null)
            {
                source.Stop();
                StopCoroutine(startingAudio);
            }
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
        
        public void GenerateFromStartLevel()
        {
            foreach (var x in Levels)
            {
                x.isComplete = false;
            }
            Transition.Play(() =>
            {
                mainMenu.SetActive(false);
                if (SpawnedLevel != null)
                {
                    Destroy(SpawnedLevel);
                }
                currentLevel = Levels[0];
                SpawnedLevel = Instantiate(Levels[0].level);
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
            Transition.Play(() =>
            {
                mainMenu.SetActive(true);
                allComplete.Invoke();

            }, () =>
            {

                StartCoroutine(PlayEndingVOs());
            });

        }

        void OnDestroy()
        {
            UtilityEventsManager.OnUserInteracted -= UserInteracted;
        }


        public void PlayNextLevel()
        {
            Debug.Log("Play Next LEvel");
            if (currentLevel != null)
            {
                int index = currentLevel.index;
                if (index < Levels.Length - 1)
                {
                    index++;
                    GenerateLevel(index);
                }
                else
                {
                    ShowMainMenu();
                }
            }
            else
            {
                Debug.LogError("ELSE CONDITION FIRED!");
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

        IEnumerator PlayEndingVOs()
        {

            UtilityEventsManager.isControlEnabled = false;
            yield return new WaitForSeconds(.5f);
            foreach (var x in endingVOs) // use sequence if needed
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
        bool iscomplete;
        public bool isComplete { get { return iscomplete; } set { iscomplete = value; filledItem.SetActive(iscomplete); } }
        public GameObject filledItem;


    }
}