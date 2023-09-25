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
		protected int _numberOfJumps = 0;
		public bool findTarget = false;
		/// <summary>
		/// On init we grab our CharacterMovement ability
		/// </summary>
		public override void Initialization()
		{
			if (!ShouldInitialize) return;
			base.Initialization();
			_characterMovement = this.gameObject.GetComponentInParent<Character>()?.FindAbility<CharacterMovement>();
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
			if(findTarget)
            {
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
				if(!findTarget)
                {
					_direction = (_brain.Target.position - this.transform.position).normalized;
					_characterMovement.SetMovement(_direction);
				}
			}
			Debug.Log("Dash Speed In Action Dash : " + dashSpeed);
			findTarget = true;

		}

		/// <summary>
		/// On exit state we stop our movement
		/// </summary>
		public override void OnExitState()
		{
			base.OnExitState();
			findTarget = false;
			_characterMovement?.SetHorizontalMovement(0f);
			_characterMovement?.SetVerticalMovement(0f);
		}
	}

}


