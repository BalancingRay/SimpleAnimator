# SimpleAnimator
Simple solution of program animation for Unity

You can animating Color property and Alpha channel for elements of UI or Renderer components. Also you can animate gameObject and its children gameObjects with supported component.

Supported graphical components:
  - Text
  - Image
  - RawImage
  - LineRenderer
  - MeshRenderer

You can animate localPosition for Transform component. Also you can generate multi-step animation.

1. How to use SimpleAnimator for Unity
- Add **ElementsAnimator** to Scene.
- Find reference on ElementsAnimator component. (You can use Dependency Injection, Singleton or direct finding on scene)
- Send gameobject and animation parameters to ElementsAnimator methods.

2. Animate Alpha channel 

        ElementsAnimator programAnimator = transform.GetComponentInParent<ElementsAnimator>();

        Text text = GetComponentInChildren<Text>();
        RawImage icon = GetComponentInChildren<RawImage>();

        float startValue = 0;
        float finishValue = 0.5f;
        float time = 2f;

        programAnimator.AnimateAlpha(text.gameObject, startValue, finishValue, time);

        programAnimator.AnimateAlphaFromCurrentState(icon.gameObject, finishValue, time);
        
3. Send ***animateChildren*** parameter for animate all children

        float delay = 0.3f;
        bool animateChildren = true;

        programAnimator.AnimateAlphaFromCurrentState(gameObject, finishValue, time, delay, animateChildren);

4. Animate Color

        LineRenderer line = GetComponentInChildren<LineRenderer>();

        Color startColor = Color.white;
        Color finishColor = Color.green;

        programAnimator.AnimateColor(line.gameObject, startColor, finishColor, time);
        
5. Animate position

        Vector2 startPosition = new Vector2(0, 0);
        Vector2 finishPosition = new Vector2(10, -5);

        programAnimator.AnimatePosition(gameObject.transform, startPosition, finishPosition, time);

6. Use ***IEnumerator*** type methods to create Coroutine with animation and start it manually.

    ```
    void StartAnimation()
    {
        float time = 2f;
        Vector2 shift = new Vector2(5, 5);
        StartCoroutine(ReverceAnimation(shift, time));
    }

    private IEnumerator ReverceAnimation(Vector2 shift, float time)
    {
        yield return _programAnimator.GetShiftPositionAnimation(gameObject.transform, shift, time);
        yield return _programAnimator.GetShiftPositionAnimation(gameObject.transform, -shift, time);
    }
    ```