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


    public class SpriteBtn : MonoBehaviour
    {
        Vector3 currentScale, childScale;
        bool tookDefaultScale, tookChildScale;
        public UnityEvent OnClicked;
        public float scaleUpValue;
        public float duration;
        AudioSource source;

        [System.Serializable]
        public struct DelayedEvents
        {
            public UnityEvent m_event;
            public float delay;
        }
        public DelayedEvents[] delayedEvents;

        public AudioClip clip;

        [SerializeField] bool useChildEffect;
        [SerializeField] float childScaleModifier = .8f;
        public bool willBlockControls = true;
        public string Answer;
        public bool WillBeUsedInTutorial = false;
        public bool hideOnClick = false;

        public GameObject particle;
        public bool WillScaleItems;

        public bool ConsiderSuccessfulClick;
        public bool PlayParticle;
        public float playParticleDelay;


        void Start()
        {
            source = gameObject.AddComponent<AudioSource>();
        }

        void OnMouseDown()
        {
            Debug.Log("Clciked here ", this);
            if (!UtilityEventsManager.isControlEnabled)
            {
                return;
            }
            if (PlayParticle)
            {
                DOVirtual.DelayedCall(playParticleDelay, () =>
                {
                    MiscSpawnables.GetParticleSpawnable(this.transform.position);
                });
            }


            if (willBlockControls)
            {
                UtilityEventsManager.isControlEnabled = false;
            }
            if (WillBeUsedInTutorial)
            {
                UtilityEventsManager.CorrectAnswer += ShakeEffect;
                UtilityEventsManager.OnAnswerProvided?.Invoke(Answer);
            }

            foreach (var x in FindObjectsByType<Piece>(sortMode: FindObjectsSortMode.None))
            {
                if (x.isComplete)
                {
                    x.ScaleHighLight();
                }
            }

            if (!tookDefaultScale)
            {
                currentScale = transform.localScale;
                tookDefaultScale = true;
            }
            if (clip && !source.isPlaying)
            {
                source.PlayOneShot(clip);
            }
            transform.DOScale(currentScale * scaleUpValue, duration).OnComplete(() =>
            {
                transform.DOScale(currentScale, duration).OnComplete(() =>
                {
                    OnClicked?.Invoke();
                    if (hideOnClick)
                    {
                        HidewithAnimation();

                    }
                });
            });

            if (particle)
            {
                var p = Instantiate(particle, this.transform.parent).GetComponent<ParticleSystem>();
                p.transform.position = this.transform.position + new Vector3(0, 0, -3);
                p.transform.Rotate(-90, 0, 0);
                p.Play();
                Destroy(p.gameObject, 3f);
            }

            if (useChildEffect)
                ChildEffectOne();

            UtilityEventsManager.CorrectAnswer -= ShakeEffect;
            ExecuteDelayedEvents();

        }

        void ExecuteDelayedEvents()
        {
            if (ConsiderSuccessfulClick)
            {
                UtilityEventsManager.OnUserInteracted?.Invoke(this, new UtilityEventsManager.UserInteracted(this.gameObject));
            }
            foreach (var x in delayedEvents)
            {
                DOVirtual.DelayedCall(x.delay, () => { x.m_event?.Invoke(); });
            }
        }


        void ShakeEffect(bool val)
        {

            if (!val)
            {
                float interval = .1f;
                DOVirtual.DelayedCall(.3f, () =>
                {
                    if (transform.childCount > 0)
                    {
                        float pos = transform.GetChild(0).localPosition.x;
                        transform.GetChild(0).DOLocalMoveX(.3f, interval);
                        transform.GetChild(0).DOLocalMoveX(-.3f, interval).SetDelay(interval * 2);
                        transform.GetChild(0).DOLocalMoveX(.3f, interval).SetDelay(interval * 3);
                        transform.GetChild(0).DOLocalMoveX(-.3f, interval).SetDelay(interval * 4);
                        transform.GetChild(0).DOLocalMoveX(.2f, interval).SetDelay(interval * 5);
                        transform.GetChild(0).DOLocalMoveX(-.2f, interval).SetDelay(interval * 6);
                        transform.GetChild(0).DOLocalMoveX(pos, interval).SetDelay(interval * 7);
                    }
                });
            }
        }

        public void HidewithAnimation()
        {
            DOVirtual.DelayedCall(.1f, () => { transform.DOScale(Vector3.zero, duration); });
        }


        public void ChildEffectOne()
        {
            if (transform.childCount > 0)
            {
                var child = transform.GetChild(0);
                if (!tookChildScale)
                {
                    childScale = child.transform.localScale;
                    tookChildScale = true;
                }
                child.transform.DOScale(childScale * childScaleModifier, duration).OnComplete(() =>
                {
                    child.transform.DOScale(childScale, duration);
                });

            }

        }

    }
}