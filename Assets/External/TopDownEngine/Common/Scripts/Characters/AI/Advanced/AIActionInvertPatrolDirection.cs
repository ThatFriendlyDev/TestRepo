﻿using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// Inverts the direction of a patrol on PerformAction
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/AI/Actions/AIActionInvertPatrolDirection")]
    public class AIActionInvertPatrolDirection : AIAction
    {
        [Header("Invert Patrol Action Bindings")]
        /// the AIActionMovePatrol2D to invert the patrol direction on 
 
        /// the AIActionMovePatrol3D to invert the patrol direction on 
        [Tooltip("the AIActionMovePatrol3D to invert the patrol direction on")]
        public AIActionMovePatrol3D _movePatrol3D;
        
        /// <summary>
        /// On init we grab our actions
        /// </summary>
        public override void Initialization()
        {
 
            if (_movePatrol3D == null)
            {
                _movePatrol3D = this.gameObject.GetComponentInParent<AIActionMovePatrol3D>();    
            }
        }

        /// <summary>
        /// Inverts the patrol direction
        /// </summary>
        public override void PerformAction()
        { 
            _movePatrol3D?.ChangeDirection();
        }
    }
}
