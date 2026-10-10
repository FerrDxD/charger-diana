namespace Core
{
    /// <summary>
    /// Logika murni untuk baterai senter, dipisah agar mudah diuji (E7).
    /// </summary>
    public class BatteryModel
    {
        public float CurrentBattery { get; private set; }
        public float MaxBattery { get; set; } = 100f;
        public float LowThreshold { get; set; } = 10f;
        
        // Tuning
        public float FlashlightDrainRate { get; set; } = 0.08f; // ~20 min full
        public float IdleDrainRate { get; set; } = 0.01f;

        public bool IsFlashlightOn { get; private set; } = true;
        public bool IsDepleted => CurrentBattery <= 0f;
        public bool IsLow => CurrentBattery > 0f && CurrentBattery <= LowThreshold;

        public BatteryModel(float startBattery)
        {
            CurrentBattery = startBattery;
        }

        public void SetFlashlight(bool isOn)
        {
            if (IsDepleted && isOn)
            {
                IsFlashlightOn = false;
                return;
            }
            IsFlashlightOn = isOn;
        }

        public void Update(float dt)
        {
            if (IsDepleted) return;

            float drain = IsFlashlightOn ? FlashlightDrainRate : IdleDrainRate;
            CurrentBattery -= drain * dt;

            if (CurrentBattery <= 0f)
            {
                CurrentBattery = 0f;
                IsFlashlightOn = false;
            }
        }

        public void AddCharge(float amount)
        {
            CurrentBattery += amount;
            if (CurrentBattery > MaxBattery)
            {
                CurrentBattery = MaxBattery;
            }
        }

        public void SetCharge(float amount)
        {
            CurrentBattery = amount;
            if (CurrentBattery > MaxBattery) CurrentBattery = MaxBattery;
            if (CurrentBattery < 0f) CurrentBattery = 0f;
        }
    }
}
