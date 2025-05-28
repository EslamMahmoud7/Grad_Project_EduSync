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
        Admin = 2
    }

}
