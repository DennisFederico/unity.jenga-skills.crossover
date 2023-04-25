using System;

namespace model {
    
    /// <summary>
    /// Class that represents the mastery level of a specific domain skill.
    /// </summary>

    [Serializable]
    public class SkillData {

        // ReSharper disable IdentifierTypo
        public int id;
        public string subject;
        public string grade;
        public int mastery;
        public string domainid;
        public string domain;
        public string cluster;
        public string standardid;
        public string standarddescription;
        // ReSharper restore IdentifierTypo

        public SkillDataKey GetSkillOrderKey() {
            return new SkillDataKey(domain, cluster, standardid, id);
        }

        public override string ToString() {
            return $"<b><color=\"green\">{grade}: {domain}</color></b>\n" +
                   $"<b><i>{cluster}</i></b>\n" +
                   $"{standardid} - {standarddescription}";
        }
        
        public struct SkillDataKey : IComparable<SkillDataKey> {
            private readonly string _domain;
            private readonly string _cluster;
            private readonly string _standardId;
            private readonly int _id;

            public SkillDataKey(string domain, string cluster, string standardId, int id) {
                _domain = domain;
                _cluster = cluster;
                _standardId = standardId;
                _id = id;
            }

            public int CompareTo(SkillDataKey other) {
                var domainComparison = string.Compare(_domain, other._domain, StringComparison.Ordinal);
                if (domainComparison != 0) return domainComparison;
                var clusterComparison = string.Compare(_cluster, other._cluster, StringComparison.Ordinal);
                if (clusterComparison != 0) return clusterComparison;
                var standardIdComparison = string.Compare(_standardId, other._standardId, StringComparison.Ordinal);
                if (standardIdComparison != 0) return standardIdComparison;
                return _id.CompareTo(other._id);
            }
        }
    }
}