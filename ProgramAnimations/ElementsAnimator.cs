using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ProgramAnimations
{
    public class ElementsAnimator : MonoBehaviour
    {
        #region Fields

        private int _framerate = 60;

        private ColorItemBuilder _colorItemBuilder;

        #endregion Fields

        #region Properties

        public int FrameRate
        {
            get => _framerate;
            set => _framerate = value;
        }

        #endregion Properties

        #region Start

        public void Start()
        {
            _colorItemBuilder = new ColorItemBuilder();
        }

        #endregion Start

        #region Public Methods

        public void AnimateAlpha(GameObject gameObject, float startAlpha, float finishAlpha, float time, float delay = 0, bool animateChildren = false)
        {
            var gameObjects = GetObjects(gameObject, animateChildren);

            foreach (GameObject item in gameObjects)
            {
                if (TryBuildColorItem(item, out var colorItem))
                {
                    StartCoroutine(TransformAlpha(colorItem, startAlpha, finishAlpha, time, delay));
                }
            }
        }

        public void AnimateAlphaFromCurrentState(GameObject gameObject, float finishAlpha, float time, float delay = 0, bool animateChildren = false)
        {
            var gameObjects = GetObjects(gameObject, animateChildren);

            foreach (GameObject item in gameObjects)
            {
                if (TryBuildColorItem(item, out var colorItem))
                {
                    float startAlpha = colorItem.GetColor().a;

                    StartCoroutine(TransformAlpha(colorItem, startAlpha, finishAlpha, time, delay));
                }
            }
        }

        public void AnimateColor(GameObject gameObject, Color startColor, Color finishColor, float time, float delay = 0)
        {
            if (TryBuildColorItem(gameObject, out var colorItem))
            {
                StartCoroutine(TransformColor(colorItem, startColor, finishColor, time, delay));
            }
        }

        public void AnimateColorFromCurrentState(GameObject gameObject, Color finishColor, float time, float delay = 0, bool animateChildren = false)
        {
            var gameObjects = GetObjects(gameObject, animateChildren);

            foreach (GameObject item in gameObjects)
            {
                if (TryBuildColorItem(item, out var colorItem))
                {
                    var startColor = colorItem.GetColor();

                    StartCoroutine(TransformColor(colorItem, startColor, finishColor, time, delay));
                }
            }
        }

        public void AnimatePosition(Transform transformComponent, Vector2 startPosition, Vector2 finishPosition, float time, float delay = 0)
        {
            var animation = GetAnimatePosition(transformComponent, startPosition, finishPosition, time, delay);

            if (animation != null)
            {
                StartCoroutine(animation);
            }
        }

        public IEnumerator GetAnimatePosition(Transform transformComponent, Vector2 startPosition, Vector2 finishPosition, float time, float delay = 0)
        {
            if (transformComponent != null)
            {
                return TransformPositions(transformComponent, startPosition, finishPosition, time, delay);
            }
            else
            {
                return null;
            }
        }

        public void AnimatePositionFromCurrentState(Transform transformComponent, Vector2 finishPosition, float time, float delay = 0)
        {
            if (transformComponent != null)
            {
                Vector2 startPosition = transformComponent.localPosition;

                StartCoroutine(TransformPositions(transformComponent, startPosition, finishPosition, time, delay));
            }
        }

        public void AnimatePositionShift(Transform transformComponent, Vector2 shift, float time, float delay = 0)
        {
            var animation = GetShiftPositionAnimation(transformComponent, shift, time, delay);

            if (animation != null)
            {
                StartCoroutine(animation);
            }
        }

        public IEnumerator GetShiftPositionAnimation(Transform transformComponent, Vector2 shift, float time, float delay = 0)
        {
            if (transformComponent != null)
            {
                Vector2 startPosition = transformComponent.localPosition;
                Vector2 finishPosition = startPosition + shift;

                return TransformPositions(transformComponent, startPosition, finishPosition, time, delay);
            }
            else
            {
                return null;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private bool TryBuildColorItem(GameObject gameObject, out IColorItem colorItem)
        {
            colorItem = _colorItemBuilder.Build(gameObject);

            return colorItem != null;
        }

        private List<GameObject> GetObjects(GameObject gameObject, bool animateChildren)
        {
            var resultObjects = new List<GameObject>() { gameObject };

            if (animateChildren)
            {
                var transformObjects = gameObject.GetComponentsInChildren(typeof(Transform), true);

                foreach (Transform item in transformObjects)
                {
                    resultObjects.Add(item.gameObject);
                }
            }

            return resultObjects;
        }

        #endregion Private Methods

        #region IEnumerators

        private IEnumerator TransformPositions(Transform transform, Vector2 startPos, Vector2 finishPos, float time, float delay)
        {
            transform.localPosition = startPos;

            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            Vector2 position = startPos;
            Vector2 step = (finishPos - startPos) / time / _framerate;

            int dim = 0;
            if (startPos[dim] == finishPos[dim]) dim = 1;

            int direction = 1;
            if (finishPos[dim] < startPos[dim]) direction = -1;

            while (direction * position[dim] < direction * finishPos[dim])
            {
                transform.localPosition = position;
                position += step;
                yield return null;
            }

            transform.localPosition = finishPos;
        }

        private IEnumerator TransformColor(IColorItem colorItem, Color startColor, Color finishColor, float time, float delay)
        {
            colorItem.SetColor(startColor);

            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            int channel = 0;
            while (startColor[channel] == finishColor[channel] && channel < 3)
            {
                channel++;
            }

            int direction = 1;
            if (finishColor[channel] < startColor[channel])
            {
                direction = -1;
            }

            Color step = (finishColor - startColor) / time / _framerate;
            Color color = startColor;

            while (direction * color[channel] < direction * finishColor[channel])
            {
                colorItem.SetColor(color);
                color += step;
                yield return null;
            }

            colorItem.SetColor(finishColor);
        }

        private IEnumerator TransformAlpha(IColorItem colorItem, float startAlpha, float finishAlpha, float time, float delay)
        {
            Color color = colorItem.GetColor();
            color.a = startAlpha;
            colorItem.SetColor(color);

            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            float step = (finishAlpha - startAlpha) / time / _framerate;

            float alpha = startAlpha;

            int direction = 1;
            if (finishAlpha < startAlpha)
            {
                direction = -1;
            }

            while (direction * alpha < direction * finishAlpha)
            {
                color.a = alpha;
                colorItem.SetColor(color);
                alpha += step;
                yield return null;
            }

            color.a = finishAlpha;
            colorItem.SetColor(color);
        }

        #endregion IEnumerators
    }
}