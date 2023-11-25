using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;


namespace MoreMountains.TopDownEngine
{
    public class AIActionDash : AIAction
    {/// if this is true, movement will be constrained to not overstep a certain distance to the target on the x axis
		[Tooltip("if this is true, movement will be constrained to not overstep a certain distance to the target on the x axis")]
		public bool UseMinimumXDistance = true;
		/// the minimum distance from the target this Character can reach on the x axis.
		[Tooltip("the minimum distance from the target this Character can reach on the x axis.")]
		public float MinimumXDistance = 1f;
		public float dashSpeed = 40f;

		protected Vector2 _direction;
		protected CharacterMovement _characterMovement;
		protected CharacterOrientation2D _characterOrientation;
		protected Health _health;
		protected int _numberOfJumps = 0;
		public bool findTarget = false;
		public bool isDashReady= false;
		protected Character character;

		[SerializeField] GameObject warningArea;
		
		/// <summary>
		/// On init we grab our CharacterMovement ability
		/// </summary>
		public override void Initialization()
		{
			if (!ShouldInitialize) return;
			base.Initialization();
			_characterMovement = this.gameObject.GetComponentInParent<Character>()?.FindAbility<CharacterMovement>();
            _health = this.gameObject.GetComponent<Health>();
			_characterOrientation = this.gameObject.GetComponent<CharacterOrientation2D>();
			character = this.gameObject.GetComponent<Character>();

        }

        /// <summary>
        /// On PerformAction we move
        /// </summary>
        public override void PerformAction()
		{
			Dash();
		}

        /// <summary>
        /// Moves the character towards the target if needed
        /// </summary>
        protected virtual void Dash()
		{
			if (_brain.Target == null)
			{
				return;
			}
			if(findTarget || !isDashReady)
            {
                _direction = (_brain.Target.position - this.transform.position).normalized;
				float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
				warningArea.transform.rotation = Quaternion.Slerp(warningArea.transform.rotation, Quaternion.AngleAxis(angle + 90f, Vector3.forward), Time.deltaTime * 5);
                return;
            }
			if (UseMinimumXDistance)
			{
				_characterMovement.SetDashSpeed(dashSpeed);
				if (this.transform.position.x < _brain.Target.position.x)
				{
					_characterMovement.SetHorizontalMovement(1f);
				}
				else
				{
					_characterMovement.SetHorizontalMovement(-1f);
				}

				if (this.transform.position.y < _brain.Target.position.y)
				{
					_characterMovement.SetVerticalMovement(1f);
				}
				else
				{
					_characterMovement.SetVerticalMovement(-1f);
				}

				if (Mathf.Abs(this.transform.position.x - _brain.Target.position.x) < MinimumXDistance)
				{
					_characterMovement.SetHorizontalMovement(0f);
				}

				if (Mathf.Abs(this.transform.position.y - _brain.Target.position.y) < MinimumXDistance)
				{
					_characterMovement.SetVerticalMovement(0f);
				}
			}
			else
			{
                if (!findTarget)
                {
                    _characterMovement.SetDashSpeed(dashSpeed);
                    _direction = (_brain.Target.position - this.transform.position).normalized;
					_characterMovement.SetMovement(_direction);
                }
			}
			findTarget = true;
			
		}

		IEnumerator DashAnim()
		{
            _direction = (_brain.Target.position - this.transform.position).normalized;

			if (_direction.x < 0)
			{
				character.CharacterModel.gameObject.transform.localScale = new Vector3(-1, 1, 1);
			}
			else
				character.CharacterModel.gameObject.transform.localScale = new Vector3(1, 1, 1);

            warningArea.SetActive(true);

            character._animator.SetBool("Dashing", true);
            yield return new WaitForSeconds(1f);
            warningArea.SetActive(false);
            isDashReady = true;
        }
        public override void OnEnterState()
        {
            base.OnEnterState(); 
			_health.ImmuneToKnockback = true;
			StartCoroutine("DashAnim");
        }
        /// <summary>
        /// On exit state we stop our movement
        /// </summary>
        public override void OnExitState()
		{
			base.OnExitState();
			findTarget = false;
            _health.ImmuneToKnockback = false;
			isDashReady = false;
			character._animator.SetBool("Dashing", false);
			_characterMovement.SetDashSpeed(_characterMovement.WalkSpeed);
            
            _characterMovement?.SetHorizontalMovement(0f);
			_characterMovement?.SetVerticalMovement(0f);
		}
	}

	
}


