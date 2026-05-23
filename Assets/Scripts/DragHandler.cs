using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
namespace USP.MiniGame.Addition
{

      public sealed class DragHandler : MonoBehaviour
      {
            [Header("• R E F E R E N C E S")]
            [SerializeField] private new Camera camera;

            [Header("• I N P U T ")]
            [SerializeField] private InputAction position = new("Pointer Position", InputActionType.Value, "<Pointer>/position");
            [SerializeField] private InputAction press = new("Pointer Press", InputActionType.Button, "<Pointer>/press");

            [Header("• C O N F I G U R A T I O N")]
            public ContactFilter2D ContactFilter;
            public int MaxResults = 3;

            private Collider2D[] hitResults;
            private DraggableObject current;

            private Vector2 WorldPosition => camera.ScreenToWorldPoint(position.ReadValue<Vector2>());


            private void Reset()
            {
                  camera = FindAnyObjectByType<Camera>();
            }
            private void Awake()
            {
                  hitResults = new Collider2D[MaxResults];
            }
            private void OnEnable()
            {
                  position.Enable();
                  press.Enable();

                  press.started += HandleAction;
                  press.canceled += HandleAction;
            }
            private void LateUpdate()
            {
                  if (current == null) return;

                  if (!current.enabled)
                  {
                        current = null;
                        return;
                  }

                  current.DragTo(WorldPosition);
            }

            void Update()
            {
                  if (Input.touchCount > 1)
                  {
                        current.ReleaseBackNoCallback();
                        current = null;
                  }
            }
            private void OnDisable()
            {
                  press.started -= HandleAction;
                  press.canceled -= HandleAction;
                  current = null;
                  position.Disable();
                  press.Disable();
            }

            private void HandleAction(InputAction.CallbackContext context)
            {
                  if (!UtilityEventsManager.isControlEnabled)
                  {
                        if (current)
                        {
                              current.ReleaseBackNoCallback();
                              current = null;
                        }
                        return;
                  }
                  switch (context.phase)
                  {
                        case InputActionPhase.Started:
                              {
                                    if (UtilityEventsManager.isControlEnabled)
                                    {
                                          {
                                                int count = Physics2D.OverlapPoint(WorldPosition, ContactFilter, hitResults);
                                                Debug.Log("Hit Counts " + count);
                                                for (int i = 0; i < count; i++)
                                                {
                                                      var result = hitResults[i];
                                                      if (result.TryGetComponent(out DraggableObject obj) && obj.enabled)
                                                      {
                                                            (current = obj).Pick();
                                                            break;
                                                      }
                                                }
                                          }

                                    }
                                    break;
                              }

                        case InputActionPhase.Canceled when current != null:
                              {
                                    UtilityEventsManager.isControlEnabled = false;
                                    DOVirtual.DelayedCall(.1f, () => { UtilityEventsManager.isControlEnabled = true; });
                                    current.Release();
                                    current = null;
                                    break;
                              }
                        default: return;
                  }
            }
      }

}