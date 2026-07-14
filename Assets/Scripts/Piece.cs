using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
namespace USP.MiniGame.recognitionPatterns
{

	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(SpriteRenderer), typeof(DraggableObject), typeof(Rigidbody2D))]
	public class Piece : MonoBehaviour
	{
		[Header("• R E F E R E N C E S")]
		[SerializeField] private new SpriteRenderer renderer;
		private SpriteRenderer childRenderer;
		[SerializeField] private new Collider2D collider;
		[SerializeField] private DraggableObject draggable;
		[SerializeField] private Sprite keySprite;
		private new Transform transform;

		private Slot matchingSlot;
		private string key;
		private int originalSortingOrder;

		[Header("• T W E E N   S E T T I N G S")]
		public float embiggenTarget = 1;
		[Min(0F)] public float embiggenTweenDuration = 0.3F;
		public Ease embiggenTweenEase = Ease.InOutBack;
		private Tween embiggenTween;
		[Space(2F)]
		public float attachTweenScaleTarget = 1F;
		[SerializeField] bool useOrignalScaleReference;
		[Min(0F)] public float attachTweenMoveDuration = 0.25F, attachTweenScaleDuration = 0.5F;
		public Ease attachTweenMoveEase = Ease.InOutQuint, attachTweenScaleEase = Ease.OutBounce;

		public bool IsDraggable { get => draggable.enabled; set => draggable.enabled = value; }
		public bool IsAttached { get; private set; }

		public event Action OnSelect = delegate { }, OnCancel = delegate { }, OnAttach = delegate { };
		public UnityEvent onAttach;
		public float onAttachDelay = .5f;
		Vector3 ogScale;
		Vector3 afterCompleteScale;
		public AudioClip SuccessVO, WrongVo;
		private Slot incorrectSlot;
		private StackItemDrop stackDrop;
		int _stackIndex = -1;
		int _stackID = -1;

		public int StackIndex
		{
			get { return _stackIndex; }
			set { _stackIndex = value; }
		}

		public int StackID
		{
			get { return _stackID; }
			set { _stackID = value; }
		}

		public GameObject myTarget { get; private set; }
		public bool isComplete;

		private void Reset()
		{
			renderer = GetComponent<SpriteRenderer>();
			childRenderer = GetComponent<SpriteRenderer>();
			collider = GetComponent<Collider2D>();
			draggable = GetComponent<DraggableObject>();
		}

		public void ScaleHighLight()
		{
			transform.DOScale(attachTweenScaleTarget * 1.2f, .1f).OnComplete(() => { transform.DOScale(attachTweenScaleTarget, .1f); });
		}


		private void Awake()
		{

		}

		void Start()
		{
			renderer.color = new Color(1, 1, 1, 1);
			transform = base.transform;
			if (transform.childCount > 0)
			{
				childRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
			}
			originalSortingOrder = renderer.sortingOrder;
			key = keySprite != null ? keySprite.name : renderer.sprite.name;
			GetMyAttachingObject();
		}
		private void OnEnable()
		{
			draggable.enabled = collider.enabled = true;
			draggable.OnPick.AddListener(HandleSelected);
			draggable.OnRelease.AddListener(HandleReleased);
			draggable.OnReturn.AddListener(HandleReturn);


			IsAttached = false;
			Invoke(nameof(applyBaseScale), 2);
		}
		private void OnDisable()
		{
			draggable.OnPick.RemoveListener(HandleSelected);
			draggable.OnRelease.RemoveListener(HandleReleased);
			draggable.OnReturn.RemoveListener(HandleReturn);

			draggable.enabled = collider.enabled = false;

			embiggenTween?.Kill();
		}

		void applyBaseScale() // making sure this is done after tween scale in animations handled from other script
		{
			ogScale = this.transform.localScale;
			embiggenTween = transform.DOScale(useOrignalScaleReference ? ogScale * embiggenTarget : Vector3.one * embiggenTarget, embiggenTweenDuration).SetEase(embiggenTweenEase).SetAutoKill(false).OnKill(() => embiggenTween = null).Pause();
		}
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent(out StackItemDrop stack))
			{
				stackDrop = stack;
				return;
			}
			if (!other.TryGetComponent(out Slot slot) || key != slot.Key)
			{
				incorrectSlot = slot; return;
			}

			slot.Fade(0.5F);
			matchingSlot = slot;
			incorrectSlot = null;
		}
		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.TryGetComponent(out StackItemDrop stack))
			{
				stackDrop = null;
				return;
			}
			if (!other.TryGetComponent(out Slot slot) || key != slot.Key)
			{
				incorrectSlot = null; return;
			}

			if (!IsAttached) slot.Fade(1F);
			matchingSlot = null;
		}




		private void HandleSelected()
		{
			Debug.Log("Handle Selected");
			renderer.sortingOrder = 200;
			if (childRenderer)
				childRenderer.sortingOrder = 201;
			embiggenTween.Restart();
			Debug.Log("Color change here");
			renderer.DOColor(new Color(1, 1, 1, 1), .2f);
			OnSelect();

		}
		private void HandleReleased()
		{
			//renderer.DOColor(new Color(0, 0, 0, 0), .2f);
			if (stackDrop != null)
			{
				stackDrop.Take(this.gameObject, ogScale);
				this.enabled = false;
				onAttach?.Invoke();
				UtilityEventsManager.OnItemRemoved?.Invoke(this);
				UtilityEventsManager.onDraggedObjectAttached?.Invoke(this, new UtilityEventsManager.DraggedObjectAttached(this.gameObject, null));

				return;
			}
			if (matchingSlot != null)
			{
				AttachToSlot(matchingSlot);

				return;
			}
			if (incorrectSlot)
			{
				GetComponentInParent<Level>().PlayRandomVOFromWrontPlacement();
				blockControls(1f);
			}
			else
			{
				blockControls(.3f);
			}

			UtilityEventsManager.onDraggedObjectCancelled?.Invoke(this, new UtilityEventsManager.DraggedObjectCancelled(this.gameObject));

			embiggenTween.PlayBackwards();
			renderer.DOColor(Color.white, .2f);
			if (StackIndex > -1)
			{
				UtilityEventsManager.OnGoBack?.Invoke(this);
			}
			OnCancel();
		}

		void blockControls(float sec = 2)
		{
			UtilityEventsManager.isControlEnabled = false;
			DOVirtual.DelayedCall(sec, () => { UtilityEventsManager.isControlEnabled = true; });
		}
		private void HandleReturn()
		{

			renderer.sortingOrder = originalSortingOrder;
			childRenderer.sortingOrder = originalSortingOrder + 1;
			//renderer.DOColor(Color.white, .2f);
			renderer.DOColor(new Color(1, 1, 1, 1), .2f);
			if (embiggenTween != null && !embiggenTween.IsPlaying())
			{
				embiggenTween.PlayBackwards();
			}
		}
		private void AttachToSlot(Slot slot)
		{
			IsAttached = true;
			transform.SetParent(slot.transform, true);
			OnAttach();

			UtilityEventsManager.onDraggedObjectAttached?.Invoke(this, new UtilityEventsManager.DraggedObjectAttached(this.gameObject, slot.gameObject));
			DOVirtual.DelayedCall(onAttachDelay, () => { onAttach.Invoke(); isComplete = true; /*renderer.DOColor(new Color(0, 0, 0, 0), .3f);*/ });
			DOTween.Sequence()
				  .Append(transform.DOLocalMove(Vector2.zero, attachTweenMoveDuration).SetEase(attachTweenMoveEase).OnComplete(() => { slot.Fade(0F); slot.Complete(); }))
				  .AppendCallback(() =>
				  {
					  MiscSpawnables.GetParticleSpawnable(this.transform.position);
					  UtilityEventsManager.isControlEnabled = false;
					  DOVirtual.DelayedCall(.25f, () => { UtilityEventsManager.isControlEnabled = true; });
					  if (SuccessVO != null)
					  {
						  GetComponentInParent<Level>().PlayVOFromPlacement(SuccessVO);
					  }
					  if (keySprite != null) renderer.sprite = keySprite;
					  renderer.sortingOrder = slot.Order + 1;
					  if (childRenderer)
						  childRenderer.sortingOrder = renderer.sortingOrder + 1;
				  })
				   .Append(transform.DOScale(attachTweenScaleTarget * .7f, .27f))
				  .Append(transform.DOScale(Vector3.one * attachTweenScaleTarget * 1.3f, .22f))
				  .Append(transform.DOScale(Vector3.one * attachTweenScaleTarget, .17f))
				  .Append(transform.DOScale(Vector3.one * attachTweenScaleTarget * 1.1f, .15f))
				  .Append(transform.DOScale(Vector3.one * attachTweenScaleTarget, .12f))
				  //.Append(transform.DOScale(  Vector2.one * attachTweenScaleTarget*1.2f , attachTweenScaleDuration).SetEase(attachTweenScaleEase))
				  .SetLink(gameObject)
				  .Play();

			enabled = false;
		}

		void GetMyAttachingObject()
		{
			var slots = FindObjectsByType<Slot>(sortMode: FindObjectsSortMode.None);
			foreach (var x in slots)
			{
				if (key == x.Key && !x.isComplete)
				{
					myTarget = x.gameObject;
					return;
				}
			}
		}

		public void RecalculateAttachingObject()
		{
			var slots = FindObjectsByType<Slot>(sortMode: FindObjectsSortMode.None);
			foreach (var x in slots)
			{
				if (key == x.Key && !x.isComplete)
				{
					Debug.Log("My TARGET ATTACHED==============", this);
					myTarget = x.gameObject;
					return;
				}
			}
		}
	}

}