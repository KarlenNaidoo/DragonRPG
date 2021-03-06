﻿
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{

    public class Energy : MonoBehaviour
    {
        [SerializeField] RawImage energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 1f;

        float currentEnergyPoints;
        private float newEnergyPoints;

         void Update()
        {
           if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoints();
                UpdateEnergyBar();
            } 
        }

        private void AddEnergyPoints()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }

        // Use this for initialization
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
        }

        public void ConsumeEnergy(float amount)
        {
            newEnergyPoints = (currentEnergyPoints - amount);
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        public bool IsEnergyAvailable(float amount)
        {
            return amount <= currentEnergyPoints;
        }

        private void UpdateEnergyBar()
        {
            // TODO remove magic numbers
            float xValue = -(EnergyAsPercent() / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        float EnergyAsPercent()
        {
            return  currentEnergyPoints / maxEnergyPoints; 
        }
    }

}
