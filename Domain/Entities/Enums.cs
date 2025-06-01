using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum AssignmentStatus : byte
    {
        Pending = 0,
        Submitted = 1,
        Graded = 2
    }

    public enum QuizStatus : byte
    {
        Pending = 0,
        Completed = 1,
        Graded = 2
    }

    public enum NotificationType : byte
    {
        Assignment = 0,
        Message = 1,
        System = 2,
        Alert = 3
    }
    public enum UserRole : byte
    {
        Student = 1,
        Admin = 2,
        Instructor = 3,
    }
    public enum AcademicRecordStatus : byte
    {
        Provisional = 0,
        Final = 1
    }
    public enum AssessmentType : byte
    {
        Midterm = 0,
        Final = 1,
        Coursework = 2,
        Quiz = 3,
        Project = 4,
        Attendance = 5,
        Participation = 6,
        Other = 7
    }
    public enum QuizAttemptStatus : byte
    {
        NotStarted = 0,
        InProgress = 1,
        Submitted = 2,
        Graded = 3,
        TimeExpired = 4
    }

    public enum QuestionType : byte
    {
        MultipleChoiceSingleAnswer = 0,
    }
    public enum ModelLabel : byte
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3
    }

}
