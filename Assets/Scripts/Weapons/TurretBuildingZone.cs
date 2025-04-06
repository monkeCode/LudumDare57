using Core;
using GameResources;
using UnityEngine;

namespace Weapons
{
    public class TurretBuildingZone: MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject turretPrefab;
        [SerializeField] private int buildingPrice;
        [SerializeField] private CurrencyStorage currencyStorage;
        [SerializeField] private Player.Player player;
        [SerializeField] private int CanBuildCount = 1;

        private void Start()
        {
            currencyStorage = FindFirstObjectByType<CurrencyStorage>();
            player = FindFirstObjectByType<Player.Player>();
        }

        public void Interact()
        {
            if (currencyStorage.TrySpendCurrency(buildingPrice))
            {
                SpawnTurret();
                CanBuildCount--;
                if (CanBuildCount == 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void SpawnTurret()
        {
            Instantiate(turretPrefab, player.transform.position, player.transform.rotation);
        }
    }
}