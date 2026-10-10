using System;

namespace Core
{
    /// <summary>
    /// Logika murni untuk stamina pemain (E7 balancing).
    /// </summary>
    public class StaminaModel
    {
        public float CurrentStamina { get; private set; }
        public float MaxStamina { get; set; } = 100f;
        
        // Tuning
        public float DrainRate { get; set; } = 25f; // Habis dalam 4 detik lari
        public float RegenRate { get; set; } = 15f; // Penuh kembali dalam 6.6 detik
        public float CooldownTime { get; set; } = 1.5f;

        private float _cooldownTimer;
        public bool IsExhausted => CurrentStamina <= 0f;

        public StaminaModel(float startStamina = 100f)
        {
            CurrentStamina = startStamina;
        }

        public void Update(float dt, bool isSprinting)
        {
            if (isSprinting)
            {
                CurrentStamina -= DrainRate * dt;
                _cooldownTimer = CooldownTime;

                if (CurrentStamina < 0f) CurrentStamina = 0f;
            }
            else
            {
                _cooldownTimer -= dt;
                if (_cooldownTimer <= 0f)
                {
                    _cooldownTimer = 0f;
                    CurrentStamina += RegenRate * dt;
                    if (CurrentStamina > MaxStamina) CurrentStamina = MaxStamina;
                }
            }
        }
    }
}
