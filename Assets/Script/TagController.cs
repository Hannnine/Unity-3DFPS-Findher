using UnityEngine;

public class TagSynchronizer : MonoBehaviour {
    // Set Parent and Child Tag
    public void SyncsTags(string tag) {
        // 设置父对象的标签
        gameObject.tag = tag;

        // Set all child tags
        foreach (Transform child in transform) {
            child.gameObject.tag = tag;
        }
    }
    public void start() {
        TagSynchronizer synchronizer = GetComponent<TagSynchronizer>();
        synchronizer.SyncsTags("Enemy");
    }
}