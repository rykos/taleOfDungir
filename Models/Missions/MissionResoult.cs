namespace taleOfDungir.Models
{
    public class MissionResoult
    {
        public MissionResoult(string state, object value)
        {
            this.State = state;
            this.Value = value;
        }
        /// <summary>
        /// fight, event, finished
        /// </summary>
        public string State { get; set; }
        public object Value { get; set; }
    }
}