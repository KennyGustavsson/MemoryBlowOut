using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SwitchBoard : MonoBehaviour
{   
    [Space]
    [Header("Switches")]
    
    [SerializeField] private Switch[] switches = default;
    [SerializeField] private float[] switchesVoltage = default;

    [Space]
    [Header("Buttons")]
    
    [SerializeField] private GameObject exitButton = default;

    [Space]
    [Header("Values")]
    
    [SerializeField] private float desiredTotalVoltage = 100;
    public float maxCapacity = 110;
    [SerializeField] private float overLoadDelay = 1;
    
    [Space]
    [Header("Reactions")]
    
    [SerializeField] private Reaction completeReaction = default;
    [SerializeField] private Reaction exitReaction = default;
    [SerializeField] private Reaction overloadingReaction = default;
    [SerializeField] private Reaction overloadEndReaction = default;

    private float currentVoltage;
    private float overloadTime;

    private bool overloading;


    public Action<float> onVoltageChanged;

    public bool isActive;

    private void OnValidate()
    {
        if (switchesVoltage.Length != switches.Length)
        {
            float[] temp = switchesVoltage;
            switchesVoltage = new float[switches.Length];

            for (int i = 0; i < switchesVoltage.Length; i++)
            {
                float voltage = 0;
                if (i <= temp.Length - 1) voltage = temp[i];
                switchesVoltage[i] = voltage;
            }
        }
    }

    private void Awake()
    {
        for (int i = 0; i < switches.Length; i++)
        {
            switches[i].voltage = switchesVoltage[i];
        }
    }

    private void OverloadTimer()
    {
        if(!overloading) return;
        //if(overloadingReaction && overLoadDelay > 0) overloadingReaction.TriggerReaction();
        
        if ( Time.time > overloadTime)
        {
            Detonate();
        }
    }

    private void Detonate()
    {
        Exit();
        if (overloadEndReaction)
        {
            overloading = false;
            overloadEndReaction.TriggerReaction();
        }
    }


    private void Update()
    {
        if(GameManager.Instance.isPaused) return;
        
        OverloadTimer();
        if(!isActive) return;
        
        if (!Input.GetButtonDown("LeftMouseButton")) return;
        
        GameObject clickedObject = ObjectRaycast();
        
        if (clickedObject == exitButton)
        {
            Exit();
            exitReaction.TriggerReaction();
        }

        Switch sw = FindClickedSwitch(clickedObject);
        
        if (sw)
        {
            sw.ToggleSwitch();
            RecalculateVoltage();
        }
    }
    
    private void RecalculateVoltage()
    {
        currentVoltage = switches.Where(sw => sw.isToggled).Sum(sw => sw.voltage);
        CheckOverload();
        CheckComplete();
        onVoltageChanged?.Invoke(currentVoltage);
    }

    private void CheckComplete()
    {
        if(!currentVoltage.Equals(desiredTotalVoltage)) return;
        
        Exit();
        if (completeReaction)
        {
            exitReaction.TriggerReaction();
            completeReaction.TriggerReaction();
            EventManager.OnPuzzleComplete(gameObject);
        }
    }

    private void Exit()
    {
        StartCoroutine(EnableAfterDelay());
        isActive = false;
    }

    private void CheckOverload()
    {
        if (currentVoltage > maxCapacity){
            if (!overloading && overloadingReaction)
                overloadingReaction.TriggerReaction();
            
            overloading = true;
            overloadTime = Time.time + overLoadDelay;
        }
        else
        {
            overloading = false;
        }
    }

    private Switch FindClickedSwitch(GameObject clickedObject)
    {
        return switches.FirstOrDefault(sw => sw.gameObject == clickedObject);
    }

    private static GameObject ObjectRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out RaycastHit hit) ? hit.collider.gameObject : null;
    }

    private IEnumerator EnableAfterDelay()
    {
        yield return null;
        gameObject.layer = 0;
    }
    
    
}
