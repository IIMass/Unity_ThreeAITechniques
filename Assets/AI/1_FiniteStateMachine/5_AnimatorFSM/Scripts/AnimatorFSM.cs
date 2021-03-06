﻿using System;
using UnityEngine;

namespace Animator_FSM
{
    public class AnimatorFSM : MonoBehaviour
    {
        [Header("Current State")]
        [SerializeField] private RobotStates currentRobotState;
        public enum RobotStates
        {
            Idle,
            Patrol,
            Chase,
            Attack
        }

        [Header("Distance From Player")]
        [SerializeField] private Distance currentDistanceFromPlayer;
        public enum Distance
        {
            Far,
            Near
        }

        [Header("Idle Pause Time")]
        [SerializeField] private int idlePauseTime;
        [SerializeField] private float idlePauseTimer;

        [Header("Patrol Point Check")]
        [SerializeField] private bool reachedPatrolPoint;

        [Header("Player Visibility")]
        [SerializeField] private bool canSeePlayer;
        [SerializeField] private bool playerIsDead;


        [Header("Events")]
        public Action OnStateChange;


        #region Getters and Setters
        public RobotStates GetRobotState()
        {
            return currentRobotState;
        }
        public void SetRobotState(RobotStates newState)
        {
            currentRobotState = newState;
            OnStateChange?.Invoke();
        }

        public Distance GetRobotToPlayerDistance()
        {
            return currentDistanceFromPlayer;
        }
        public void SetRobotToPlayerDistance(Distance newDistance)
        {
            currentDistanceFromPlayer = newDistance;
        }

        public int GetIdlePauseTime()
        {
            return idlePauseTime;
        }
        public void SetIdlePauseTime(int newIdleTime)
        {
            idlePauseTimer = 0f;
            idlePauseTime = newIdleTime;
        }
        public float GetIdlePauseTimer()
        {
            return idlePauseTimer;
        }
        public void SetIdlePauseTimer(float timer)
        {
            idlePauseTimer = timer;
        }

        public bool GetPatrolReachedPoint()
        {
            return reachedPatrolPoint;
        }
        public void SetPatrolReachedPoint(bool reached)
        {
            reachedPatrolPoint = reached;
        }

        public bool GetPlayerVisibility()
        {
            return canSeePlayer;
        }
        public void SetPlayerVisibility(bool visible)
        {
            canSeePlayer = visible;
        }

        public bool GetPlayerStatus()
        {
            return playerIsDead;
        }
        public void SetPlayerStatus(bool dead)
        {
            playerIsDead = dead;
        }
        #endregion
    }
}