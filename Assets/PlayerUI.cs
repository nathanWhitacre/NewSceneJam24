using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private CableGrapple cableGrapple;
    [SerializeField] private Slider charge1Slider;
    [SerializeField] private Slider charge2Slider;
    [SerializeField] private Slider charge3Slider;


    // Start is called before the first frame update
    void Start()
    {
        charge1Slider.value = 1f;
        charge2Slider.value = 1f;
        charge3Slider.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (cableGrapple.currentCharges == 0)
        {
            charge1Slider.value = cableGrapple.currentCooldown / cableGrapple.cooldown;
            charge2Slider.value = 0f;
            charge3Slider.value = 0f;
        }
        else if (cableGrapple.currentCharges == 1)
        {
            charge1Slider.value = 1f;
            charge2Slider.value = cableGrapple.currentCooldown / cableGrapple.cooldown;
            charge3Slider.value = 0f;
        }
        else if (cableGrapple.currentCharges == 2)
        {
            charge1Slider.value = 1f;
            charge2Slider.value = 1f;
            charge3Slider.value = cableGrapple.currentCooldown / cableGrapple.cooldown;
        }
        else if (cableGrapple.currentCharges == 3)
        {
            charge1Slider.value = 1f;
            charge2Slider.value = 1f;
            charge3Slider.value = 1f;
        }
    }
}
