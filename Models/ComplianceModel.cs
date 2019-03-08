

using SQLite;
using System;
using System.Collections.Generic;

namespace TaskAppWithLogin.Models
{
    public class ComplianceModel
    {
        [PrimaryKey, AutoIncrement]
        public Int64 id { get; set; }
        public string TaskPercentage { get; set; }
        public string UserId { get; set; }
        public string task_id { get; set; }
        public string task_name { get; set; }
        public string description { get; set; }
        public string deadline_date { get; set; }
        public string markTo { get; set; }
        public string taskStatus { get; set; }
        public string task_created_by { get; set; }
        public string task_creation_date { get; set; }
        public string shapes { get; set; }
        [Ignore]
        public List<ComplianceJoinTable> lstAddedCompliance { get; set; }
        public string task_mark_by { get; set; }
        public string MarkingDate { get; set; }
        public string task_marking_type { get; set; }
        [Ignore]
        public List<CommunicationModel> lstCommunication { get; set; }
        [Ignore]
        public List<TaskFilemappingModel2> lstTaskFileMapping { get; set; }
        public string RowNo { get; set; }
        public string Meeting_ID { get; set; }
        public string TaskThrough { get; set; }
        public string TaskTemplate_ID { get; set; }
        public string status { get; set; }
        [Ignore]
        public List<Comp_AttachmentModel> lstUploadedCompliance { get; set; }
        //[Ignore]
        //public List<ComplianceShapes> shapesjj { get; set; }
        public string GeoLocation { get; set; }
    }
}