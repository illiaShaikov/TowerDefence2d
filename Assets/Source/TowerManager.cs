using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Loader<TowerManager>
{
    public TowerButton towerButtonPressed { get; set; }
    SpriteRenderer spriteRenderer;
    private List<TowerControl> TowerList = new List<TowerControl>();
    private List<Collider2D> BuildList = new List<Collider2D>();
    private Collider2D buildTile;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);
            if(hit.collider.tag == "TowerSide")
            {
                buildTile = hit.collider;               
                buildTile.tag = "TowerSideFull";
                RegisterBuildSide(buildTile);
                PlaceTower(hit);
            }        
        }
        if(spriteRenderer.enabled)
        {
            FollowMouse();
        }
    }
    public void RegisterBuildSide(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }
    public void RegisterTower(TowerControl tower)
    {
        TowerList.Add(tower);
    }
    public void RenameTagBuildSide()
    {
        foreach(Collider2D buildTag in BuildList)
        {
            buildTag.tag = "TowerSide";
        }
        BuildList.Clear();
    }
    public void DestroyAllTowers()
    {
        foreach(TowerControl tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }
    public void PlaceTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && towerButtonPressed != null)
        {
            TowerControl newTower = Instantiate(towerButtonPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            BuyTower(towerButtonPressed.TowerPrice);
            RegisterTower(newTower);
            DisableDrag();
        }      
    }
    public void SelectedTower(TowerButton towerSelected)
    {        
        if(towerSelected.TowerPrice <= Manager.Instance.TotalMoney)
        {
            towerButtonPressed = towerSelected;
            EnableDrag(towerButtonPressed.DragSprite);
        }
    }
    public void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }
    public void EnableDrag(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }
    public void DisableDrag()
    {
        spriteRenderer.enabled = false;
    }
    public void BuyTower(int price)
    {
        Manager.Instance.SubstractMoney(price);
    }
}
