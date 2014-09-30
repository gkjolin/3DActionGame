﻿using System;
using UnityEngine;

namespace Cradle
{
	[Serializable]
	public class CharaStatusController
	{
		public int HP = 100;
		public int MaxHP = 100;
		public int Power = 10;
		public bool attacking = false;
		public bool died = false;
		public bool powerBoost = false;
		public string charactername = "Player";
		private float powerBoostTime = 0.0f;
		
		private IEffectController effectController;
		
		public CharaStatusController (){
		}
		
		public void SetBoostTime (float boostTime) {
			this.powerBoostTime = boostTime;
		}
		
		public float GetBoostTime () {
			return this.powerBoostTime;
		}
		
		public void CalcBoostTime() {
			this.powerBoostTime = CalcTime ();
		}
		
		public virtual float CalcTime() {
			return Mathf.Max (this.powerBoostTime - Time.deltaTime, 0.0f);
		}
		
		public void CalcHP() {
			this.HP = Math.Min ( (int) (this.HP + this.MaxHP / 2), this.MaxHP);
		}
		
		public bool CanBoost() {
			if(powerBoostTime > 0.0f) 
				return true;
			return false;
		}
		
		public void EnablePowerBoost() {
			this.powerBoost = true;
		}
		
		public void DisablePowerBoost() {
			this.powerBoost = false;
		}
		
		public bool IsPlayer(string tag) {
			if (tag == charactername)
				return true;
			return false;
		}
		
		public void SetEffectController(IEffectController effectController) {
			this.effectController = effectController;
		}
		
		public bool IsAttacking(){
			return this.attacking;
		}
		
		public bool SetAttacking(bool flg){
			return this.attacking = flg;
		}
		
	}
}
