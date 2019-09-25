using System;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using SynaptikonFramework.Interfaces.Language;

namespace ExerciseEngine.Model.ValueObjects
{
    public class DomainSettingVO : IDomainSettingVO
    {
        public DomainSettingVO(int domainId, float value)
        {
            DomainId = domainId;
            Value = value;
        }

        public DomainSettingVO(int domainId, float value, ILanguage language)
        {
            DomainId = domainId;
            Value = value;
            Name = language != null ? language.GetString("domain_name_" + domainId) : "";
        }

        public int DomainId { get; }

        public float Value { get; set; }

        public string Name { get; }
    }
}
