using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISubmittedAssignmentService
    {
        Task<SubmittedAssignmentDTO> SubmitAssignmentAsync(SubmitAssignmentDTO dto);
        Task<IReadOnlyList<SubmittedAssignmentDTO>> GetInstructorSubmittedAssignmentsAsync(string instructorId);
        Task<SubmittedAssignmentDTO> GradeSubmittedAssignmentAsync(string submittedAssignmentId, GradeSubmittedAssignmentDTO dto);
        Task<IReadOnlyList<SubmittedAssignmentDTO>> GetStudentSubmittedAssignmentsAsync(string studentId, string assignmentId = null);
        Task<SubmittedAssignmentDTO> GetSubmittedAssignmentByIdAsync(string id);
    }

}
