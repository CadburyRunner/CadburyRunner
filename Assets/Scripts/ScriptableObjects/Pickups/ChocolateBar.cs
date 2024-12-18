///<summary>
/// Author: Emmanuel
///
/// Holds the point values for chocolate bar pickups.
///
///</summary>

using UnityEngine;

namespace CadburyRunner
{
    [CreateAssetMenu(menuName = "ScriptableObject/ChocolateBar", fileName = "ChocolateBar", order = 0)]
    public class ChocolateBar : ScriptableObject
    {
        [SerializeField] private int m_pointValue;
        public int GetPointValue() { return m_pointValue; }
    }
}
