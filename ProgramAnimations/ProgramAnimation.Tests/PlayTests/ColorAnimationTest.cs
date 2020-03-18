using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

namespace Tests
{
    public class ColorAnimationTest : AnimationTestBase
    {
        #region Settings

        public ColorAnimationTest()
        {
            _defaultAnimateTime = 0.4f;

            _dynamicAccuracy = 0.05f;
            _staticAccuracy = 0.001f;
        }

        #endregion Settings

        #region Tests

        [UnityTest]
        public IEnumerator Color_OnStart()
        {
            Component graphicalObject = _mainLine;
            Color startValue = GetColor(graphicalObject);
            Color finishValue = Color.green;

            _animations.AnimateColorFromCurrentState(graphicalObject.gameObject, finishValue, _defaultAnimateTime);

            AssertColor(graphicalObject, startValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Color_partTime()
        {
            Component graphicalObject = _secondaryTextObject;
            Color startValue = GetColor(graphicalObject);
            Color finishValue = Color.blue;
            float part = 0.6f;

            _animations.AnimateColorFromCurrentState(graphicalObject.gameObject, finishValue, _defaultAnimateTime);

            yield return Sleep(_defaultAnimateTime, part);

            AssertColor(graphicalObject, startValue, finishValue, part);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Color_OnFinish()
        {
            Component graphicalObject = _secondaryTextObject;
            Color finishValue = Color.magenta;

            _animations.AnimateColorFromCurrentState(graphicalObject.gameObject, finishValue, _defaultAnimateTime);

            yield return Sleep(_defaultAnimateTime);

            AssertColor(graphicalObject, finishValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Color_AfterDelay_BeforeAnimate()
        {
            Component graphicalObject = _iconObject;
            Color startValue = GetColor(graphicalObject);
            Color finishValue = Color.yellow;

            _animations.AnimateColorFromCurrentState(graphicalObject.gameObject, finishValue, _defaultAnimateTime, _defaultDelay);

            yield return Sleep(_defaultAnimateTime, 0, _defaultDelay);

            AssertColor(graphicalObject, startValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Color_AfterDelay_AfterAnimate()
        {
            Component graphicalObject = _secondaryMesh;
            Color finishValue = Color.blue;

            _animations.AnimateColorFromCurrentState(graphicalObject.gameObject, finishValue, _defaultAnimateTime, _defaultDelay);

            yield return Sleep(_defaultAnimateTime, 1, _defaultDelay);

            AssertColor(graphicalObject, finishValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Color_AfterDelay_partTime()
        {
            Component graphicalObject = _mainLine;
            Color startValue = GetColor(graphicalObject);
            Color finishValue = Color.magenta;
            float part = 0.5f;

            _animations.AnimateColorFromCurrentState(graphicalObject.gameObject, finishValue, _defaultAnimateTime, _defaultDelay);

            yield return Sleep(_defaultAnimateTime, part, _defaultDelay);

            AssertColor(graphicalObject, startValue, finishValue, part);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Color_Child()
        {
            Color startValue = GetColor(_secondaryTextObject);
            Color finishValue = Color.red;

            _animations.AnimateColorFromCurrentState(_mainTextObject.gameObject, finishValue, _defaultAnimateTime, 0, true);

            yield return Sleep(_defaultAnimateTime);

            AssertColor(_secondaryTextObject, finishValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Color_ChildWithDifferenceValue_1()
        {
            Color startValue = Color.red;
            Color finishValue = Color.blue;

            SetColor(_secondaryMesh, startValue);
            _animations.AnimateColorFromCurrentState(_mainLine.gameObject, finishValue, _defaultAnimateTime, 0, true);

            yield return Sleep(_defaultAnimateTime);

            AssertColor(_secondaryMesh, finishValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Color_ChildWithDifferenceValue_2()
        {
            Color startValue = Color.yellow;
            Color finishValue = Color.black;

            SetColor(_iconObject, startValue);
            _animations.AnimateColorFromCurrentState(_mainTextObject.gameObject, finishValue, _defaultAnimateTime, 0, true);

            yield return Sleep(_defaultAnimateTime);

            AssertColor(_iconObject, finishValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Color_Child_partTime()
        {
            Color startValue = GetColor(_secondaryTextObject);
            Color finishValue = Color.red;
            float part = 0.7f;

            _animations.AnimateColorFromCurrentState(_mainTextObject.gameObject, finishValue, _defaultAnimateTime, 0, true);

            yield return Sleep(_defaultAnimateTime, part);

            AssertColor(_secondaryTextObject, startValue, finishValue, part);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Color_ChildWithDifferenceValue_partTime()
        {
            Color startValue = Color.blue;
            Color finishValue = Color.red;
            float part = 0.3f;

            SetColor(_secondaryMesh, startValue);
            _animations.AnimateColorFromCurrentState(_mainLine.gameObject, finishValue, _defaultAnimateTime, 0, true);

            yield return Sleep(_defaultAnimateTime, part);

            AssertColor(_secondaryMesh, startValue, finishValue, part);

            yield return null;
        }

        #endregion Tests

        #region Private Methods

        private void AssertColor(Component graphicalComponent, Color expected, bool higthAccuracy = true)
        {
            float accuracy = GetAccuracy(higthAccuracy);
            Color validDelta = new Color(accuracy, accuracy, accuracy, accuracy);

            Color actual = GetColor(graphicalComponent);

            AssertColorRGBA(expected, actual, validDelta);
        }

        private void AssertColor(Component graphicalComponent, Color startValue, Color finishValue, float stateOfCheck = 1)
        {
            float accuracy = GetAccuracy(stateOfCheck);

            Color animationDelta = finishValue - startValue;
            Color expected = startValue + animationDelta * stateOfCheck;
            Color actual = GetColor(graphicalComponent);
            Color validDelta = animationDelta * accuracy;

            AssertColorRGBA(expected, actual, validDelta);
        }

        private void AssertColorRGBA(Color expected, Color actual, Color validDelta)
        {
            for (int channelIndex = 0; channelIndex < 4; channelIndex++)
            {
                float delta = Math.Abs(validDelta[channelIndex]);

                Assert.AreEqual(expected[channelIndex], actual[channelIndex], delta, message: $"ChannelIndex: {channelIndex}");
            }
        }

        #endregion Private Methods
    }
}
