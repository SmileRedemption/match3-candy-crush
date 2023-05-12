using UnityEngine;

namespace Model.Candy
{
    [CreateAssetMenu(menuName = "Item/Candy")]
    public class CandyData : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}
