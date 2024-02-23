using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryunm_WeaponController : MonoBehaviour {
    // Fire
    public Transform bullStartPoint;
    public GameObject bullet;
    [SerializeField] float bulletStartSpeed = 100f;
    bool isFire = false;
    float fireInterval = 0.1f;

    // Back
    [SerializeField] Transform defautPoint;
    [SerializeField] Transform backPoint;
    [SerializeField] float lerpRation = 0.2f;

    // Audio
    [SerializeField] AudioSource bulletSource = null;

    // ViewController
    [SerializeField] Camera mainCam;
    [SerializeField] Camera weaponCam;
    [SerializeField] Vector3 weaponCamDefaultPoint;
    [SerializeField] Vector3 weaponCamCenterPoint;
    [SerializeField] float defaultView = 60;
    [SerializeField] float centerView = 70;
    [SerializeField] float viewRatio = 0.2f;


    // Start is called before the first frame update
    void Start() {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        weaponCam = GameObject.FindGameObjectWithTag("WeaponCam").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {
        OpenFire();
        ViewChange();

        // RayCast
        Ray _ray = new(bullStartPoint.position, bullStartPoint.forward);
        Debug.DrawRay(_ray.origin, _ray.direction * 1000, Color.red);
    }
    private void OpenFire() {
        if (Input.GetMouseButtonDown(0)) {
            isFire = true;
            StartCoroutine("Fire");
        }

        if (Input.GetMouseButtonUp(0)) {
            isFire = false;
            StopCoroutine("Fire");
        }
    }
    IEnumerator Fire() {
        while (isFire) {
            if (bullStartPoint != null || bullet != null) {
                GameObject newBullet = Instantiate(bullet, bullStartPoint.position, bullStartPoint.rotation);
                newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * bulletStartSpeed;
                // Back
                StopCoroutine("WeaponBack");
                StartCoroutine("WeaponBack");
                PlayBulletSource();

                Destroy(newBullet, 5);
            }
            yield return new WaitForSeconds(fireInterval);
        }
    }

    IEnumerator WeaponBack() {
        yield return null;

        if (defautPoint != null && backPoint != null) {
            // Back
            while (transform.localPosition != backPoint.localPosition) {
                transform.localPosition = Vector3.Lerp(transform.localPosition, backPoint.localPosition, lerpRation);
                yield return null;
            }
            // Forward
            while (transform.localPosition != defautPoint.localPosition) {
                transform.localPosition = Vector3.Lerp(transform.localPosition, defautPoint.localPosition, lerpRation);
                yield return null;
            }
        }
    }

    private void PlayBulletSource() {
        if (bulletSource) {
            bulletSource.Play();
        }
    }

    private void ViewChange() {
        if (Input.GetMouseButton(1)) {
            StopCoroutine("ViewToDefault");
            StartCoroutine("ViewToCenter");
        }
        else {
            StopCoroutine("ViewToCenter");
            StartCoroutine("ViewToDefault");
        }
    }

    IEnumerator ViewToCenter() {
        while (weaponCam.transform.localPosition != weaponCamCenterPoint) {
            weaponCam.transform.localPosition = Vector3.Lerp(weaponCam.transform.localPosition, weaponCamCenterPoint, viewRatio);
            weaponCam.fieldOfView = Mathf.Lerp(weaponCam.fieldOfView, centerView, viewRatio);
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, centerView, viewRatio);
            yield return null;
        }
    }
    IEnumerator ViewToDefault() {
        while (weaponCam.transform.localPosition != weaponCamDefaultPoint) {
            weaponCam.transform.localPosition = Vector3.Lerp(weaponCam.transform.localPosition, weaponCamDefaultPoint, viewRatio);
            weaponCam.fieldOfView = Mathf.Lerp(weaponCam.fieldOfView, defaultView, viewRatio);
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, defaultView, viewRatio);
            yield return null;
        }
    }
}
