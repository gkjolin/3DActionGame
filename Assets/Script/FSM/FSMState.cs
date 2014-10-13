﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cradle.FM;
using Cradle;

/// @http://creativecommons.org/licenses/by-sa/3.0/

namespace Cradle.FM{
	public abstract class FSMState /*: IFSMStateController*/
	{
		protected Dictionary<Transition, FSMStateID> map = new Dictionary<Transition, FSMStateID>();
		protected FSMStateID stateID;
		public FSMStateID ID { get { return stateID; }}
		protected Vector3 destPos;
		protected Transform[] waypoints;
		protected float RotSpeed;
		protected float dist;

		/*public FSMStateController fsmSController;
		
		public void OnEnable() {
			fsmSController.SetFSMStateController (this);
		}*/

		
		public void AddTransition(Transition transition, FSMStateID id)
		{
			//引数の確認
			if(CheckTransOrID(transition, id))
			{
				Debug.LogWarning("FSMState: Null transition not allowed");
				return;
			}
			
			//現在の状態が、Map(Dictionary)に存在するかを確認。
			if(map.ContainsKey(transition))
			{
				Debug.LogWarning("FSMState ERROR: transition is already inside the map");
				return;
			}
			
			map.Add (transition, id);
			Debug.Log ("Added : " + transition + "with ID : " + id);
		}
		
		//不要な遷移をDictionaryから削除
		public void DeleteTransition(Transition trans)
		{
			//null遷移か確認
			if(EmptyTrans(trans))
			{
				Debug.LogError("FSMState ERROR: NullTransition is not allowed");
				return;
			}
			
			//ペアがMAPにあるか判別
			if(map.ContainsKey(trans))
			{
				map.Remove(trans);
				return;
			}
			Debug.LogError ("FSMState ERROR: 指定された遷移はリストにありません。");
		}
		
		//この状態が遷移するときに、どの状態になるかを判別する
		public FSMStateID GetOutputState(Transition trans)
		{
			//遷移がnullか確認
			if(EmptyTrans(trans))
			{
				Debug.LogError("FSMState ERROR: 不正なnull遷移です。");
				return FSMStateID.None;
			}
			
			//mapが遷移を持つか確認
			if(map.ContainsKey(trans))
			{
				return map[trans];
			}
			
			Debug.LogError ("FSMState ERROR: " + trans+ "は存在しません。");
			return FSMStateID.None;
		}
		
		//状態が遷移すべきかの意思決定を行なう
		public abstract void Reason(Transform player, Transform npc);
		
		//NPC(敵キャラ)の処理、行動、動作を指定する
		public abstract void Act(Transform player, Transform npc);

		//ターゲット地点をプレーヤーポジションに設定
		public void SetDest(Transform player){
			this.destPos = player.position;	
		}
		public void SetDist(float f){
			this.dist = f;
		}

		public void SetWayPoints(Transform[] wp){
			this.waypoints = wp;
		}

		public void SetStateID(FSMStateID fsmState){
			this.stateID = fsmState;
		}

		public void SetRotSpeed(float f){
			this.RotSpeed = f;
		}

		public Quaternion SetTargetRot(Vector3 v){
			Quaternion targetRotation = Quaternion.LookRotation (destPos - v);
			return targetRotation;
		}
		
		public void SetRot(Transform npc, Vector3 v){
			npc.rotation = Quaternion.Slerp (npc.rotation, SetTargetRot(v), Time.deltaTime * RotSpeed);
		}

		//TansitionとIDのチェック
		public bool CheckTransOrID(Transition transition, FSMStateID id){
			if(EmptyTrans(transition) || EmptyID(id))
				return true;
			return false;
		}
		
		public bool EmptyTrans(Transition t){
			if(t == Transition.None)
				return true;
			return false;
		}
		
		public bool EmptyID(FSMStateID f){
			if(f == FSMStateID.None)
				return true;
			return false;
		}



		//distが一定の距離内かチェック
		public bool CheckDist(float f, float l, float o){
			if (MoreThanCheckReach (f, l) && LessCheckReach (f, o))
				return true;
			return false;
		}
		
		public bool MoreThanCheckReach(float f, float l){
			if (f >= l)
				return true;
			return false;
		}
		
		public bool LessCheckReach(float f, float o){
			if (f < o)
				return true;
			return false;
		}

		public bool LessThanCheckReach(float f, float o){
			if (f <= o)
				return true;
			return false;
		}

		//次の索敵ポイントを指定する。乱数で動作
		public void FindNextPoint()
		{
			Debug.Log("Finding next point");
			SetFindDest ();
			Debug.Log ("destPos :" + destPos);
		}

		public int RndIndex(){
			int rndIndex = Random.Range(0, waypoints.Length);
			return rndIndex;
		}

		public Vector3 RndPosition(){
			Vector3 rndPosition = Vector3.zero;
			return rndPosition;
		}

		public void SetFindDest(){
			this.destPos = waypoints[RndIndex()].position + RndPosition();
		}

		//次のポジションが、現在の位置と同じかチェックする
		protected bool IsInCurrentRange(Transform trans, Vector3 pos)
		{
			if (CheckRange (CheckXRange (pos.x, trans.position.x), CheckZRange (pos.z, trans.position.z)))
						return true;
					return false;
		}

		public float CheckXRange(float f, float l){
			float xPos = Mathf.Abs (f - l);	
			return xPos;
		}
		
		public float CheckZRange(float f, float l){
			float zPos = Mathf.Abs (f - l);	
			return zPos;
		}
		
		public bool LessThanXPos(float xPos){
			if(xPos <= 10)	
				return true;
			return false;
		}
		
		public bool LessThanZPos(float zPos){
			if(zPos <= 10)	
				return true;
			return false;
		}

		public bool CheckRange(float xPos, float zPos){
			if (LessThanXPos(xPos) && LessThanZPos(zPos))
				return true;
			return false;
		}

	}
}