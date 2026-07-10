using UnityEngine;
using UnityEngine.Events;

namespace USP.MiniGame.recognitionPatterns
{
    public class LevelSetManager : MonoBehaviour
    {
        public GameObject[] levels;
        int currentCount = 0;

        public AudioID[] EndingAudios;
        public UnityEvent onLevelSetComplete;

        void OnEnable()
        {
            UtilityEventsManager.onLevelFinish += OnLevelComplete;
            currentCount = 0;
            levels[0].SetActive(true);
        }

        void OnDisable()
        {
            UtilityEventsManager.onLevelFinish -= OnLevelComplete;
        }
        public void OnLevelComplete()
        {
            currentCount++;

            if (currentCount < levels.Length)
            {
                foreach (var x in levels)
                {
                    x.SetActive(false);

                }
                levels[currentCount].SetActive(true);
            }
            else
            {

                MainLevelScript.instance.MarkedLevelComplete();
                MainLevelScript.instance.PlayNextLevel();
                onLevelSetComplete?.Invoke();


            }

        }
    }
}
