using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class HuiMing : MonoBehaviour {

    public GameObject MingIcon;
    public Material MingMaterial;
    public GameObject HuiIcon;
    public Material HuiMaterial;
    private HashSet<GameObject> swapped = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider enemy) {
        if (swapped.Contains(enemy.gameObject)) { return; }

        if (enemy.transform.Find("Hui(Clone)") != null || enemy.transform.Find("Ming(Clone)") != null) {
            Debug.Log("HHHHHHHHHHHHH");
            swapped.Add(enemy.gameObject);
            StartCoroutine(swapAttribute(enemy.gameObject));
        }
    }

    IEnumerator swapAttribute(GameObject enemy) {
        if (enemy.transform.Find("Hui(Clone)") != null) {
            Transform current = enemy.transform.Find("Hui(Clone)");
            Vector3 localPos = current.localPosition;
            Quaternion localRotation = current.localRotation;
            Transform parent = current.parent;

            Transform child = MingIcon.transform.GetChild(0);
            Vector3 childLocalPos = child.localPosition;
            Quaternion childLocalRotation = child.localRotation;

            Destroy(current.gameObject);
            yield return null;

            GameObject replacement;
            replacement = Instantiate(MingIcon, parent);

            replacement.name = "Ming(Clone)";
            replacement.transform.localPosition = localPos;
            replacement.transform.localRotation = localRotation;

            Transform newChild = replacement.transform.GetChild(0);
            newChild.transform.localPosition = childLocalPos;
            newChild.transform.localRotation = childLocalRotation;
            newChild.GetComponent<Image>().material = new Material(MingMaterial);
            Debug.Log("Swapped Hui");

        }
        else if (enemy.transform.Find("Ming(Clone)") != null) {
            Transform current = enemy.transform.Find("Ming(Clone)");
            Vector3 localPos = current.localPosition;
            Quaternion localRotation = current.localRotation;
            Transform parent = current.parent;

            Transform child = HuiIcon.transform.GetChild(0);
            Vector3 childLocalPos = child.localPosition;
            Quaternion childLocalRotation = child.localRotation;

            Destroy(current.gameObject);
            yield return null;

            GameObject replacement;
            replacement = Instantiate(HuiIcon, parent);
            replacement.name = "Hui(Clone)";
            replacement.transform.localPosition = localPos;
            replacement.transform.localRotation = localRotation;

            Transform newChild = replacement.transform.GetChild(0);
            newChild.transform.localPosition = childLocalPos;
            newChild.transform.localRotation = childLocalRotation;
            newChild.GetComponent<Image>().material = new Material(HuiMaterial);
            Debug.Log("Swapped Ming");
        }
        yield return new WaitForSeconds(10f);
        swapped.Remove(enemy);
    }
}
