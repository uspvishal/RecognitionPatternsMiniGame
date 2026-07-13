using DG.Tweening;
using UnityEngine;

namespace USP.MiniGame.recognitionPatterns
{
    public class TutorialManager : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public Transform targetGraphic;
        public float TimeToShow;
        public float secondTimer;
        float currentTimer;

        bool isTargetGraphicShowing;
        bool startTimer;
        Vector3 currentPos;
        Vector3 FinalPoint;

        public float movementDelay = .2f;
        public float ScaleDelay = .2f;
        private Sequence seq;
        public float scaleOnPickAnim = .8f;
        public float scaleOnPickAnimTimer = .2f;
        public float gapbetweenTween = .5f;
        Vector3 ogScale;
        public LineRenderer Line;
        //DragAlongLineRenderer2D currentDraggable;
        float dragfollowProgress = 0;
        public static TutorialManager instance;
        SpriteRenderer renderer;

        void Awake()
        {
            instance = this;
        }

        void OnEnable()
        {
            isTargetGraphicShowing = false;
            renderer = targetGraphic.GetComponent<SpriteRenderer>();
            currentTimer = TimeToShow;
            UtilityEventsManager.onLevelFinish += OnTimerStop;
            UtilityEventsManager.onLevelFinish += ResetTimer;
            UtilityEventsManager.OnLevelStart += OnTimerStart;
            UtilityEventsManager.OnUserInteracted += UserInteracted;
            UtilityEventsManager.onDraggedObjectCancelled += UserInteractedCancel;
            UtilityEventsManager.onDraggedObjectAttached += UserInteractedAttached;
            UtilityEventsManager.OnAnswerProvided += OnButtonClicked;
            ogScale = targetGraphic.transform.localScale;
            startTimer = true;
        }

        public void ResetEverything()
        {
            targetGraphic.GetComponent<SpriteRenderer>().enabled = false;
            ResetTimer();
            if (seq == null) return;
            seq.Pause();
            seq.Rewind();
            isTargetGraphicShowing = false;
        }

        void OnButtonClicked(string answer)
        {
            targetGraphic.GetComponent<SpriteRenderer>().enabled = false;
            ResetTimer();
            if (seq == null) return;
            seq.Pause();
            seq.Rewind();
            isTargetGraphicShowing = false;
        }

        void UserInteracted(object sender, UtilityEventsManager.UserInteracted data)
        {
            targetGraphic.GetComponent<SpriteRenderer>().enabled = false;
            ResetTimer();
            if (seq == null) return;
            seq.Pause();
            seq.Rewind();
            isTargetGraphicShowing = false;
        }

        void UserInteractedCancel(object sender, UtilityEventsManager.DraggedObjectCancelled data)
        {
            targetGraphic.GetComponent<SpriteRenderer>().enabled = false;
            ResetTimer();
            if (seq == null) return;
            seq.Pause();
            seq.Rewind();
            isTargetGraphicShowing = false;
        }

        void UserInteractedAttached(object sender, UtilityEventsManager.DraggedObjectAttached data)
        {
            targetGraphic.GetComponent<SpriteRenderer>().enabled = false;
            ResetTimer();
            if (seq == null) return;
            seq.Pause();
            seq.Rewind();
            isTargetGraphicShowing = false;
        }

        public void ResetManual()
        {
            targetGraphic.GetComponent<SpriteRenderer>().enabled = false;
            ResetTimer();
            if (seq == null) return;
            seq.Pause();
            seq.Rewind();
            isTargetGraphicShowing = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!UtilityEventsManager.isControlEnabled)
            {
                isTargetGraphicShowing = renderer.enabled = false;
                return;
            }
            if (startTimer /*&& UtilityEventsManager.isControlEnabled*/)
            {
                if (currentTimer > 0)
                {
                    isTargetGraphicShowing = renderer.enabled;
                    currentTimer -= Time.deltaTime;
                }
                else
                {

                    if (!isTargetGraphicShowing)
                    {
                        ShowTargetGraphic();
                    }
                }


            }
        }


        void BuildSequence()
        {
            targetGraphic.GetComponent<SpriteRenderer>().enabled = true;
            bool isTargetFound = false;
            var lvl = FindObjectOfType<Level>();
            if (lvl != null)
            {
                var highlightingPiece = lvl.GetCurrentObjectToHighlight()?.GetComponent<Piece>();
                if (highlightingPiece)
                {
                    highlightingPiece.RecalculateAttachingObject();
                    currentPos = highlightingPiece.transform.position;
                    FinalPoint = highlightingPiece.myTarget.transform.position;
                    isTargetFound = true;
                }
                else
                {
                    isTargetFound = false;
                }
            }
            if (isTargetFound == false) // if no highlighting piece not found then find any random. this si for sequence of drag and drop elements stated by level
            {
                var pieces = FindObjectsByType<Piece>(sortMode: FindObjectsSortMode.None);
                if (pieces != null && pieces.Length > 0)
                {
                    foreach (var x in pieces)
                    {
                        if (!x.isComplete)
                        {
                            x.RecalculateAttachingObject();
                            isTargetFound = true;
                            currentPos = x.transform.position;
                            if (x.myTarget)
                                FinalPoint = x.myTarget.transform.position;

                            else
                            {
                                var obj = FindAnyObjectByType<StackItemDrop>();
                                if (obj)
                                {
                                    FinalPoint = obj.transform.position;
                                }
                            }
                            break;
                        }
                    }
                }


                /* if (isTargetFound == false)
                 {
                     var dragableObjects = FindObjectsByType<DragAlongLineRenderer2D>(sortMode: FindObjectsSortMode.None);
                     if (dragableObjects != null && dragableObjects.Length > 0)
                     {
                         if (dragableObjects[0].gameObject.activeInHierarchy)
                         {
                             currentDraggable = dragableObjects[0];
                             currentPos = dragableObjects[0].transform.position;
                             FinalPoint = dragableObjects[0].GetLastPointWorld();
                             isTargetFound = true;
                         }
                     }
                     else
                     {
                         seq = null;
                         return;
                     }

                 }*/
            }

            if (isTargetFound == false)
            {
                var buttons = FindObjectsByType<SpriteBtn>(sortMode: FindObjectsSortMode.None);
                foreach (var x in buttons)
                {
                    if (x.WillBeUsedInTutorial)
                    {
                        isTargetFound = true;
                        currentPos = x.transform.position;
                        FinalPoint = x.transform.position;
                        break;
                    }
                }
            }

            if (isTargetFound == false)
            {
                return;
            }
            targetGraphic.transform.position = currentPos - new Vector3(Random.Range(-.1f, .1f), Random.Range(-.1f, .1f));
            seq?.Kill();
            seq = DOTween.Sequence()
                .SetAutoKill(false)     // keep it reusable
                .Pause();
            seq.Append(targetGraphic.GetComponent<SpriteRenderer>().DOColor(Color.white, .5f));
            seq.Append(targetGraphic.DOMove(currentPos, movementDelay)).SetEase(Ease.OutQuad);
            seq.Append(targetGraphic.DOScale(scaleOnPickAnim * ogScale, scaleOnPickAnimTimer).SetEase(Ease.OutQuad));
            seq.Append(targetGraphic.DOMove(FinalPoint, movementDelay).SetDelay(.3f)).SetEase(Ease.OutQuad);
            /*if (!currentDraggable)
            {
               
            }
            else
            {
                seq.Append(DOTween.To(() => dragfollowProgress,
                 x => dragfollowProgress = x, 1f, 2f).OnUpdate(() =>
                 {
                     targetGraphic.DOMove(currentDraggable.GetPositionByProgress(dragfollowProgress), movementDelay);
                 }).SetDelay(.3f));
            }*/
            seq.Append(targetGraphic.DOScale(ogScale, scaleOnPickAnimTimer).SetDelay(.3f)).SetEase(Ease.OutQuad);
            seq.Append(targetGraphic.GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 0), .3f).SetDelay(.2f));
            if (gapbetweenTween > 0f) seq.AppendInterval(gapbetweenTween);
            seq.SetLoops(-1, LoopType.Restart);
        }

        void ShowTargetGraphic()
        {
            isTargetGraphicShowing = true;
            BuildSequence();
            if (seq != null)
            {
                seq.PlayForward();
            }
        }

        void OnTimerStop()
        {
            startTimer = false;
            if (seq == null) return;
            seq.Pause();
            seq.Rewind(); // returns to the beginning (because SetAutoKill(false))
        }

        void OnTimerStart()
        {
            startTimer = true;
        }

        void ResetTimer()
        {
            currentTimer = secondTimer;
        }

        void OnDestroy()
        {
            UtilityEventsManager.onLevelFinish -= OnTimerStop;
            UtilityEventsManager.onLevelFinish -= ResetTimer;
            UtilityEventsManager.OnLevelStart -= OnTimerStart;
            UtilityEventsManager.OnUserInteracted -= UserInteracted;
            UtilityEventsManager.onDraggedObjectCancelled -= UserInteractedCancel;
            UtilityEventsManager.onDraggedObjectAttached -= UserInteractedAttached;
            UtilityEventsManager.OnAnswerProvided -= OnButtonClicked;
        }

        void OnDisable()
        {
            UtilityEventsManager.onLevelFinish -= OnTimerStop;
            UtilityEventsManager.onLevelFinish -= ResetTimer;
            UtilityEventsManager.OnLevelStart -= OnTimerStart;
            UtilityEventsManager.OnUserInteracted -= UserInteracted;
            UtilityEventsManager.onDraggedObjectCancelled -= UserInteractedCancel;
            UtilityEventsManager.onDraggedObjectAttached -= UserInteractedAttached;
            UtilityEventsManager.OnAnswerProvided -= OnButtonClicked;
        }
    }
}

