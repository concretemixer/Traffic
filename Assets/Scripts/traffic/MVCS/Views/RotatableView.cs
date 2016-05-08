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
            float ratio = (float)height / (float)width;

            GameObject[] vert = GameObject.FindGameObjectsWithTag("UI Vertical");
            foreach (var o in vert)
            {
                if (o.GetComponent<Image>() != null) {                    
                    o.GetComponent<Image>().enabled = isVertical;
                    if (ratio>1 && o.GetComponent<RectTransform>().rect.width > 599)
                    {
                        float k = (960 / ratio) / o.GetComponent<RectTransform>().rect.width;
                        if (k < 1)
                            k = 1;

                        o.GetComponent<RectTransform>().localScale = new Vector3(k,1 , 1);                        
                    }
                }
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