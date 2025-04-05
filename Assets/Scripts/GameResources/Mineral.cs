using Interfaces;
using UnityEngine;

namespace GameResources
{
    public class Mineral: MonoBehaviour, IMineral
    {
        public float Size { get; } = 1;
        public uint Cost { get; } = 1;
    }
}