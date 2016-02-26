using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Traffic.MVCS.Views.UI
{
    public class RotatableView : View
    {
        public virtual void Layout(int width, int height)
        {
            bool isVertical = height > width;
            GameObject[] vert = GameObject.FindGameObjectsWithTag("UI Vertical");
            foreach (var o in vert)
            {
                if (o.GetComponent<Image>() != null)
                    o.GetComponent<Image>().enabled = isVertical;
                if (o.GetComponent<Text>() != null)
                    o.GetComponent<Text>().enabled = isVertical;
            }
            GameObject[] hor = GameObject.FindGameObjectsWithTag("UI Horizontal");
            foreach (var o in hor)
            {
                if (o.GetComponent<Image>() != null)
                    o.GetComponent<Image>().enabled = !isVertical;
                if (o.GetComponent<Text>() != null)
                    o.GetComponent<Text>().enabled = !isVertical;
            }
        }
    }
}