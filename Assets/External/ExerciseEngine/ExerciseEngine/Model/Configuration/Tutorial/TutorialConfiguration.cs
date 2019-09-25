using System;
using ExerciseEngine.Model.Tutorial.Interfaces;
using Newtonsoft.Json;

namespace ExerciseEngine.Model.Tutorial
{
    public class TutorialConfiguration : ITutorialConfiguration
    {
        public Type TriggerType { get { return Type.GetType(TriggerClassName); } }
        public Type HandlerType { get { return Type.GetType(HandlerClassName); } }

        [JsonProperty(PropertyName = "triggerClassName")]
        public string TriggerClassName { get; set; }

        [JsonProperty(PropertyName = "handlerClassName")]
        public string HandlerClassName { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public int Priority { get; set; }

        [JsonProperty(PropertyName = "repeats")]
        public int Repeats { get; set; }
    }
}
