using Interfaces;
using UnityEngine;

namespace GameResources
{
    public class Mineral: MonoBehaviour, IMineral
    {
        [SerializeField] private float size = 1;
        [SerializeField] private uint cost = 1;
        public float Size  => size;
        public uint Cost => cost;
    }
}