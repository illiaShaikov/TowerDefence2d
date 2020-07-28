using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField] GameObject towerObject;
    [SerializeField] Sprite dragSprite;
    public GameObject TowerObject
    {
        get
        {
            return towerObject;
        }
    }
    public Sprite DragSprite
    {
        get
        {
            return dragSprite;
        }
    }
}
