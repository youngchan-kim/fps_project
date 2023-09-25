using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Person/Enemy Data",order =2)]
public class EnemyData : MonoBehaviour
{
    [SerializeField] LayerMask target_Layer , cover_Layer, item_Layer;
    [SerializeField][Range(0, 100)] float searchRange = 20.0f;
    [SerializeField][Range(0, 100)] float max_Hp = 100.0f;
    [SerializeField][Range(0, 20)] float speed = 20.0f;
    [SerializeField][Range(0, 120)] float viewAngle=120.0f;

    public LayerMask TargetLayer => target_Layer;
    public LayerMask Cover_Layer => cover_Layer;
    public LayerMask Item_Layer => item_Layer;
    public float SearchRange => searchRange;
    public float Max_Hp => max_Hp;
    public float Speed => speed;
    public float ViewAngle => viewAngle;
}
