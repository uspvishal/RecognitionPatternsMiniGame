using System;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// AUTHOR: VISHAL LAKHANI 
/// EMAIL : usp.vishal@gmail.com
/// DESCRIPTION: stays in the game for events make sure you rename the namespace 
/// 
/// </summary>
namespace USP.MiniGame.recognitionPatterns
{
    public static class UtilityEventsManager
    {
        public static EventHandler<DraggedObjectAttached> onDraggedObjectAttached;
        public static EventHandler<UserInteracted> OnUserInteracted;
        public static EventHandler<UserInteracted> OnUserInteractedWrong;

        public static EventHandler<DraggedObjectCancelled> onDraggedObjectCancelled;

        public static bool isControlEnabled
        {
            get { /*Debug.Log("Check Control Enabled " + controlEnabled);*/ return controlEnabled; }
            set
            {

                controlEnabled = value;
                Debug.Log("<color=RED> control Enabled: " + controlEnabled);
            }
        }

        public static DraggableObject CurrentPieceDragged;
        static bool controlEnabled;

        public static UnityAction onLevelFinish, OnLevelStart, OnEndingVOStart;
        public static UnityAction<string> OnAnswerProvided;

        public static UnityAction<bool> CorrectAnswer;


        public static UnityAction onLevelExit;

        public static UnityAction<float> onTransistion;

        public static UnityAction resetTimers;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public class DraggedObjectAttached : EventArgs
        {
            public GameObject targetObject { get; }
            public GameObject attachedOn { get; }

            public DraggedObjectAttached(GameObject obj, GameObject attachedOn)
            {
                targetObject = obj;
                this.attachedOn = attachedOn;
            }
        }

        public static UnityAction<Piece> OnItemRemoved, OnGoBack;

        public class DraggedObjectCancelled : EventArgs
        {
            public GameObject targetObject { get; }


            public DraggedObjectCancelled(GameObject obj)
            {
                targetObject = obj;
            }
        }

        public class UserInteracted : EventArgs
        {
            public GameObject targetObject { get; }


            public UserInteracted(GameObject obj)
            {
                targetObject = obj;

            }
        }

    }
}
