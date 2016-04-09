using UnityEngine;
using UnityEngine.UI;
using Traffic.MVCS.Commands.Signals;
using System.Threading;
using Traffic.Components;
using System.Collections.Generic;

namespace Traffic.MVCS.Models
{


    public class LocaleService : ILocaleService 
    {
        [Inject]
        public IAPService iapService { get; set; }

        Dictionary<string, string> entries = new Dictionary<string,string>();

        public LocaleService()
        {
           entries.Add("%START%", "START!");
            entries.Add("%MUSIC%", "MUSIC");
            entries.Add("%SOUND%", "SOUND");
            entries.Add("%PROMO_TITLE%", "PROMOTION");
            entries.Add("%ENTER_CODE%", "ENTER CODE");
            entries.Add("%SETTINGS%", "SETTINGS");
            entries.Add("%SHOP%", "SHOP");
            entries.Add("%BUY%", " BUY");
            entries.Add("%HOME%", " HOME");
            entries.Add("%PAUSE%", " PAUSE");
            entries.Add("%LOADING%", " LOADING...");
            //entries.Add("%BUY%", "йсохрэ");

            entries.Add("%LEVELS_DESC_SHOP%", "12 ADDITIONAL LEVELS FOR %PRICE_NO_ADS%");
            entries.Add("%NO_ADS_DESC_SHOP%", "TURN OFF ADVERTISMENTS FOR %PRICE_LEVELS%");

            entries.Add("%PURCHASE_OK%", "PURCHASE_OK");
            entries.Add("%PURCHASE_FAILED%", "PURCHASE FAILED");

            entries.Add("%LEVELS_BOUGHT%", "You have purchased 12 additional levels for  %PRICE_LEVELS%");
            entries.Add("%NO_ADS_BOUGHT%", "You have purchased the permanent advert removal for %PRICE_NO_ADS%");

            entries.Add("%LEVELS_LOCKED_TITLE%", "ATTENTION");
            entries.Add("%LEVELS_LOCKED_TEXT%", "YOU CAN BUY ADDITIONAL 12 LEVELS FOR %PRICE_LEVELS%");
            entries.Add("%NO_TRIES_TITLE%", "NO MORE TRIES LEFT");
            entries.Add("%NO_TRIES_DESC%", "BUY UNLIMITED TRIES FOR %PRICE_NO_ADS% OR WATCH AN AVDERTISMENT TO REFRESH !");
            entries.Add("%NO_TRIES_TIMER%", "TRIES WILL REFRESH AUTOMATICALY IN {0}:{1}");

            entries.Add("%NO_TRIES_ADVERT%", "VIEW\nADVERT");

            entries.Add("%NEW_TRIES_TITLE%", "CONGRATULATIONS");
            entries.Add("%NEW_TRIES_DESC%", "YOU TRIES HAS BEEN RESTORED!");

            entries.Add("%TUTOR_STEP_0%", "The cars are about to crash! To avoid the crash, <color=lime>TAP</color> the car to accelerate it.");
            entries.Add("%TUTOR_STEP_1%", "Progress bar. It grows as a vehicle reaches the edge of the screen.");
            entries.Add("%TUTOR_STEP_2%", "Score. Accelerate vehicles to gain more point!");
            entries.Add("%TUTOR_STEP_3%", "Attempts count. It decreases with every crash. Once it reaches zero, attempts should be refilled.");
            entries.Add("%TUTOR_STEP_4%", "There is an obstacle ahead! <color=lime>SWIPE</color> the car to stop it or to decelerate.");
            entries.Add("%TUTOR_STEP_5%", "<color=lime>TAP</color> the car to make it move again and avoid being hitting from behind.");
            entries.Add("%TUTOR_STEP_6%", "This is the <color=yellow>bus stop</color>. The place where buses stop :)");
            entries.Add("%TUTOR_STEP_7%", "And here is the <color=yellow>bus</color> heading for the <color=yellow>bus stop</color>.\nYou <color=#ff5500>CANNOT CONTROL</color> buses, so be careful and watch the other vehicles.");

            entries.Add("%LEVEL_LOST_1%", "CRASHED!");
            entries.Add("%LEVEL_LOST_2%", "NICE TRY, BUT...");
            entries.Add("%LEVEL_LOST_3%", "ALMOST THERE...");

            entries.Add("%LEVEL_WON_1%", "DONE IT!");
            entries.Add("%LEVEL_WON_2%", "EXCELLENT!");
            entries.Add("%LEVEL_WON_3%", "PERFECT!");

            entries.Add("%YOUR_SCORE%", "YOUR SCORE:");

            entries.Add("%FREE_LEVELS_COMPLETE_CAPTION%", "INCREDIBLE!");
            entries.Add("%FREE_LEVELS_COMPLETE%",
                "YOU HAVE COMPLETED ALL THE FREE LEVELS OF THE GAME! 12 MORE EXCITING LEVELS ARE JUST AROUND THE CORNER FOR THE <color=white>%PRICE_LEVELS%</color> ONLY!");

            entries.Add("%GAME_COMPLETE_CAPTION%", "UNBELIEVABLE!");
            entries.Add("%GAME_COMPLETE%", "YOU HAVE COMPLETED ALL THE LEVELS! WE ARE IMPRESSED.");

            entries.Add("%TUTORIAL_COMPLETE%", "Tutorial complete. Now you are ready for the real TRAFFIC STORM!");
            entries.Add("%TUTORIAL_FAILED%", "No pain - no gain! Try once more.");

            entries.Add("%CODE_CAPTION%", "INFORMATION");
            entries.Add("%CODE_OK%", "CODE OK!");
            entries.Add("%CODE_FAIL%", "UNKNOWN CODE");
        
/*            
            TextAsset locale;
            locale = UnityEngine.Resources.Load<TextAsset>("locale/default");           
            string[] lines = locale.text.Split(new char[] {'\r','\n'});

            foreach (var line in lines) 
            {
                string[] parts = line.Split(new char[] { '|' });
                if (parts.Length >= 2)
                {
                    entries.Add(parts[0],parts[1].Replace("<br>","\n"));
                }
            }  */           
        }


        bool priceStringsOk = false;
        [PostConstruct]
        public void UpdatePriceStrings()
        {
            if (priceStringsOk)
                return;

            float price = 1000;
            string currency = "?";

            if (iapService.GetProductPrice(IAPType.NoAdverts, out price, out currency))
            {
                entries.Add("%PRICE_NO_ADS%", currency + (currency.Length > 1 ? " " : "") + price.ToString("F2"));
                priceStringsOk = true;
            }
            if (iapService.GetProductPrice(IAPType.AdditionalLevels, out price, out currency))
            {
                entries.Add("%PRICE_LEVELS%", currency + (currency.Length > 1 ? " " : "") + price.ToString("F2"));
                priceStringsOk = true;
            }
        }

        public void SetAllTexts(GameObject root)         
        {
            UpdatePriceStrings();
            foreach (Text textField in root.GetComponentsInChildren<Text>())
            {
                textField.text = ProcessString(textField.text);
            }
        }

        public string ProcessString(string template)
        {
            string text = template;

            for (int a = 0; a < 3; a++)
            {
                bool changed = false;
                foreach (var entry in entries)
                {
                    if (text.Contains(entry.Key))
                    {
                        text = text.Replace(entry.Key, entry.Value);
                        changed = true;
                    }
                }

                if (!changed)
                    break;
            }

            return text;
        }

    }
}
