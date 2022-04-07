using System;
using System.Collections.Generic;
using UnityEngine;

// namespace Core
// {
public static class CoreExtensions
{
    public static void DoReplacePos(this ChipsEntity e, Contexts contexts, int xx, int yy)
    {
        var (oldX, oldY) = (e.position.x, e.position.y);
        contexts.game.ctx.dynamicData.ReplacePos(e, oldX, oldY, xx, yy);
        e.ReplacePosition(xx, yy);
    }
    public static void DoAddPos(this ChipsEntity e, Contexts contexts, int xx, int yy)
    {
        contexts.game.ctx.dynamicData.AddPos(e, xx, yy);
        e.AddPosition(xx, yy);
    } 
        
    public static void DoRemovePos(this ChipsEntity e, Contexts contexts, bool removeComponent = true)
    {
        contexts.game.ctx.dynamicData.RemovePos(e, e.position.x, e.position.y);
        if(removeComponent)
            e.RemovePosition();
    }
        
    public static void DoAddDamage(this ChipsEntity e, DamageInfo damage)
    {
        var list = e.hasDamage ? e.damage.values : new List<DamageInfo>();
        list.Add(damage);
        e.ReplaceDamage(list);
    }
        
    public static void DoClearDamage(this ChipsEntity e)
    {
        if(e.hasDamage)
            e.damage.values?.Clear();
    }

    public static bool IsHole(this Contexts contexts, int x, int y)
        => contexts.game.ctx.levelConfig.Holes.Exists(v => v.x == x && v.y == y);
    
    public static ChipsEntity ChipByPos(this Contexts contexts, int x, int y)
    {
        var e = contexts.game.ctx.dynamicData.GetItem(x, y);
        if (e == null || e.isDead /*|| !e.isBaseLayer*/)
            return null;
            
        return e;
    }

    public static ChipsEntity RawChipByPos(this Contexts contexts, int x, int y)
        => contexts.game.ctx.dynamicData.GetItem(x, y);

    public static void CallIfHasChip(this Contexts contexts, int x, int y, Action<ChipsEntity> cb)
    {
        var entity = contexts.ChipByPos(x, y);
        if (entity != null)
            cb(entity);
    }
        
    public static void FindNeighbours(this Contexts contexts, ChipsEntity eOrigin, int x, int y, List<ChipsEntity> list)
    {
        var neighbour = contexts.game.ctx.dynamicData.GetItem(x, y);
            
        if (neighbour == null || neighbour.isDead)
            return;
            
        // if (neighbour.chipConfig.value.name != eOrigin.chipConfig.value.name )
        //     return;
            
        if(neighbour.isVisited)
            return;
        neighbour.isVisited = true;

        list.Add(neighbour);

        var neighbourPos = neighbour.position;
        FindNeighbours(contexts, eOrigin, neighbourPos.x + 1, neighbourPos.y + 0, list);
        FindNeighbours(contexts, eOrigin, neighbourPos.x - 1, neighbourPos.y + 0, list);
        FindNeighbours(contexts, eOrigin, neighbourPos.x + 0, neighbourPos.y + 1, list);
        FindNeighbours(contexts, eOrigin, neighbourPos.x + 0, neighbourPos.y -1, list);
    }
}
// }