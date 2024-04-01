using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidManager : Singleton<LiquidManager>
{
    [SerializeField] Transform liquidPrefab;
    [SerializeField] ReflectionProbe reflectionProbe;

    bool isLiquidGenerated = false;

    public void GenerateLiquid(Vector3 position, Transform parent, ZibraLiquidCollider zibraLiquidCollider)
    {
        if (isLiquidGenerated) return;
        Transform liquid = Instantiate(liquidPrefab, position, Quaternion.identity, parent);
        liquid.GetComponent<ZibraLiquid>().ReflectionProbeBRP = reflectionProbe;
        liquid.localScale = new Vector3(1, 1, 1);
        AddCollider(zibraLiquidCollider);
        isLiquidGenerated = true;
    }

    private void AddCollider(ZibraLiquidCollider zibraLiquidCollider)
    {
        liquidPrefab.GetComponent<ZibraLiquid>().AddCollider(zibraLiquidCollider);
    }
}
