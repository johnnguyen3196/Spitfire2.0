using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public PlayerAttack FindGun(int gun)
    {
        switch (gun)
        {
            case 0:
                return new PlayerAttack();
            case 1:
                return new DoubleShot();
            case 2:
                return new TripleShot();
            case 3:
                return new QuadShot();
            case 4:
                return new UpgradedTripleShot();
            case 5:
                return new AutoCannon();
            case 6:
                return new HighVelocityShot();
            case 7:
                return new UpgradedDoubleShot();
            case 8:
                return new DoubleAutoCannon();
            case 9:
                return new SmartHighVelocityShot();
            case 10:
                return new Shotgun();
            default:
                return new PlayerAttack();
        }
    }

    public PlayerAttack FindMissile(int missile)
    {
        switch (missile)
        {
            case 0:
                return new PlayerAttack();
            case 1:
                return new SingleMissile();
            case 2:
                return new DoubleMissile();
            case 3:
                return new TripleMissile();
            case 4:
                return new SwarmerMissile();
            case 5:
                return new SinglePoisonMissile();
            default:
                return new PlayerAttack();
        }
    }

    public GameObject FindEscort(string name)
    {
        GameObject o = Resources.Load(name) as GameObject;

        if (o == null)
        {
            UnityEngine.Debug.LogWarning("Escort: " + name + " not found!");
            return null;
        }
        return o;
    }
}
