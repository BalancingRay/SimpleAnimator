using UnityEngine;

namespace Assets.ProgramAnimations.ColorItems
{
    public class MeshColor : IColorItem
    {
        private MeshRenderer _element;

        public MeshColor(MeshRenderer element)
        {
            _element = element;
        }

        public Color GetColor()
        {
            return _element?.material?.color ?? Color.clear;
        }

        public void SetColor(Color color)
        {
            if (_element?.material != null)
            {
                _element.material.color = color;
            }
        }
    }
}
