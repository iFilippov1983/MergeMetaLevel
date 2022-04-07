using System;
using Data;

namespace Features._Events
{
    public class DataEvents
    {
        public Action<ResourceType, int, int> OnResourceChanged; 
    }
}