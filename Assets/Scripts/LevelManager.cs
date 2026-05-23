using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
namespace USP.MiniGame.Addition
{

    public class LevelManager : MonoBehaviour
    {
        public GameObject[] Levels;
        int currentLevel;
        public float delayinBetweenlevels = 2;
        public int[] skipTransistionOnLevels;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Application.targetFrameRate = 60;
            UtilityEventsManager.onLevelFinish += ChangeLevel;
#if UNITY_EDITOR
            int counter = 0;
            foreach (var x in Levels)
            {
                if (x.activeInHierarchy)
                {
                    currentLevel = counter;
                    break;
                }
                counter++;
            }


#else
           if(!Levels[0].activeInHierarchy){

  foreach (var x in Levels)
            {
                x.SetActive(false);

            }
            Levels[0].SetActive(true);
                
           }
#endif
        }

        void ChangeLevel()
        {
            if (!skipTransistionOnLevels.Contains(currentLevel)) //skip transisiton animation which has in skiptransisiton on levels;
            {
                UtilityEventsManager.onTransistion?.Invoke(delayinBetweenlevels - .6f);
                DOVirtual.DelayedCall(delayinBetweenlevels, () =>
                {
                    if (currentLevel < Levels.Length - 1)
                    {
                        currentLevel++;
                        foreach (var x in Levels)
                        {
                            x.SetActive(false);
                        }
                        Levels[currentLevel].SetActive(true);
                    }
                    else
                    {
                        UtilityEventsManager.onLevelExit?.Invoke();
                    }
                });
            }
            else
            {

                if (currentLevel < Levels.Length - 1)
                {
                    currentLevel++;
                    foreach (var x in Levels)
                    {
                        x.SetActive(false);
                    }
                    Levels[currentLevel].SetActive(true);
                }
                else
                {
                    UtilityEventsManager.onLevelExit?.Invoke();
                }

            }
        }

        void OnDestroy()
        {
            UtilityEventsManager.onLevelFinish -= ChangeLevel;
        }

        void OnDisable()
        {
            UtilityEventsManager.onLevelFinish -= ChangeLevel;
        }


    }
}
