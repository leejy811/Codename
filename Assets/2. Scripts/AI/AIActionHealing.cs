using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;


namespace MoreMountains.TopDownEngine
{
    public class AIActionHealing : AIAction
    {
		/// the possible refill modes :
		/// - linear : constant health refill at a certain rate per second
		/// - bursts : periodic bursts of health
		public enum RefillModes { Linear, Bursts }

		[Header("Mode")]
		/// the selected refill mode 
		[Tooltip("the selected refill mode ")]
		public RefillModes RefillMode;


		[Header("Refill Settings")]
		/// if this is true, health will refill itself when not at full health
		[Tooltip("if this is true, health will refill itself when not at full health")]
		public bool RefillHealth = true;
		/// the amount of health per second to restore when in linear mode
		[MMEnumCondition("RefillMode", (int)RefillModes.Linear)]
		[Tooltip("the amount of health per second to restore when in linear mode")]
		public float HealthPerSecond;
		/// the amount of health to restore per burst when in burst mode
		[MMEnumCondition("RefillMode", (int)RefillModes.Bursts)]
		[Tooltip("the amount of health to restore per burst when in burst mode")]
		public float HealthPerBurst = 5;
		/// the duration between two health bursts, in seconds
		[MMEnumCondition("RefillMode", (int)RefillModes.Bursts)]
		[Tooltip("the duration between two health bursts, in seconds")]
		public float DurationBetweenBursts = 2f;

		protected Health _health;
		protected int prevHealth;
		protected int curHealth;
		protected float _healthToGive = 0f;

		protected float _lastBurstTimestamp;
		protected Character character;

		private bool isHealingReady = false;
        public override void Initialization()
        {
			if (!ShouldInitialize)
				return;
            base.Initialization();
			_health = this.gameObject.GetComponent<Health>();
			character = this.gameObject.GetComponent<Character>();
		}

        public override void PerformAction()
        {
			ProcessRefillHealth();
        }

		/// <summary>
		/// Tests if a refill is needed and processes it
		/// </summary>
		protected virtual void ProcessRefillHealth()
		{
			if (!RefillHealth || !isHealingReady)
			{
				return;
			}

			if (_health.CurrentHealth < _health.MaximumHealth)
			{
				switch (RefillMode)
				{
					case RefillModes.Bursts:
						if (Time.time - _lastBurstTimestamp > DurationBetweenBursts)
						{
							_health.ReceiveHealth(HealthPerBurst, this.gameObject);
							_lastBurstTimestamp = Time.time;
						}
						break;

					case RefillModes.Linear:
						_healthToGive += HealthPerSecond * Time.deltaTime;
						if (_healthToGive > 1f)
						{
							float givenHealth = _healthToGive;
							_healthToGive -= givenHealth;
							_health.ReceiveHealth(givenHealth, this.gameObject);
						}
						break;
				}
			}
		}

        public override void OnEnterState()
        {
            StartCoroutine("BeforeHealingAnim");
            base.OnEnterState();
			_health.ImmuneToDamage = true;

		}

        public override void OnExitState()
        {
            StartCoroutine(ExitState());
            
			RefillHealth = true;
			_health.ImmuneToDamage = false;

			base.OnExitState();
        }

		public IEnumerator BeforeHealingAnim()
		{

			character._animator.SetTrigger("doHeal");
			yield return new WaitForSeconds(2f);
			isHealingReady = true;

        }
        public IEnumerator ExitState()
        {
            character._animator.SetBool("HealingEnd", true);
            yield return new WaitForSeconds(2f);
            character._animator.SetBool("HealingEnd", false);
        }
    }
}


