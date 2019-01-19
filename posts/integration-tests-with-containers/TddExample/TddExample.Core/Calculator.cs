using System;

namespace TddExample.Core
{
    public class Calculator
    {
        public int Memory { get; set; }

        public object Add(int v1, int v2)
        {
            return v1 + v2;
        }

        public void SaveMemory(int val)
        {
            Memory = val;
        }
    }
}
