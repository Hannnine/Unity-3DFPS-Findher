using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ryunm_WeaponController : MonoBehaviour {
    // Fire
    public Transform bullStartPoint;
    public GameObject bullet;
    [SerializeField] float bulletStartSpeed = 100f;
    bool isFire = false;
    [SerializeField] float fireInterval = 0.1f;

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

    // Ammo
    [SerializeField] float maxAmmoCount = 18;
    [SerializeField] float currentAmmoCount = 18;
    [SerializeField] float reloadTime = 1;
    [SerializeField] Slider ammoSlider;

    // Closed Attack
    [SerializeField] Transform closeAtackPoint;
    [SerializeField] float closeAtackRation = 0.5f;

    // Audio
    [SerializeField] AudioSource reloadSorce;


    // Start is called before the first frame update
    void Start() {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        weaponCam = GameObject.FindGameObjectWithTag("WeaponCam").GetComponent<Camera>();

        currentAmmoCount = maxAmmoCount;
        ammoSlider.value = 1;

        if (ammoSlider) {
            ammoSlider.value = currentAmmoCount / maxAmmoCount;
        }
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

        if (Input.GetKeyDown(KeyCode.R)) {
            // Reload ammo
            StopCoroutine("ReloadAmmo");
            StartCoroutine("ReloadAmmo");
            if (reloadSorce) {
                reloadSorce.Play();
            }
        }
        
        if(Input.GetKeyUp(KeyCode.F)) {
            StopCoroutine("WeaponCloseAttack");
            StartCoroutine("WeaponCloseAttack");
        }
    }
    IEnumerator Fire() {
        while (isFire && currentAmmoCount>0) {
            if (bullStartPoint != null || bullet != null) {
                GameObject newBullet = Instantiate(bullet, bullStartPoint.position, bullStartPoint.rotation);
                newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * bulletStartSpeed;
                newBullet.GetComponent<Ryunm_BulletController>().bulletType = BulletType.Player_Bullet  ;
                // Recoil
                StopCoroutine("WeaponRecoil");
                StartCoroutine("WeaponRecoil");
                PlayBulletSource();

                currentAmmoCount--;
                if (ammoSlider) {
                    ammoSlider.value = currentAmmoCount / maxAmmoCount;
                }

                Destroy(newBullet, 5);
            }
            yield return new WaitForSeconds(fireInterval);
        }
    }

    IEnumerator WeaponRecoil() {
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
    IEnumerator WeaponCloseAttack() {
        yield return null;

        if (defautPoint != null && closeAtackPoint != null) {
            // Attack
            while (transform.localPosition != closeAtackPoint.localPosition) {
                transform.localPosition = Vector3.Lerp(transform.localPosition, closeAtackPoint.localPosition, closeAtackRation);
                yield return null;
            }
            // Back
            while (transform.localPosition != defautPoint.localPosition) {
                transform.localPosition = Vector3.Lerp(transform.localPosition, defautPoint.localPosition, closeAtackRation);
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
    IEnumerator ReloadAmmo() {
        while (!isFire && currentAmmoCount<maxAmmoCount ) {
            var loadValue = maxAmmoCount / reloadTime * Time.deltaTime;
            currentAmmoCount += loadValue;
            if (ammoSlider) {
                ammoSlider.value = currentAmmoCount / maxAmmoCount;
            }
            yield return null;
        }
    }
}
