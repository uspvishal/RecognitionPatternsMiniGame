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
            foreach (var x in levels)
            {
                x.SetActive(false);

            }
            if (currentCount < levels.Length)
            {
                levels[currentCount].SetActive(true);
            }
            else
            {
                onLevelSetComplete?.Invoke();
            }
        }
    }
}
