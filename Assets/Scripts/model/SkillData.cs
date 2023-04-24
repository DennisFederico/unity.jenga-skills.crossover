using System;

namespace model {
    
    /// <summary>
    /// Class that represents the mastery level of a specific domain skill.
    /// </summary>

    [System.Serializable]
    public class SkillData {

        public int id;
        public string subject;
        public string grade;
        public int mastery;
        public string domainid;
        public string domain;
        public string cluster;
        public string standardid;
        public string standarddescription;

        public SkillDataKey GetSkillOrderKey() {
            return new SkillDataKey(domain, cluster, standardid, id);
        }

        public override string ToString() {
            return $"{subject} {grade} {domainid} {mastery}";
        }
        
        public struct SkillDataKey : IComparable<SkillDataKey> {
            private string domain;
            private string cluster;
            private string standardid;
            private int id;

            public SkillDataKey(string domain, string cluster, string standardid, int id) {
                this.domain = domain;
                this.cluster = cluster;
                this.standardid = standardid;
                this.id = id;
            }

            public int CompareTo(SkillDataKey other) {
                var domainComparison = string.Compare(domain, other.domain, StringComparison.Ordinal);
                if (domainComparison != 0) return domainComparison;
                var clusterComparison = string.Compare(cluster, other.cluster, StringComparison.Ordinal);
                if (clusterComparison != 0) return clusterComparison;
                var standardidComparison = string.Compare(standardid, other.standardid, StringComparison.Ordinal);
                if (standardidComparison != 0) return standardidComparison;
                return id.CompareTo(other.id);
            }
            
            public override string ToString() {
                return $"{domain} {cluster} {standardid} {id}";
            }
        }
    }
}