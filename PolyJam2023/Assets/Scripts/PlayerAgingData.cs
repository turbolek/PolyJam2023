using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "My Assets/Aging Data")]
    public class PlayerAgingData : SerializedScriptableObject
    {
        public List<AgeData> AgeDataList = new List<AgeData>();
    }
}