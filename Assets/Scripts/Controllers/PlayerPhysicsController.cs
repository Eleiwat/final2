using Assets.Scripts.Enums;
using Assets.Scripts.Managers;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;


public class PlayerPhysicsController : MonoBehaviour
{
    #region Self Variables

    #region Serialized Variables

    [SerializeField] private PlayerManager manager;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Collider collider;

    #endregion

    #region Private Variables

    [ShowInInspector] private bool _isInCuttingState;

    #endregion

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Cuttable"))
        {
            manager.ChangeAnimationState(AnimationStates.Cut);
        }

        if (other.CompareTag("Buyable"))
        {
            var buyableManager = other.GetComponent<BuyableManager>();
            var data = other.GetComponent<BuyableManager>().BuyableData;
            var priceresultWood = data.WoodRequirement - InGameEconomyManager._wood;
            var priceresultStone = data.StoneRequirement - InGameEconomyManager._stone;
            var priceresultGold = data.GoldRequirement - InGameEconomyManager._gold;
            if (priceresultWood <= 0 && priceresultStone <= 0 && priceresultGold <= 0 && !data.IsBought)
            {
                Debug.LogWarning(priceresultWood);
                EventManager.Instance.onUpdateInGameEconomy?.Invoke(CollectableTypes.Wood, -(InGameEconomyManager._wood + priceresultWood));
                EventManager.Instance.onUpdateInGameEconomy?.Invoke(CollectableTypes.Stone, -(InGameEconomyManager._stone + priceresultStone));
                EventManager.Instance.onUpdateInGameEconomy?.Invoke(CollectableTypes.Gold, -(InGameEconomyManager._gold + priceresultGold));
                buyableManager.BuyTheObject();
            }
            else { }


            if (priceresultWood > 0)
            {
                data.WoodRequirement -= InGameEconomyManager._wood;
                EventManager.Instance.onUpdateInGameEconomy?.Invoke(CollectableTypes.Wood, -(InGameEconomyManager._wood + priceresultWood));
            }

            if (priceresultStone > 0)
            {

                data.StoneRequirement -= InGameEconomyManager._stone;
                EventManager.Instance.onUpdateInGameEconomy?.Invoke(CollectableTypes.Stone, -(InGameEconomyManager._stone + priceresultStone));
            }

            if (priceresultGold > 0)
            {

                data.GoldRequirement -= InGameEconomyManager._gold;
                EventManager.Instance.onUpdateInGameEconomy?.Invoke(CollectableTypes.Gold, -(InGameEconomyManager._gold + priceresultGold));
            }

        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cuttable"))
        {
            if (!_isInCuttingState)
            {
                _isInCuttingState = true;
                DOVirtual.DelayedCall(7, () =>
                {
                    
                    UpdateEconomy(other);

                }).OnComplete(() =>
            {
                manager.DisableCuttingAnimation();
                _isInCuttingState = false;
            });
            }
        }

    }

    private void UpdateEconomy(Collider other)
    {
        manager.UpdateInGameEconomy(other.GetComponent<CollectableManager>().Type, 5);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cuttable"))
        {
            manager.DisableCuttingAnimation();
            manager.ChangeAnimationState(AnimationStates.Idle);
        }
    }

}
