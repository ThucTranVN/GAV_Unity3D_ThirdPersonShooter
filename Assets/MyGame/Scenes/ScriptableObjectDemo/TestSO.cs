using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace TPS
{
    public class TestSO : MonoBehaviour
    {
        public TextMeshProUGUI health;
        public TextMeshProUGUI damage;
        public TextMeshProUGUI strength;
        public TextMeshProUGUI ability;
        public TextMeshProUGUI inteligence;

        private void Awake()
        {
            DataManager.Instance.Init();
        }

        void Start()
        {
            LoadData();
        }

        public void UpdatePlayerData(int amount)
        {
            var dataSO = DataManager.Instance.PlayerData;
            dataSO.playerData.health += amount;
            dataSO.playerData.damage += amount;
            dataSO.playerData.strength += amount;
            dataSO.playerData.ability += amount;
            dataSO.playerData.inteligence += amount;
            DataManager.Instance.SavePlayerData();
            LoadData();
        }

        public void ChangeScence(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        private void LoadData()
        {
            var dataSO = DataManager.Instance.PlayerData;
            health.SetText($"Health {dataSO.playerData.health}");
            damage.SetText($"Damage {dataSO.playerData.damage}");
            strength.SetText($"Strength {dataSO.playerData.strength}");
            ability.SetText($"Ability {dataSO.playerData.ability}");
            inteligence.SetText($"Inteligence {dataSO.playerData.inteligence}");
        }
    }
}

