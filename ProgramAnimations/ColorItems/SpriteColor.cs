using UnityEngine;

namespace Assets.ProgramAnimations.ColorItems
{
    public class SpriteColor : IColorItem
    {
        private SpriteRenderer _element;

        public SpriteColor(SpriteRenderer element)
        {
            _element = element;
        }

        public Color GetColor()
        {
            return _element?.color ?? Color.clear;
        }

        public void SetColor(Color color)
        {
            if (_element != null)
            {
                _element.color = color;
            }
        }
    }
}
