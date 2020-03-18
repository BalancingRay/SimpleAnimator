using UnityEngine;

namespace Assets.ProgramAnimations.ColorItems
{
    public class LineColor : IColorItem
    {
        private LineRenderer _element;

        public LineColor(LineRenderer element)
        {
            _element = element;
        }

        public Color GetColor()
        {
            return _element?.startColor ?? Color.clear;
        }

        public void SetColor(Color color)
        {
            if (_element != null)
            {
                _element.startColor = color;
                _element.endColor = color;
            }
        }
    }
}
