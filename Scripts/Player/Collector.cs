using System;
using System.Collections;
using System.Collections.Generic;
using Classes.Entities;
using Classes.World;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private List<string> tagsWhiteList;
    
    protected readonly LinkedList<IGenerable> Resources = new LinkedList<IGenerable>(); 
    protected readonly LinkedList<IDamageable> Enemies = new LinkedList<IDamageable>();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!tagsWhiteList.Contains(collision.tag)) return;
        
        if (!collision.TryGetComponent<IGenerable>(out var generable))
        {
            /*
            if (!collision.TryGetComponent<IDamageable>(out var enemy))
            {
            }
            else if (!_enemies.Contains(enemy))
            {
                
            }*/
        }
        else if (!Resources.Contains(generable) && HasHighlight(generable))
        {
            Resources.AddLast(generable);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<IGenerable>(out var generable))
        {
            /*
            if (!collision.TryGetComponent<IDamageable>(out var enemy))
            {
            }
            else if (!_enemies.Contains(enemy))
            {
                
            }*/
        }
        else if (Resources.Contains(generable) && HasHighlight(generable))
        {
            Resources.Remove(generable);
        }
    }
    
    protected virtual bool HasHighlight(IEntity target)
    {
        if (target.GameObject.TryGetComponent<ResourceSource>(out _)) return true;
        if (target.GameObject.TryGetComponent<Obstacle>(out var obs))
            return obs.Variety.viewsArray.Length >= 1 && obs.Variety.viewsArray[0].outlineTexture;

        return false;
    }
}
