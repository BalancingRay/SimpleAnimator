using UnityEngine;
using UnityEngine.UI;

namespace Assets.ProgramAnimations.ColorItems
{
    public class UiColor : IColorItem
    {
        private MaskableGraphic _element;

        public UiColor(MaskableGraphic element)
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
