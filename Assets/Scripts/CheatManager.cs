using System.Collections;
using System.Collections.Generic;
using CodingEssentials;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public bool Invulnerable;

    public bool InfiniteAmmo;
    // Start is called before the first frame update
    IEnumerable Start()
    {
        while (Toolbox.Instance == null || Toolbox.Instance.PlayerRoot == null)
        {
            yield return new WaitForFixedUpdate();
        }
        if (Invulnerable)
        {
            Toolbox.Instance.PlayerRoot.gameObject.AddComponent<Invulnerable>().Duration = 1000000;
        }

        if (InfiniteAmmo)
        {
            Toolbox.Instance.PlayerRoot.gameObject.AddComponent<AmmoRegen>();

        }
    }

}
