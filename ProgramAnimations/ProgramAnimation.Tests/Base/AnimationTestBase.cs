using Assets.ProgramAnimations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Tests
{
    abstract public class AnimationTestBase
    {
        #region Fields

        private GameObject _mainObject;

        protected ElementsAnimator _animations;
        protected LineRenderer _mainLine;
        protected MeshRenderer _secondaryMesh;
        protected Text _mainTextObject;
        protected Text _secondaryTextObject;
        protected RawImage _iconObject;

        private int _framerate = 60;
        protected float _defaultAnimateTime = 0.1f;
        protected float _defaultDelay = 0.15f;
        protected float _dynamicAccuracy = 0.05f;
        protected float _staticAccuracy = 0.01f;

        #endregion Fields

        #region Support

        [SetUp]
        public void Setup()
        {
            _mainObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>($"Prefabs/TestObject"));

            _animations = _mainObject.AddComponent<ElementsAnimator>();
            Application.targetFrameRate = _framerate;
            _animations.FrameRate = _framerate;

            _secondaryMesh = _mainObject.GetComponentInChildren<MeshRenderer>();
            _mainLine = _mainObject.GetComponentInChildren<LineRenderer>();

            var canvas = _mainObject.GetComponentInChildren<Canvas>().gameObject;

            _mainTextObject = canvas.GetComponentInChildren<Text>();
            _secondaryTextObject = _mainTextObject.transform.Find("Text_1").GetComponent<Text>();
            _iconObject = _secondaryTextObject.GetComponentInChildren<RawImage>();
        }

        [TearDown]
        public void Teardown()
        {
            GameObject.Destroy(_mainObject);
        }

        #endregion Support

        #region PrivateMethods Tests

        [Test]
        public void CheckEnable()
        {
            Assert.IsTrue(_mainLine.enabled && _mainLine.gameObject.activeInHierarchy);
            Assert.IsTrue(_secondaryMesh.enabled && _secondaryMesh.gameObject.activeInHierarchy);

            Assert.IsTrue(_mainTextObject.isActiveAndEnabled);
            Assert.IsTrue(_secondaryTextObject.isActiveAndEnabled);
            Assert.IsTrue(_iconObject.isActiveAndEnabled);
        }

        [Test]
        public void ChechTestingColorGetter()
        {
            var components = new List<Component>()
            {
            _mainLine,
            _mainTextObject,
            _secondaryTextObject,
            _iconObject,
            _secondaryMesh
            };

            try
            {
                foreach (var item in components)
                {
                    GetColor(item);
                }
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
                return;
            }

            Assert.IsTrue(true);
        }

        [Test]
        public void ChechTestingColorSetter()
        {
            Color newColor = Color.green;

            var components = new List<Component>()
            {
            _mainLine,
            _mainTextObject,
            _secondaryTextObject,
            _iconObject,
            _secondaryMesh
            };

            foreach (var item in components)
            {
                SetColor(item, newColor);
            }

            foreach (var item in components)
            {
                Assert.IsTrue(GetColor(item) == newColor);
            }
        }

        #endregion PrivateMethods Tests

        #region protected Methods

        protected Color GetColor(Component component)
        {
            if (component is LineRenderer lineRenderer)
            {
                return lineRenderer.startColor;
            }
            else if (component is MaskableGraphic ui)
            {
                return ui.color;
            }
            else if (component is MeshRenderer meshRenderer)
            {
                return meshRenderer.material.color;
            }
            else
            {
                throw new ArgumentException($"Component type {component?.GetType()} not support");
            }
        }

        protected void SetColor(Component component, Color color)
        {
            if (component is LineRenderer lineRender)
            {
                lineRender.startColor = lineRender.endColor = color;
            }
            else if (component is MaskableGraphic ui)
            {
                ui.color = color;
            }
            else if (component is MeshRenderer meshRenderer)
            {
                meshRenderer.material.color = color;
            }
            else
            {
                throw new ArgumentException($"Component type {component?.GetType()} not support");
            }
        }

        protected bool AreEqual(float expected, float actual, float animationDelta, float accuracy)
        {
            float validDelta = Math.Abs(animationDelta) * accuracy;

            return Math.Abs(expected - actual) < validDelta;
        }

        protected bool AreEqual(Color expected, Color actual, Color animationDelta, float accuracy)
        {
            for (int channelIndex = 0; channelIndex < 4; channelIndex++)
            {
                float validDelta = Math.Abs(animationDelta[channelIndex] * accuracy);
                float actualChannelDelta = Math.Abs(expected[channelIndex] - actual[channelIndex]);

                if (actualChannelDelta > validDelta)
                {
                    return false;
                }
            }
            return true;
        }

        protected float GetAccuracy(float stateOfCheck)
        {
            if (stateOfCheck == 0 || stateOfCheck >= 1)
            {
                return _staticAccuracy;
            }
            else
            {
                return _dynamicAccuracy;
            }
        }

        protected float GetAccuracy(bool higthAccuracy)
        {
            return higthAccuracy ? 0.001f : 0.1f;
        }

        protected IEnumerator Sleep(float animateTime, float stateOfCheck = 1, float delayTime = 0)
        {
            if (delayTime > 0)
            {
                yield return new WaitForSeconds(delayTime);
            }

            if (stateOfCheck > 0)
            {
                yield return new WaitForSeconds(animateTime * stateOfCheck);
            }
        }

        protected float GetSleepTime(float animateTime, float stateOfCheck = 1, float delayTime = 0)
        {
            float sleepTime = 0;

            if (delayTime > 0)
            {
                sleepTime += delayTime;
            }

            if (stateOfCheck > 0)
            {
                sleepTime += animateTime * stateOfCheck;
            }

            return sleepTime;
        }

        #endregion Private Methods
    }
}
