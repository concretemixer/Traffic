using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using Traffic.Core;

namespace Traffic.MVCS.Views.UI
{
    public class LevelListScreenView : RotatableView
    {
        [SerializeField]
        Button nextButton;

        [SerializeField]
        Button prevButton;

        [SerializeField]
        Button homeButton;

        [SerializeField]
        Button[] levelButtons;

        [SerializeField]
        Text debugMessage;

        [SerializeField]
        Image Locked;

        [SerializeField]
        Button buyButton;


        public readonly Signal onButtonNext = new Signal();
        public readonly Signal onButtonPrev = new Signal();
        public readonly Signal onButtonHome = new Signal();
        public readonly Signal<int> onButtonLevel = new Signal<int>();

        private int page = 0;
        private bool firstLevelTutorial = false;


        protected override void Awake()
        {
            nextButton.onClick.AddListener(onButtonNext.Dispatch);
            prevButton.onClick.AddListener(onButtonPrev.Dispatch);
            homeButton.onClick.AddListener(onButtonHome.Dispatch);

            for (int a = 0; a < levelButtons.Length; a++)
            {
                int i = a+1;

                
                levelButtons[a].onClick.AddListener( delegate {
                    int index = i + page * levelButtons.Length;
                    if (index == 1 && firstLevelTutorial)
                        index = 0;
                    onButtonLevel.Dispatch(index);
                });
            }

            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            base.Awake();
        }

       

        protected override void OnDestroy()
        {
            nextButton.onClick.RemoveListener(onButtonNext.Dispatch);
            prevButton.onClick.RemoveListener(onButtonPrev.Dispatch);
            homeButton.onClick.RemoveListener(onButtonHome.Dispatch);
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

            Sprite spriteCurrent = Resources.Load<Sprite>("UI/Sprites/Level_Current");
            Sprite spriteReg = Resources.Load<Sprite>("UI/Sprites/Level_0");
            Sprite sprite1Star = Resources.Load<Sprite>("UI/Sprites/Level_1");
            Sprite sprite2Star = Resources.Load<Sprite>("UI/Sprites/Level_2");
            Sprite sprite3Star = Resources.Load<Sprite>("UI/Sprites/Level_3");
            Sprite spriteLocked = Resources.Load<Sprite>("UI/Sprites/Level_Next");

            this.page = page;

            prevButton.GetComponent<Button>().interactable = (page == 1);
            nextButton.GetComponent<Button>().interactable = (page == 0);

            for (int a = 0; a < levelButtons.Length; a++) {
                int n = a + levelButtons.Length * page + 1;
                levelButtons[a].GetComponentInChildren<Text>().text = n.ToString();

                if (levels.GetLevelState(n) == LevelState.Locked)
                {
                    levelButtons[a].GetComponent<Image>().sprite = spriteLocked;
                    levelButtons[a].GetComponent<Button>().interactable = false;
                    levelButtons[a].GetComponentInChildren<Text>().color = new Color32(95, 115, 139,255);
                    if (n == 1)
                    {
                        firstLevelTutorial = true;
                        levelButtons[a].GetComponent<Button>().interactable = true;
                        levelButtons[a].GetComponentInChildren<Text>().color = Color.white;
                    }
                    
                }
                else
                {
                    levelButtons[a].GetComponentInChildren<Text>().color = Color.white;
                    levelButtons[a].GetComponent<Image>().sprite = spriteReg;
                    if (levels.GetLevelState(n) == LevelState.Playable)
                        levelButtons[a].GetComponent<Image>().sprite = spriteCurrent;
                    if (levels.GetLevelState(n) == LevelState.PassedOneStar)
                        levelButtons[a].GetComponent<Image>().sprite = sprite1Star;
                    if (levels.GetLevelState(n) == LevelState.PassedTwoStars)
                        levelButtons[a].GetComponent<Image>().sprite = sprite2Star;
                    if (levels.GetLevelState(n) == LevelState.PassedThreeStars)
                        levelButtons[a].GetComponent<Image>().sprite = sprite3Star;
                    levelButtons[a].GetComponent<Button>().interactable = true;
                }
                
            }

            Locked.gameObject.SetActive(page == 1);

        }

        public void SetDebugMessage(string text)
        {
            debugMessage.text = text;
        }

        public override void Layout()
        {
            base.Layout();

            float ratio = (float)Screen.height / (float)Screen.width;

            if (ratio < 1)
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960 * ratio);
            }
            else
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960 / ratio);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960);
            }


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
                        levelButtons[n].GetComponent<RectTransform>().anchoredPosition = new Vector2(-270 + b * 180, 50 - 130 * a);

                    }
                    else
                    {
                        levelButtons[n].GetComponent<RectTransform>().anchoredPosition = new Vector2(-180 + b * 180, 160 - 130 * a);

                    }

                    n++;
                }
            }

            if (ratio < 1)
            {
                //backBack.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
              //  backFront.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                prevButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-410, -80);
                nextButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(410, -80);
            }
            else
            {
               // backBack.GetComponent<RectTransform>().localScale = new Vector3(ratio, ratio, ratio );
               // backFront.GetComponent<RectTransform>().localScale = new Vector3(ratio , ratio , ratio );
                prevButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-220, -360);
                nextButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(220, -360);
            }

            debugMessage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 5);
            debugMessage.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);           
        }

    }
}