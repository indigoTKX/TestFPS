using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestFPS.Gameplay
{
    public class PlayerReference : SingletonBase<PlayerReference>
    {
        public GameObject Player { get; private set; }

        protected new void Awake()
        {
            base.Awake();
            Player = gameObject;
        }
    }
}