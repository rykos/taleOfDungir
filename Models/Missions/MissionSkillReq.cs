using System;

namespace taleOfDungir.Models
{
    [Serializable]
    public struct MissionSkillReq
    {
        public Int64 EventId;
        /// <summary>
        /// Minimum required value
        /// </summary>
        public int Value;

    }
}