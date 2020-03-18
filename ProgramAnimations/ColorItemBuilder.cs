using Assets.ProgramAnimations.ColorItems;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.ProgramAnimations
{
    public class ColorItemBuilder
    {
        public IColorItem Build(GameObject gameObject)
        {
            var components = gameObject.GetComponents(typeof(Component));

            foreach (var component in components)
            {
                IColorItem colorItem = Build(component);

                if (colorItem != null)
                {
                    return colorItem;
                }
            }
            return null;
        }

        private IColorItem Build(Component component)
        {
            if (component is MaskableGraphic uiElement)
            {
                return new UiColor(uiElement);
            }
            else if (component is SpriteRenderer spriteElement)
            {
                return new SpriteColor(spriteElement);
            }
            else if (component is LineRenderer lineElement)
            {
                return new LineColor(lineElement);
            }
            else if (component is MeshRenderer meshElement)
            {
                return new MeshColor(meshElement);
            }

            return null;
        }
    }
}