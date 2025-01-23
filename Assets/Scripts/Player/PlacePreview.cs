using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlaceType
{
    Unknown,
    BeltV1,
    BeltV2,
    SmelterV1,
    MinerV1
}
public static class PlaceTypeExtension
{
    public static PlaceType ToPlaceType(this string value){
        switch (value)
        {
            case "BeltV1": return PlaceType.BeltV1;
            case "BeltV2": return PlaceType.BeltV2;
            case "SmelterV1": return PlaceType.SmelterV1;
            case "MinerV1": return PlaceType.MinerV1;
            default: return PlaceType.Unknown;
        }
    }
}
public class PlacePreview : MonoBehaviour
{
    PlaceType placeType = PlaceType.Unknown;
    private GameObject GameContentContainer;
    private GameObject previewPrefab;
    private GameObject Preview;
    
    public Material PreviewMaterial;
    private List<Material> _OriginalMats = new List<Material>();
    void Start()
    {
        GameContentContainer = GameObject.FindWithTag("GameContentContainer");

    }

    // Update is called once per frame
    void Update()
    {
        if (placeType != PlaceType.Unknown)
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, Camera.main.transform.forward, out hit, 
                    150f, LayerMask.GetMask("Ground", "Buildings")))
            {
                Preview.transform.position = hit.point + new Vector3(0, 0.1f, 0);
            }

            if (Input.GetMouseButtonDown(Keys.BMouseLeft) &&
                PreviewMaterial.color == BuildingPlaceable.yes)//if place button pressed and building in placeable spot
            {
                //make solid
                ChangeMats(Preview.GetComponentInChildren<Renderer>(), false);
                Preview.GetComponent<MeshCollider>().providesContacts = true;
                Preview.GetComponent<MeshCollider>().isTrigger = false;
                Preview.layer = LayerMask.NameToLayer("Buildings");
                //reset variables
                placeType = PlaceType.Unknown;
                previewPrefab = null;
            }
        }
    }

    public void LoadPlacePrefab(PlaceType PrefabType)
    {
        placeType = PrefabType;
        previewPrefab = Resources.Load<GameObject>(PrefabType.ToString());
        
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit))
        {
            Preview = GameObject.Instantiate(previewPrefab, hit.point, Quaternion.identity);
            Preview.transform.SetParent(GameContentContainer.transform);

            ChangeMats(Preview.GetComponentInChildren<Renderer>(), true);
            Preview.layer = LayerMask.NameToLayer("Preview");
            //TODO Naming
            //TODO disable future scripts that might be on the game-content-object
            ///////////////////////
            /////////////////////// 
        }
    }

    private void ChangeMats(Renderer render, bool toTransparent)//never call twice!!! with toTransp true!
    {
        if (toTransparent)
        {
            render.GetMaterials(_OriginalMats);
            List<Material> mats = new List<Material>();
            foreach (var item in _OriginalMats)
            {
                mats.Add(PreviewMaterial);
            }
            render.SetMaterials(mats);
        }
        else
        {
            render.SetMaterials(_OriginalMats); 
        }

    }
}
