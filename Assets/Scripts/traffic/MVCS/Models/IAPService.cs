using UnityEngine;

namespace Traffic.MVCS.Models
{
    public enum IAPType {
        AdditionalLevels,
        NoAdverts
    }

    public interface IAPService
    {
        bool IsBought(IAPType what);
        bool Buy(IAPType what);
    }

    public class IAPServiceDummy : IAPService
    {
        public bool IsBought(IAPType what)
        {
            return PlayerPrefs.GetInt("iap." + what.ToString(), 0) == 1;
        }

        public bool Buy(IAPType what)
        {
            PlayerPrefs.SetInt("iap." + what.ToString(), 1);
            return true;
        }
    }
}