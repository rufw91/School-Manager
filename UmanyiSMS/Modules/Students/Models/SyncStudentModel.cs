﻿using System.Collections.Generic;
using UmanyiSMS.Modules.Exams.Sync.Models;
using UmanyiSMS.Modules.Fees.Sync.Models;

namespace UmanyiSMS.Modules.Students.Sync.Model
{
    public class SyncStudentModel
    {
        public string SchoolID { get; set; }

        public string StudentID { get; set; }

        public string NameOfStudent { get; set; }

        public string Address { get; set; }

        public string NameOfGuardian { get; set; }

        public string GuardianPhoneNo { get; set; }

        public string City { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public byte[] SPhoto { get; set; }

        public string Email { get; set; }

        public string PostalCode { get; set; }

        public string PrevInstitution { get; set; }

        public string BedNo { get; set; }

        public string NameOfClass { get; set; }

        public string DateOfAdmission { get; set; }

        public string DateOfBirth { get; set; }

        public string NameOfDormitory { get; set; }

        public string PrevBalance { get; set; }

        public bool IsActive { get; set; }
        
        public string Gender { get; set; }

        public string KCPEScore { get; set; }

        public SyncTranscriptModel Transcript
        { get; set; }

        public SyncStatementModel Statement
        { get; set; }

        public List<SyncExamResultModel> ExamResults
        { get; set; }
    }
}
