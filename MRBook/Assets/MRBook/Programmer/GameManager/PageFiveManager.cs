using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class PageFiveManager : BasePage
{
    [SerializeField]
    HoloCharacter urashima = null;

    [SerializeField]
    HoloCharacter turtle = null;

    [SerializeField]
    HoloItem secretBox_Lid = null;
    [SerializeField]
    HoloItem secretBox_Box = null;

    public override void PageStart()
    {
        base.PageStart();
        
        if (FlagManager.I.GetFlag("IsCloseSecretBox",3, false))
        {
            //蓋をする
            secretBox_Box.ItemSaucer.SetItem(secretBox_Lid, false);
        }

        if(FlagManager.I.GetFlag("UrashimaIsMacho", 3, false))
        {
            //浦島はマッチョだった
            ChangeMesh(urashima, "UrashimaMacho", "UrashimaMachoMat");
            urashima.ChangeScale(1.25f);
        }

        turtle.gameObject.SetActive(false);
        secretBox_Box.gameObject.SetActive(false);
        secretBox_Lid.gameObject.SetActive(false);

        MainSceneManager.I.OnPlayEnd += (success) =>
        {
            ResultManager.I.ShowTotalResult();

            Utilities.Delay(2.0f, () =>
            {
                ResultManager.I.ShowTitleBack();
            }, this);

        };
    }

    void ChangeMesh(HoloObject obj, string meshName, string materialName)
    {
        Mesh mesh = MyAssetStore.I.GetAsset<Mesh>(meshName, "Meshes/");
        if (mesh == null) return;
        Material material = MyAssetStore.I.GetAsset<Material>(materialName, "Materials/");
        if (material == null) return;

        SkinnedMeshRenderer rend;
        rend = obj.GetComponentInChildren<SkinnedMeshRenderer>();

        if (rend == null) return;

        rend.sharedMesh = mesh;
        rend.material = material;
    }
}
