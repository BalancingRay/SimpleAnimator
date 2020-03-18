using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

namespace Tests
{
    public class AlphaAnimationTest : AnimationTestBase
    {
        #region Setting

        public AlphaAnimationTest()
        {
            _defaultAnimateTime = 0.5f;

            _dynamicAccuracy = 0.05f;
            _staticAccuracy = 0.001f;
        }

        #endregion Setting

        #region Tests

        [UnityTest]
        public IEnumerator Alpha_OnStart()
        {
            Component graphicalObject = _mainLine;
            float startValue = GetAlpha(graphicalObject);
            float finishValue = 0;

            _animations.AnimateAlphaFromCurrentState(graphicalObject.gameObject, finishValue, _defaultAnimateTime);

            AssertAlpha(graphicalObject, startValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_partTime()
        {
            Component graphicalObject = _secondaryMesh;
            float startValue = GetAlpha(graphicalObject);
            float finishValue = 0.85f;
            float part = 0.5f;

            _animations.AnimateAlphaFromCurrentState(graphicalObject.gameObject, finishValue, _defaultAnimateTime);

            yield return Sleep(_defaultAnimateTime, part);

            AssertAlpha(graphicalObject, startValue, finishValue, part);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_FromTo_partTime()
        {
            Component graphicalObject = _iconObject;
            float startValue = 0;
            float finishValue = 1;
            float part = 0.8f;

            _animations.AnimateAlpha(graphicalObject.gameObject, startValue, finishValue, _defaultAnimateTime);

            yield return Sleep(_defaultAnimateTime, part);

            AssertAlpha(graphicalObject, startValue, finishValue, part);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_FromTo_TheSameValue()
        {
            Component graphicalObject = _iconObject;
            float startValue = 0;
            float finishValue = 0;
            float part = 0.2f;

            _animations.AnimateAlpha(graphicalObject.gameObject, startValue, finishValue, _defaultAnimateTime);

            yield return Sleep(_defaultAnimateTime, part);

            AssertAlpha(graphicalObject, startValue, finishValue, part);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_FromTo_OnFinish()
        {
            Component graphicalObject = _iconObject;
            float startValue = 0;
            float finishValue = 0.6f;

            _animations.AnimateAlpha(graphicalObject.gameObject, startValue, finishValue, _defaultAnimateTime);

            yield return Sleep(_defaultAnimateTime);

            AssertAlpha(graphicalObject, finishValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_FromTo_OnStart()
        {
            Component graphicalObject = _iconObject;
            float startValue = 0;
            float finishValue = 1;

            _animations.AnimateAlpha(graphicalObject.gameObject, startValue, finishValue, _defaultAnimateTime);

            AssertAlpha(graphicalObject, startValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_OnFinish()
        {
            Component graphicalObject = _mainTextObject;
            float finishValue = 0.15f;

            _animations.AnimateAlphaFromCurrentState(graphicalObject.gameObject, finishValue, _defaultAnimateTime);

            yield return Sleep(_defaultAnimateTime);

            AssertAlpha(graphicalObject, finishValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_Delay_BeforeAnimate()
        {
            Component graphicalObject = _iconObject;
            float startValue = GetAlpha(graphicalObject);
            float finishValue = 0.61f;

            _animations.AnimateAlphaFromCurrentState(graphicalObject.gameObject, finishValue, _defaultAnimateTime, _defaultDelay);

            yield return Sleep(_defaultAnimateTime, 0, _defaultDelay);

            AssertAlpha(graphicalObject, startValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_Delay_OnFinsh()
        {
            Component graphicalObject = _mainLine;
            float finishValue = 0.02f;

            _animations.AnimateAlphaFromCurrentState(graphicalObject.gameObject, finishValue, _defaultAnimateTime, _defaultDelay);

            yield return Sleep(_defaultAnimateTime, 1, _defaultDelay);

            AssertAlpha(graphicalObject, finishValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_Delay_partTime()
        {
            Component graphicalObject = _mainTextObject;
            float startValue = GetAlpha(graphicalObject);
            float finishValue = 0.1f;
            float part = 0.45f;

            _animations.AnimateAlphaFromCurrentState(graphicalObject.gameObject, finishValue, _defaultAnimateTime, _defaultDelay);

            yield return Sleep(_defaultAnimateTime, part, _defaultDelay);

            AssertAlpha(graphicalObject, startValue, finishValue, part);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_Child()
        {
            float finishValue = 0.02f;

            _animations.AnimateAlphaFromCurrentState(_mainTextObject.gameObject, finishValue, _defaultAnimateTime, animateChildren: true);

            yield return Sleep(_defaultAnimateTime);

            AssertAlpha(_secondaryTextObject, finishValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_ChildWithDifferenceValue()
        {
            float startValue = 0.5f;
            float finishValue = 0.1f;

            SetAlpha(_secondaryTextObject, startValue);
            _animations.AnimateAlphaFromCurrentState(_mainTextObject.gameObject, finishValue, _defaultAnimateTime, animateChildren: true);

            yield return Sleep(_defaultAnimateTime);

            AssertAlpha(_secondaryTextObject, finishValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_ChildWithDifferenceValue_1()
        {
            float startValue = 0.3f;
            float finishValue = 0.7f;

            SetAlpha(_iconObject, startValue);
            _animations.AnimateAlphaFromCurrentState(_mainTextObject.gameObject, finishValue, _defaultAnimateTime, animateChildren: true);

            yield return Sleep(_defaultAnimateTime);

            AssertAlpha(_iconObject, finishValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_ChildWithDifferenceValue_2()
        {
            float startValue = 0.3f;
            float finishValue = 0.7f;

            SetAlpha(_secondaryMesh, startValue);
            _animations.AnimateAlphaFromCurrentState(_mainLine.gameObject, finishValue, _defaultAnimateTime, animateChildren: true);

            yield return Sleep(_defaultAnimateTime);

            AssertAlpha(_secondaryMesh, finishValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_Child_partTime()
        {
            float startValue = GetAlpha(_secondaryTextObject);
            float finishValue = 0.02f;
            float part = 0.45f;

            _animations.AnimateAlphaFromCurrentState(_mainTextObject.gameObject, finishValue, _defaultAnimateTime, animateChildren: true);

            yield return Sleep(_defaultAnimateTime, part);

            AssertAlpha(_secondaryTextObject, startValue, finishValue, part);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Alpha_ChildWithDifferenceValue_partTime()
        {
            float startValue = 0.5f;
            float finishValue = 0.3f;
            float part = 0.4f;

            SetAlpha(_iconObject, startValue);
            _animations.AnimateAlphaFromCurrentState(_mainTextObject.gameObject, finishValue, _defaultAnimateTime, animateChildren: true);

            yield return Sleep(_defaultAnimateTime, part);

            AssertAlpha(_iconObject, startValue, finishValue, part);

            yield return null;
        }

        #endregion Tests

        #region Private Methods

        private void AssertAlpha(Component graphicalComponent, float expected)
        {
            float validDelta = GetAccuracy(false);

            float actual = GetAlpha(graphicalComponent);

            Assert.AreEqual(expected, actual, validDelta);
        }

        private void AssertAlpha(Component graphicalComponent, float startValue, float finishValue, float stateOfCheck = 1)
        {
            float actual = GetAlpha(graphicalComponent);
            float animationDelta = finishValue - startValue;
            float expected = startValue + animationDelta * stateOfCheck;
            float accuracy = GetAccuracy(stateOfCheck);
            float validDelta = Math.Abs(animationDelta) * accuracy;

            Assert.AreEqual(expected, actual, validDelta);
        }

        private float GetAlpha(Component graphicalComponent)
        {
            return GetColor(graphicalComponent).a;
        }

        private void SetAlpha(Component component, float value)
        {
            Color color = GetColor(component);
            color.a = value;
            SetColor(component, color);
        }

        #endregion Private Methods
    }
}
