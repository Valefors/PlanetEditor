using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetsManager 
{
    public static Material baseMaterial = Resources.Load("Materials/BaseMaterial", typeof(Material)) as Material;
    public static Material hooverMaterial = Resources.Load("Materials/HooverMaterial", typeof(Material)) as Material;
    public static Material selectionMaterial = Resources.Load("Materials/SelectionMaterial", typeof(Material)) as Material;

    public static Color sandColor = new Color32(255, 227, 112, 1);
    public static Color rockColor = new Color32(229, 204, 255, 1);
    public static Color grassColor = new Color32(78, 225, 55, 1);
    public static Color waterColor = new Color32(60, 220, 245, 1);
}
