using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestFPS.Gameplay
{
    public class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
    {
        public static T Instance { get; private set; }

        protected void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                //TODO: uncomment when proper reset algorithm is ready 
                // throw new System.Exception("An instance of this singleton already exists.");
            }
            else
            {
                Instance = (T)this;
            }
        }
    }
}