
using UnityEngine;

namespace USP.MiniGame.recognitionPatterns.Bunny
{
    /// <summary>
    /// Script by Vishal Lakhani
    /// Email: usp.vishal@gmail.com
    /// Description:
    /// </summary>
    public class Bunny : MonoBehaviour
    {
        public Animator anim;
        public string SuccessTrigger;
        public string wrongTrigger;

        public string idleTrigger;
        public string celebratory;

        void Start()
        {
            anim = GetComponent<Animator>();
            UtilityEventsManager.onDraggedObjectAttached += OnSuccess;
            UtilityEventsManager.onDraggedObjectCancelled += OnWrong;
            UtilityEventsManager.OnEndingVOStart += CelebrateAnim;
            UtilityEventsManager.OnLevelStart += OnLevelChange;
            UtilityEventsManager.CorrectAnswer += OnAnswerProvided;
        }

        public void OnSuccess(object sender, UtilityEventsManager.DraggedObjectAttached a)
        {
            anim.SetTrigger(SuccessTrigger);
        }

        public void OnWrong(object sender, UtilityEventsManager.DraggedObjectCancelled a)
        {
            anim.SetTrigger(wrongTrigger);
        }

        public void OnLevelChange()
        {
            anim.SetTrigger(idleTrigger);
        }

        void OnAnswerProvided(bool val)
        {
            if (val)
            {
                anim.SetTrigger(SuccessTrigger);
            }
            else
            {
                anim.SetTrigger(wrongTrigger);
            }
        }

        public void CelebrateAnim()
        {
            anim.SetTrigger(celebratory);

        }

        void OnDestroy()
        {
            UtilityEventsManager.onDraggedObjectAttached -= OnSuccess;
            UtilityEventsManager.onDraggedObjectCancelled -= OnWrong;
            UtilityEventsManager.OnEndingVOStart -= CelebrateAnim;
            UtilityEventsManager.OnLevelStart -= OnLevelChange;
        }

        void OnDisable()
        {
            UtilityEventsManager.onDraggedObjectAttached -= OnSuccess;
            UtilityEventsManager.onDraggedObjectCancelled -= OnWrong;
            UtilityEventsManager.OnEndingVOStart -= CelebrateAnim;
            UtilityEventsManager.OnLevelStart -= OnLevelChange;
        }
    }




}