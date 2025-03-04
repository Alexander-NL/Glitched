using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    public Stats S; // Current ammo count
    public Image[] ammoImages; // Array of UI Images for different ammo amounts

    void Update()
    {
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {

        // Loop through all images and set them active/inactive based on the ammo count
        for (int i = 0; i < ammoImages.Length; i++)
        {
            if (ammoImages[i] != null)
            {
                ammoImages[i].gameObject.SetActive(i == S.M2_Ammo);
            }
        }
    }
}