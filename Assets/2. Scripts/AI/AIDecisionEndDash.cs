using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine
{
    public class AIDecisionEndDash : AIDecision
    {
		/// The number of hits required to return true
		[Tooltip("The number of hits required to return true")]
		public int NumberOfHits = 1;

		protected int _hitCounter;
		protected BoxCollider2D boxCollider;

		/// <summary>
		/// On init we grab our Health component
		/// </summary>
		public override void Initialization()
		{
			boxCollider = _brain.gameObject.GetComponentInParent<BoxCollider2D>();
			_hitCounter = 0;
		}

		/// <summary>
		/// On Decide we check whether we've been hit
		/// </summary>
		/// <returns></returns>
		public override bool Decide()
		{
			return EvaluateHits();
		}

		/// <summary>
		/// Checks whether we've been hit enough times
		/// </summary>
		/// <returns></returns>
		protected virtual bool EvaluateHits()
		{
			return (_hitCounter >= NumberOfHits);
		}

		/// <summary>
		/// On EnterState, resets the hit counter
		/// </summary>
		public override void OnEnterState()
		{
			base.OnEnterState();
			_hitCounter = 0;
		}

		/// <summary>
		/// On exit state, resets the hit counter
		/// </summary>
		public override void OnExitState()
		{
			base.OnExitState();
			_hitCounter = 0;
		}

		/// <summary>
		/// When we get hit we increase our hit counter
		/// </summary>
		protected virtual void OnHit()
		{
			_hitCounter++;
		}

        private void OnTriggerEnter(Collider other)
        {
			Debug.Log("Hit : " + other.gameObject.name + " layer : " + other.gameObject.layer);
			if (other.gameObject.layer == 8 || other.gameObject.layer == 10)
				_hitCounter++;
        }
    }
}
