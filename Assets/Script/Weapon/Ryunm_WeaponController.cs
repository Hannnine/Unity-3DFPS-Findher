using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Ryunm_WeaponController : MonoBehaviour {
    // PlayerControls
    PlayerControlls playerControlls;
    InputAction fire;
    InputAction aim;
    InputAction reload;
    InputAction closeAttack;
    private void Awake() {
        playerControlls = new PlayerControlls();
    }
    private void OnEnable() {
        fire = playerControlls.FPS_Player_Controller.Fire;
        fire.Enable();
        aim = playerControlls.FPS_Player_Controller.Aim;
        aim.Enable();
        reload = playerControlls.FPS_Player_Controller.Reload;
        reload.Enable();
        closeAttack = playerControlls.FPS_Player_Controller.CloseAttack;
        closeAttack.Enable();
    }
    private void OnDisable() {
        fire.Disable();
        aim.Disable();
        reload.Disable();
        closeAttack.Disable();
    }

    // Fire
    public Transform bullStartPoint;
    public GameObject bullet;
    [SerializeField] float bulletStartSpeed = 100f;
    bool isFire = false;
    public float fireInterval = 0.1f;

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
    [SerializeField] bool isAim = false;

    // Ammo
    public float maxAmmoCount = 18;
    public float currentAmmoCount = 18;
    [SerializeField] float reloadTime = 1;
    [SerializeField] Slider ammoSlider;

    // Closed Attack
    [SerializeField] Transform closeAtackPoint;
    [SerializeField] float closeAtackRation = 0.5f;
    [SerializeField] bool isCloseAttack;

    // Audio
    [SerializeField] AudioSource reloadSorce;
   


    // Start is called before the first frame update
    void Start() {
        currentAmmoCount = maxAmmoCount;
        ammoSlider.value = 1;

        if (ammoSlider) {
            ammoSlider.value = currentAmmoCount / maxAmmoCount;
        }
    }

    // Update is called once per frame
    void Update() {
        OnFire();
        WeaponClosedAttack();
        Aim();
        WeaponReload();

        // RayCast
        Ray _ray = new(bullStartPoint.position, bullStartPoint.forward);
        Debug.DrawRay(_ray.origin, _ray.direction * 1000, Color.red);
    }
    private void OnFire() {
        if (fire.triggered) {
            if (!isFire) {
                isFire = true;
                StartCoroutine(Fire());
            }
            else {
                isFire = false;
                StopCoroutine(Fire());
            }
        }   
    }
    private void Aim() {
        if (aim.triggered) {
            if(!isAim) {
                isAim = true;
                StopCoroutine("ViewToDefault");
                StartCoroutine("ViewToCenter");
            }
            else if (isAim) {
                isAim = false;
                StopCoroutine("ViewToCenter");
                StartCoroutine("ViewToDefault");
            }
        }
    }
    private void WeaponClosedAttack() {
        if (closeAttack.triggered) {
            if (!isCloseAttack) {
                isCloseAttack = true;
                StopCoroutine("WeaponCloseAttackToDefaut");
                StartCoroutine("WeaponCloseAttackToAttack");
            }
            else if (isCloseAttack) {
                isCloseAttack = false;
                StopCoroutine("WeaponCloseAttackToAttack");
                StartCoroutine("WeaponCloseAttackToDefaut");
            }
        }
    }
    private void WeaponReload() {
        if (reload.triggered) {
            StopCoroutine(ReloadAmmo());
            StartCoroutine(ReloadAmmo());
            if (reloadSorce) {
                reloadSorce.Play();
            }
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
    IEnumerator WeaponCloseAttackToAttack() {
        // Attack
        while (transform.localPosition != closeAtackPoint.localPosition) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, closeAtackPoint.localPosition, closeAtackRation);
            yield return null;
        }
    }
    IEnumerator WeaponCloseAttackToDefaut() {
        // Back
        while (transform.localPosition != defautPoint.localPosition) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defautPoint.localPosition, closeAtackRation);
            yield return null;
        }
    }
    private void PlayBulletSource() {
        if (bulletSource) {
            bulletSource.Play();
        }
    }
    IEnumerator ViewToCenter() {
        while (weaponCam.transform.localPosition != weaponCamCenterPoint) {
            weaponCam.transform.localPosition = Vector3.Lerp(weaponCam.transform.localPosition, weaponCamCenterPoint, viewRatio);
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, centerView, viewRatio);
            weaponCam.fieldOfView = Mathf.Lerp(weaponCam.fieldOfView, centerView, viewRatio);
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
