using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryunm_LayoutFloors : MonoBehaviour {
    [SerializeField] GameObject Map;
    [SerializeField] Vector3 cellCnt = Vector3.zero;  // the number of each axis
    [SerializeField] Vector3 FloorSize = Vector3.zero;// the size of floor


    public List<Transform> FloorTransforms = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Reset Floor LayOut")]
    private void ResetFloorLayOut() {
        FloorTransforms.Clear();
        var ChildrenTransforms = GetComponentsInChildren<Transform>();

        // Get all the object of Floor
        for (int i = 1; i < ChildrenTransforms.Length; i++) {   //skip the parent node
            if (ChildrenTransforms[i].gameObject.CompareTag("Floor")) {
                FloorTransforms.Add(ChildrenTransforms[i]);
            }
        }

        // set axis(X, Z)
        int CurrentIndex = 0;
        for (int x = 0; x < cellCnt.x; x++) {
            for(int z = 0; z < cellCnt.z; z++) {
                Vector3 oPos = new Vector3(x*FloorSize.x, 0, z*FloorSize.z);
                FloorTransforms[CurrentIndex].localPosition = oPos;
                CurrentIndex++;
            }
        }

        // Initialize the Map
        float totalWidth = cellCnt.x * FloorSize.x;
        float totalHeight = cellCnt.z * FloorSize.z;
        // Calculate the distance
        Vector3 centerOffset = new Vector3(-totalWidth / 2, 0, -totalHeight / 2);

        // Adjust the position
        Map.transform.localPosition = centerOffset;
    }
}
