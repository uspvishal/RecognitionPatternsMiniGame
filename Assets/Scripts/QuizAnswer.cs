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
    public class QuizAnswer : MonoBehaviour
    {
        public string TargetAnswer;

        public UnityEvent OnCorrect;
        public UnityEvent OnWrongAnswer;



        void Start()
        {
            UtilityEventsManager.OnAnswerProvided += ProvideAnswer;
        }
        public void ProvideAnswer(string val)
        {
            Debug.Log("Answer Provided" + val);
            if (TargetAnswer == val)
            {
                OnCorrect?.Invoke();
                UtilityEventsManager.CorrectAnswer?.Invoke(true);
                GetComponentInParent<Level>().PlayRandomVOFromCorrectPlacement();
                GetComponentInParent<Level>().LevelFinishEventCall(delay: 0);
            }
            else
            {
                OnWrongAnswer?.Invoke();
                GetComponentInParent<Level>().PlayRandomVOFromWrontPlacement();
                DOVirtual.DelayedCall(2, () =>
                {
                    UtilityEventsManager.isControlEnabled = true;
                });
                UtilityEventsManager.CorrectAnswer?.Invoke(false);
            }
        }

        void OnDisable()
        {
            UtilityEventsManager.OnAnswerProvided -= ProvideAnswer;
        }

        void OnDestroy()
        {
            UtilityEventsManager.OnAnswerProvided -= ProvideAnswer;
        }
    }
}