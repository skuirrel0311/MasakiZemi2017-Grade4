using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KKUtilities
{
    public class Singleton<T> where T : class, new()
    {
        private static readonly T instance = new T();

        public static T I
        {
            get
            {
                return instance;
            }
        }
    }
}