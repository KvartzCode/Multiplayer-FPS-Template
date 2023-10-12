using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace AlterunaFPS
{
	
	public class Health : MonoBehaviour
	{

		public Health Parent;
		
		[Space]
		public EfxManager.ImpactType MaterialType = EfxManager.ImpactType.Stone;
		
		public float PenetrationResistance = 0.5f;
		public float DamageMultiplier = 1f;
		public float HealthPoints = 0f;
		public int ScorePoints = 20;
		
		public UnityEvent OnDeath;
		
		// Only apply once per health family.
		private float _lastDamage;
		private int _lastDamageIndex;

		public bool Alive
		{
			get
			{
				return HealthPoints > 0f;
			}
		}

		public bool TakeDamage(float damage, out int score) => TakeDamage(damage, Time.frameCount, out score);

		private bool TakeDamage(float damage, int damageIndex, out int score)
		{
			score = ScorePoints; // return no damage if
			damage *= DamageMultiplier;

			// Check if damage is already applied.
			if (_lastDamageIndex == damageIndex)
			{
				// Undo last damage before applying new damage.
				if (damage > _lastDamage)
					TakeDamage(-_lastDamage, damageIndex, out score);
				// If new damage is less than last damage, ignore.
				else
					return false;
			}
			_lastDamage = damage;
			_lastDamageIndex = damageIndex;
			
			// apply damage
			if (Parent != null)
			{
				return Parent.TakeDamage(damage, out score);
			}
			else if (Alive)
			{
				HealthPoints -= damage;
				//score = ScorePoints;
				if (HealthPoints <= 0f)
				{
					HealthPoints = 0f;
					OnDeath.Invoke();
					return true;
				}
			}

			return false;
		}
	}
}