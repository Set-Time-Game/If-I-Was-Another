using System.Collections.Generic;
using Classes.World;
using Interfaces;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private List<string> tagsWhiteList;
    
    public readonly LinkedList<ICollectable> Resources = new LinkedList<ICollectable>(); 
    public readonly LinkedList<IDamageable> Enemies = new LinkedList<IDamageable>();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!tagsWhiteList.Contains(collision.tag)) return;
        
        if (!collision.TryGetComponent<ICollectable>(out var collectable))
        {
            
            if (!collision.TryGetComponent<IDamageable>(out var enemy))
            {
            }
            else if (!Enemies.Contains(enemy))
            {
                Enemies.AddLast(enemy);
            }
        }
        else if (!Resources.Contains(collectable) && collectable.PickedTexture && collectable.OutlineTexture && collectable.DefaultTexture)
        {
            Resources.AddLast(collectable);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<ICollectable>(out var collectable))
        {
            
            if (!collision.TryGetComponent<IDamageable>(out var enemy))
            {
            }
            else if (!Enemies.Contains(enemy))
            {
                Enemies.Remove(enemy);
            }
        }
        else if (Resources.Contains(collectable))
        {
            Resources.Remove(collectable);
        }
    }
    
    /*
    protected virtual bool HasHighlight(IEntity target)
    {
        if (target.GameObject.TryGetComponent<ResourceSource>(out _)) return true;
        if (target.GameObject.TryGetComponent<Obstacle>(out var obs))
            return obs.Variety.viewsArray.Length >= 1 && obs.Variety.viewsArray[0].outlineTexture;

        return false;
    }
    */
}
