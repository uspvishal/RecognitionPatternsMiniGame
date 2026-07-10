using UnityEngine;

namespace USP.MiniGame.recognitionPatterns
{
    /// <summary>
    /// Script by Vishal Lakhani
    /// Email: usp.vishal@gmail.com
    /// Description:
    /// </summary>
    public class MoveWithScreenPoint : MonoBehaviour
    {
        #region Variables
        public enum ScreenPoints
        {
            LeftTop, LeftMiddle, LeftBottom, MiddleTop, MiddleCenter, MiddleBottom, RightTop, RightMiddle, RightBottom
        }

        public Camera targetCamera;

        public ScreenPoints AnchorPoints;

        public Vector2 Offset, OffsetIpad;
        Vector2 offset_f;
        private float worldDistanceFromCamera;



        #endregion

        #region Unity Methods


        private void Start()
        {
            if (targetCamera == null)
            {
                targetCamera = CameraAutoFit.instance.Camera;
            }
            offset_f = CameraAutoFit.IsWideAspect ? Offset : OffsetIpad;
            worldDistanceFromCamera = Vector3.Dot(
                transform.position - targetCamera.transform.position,
                targetCamera.transform.forward
            );
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (targetCamera == null) return;

            Vector2 viewportPoint = GetViewportPoint(AnchorPoints);

            Vector3 worldPoint = targetCamera.ViewportToWorldPoint(
                new Vector3(viewportPoint.x, viewportPoint.y, worldDistanceFromCamera)
            );

            transform.position = new Vector3(
                worldPoint.x + offset_f.x,
                worldPoint.y + offset_f.y,
                transform.position.z
            );
        }

        private Vector2 GetViewportPoint(ScreenPoints anchorPoint)
        {
            switch (anchorPoint)
            {
                case ScreenPoints.LeftTop:
                    return new Vector2(0f, 1f);

                case ScreenPoints.LeftMiddle:
                    return new Vector2(0f, 0.5f);

                case ScreenPoints.LeftBottom:
                    return new Vector2(0f, 0f);

                case ScreenPoints.MiddleTop:
                    return new Vector2(0.5f, 1f);

                case ScreenPoints.MiddleCenter:
                    return new Vector2(0.5f, 0.5f);

                case ScreenPoints.MiddleBottom:
                    return new Vector2(0.5f, 0f);

                case ScreenPoints.RightTop:
                    return new Vector2(1f, 1f);

                case ScreenPoints.RightMiddle:
                    return new Vector2(1f, 0.5f);

                case ScreenPoints.RightBottom:
                    return new Vector2(1f, 0f);

                default:
                    return new Vector2(0.5f, 0.5f);
            }
        }



    }



        #endregion



}