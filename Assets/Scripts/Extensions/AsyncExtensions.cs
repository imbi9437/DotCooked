using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomExtensions
{
    public static class AsyncExtensions
    {
        private const float SceneProgressLimit = 0.9f;
    
        public static bool WaitUntilLoadScene(this AsyncOperation operation)
        {
            return operation.progress >= SceneProgressLimit;
        }
    }
}