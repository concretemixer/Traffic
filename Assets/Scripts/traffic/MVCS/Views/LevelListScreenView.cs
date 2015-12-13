using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using Traffic.Core;

namespace Traffic.MVCS.Views.UI
{
    public class LevelListScreenView : View
    {
        [SerializeField]
        Button nextButton;

        [SerializeField]
        Button prevButton;

        [SerializeField]
        Button[] levelButtons;

        public readonly Signal onButtonNext = new Signal();
        public readonly Signal onButtonPrev = new Signal();
        public readonly Signal<int> onButtonLevel = new Signal<int>();

        private int page = 0;

        protected override void Awake()
        {
            nextButton.onClick.AddListener(onButtonNext.Dispatch);
            prevButton.onClick.AddListener(onButtonPrev.Dispatch);

            for (int a = 0; a < levelButtons.Length; a++)
            {
                int i = a;
                levelButtons[a].onClick.AddListener( delegate {                
                    onButtonLevel.Dispatch(i + page *  levelButtons.Length);
                });
            }

            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            base.Awake();
        }

       

        protected override void OnDestroy()
        {
            nextButton.onClick.RemoveListener(onButtonNext.Dispatch);
            prevButton.onClick.RemoveListener(onButtonPrev.Dispatch);
            for (int a = 0; a < levelButtons.Length; a++)
            {
                levelButtons[a].onClick.RemoveAllListeners();
            }
            base.OnDestroy();
        }

        public void SetPage(int page,ILevelListModel levels) 
        {
            if (page != 1 && page != 0)
                return;

            this.page = page;

            prevButton.gameObject.SetActive(page == 1);
            nextButton.gameObject.SetActive(page == 0);

            for (int a = 0; a < levelButtons.Length; a++) {
                int n = a + levelButtons.Length * page;
                levelButtons[a].GetComponentInChildren<Text>().text = (n+1).ToString();

                if (levels.GetLevelState(n) == LevelState.Locked)
                    levelButtons[a].GetComponent<Button>().interactable = false;
                else
                {
                 //   if (levels.GetLevelState(n) == LevelState.PassedThreeStars)
                        //levelButtons[a].GetComponent<Image>().sprite = new Sprite(
                    levelButtons[a].GetComponent<Button>().interactable = true;
                }
            }

        }

        public void Layout()
        {

            float ratio = (float)Screen.height / (float)Screen.width;

            this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960);
            this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960 * ratio);

            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            int w = ratio < 1 ? 4 : 3;
            int h = ratio < 1 ? 3 : 4;

            int n = 0;
            for (int a = 0; a < h; a++)
            {
                for (int b = 0; b < w; b++)
                {
                    if (ratio < 1)
                    {
                        levelButtons[n].GetComponent<RectTransform>().anchoredPosition = new Vector2(-225 + b * 150, 110 - 110 * a);
                        levelButtons[n].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        levelButtons[n].GetComponent<RectTransform>().anchoredPosition = new Vector2(-210 + b * 210, 240 - 160 * a);
                        levelButtons[n].GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    }

                    n++;
                }
            }
        }

    }
}