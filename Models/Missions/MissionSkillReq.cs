using System;
using System.Collections.Generic;

namespace taleOfDungir.Models
{
    [Serializable]
    public struct MissionSkillReq
    {
        public Int64 EventId { get; set; }
        // /// <summary>
        // /// Minimum required value
        // /// </summary>
        // public int[] Value { get; set; }

        public Dictionary<Int64, int> EventActionIdToValue { get; set; }
    }
}