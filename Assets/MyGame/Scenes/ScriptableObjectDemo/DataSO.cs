using UnityEngine;

namespace TPS
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Custom Data/Player Data", order = 1)]
    public class DataSO : ScriptableObject
    {
        public PlayerData playerData;
    }
}

