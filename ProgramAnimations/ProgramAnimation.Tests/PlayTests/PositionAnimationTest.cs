using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

namespace Tests
{
    public class PositionAnimationTest : AnimationTestBase
    {
        #region Setting  

        private Vector2 _defaultShift = new Vector2(-5, -10);

        public PositionAnimationTest()
        {
            _defaultAnimateTime = 0.5f;

            _dynamicAccuracy = 0.05f;
            _staticAccuracy = 0.001f;
        }

        #endregion Setting

        #region Tests

        [UnityTest]
        public IEnumerator Position_OnStart()
        {
            Component component = _mainLine;
            Vector2 startValue = GetLocalPosition(component);
            Vector2 finishValue = startValue + _defaultShift;

            _animations.AnimatePositionFromCurrentState(component.transform, finishValue, _defaultAnimateTime);

            AssertPosition(component, startValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Position_FromTo_OnStart()
        {
            Component component = _mainLine;
            Vector2 startValue = GetLocalPosition(component) - _defaultShift;
            Vector2 finishValue = startValue + 2 * _defaultShift;

            _animations.AnimatePosition(component.transform, startValue, finishValue, _defaultAnimateTime);

            AssertPosition(component, startValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Position_partTime_1()
        {
            Component component = _mainTextObject;
            Vector2 startValue = GetLocalPosition(component);
            Vector2 finishValue = startValue + _defaultShift;
            float part = 0.4f;

            _animations.AnimatePositionFromCurrentState(component.transform, finishValue, _defaultAnimateTime);

            yield return Sleep(_defaultAnimateTime, part);

            AssertPosition(component, startValue, finishValue, part);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Position_partTime_2()
        {
            Component component = _mainLine;
            Vector2 startValue = GetLocalPosition(component) - _defaultShift;
            Vector2 finishValue = startValue + 2 * _defaultShift;
            float part = 0.5f;

            _animations.AnimatePosition(component.transform, startValue, finishValue, _defaultAnimateTime);

            yield return Sleep(_defaultAnimateTime, part);

            AssertPosition(component, startValue, finishValue, part);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Position_Shift_partTime()
        {
            Component component = _mainTextObject;
            Vector2 startValue = GetLocalPosition(component);
            Vector2 finishValue = startValue + _defaultShift;
            float part = 0.8f;

            _animations.AnimatePositionShift(component.transform, _defaultShift, _defaultAnimateTime);

            yield return Sleep(_defaultAnimateTime, part);

            AssertPosition(component, startValue, finishValue, part);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Position_OnFinish()
        {
            Component component = _iconObject;
            Vector2 startValue = GetLocalPosition(component);
            Vector2 finishValue = startValue + _defaultShift;

            _animations.AnimatePositionFromCurrentState(component.transform, finishValue, _defaultAnimateTime);

            yield return Sleep(_defaultAnimateTime);

            AssertPosition(component, startValue, finishValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Position_Sequention_Rewers()
        {
            Component component = _iconObject;
            Vector2 startValue = GetLocalPosition(component);

            Vector2 step1 = _defaultShift;
            Vector2 step2 = -_defaultShift;

            yield return _animations.GetShiftPositionAnimation(component.transform, step1, _defaultAnimateTime);
            yield return _animations.GetShiftPositionAnimation(component.transform, step2, _defaultAnimateTime);

            AssertPosition(component, startValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Position_Sequention_twoStep()
        {
            Component component = _iconObject;
            Vector2 startValue = GetLocalPosition(component);

            Vector2 step1 = _defaultShift;
            Vector2 step2 = _defaultShift * 2;

            yield return _animations.GetShiftPositionAnimation(component.transform, step1, _defaultAnimateTime);
            yield return _animations.GetShiftPositionAnimation(component.transform, step2, _defaultAnimateTime);

            AssertPosition(component, startValue + step1 + step2);

            yield return null;
        }

        #endregion Tests

        #region Private Methods

        private void AssertPosition(Component component, Vector2 expected, bool higthAccuracy = true)
        {
            Vector2 actual = GetLocalPosition(component);
            float delta = GetAccuracy(higthAccuracy);
            Vector2 valid = new Vector2(delta, delta);

            AssertVector2(expected, actual, valid);
        }

        private void AssertPosition(Component component, Vector2 startValue, Vector2 finishValue, float stateOfCheck = 1)
        {
            float accuracy = GetAccuracy(stateOfCheck);

            Vector2 animationDelta = finishValue - startValue;
            Vector2 validDelta = (finishValue - startValue) * accuracy;
            Vector2 expected = startValue + animationDelta * stateOfCheck;
            Vector2 actual = GetLocalPosition(component);

            AssertVector2(expected, actual, validDelta);
        }

        private void AssertVector2(Vector2 expected, Vector2 actual, Vector2 validDelta)
        {
            for (int dim = 0; dim < 2; dim++)
            {
                float delta = Math.Abs(validDelta[dim]);

                Assert.AreEqual(expected[dim], actual[dim], delta, message: $"channelIndex {dim}");
            }
        }

        private Vector2 GetLocalPosition(Component component)
        {
            return component.transform.localPosition;
        }

        private void SetLocalPosition(Component component, Vector2 value)
        {
            component.transform.localPosition = value;
        }

        #endregion Private Methods
    }
}
