using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class QuizService : IQuizService
    {
        private readonly MainDbContext _db;

        public QuizService(MainDbContext db)
        {
            _db = db;
        }

        private QuizDTO MapQuizToDTO(Quiz quiz, bool includeModels = true)
        {
            var dto = new QuizDTO
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                GroupId = quiz.GroupId,
                GroupLabel = quiz.Group?.Label ?? "N/A",
                InstructorId = quiz.InstructorId,
                InstructorName = $"{quiz.Instructor?.FirstName} {quiz.Instructor?.LastName}" ?? "N/A",
                DueDate = quiz.DueDate,
                DurationMinutes = quiz.DurationMinutes,
                ShuffleQuestions = quiz.ShuffleQuestions,
                MaxAttempts = quiz.MaxAttempts,
                IsPublished = quiz.IsPublished,
                DateCreated = quiz.DateCreated,
                DateModified = quiz.DateModified,
                QuizModels = includeModels && quiz.QuizModels != null
                    ? quiz.QuizModels.Select(qm => MapQuizModelToDTO(qm, false)).ToList()
                    : new List<QuizModelDTO>()
            };
            return dto;
        }

        private QuizModelDTO MapQuizModelToDTO(QuizModel quizModel, bool includeQuestions = false)
        {
            return new QuizModelDTO
            {
                Id = quizModel.Id,
                QuizId = quizModel.QuizId,
                ModelIdentifier = quizModel.ModelIdentifier,
                DateCreated = quizModel.DateCreated,
                Questions = includeQuestions && quizModel.Questions != null
                    ? quizModel.Questions.Select(q => MapQuestionToDTO(q, true)).ToList()
                    : new List<QuestionDTO>()
            };
        }

        // This is a new helper DTO required for CSV parsing, you can place it at the bottom of the file or in its own file.
        private class ParsedQuestionDTO
        {
            public string QuestionText { get; set; } = string.Empty;
            public List<string> Options { get; set; } = new List<string>();
            public int CorrectOptionIndex { get; set; }
            public int Points { get; set; }
        }


        private QuestionDTO MapQuestionToDTO(Question question, bool includeCorrectnessInfoForInstructor = false, bool forStudentAttempt = false)
        {
            return new QuestionDTO
            {
                Id = question.Id,
                Text = question.Text,
                Points = question.Points,
                Type = question.Type,
                Options = question.Options?.Select(o => new QuestionOptionDTO
                {
                    Id = o.Id,
                    Text = o.Text,
                    IsCorrect = forStudentAttempt ? (bool?)null : (includeCorrectnessInfoForInstructor ? o.IsCorrect : (bool?)null)
                }).ToList() ?? new List<QuestionOptionDTO>()
            };
        }

        public async Task<QuizDTO> CreateQuizAsync(CreateQuizDTO dto, string instructorId)
        {
            var group = await _db.Groups.FindAsync(dto.GroupId);
            if (group == null) throw new ArgumentException("Group not found.");

            var instructorExists = await _db.Users.AnyAsync(u => u.Id == instructorId);
            if (!instructorExists) throw new ArgumentException("Instructor not found.");


            var quiz = new Quiz
            {
                Title = dto.Title,
                Description = dto.Description,
                GroupId = dto.GroupId,
                InstructorId = instructorId,
                DueDate = dto.DueDate,
                DurationMinutes = dto.DurationMinutes,
                ShuffleQuestions = dto.ShuffleQuestions,
                MaxAttempts = dto.MaxAttempts,
                IsPublished = dto.IsPublished,
                DateCreated = DateTime.UtcNow
            };

            _db.Quizzes.Add(quiz);
            await _db.SaveChangesAsync();

            await _db.Entry(quiz).Reference(q => q.Group).LoadAsync();
            await _db.Entry(quiz).Reference(q => q.Instructor).LoadAsync();

            return MapQuizToDTO(quiz);
        }

        public async Task<QuizDTO> UpdateQuizAsync(string quizId, UpdateQuizDTO dto, string requestingInstructorId)
        {
            var quiz = await _db.Quizzes
                .Include(q => q.Group)
                .Include(q => q.Instructor)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null) throw new ArgumentException("Quiz not found.");

            if (quiz.InstructorId != requestingInstructorId)
            {
                throw new ArgumentException($"Quiz does not belong to instructor {requestingInstructorId}. It belongs to {quiz.InstructorId}");
            }


            if (dto.Title != null) quiz.Title = dto.Title;
            if (dto.Description != null) quiz.Description = dto.Description;
            if (dto.DueDate.HasValue) quiz.DueDate = dto.DueDate.Value;
            if (dto.DurationMinutes.HasValue) quiz.DurationMinutes = dto.DurationMinutes.Value;
            if (dto.ShuffleQuestions.HasValue) quiz.ShuffleQuestions = dto.ShuffleQuestions.Value;
            if (dto.MaxAttempts.HasValue) quiz.MaxAttempts = dto.MaxAttempts.Value;
            if (dto.IsPublished.HasValue) quiz.IsPublished = dto.IsPublished.Value;
            quiz.DateModified = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return MapQuizToDTO(quiz);
        }

        public async Task<QuizModelDTO> AddQuizModelAsync(UploadQuizModelCsvDTO dto, string instructorId)
        {
            var quiz = await _db.Quizzes.Include(q => q.QuizModels)
                                         .FirstOrDefaultAsync(q => q.Id == dto.QuizId);
            if (quiz == null) throw new ArgumentException("Quiz not found.");

            if (quiz.InstructorId != instructorId)
            {
                throw new ArgumentException($"Quiz does not belong to instructor {instructorId}.");
            }
            if (quiz.QuizModels.Count >= 4) throw new InvalidOperationException("A quiz cannot have more than 4 models.");
            if (quiz.QuizModels.Any(qm => qm.ModelIdentifier.Equals(dto.ModelIdentifier, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"Model with identifier '{dto.ModelIdentifier}' already exists for this quiz.");
            }

            var parsedQuestions = new List<ParsedQuestionDTO>();
            try
            {
                using (var reader = new StreamReader(dto.CsvFile.OpenReadStream()))
                {
                    string? line;
                    bool isFirstLine = true;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (isFirstLine) { isFirstLine = false; continue; }
                        var values = line.Split(',');
                        if (values.Length < 7) continue;
                        parsedQuestions.Add(new ParsedQuestionDTO
                        {
                            QuestionText = values[0].Trim(),
                            Options = new List<string> { values[1].Trim(), values[2].Trim(), values[3].Trim(), values[4].Trim() },
                            CorrectOptionIndex = int.TryParse(values[5].Trim(), out int idx) ? idx : -1,
                            Points = int.TryParse(values[6].Trim(), out int pts) ? pts : 1
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error parsing CSV file: {ex.Message}");
            }

            if (!parsedQuestions.Any()) throw new ArgumentException("CSV file contains no valid questions or is improperly formatted.");
            if (parsedQuestions.Any(pq => pq.CorrectOptionIndex < 0 || pq.CorrectOptionIndex >= pq.Options.Count))
            {
                throw new ArgumentException("CSV contains one or more questions with an invalid CorrectOptionIndex.");
            }

            var quizModel = new QuizModel
            {
                QuizId = quiz.Id,
                ModelIdentifier = dto.ModelIdentifier,
                DateCreated = DateTime.UtcNow
            };
            _db.QuizModels.Add(quizModel);
            await _db.SaveChangesAsync();

            foreach (var pq in parsedQuestions)
            {
                var question = new Question
                {
                    QuizModelId = quizModel.Id,
                    Text = pq.QuestionText,
                    Points = pq.Points,
                    Type = QuestionType.MultipleChoiceSingleAnswer
                };
                _db.Questions.Add(question);
                await _db.SaveChangesAsync();

                for (int i = 0; i < pq.Options.Count; i++)
                {
                    var option = new QuestionOption
                    {
                        QuestionId = question.Id,
                        Text = pq.Options[i],
                        IsCorrect = (i == pq.CorrectOptionIndex)
                    };
                    _db.QuestionOptions.Add(option);
                }
            }
            await _db.SaveChangesAsync();

            var fullQuizModel = await _db.QuizModels
                .Include(qm => qm.Questions)
                    .ThenInclude(q => q.Options)
                .AsNoTracking()
                .FirstOrDefaultAsync(qm => qm.Id == quizModel.Id);

            return MapQuizModelToDTO(fullQuizModel!, true);
        }

        public async Task DeleteQuizAsync(string quizId, string instructorId)
        {
            var quiz = await _db.Quizzes.FirstOrDefaultAsync(q => q.Id == quizId);
            if (quiz == null) throw new ArgumentException("Quiz not found.");
            if (quiz.InstructorId != instructorId)
            {
                throw new ArgumentException($"Quiz does not belong to instructor {instructorId}.");
            }
            _db.Quizzes.Remove(quiz);
            await _db.SaveChangesAsync();
        }

        public async Task<QuizDTO> GetQuizByIdForInstructorAsync(string quizId, string instructorId)
        {
            var quiz = await _db.Quizzes
                .Include(q => q.Group)
                .Include(q => q.Instructor)
                .Include(q => q.QuizModels)
                    .ThenInclude(qm => qm.Questions)
                        .ThenInclude(ques => ques.Options)
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null) throw new ArgumentException("Quiz not found.");

            // Although we check ownership in the controller, an extra check here is good practice.
            if (quiz.InstructorId != instructorId) throw new ArgumentException("Quiz does not belong to the requesting instructor.");

            var quizDto = MapQuizToDTO(quiz, true);
            quizDto.QuizModels.ForEach(qmDto =>
            {
                var modelEntity = quiz.QuizModels.First(m => m.Id == qmDto.Id);
                qmDto.Questions = modelEntity.Questions.Select(q => MapQuestionToDTO(q, true)).ToList();
            });

            return quizDto;
        }

        public async Task<IReadOnlyList<QuizDTO>> GetQuizzesByGroupIdForInstructorAsync(string groupId, string instructorId)
        {
            var quizzes = await _db.Quizzes
                .Where(q => q.GroupId == groupId && q.InstructorId == instructorId)
                .Include(q => q.Group)
                .Include(q => q.Instructor)
                .Include(q => q.QuizModels)
                .AsNoTracking()
                .OrderByDescending(q => q.DateCreated)
                .ToListAsync();
            return quizzes.Select(q => MapQuizToDTO(q, true)).ToList();
        }

        public async Task<IReadOnlyList<QuizDTO>> GetQuizzesByInstructorAsync(string instructorId)
        {
            var quizzes = await _db.Quizzes
                .Where(q => q.InstructorId == instructorId)
                .Include(q => q.Group)
                .Include(q => q.Instructor)
                .Include(q => q.QuizModels)
                .AsNoTracking()
                .OrderByDescending(q => q.DateCreated)
                .ToListAsync();
            return quizzes.Select(q => MapQuizToDTO(q, true)).ToList();
        }

        public async Task<IReadOnlyList<StudentQuizListItemDTO>> GetAvailableQuizzesForStudentAsync(string studentId)
        {
            var studentGroupIds = await _db.GroupStudents
                .Where(gs => gs.StudentId == studentId)
                .Select(gs => gs.GroupId)
                .ToListAsync();

            if (!studentGroupIds.Any()) return new List<StudentQuizListItemDTO>();

            var quizzes = await _db.Quizzes
                .Where(q => studentGroupIds.Contains(q.GroupId) && q.IsPublished && q.QuizModels.Any())
                .Include(q => q.Group).ThenInclude(g => g.Course)
                .Include(q => q.Attempts.Where(a => a.StudentId == studentId))
                .AsNoTracking()
                .OrderByDescending(q => q.DueDate)
                .ToListAsync();

            return quizzes.Select(q => new StudentQuizListItemDTO
            {
                QuizId = q.Id,
                Title = q.Title,
                Description = q.Description,
                CourseTitle = q.Group?.Course?.Title ?? "N/A",
                GroupLabel = q.Group?.Label ?? "N/A",
                DueDate = q.DueDate,
                DurationMinutes = q.DurationMinutes,
                MaxAttempts = q.MaxAttempts,
                AttemptsMade = q.Attempts.Count(a => a.Status == QuizAttemptStatus.Submitted || a.Status == QuizAttemptStatus.Graded || a.Status == QuizAttemptStatus.TimeExpired),
                LastAttemptStatus = q.Attempts.OrderByDescending(a => a.StartTime).FirstOrDefault()?.Status.ToString()
            }).ToList();
        }

        public async Task<StudentQuizAttemptDTO> StartQuizAttemptAsync(string quizId, string studentId)
        {
            var quiz = await _db.Quizzes
                .Include(q => q.QuizModels)
                .Include(q => q.Attempts.Where(a => a.StudentId == studentId))
                .FirstOrDefaultAsync(q => q.Id == quizId && q.IsPublished);

            if (quiz == null) throw new ArgumentException("Quiz not found or not published.");
            if (!quiz.QuizModels.Any()) throw new InvalidOperationException("Quiz has no models to attempt.");

            var completedAttempts = quiz.Attempts.Count(a => a.Status == QuizAttemptStatus.Submitted || a.Status == QuizAttemptStatus.Graded || a.Status == QuizAttemptStatus.TimeExpired);
            if (completedAttempts >= quiz.MaxAttempts)
            {
                throw new InvalidOperationException("You have reached the maximum number of attempts for this quiz.");
            }

            var existingInProgressAttempt = quiz.Attempts
                                                .FirstOrDefault(a => a.Status == QuizAttemptStatus.InProgress);
            if (existingInProgressAttempt != null)
            {
                return await GetStudentQuizAttemptDetailsAsync(existingInProgressAttempt.Id, studentId, quiz.ShuffleQuestions);
            }

            var random = new Random();
            int modelIndex = random.Next(quiz.QuizModels.Count);
            var selectedModel = quiz.QuizModels.ToList()[modelIndex];

            var attempt = new StudentQuizAttempt
            {
                StudentId = studentId,
                QuizId = quiz.Id,
                QuizModelId = selectedModel.Id,
                AttemptNumber = completedAttempts + 1,
                StartTime = DateTime.UtcNow,
                Status = QuizAttemptStatus.InProgress
            };

            _db.StudentQuizAttempts.Add(attempt);
            await _db.SaveChangesAsync();

            return await GetStudentQuizAttemptDetailsAsync(attempt.Id, studentId, quiz.ShuffleQuestions);
        }

        public async Task<StudentQuizAttemptDTO> GetStudentQuizAttemptInProgressAsync(string quizId, string studentId)
        {
            var attempt = await _db.StudentQuizAttempts
                .Include(a => a.Quiz)
                .Where(a => a.QuizId == quizId && a.StudentId == studentId && a.Status == QuizAttemptStatus.InProgress)
                .OrderByDescending(a => a.StartTime)
                .FirstOrDefaultAsync();

            if (attempt == null) throw new ArgumentException("No in-progress attempt found for this quiz.");

            var expectedEndTime = attempt.StartTime.AddMinutes(attempt.Quiz.DurationMinutes);
            if (DateTime.UtcNow > expectedEndTime)
            {
                await AutoSubmitIfTimeExpired(attempt.Id);
                throw new InvalidOperationException("Quiz time has expired. The attempt has been auto-submitted.");
            }

            return await GetStudentQuizAttemptDetailsAsync(attempt.Id, studentId, attempt.Quiz.ShuffleQuestions);
        }

        private async Task<StudentQuizAttemptDTO> GetStudentQuizAttemptDetailsAsync(string attemptId, string studentId, bool shuffleQuestions)
        {
            var attempt = await _db.StudentQuizAttempts
                .Include(a => a.Quiz)
                .Include(a => a.QuizModel)
                    .ThenInclude(qm => qm.Questions)
                        .ThenInclude(q => q.Options)
                .Include(a => a.Answers)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == attemptId && a.StudentId == studentId);

            if (attempt == null) throw new ArgumentException("Attempt not found.");

            var questionsForModel = attempt.QuizModel.Questions.ToList();
            if (shuffleQuestions)
            {
                var random = new Random();
                questionsForModel = questionsForModel.OrderBy(q => random.Next()).ToList();
            }

            var questionDTOs = questionsForModel.Select(q => MapQuestionToDTO(q, false, true)).ToList();

            return new StudentQuizAttemptDTO
            {
                AttemptId = attempt.Id,
                QuizId = attempt.QuizId,
                QuizModelId = attempt.QuizModelId,
                QuizTitle = attempt.Quiz.Title,
                StartTime = attempt.StartTime,
                DurationMinutes = attempt.Quiz.DurationMinutes,
                ShuffleQuestions = shuffleQuestions,
                Questions = questionDTOs,
                Status = attempt.Status
            };
        }

        public async Task<QuizAttemptResultDTO> SubmitQuizAttemptAsync(StudentQuizSubmissionDTO submissionDto, string studentId)
        {
            var attempt = await _db.StudentQuizAttempts
                .Include(a => a.Quiz)
                .Include(a => a.Answers)
                .FirstOrDefaultAsync(a => a.Id == submissionDto.AttemptId && a.StudentId == studentId);

            if (attempt == null) throw new ArgumentException("Attempt not found.");
            if (attempt.StudentId != studentId) throw new ArgumentException("Submission studentId does not match attempt's studentId.");
            if (attempt.Status != QuizAttemptStatus.InProgress) throw new InvalidOperationException("This attempt cannot be submitted.");

            var expectedEndTime = attempt.StartTime.AddMinutes(attempt.Quiz.DurationMinutes);
            if (DateTime.UtcNow > expectedEndTime && attempt.Status == QuizAttemptStatus.InProgress)
            {
                attempt.Status = QuizAttemptStatus.TimeExpired;
            }
            else
            {
                attempt.Status = QuizAttemptStatus.Submitted;
            }
            attempt.EndTime = DateTime.UtcNow;

            _db.StudentQuizAnswers.RemoveRange(attempt.Answers);
            await _db.SaveChangesAsync();

            if (attempt.Status == QuizAttemptStatus.Submitted)
            {
                foreach (var submittedAnswer in submissionDto.Answers)
                {
                    _db.StudentQuizAnswers.Add(new StudentQuizAnswer
                    {
                        StudentQuizAttemptId = attempt.Id,
                        QuestionId = submittedAnswer.QuestionId,
                        SelectedOptionId = submittedAnswer.SelectedOptionId
                    });
                }
            }

            await _db.SaveChangesAsync();
            await AutoGradeAttemptAsync(attempt.Id);

            return await GetStudentQuizAttemptResultAsync(attempt.Id, studentId);
        }

        private async Task AutoGradeAttemptAsync(string attemptId)
        {
            var attempt = await _db.StudentQuizAttempts
                .Include(a => a.Answers)
                    .ThenInclude(ans => ans.Question)
                        .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(a => a.Id == attemptId);

            if (attempt == null) return;

            double totalScore = 0;
            foreach (var answer in attempt.Answers)
            {
                var correctOption = answer.Question.Options.FirstOrDefault(o => o.IsCorrect);
                answer.IsCorrect = (correctOption != null && answer.SelectedOptionId == correctOption.Id);
                answer.PointsAwarded = answer.IsCorrect == true ? answer.Question.Points : 0;
                if (answer.PointsAwarded.HasValue)
                {
                    totalScore += answer.PointsAwarded.Value;
                }
            }

            attempt.Score = totalScore;
            if (attempt.Status != QuizAttemptStatus.TimeExpired)
            {
                attempt.Status = QuizAttemptStatus.Graded;
            }

            await _db.SaveChangesAsync();
        }

        private async Task AutoSubmitIfTimeExpired(string attemptId)
        {
            var attempt = await _db.StudentQuizAttempts
                .Include(a => a.Quiz)
                .FirstOrDefaultAsync(a => a.Id == attemptId);

            if (attempt == null || attempt.Status != QuizAttemptStatus.InProgress) return;

            var expectedEndTime = attempt.StartTime.AddMinutes(attempt.Quiz.DurationMinutes);
            if (DateTime.UtcNow > expectedEndTime)
            {
                attempt.EndTime = expectedEndTime;
                attempt.Status = QuizAttemptStatus.TimeExpired;
                await _db.SaveChangesAsync();
                await AutoGradeAttemptAsync(attempt.Id);
            }
        }

        public async Task<QuizAttemptResultDTO> GetStudentQuizAttemptResultAsync(string attemptId, string studentId)
        {
            var attempt = await _db.StudentQuizAttempts
                .Include(a => a.Student)
                .Include(a => a.Quiz)
                .Include(a => a.QuizModel)
                    .ThenInclude(qm => qm.Questions)
                .Include(a => a.Answers)
                    .ThenInclude(ans => ans.Question)
                        .ThenInclude(q => q.Options)
                .Include(a => a.Answers)
                    .ThenInclude(ans => ans.SelectedOption)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == attemptId && a.StudentId == studentId);

            if (attempt == null) throw new ArgumentException("Attempt result not found.");
            if (attempt.StudentId != studentId) throw new ArgumentException("Attempt does not belong to specified student.");


            int totalPointsPossible = attempt.QuizModel.Questions.Sum(q => q.Points);

            return new QuizAttemptResultDTO
            {
                AttemptId = attempt.Id,
                QuizId = attempt.QuizId,
                QuizTitle = attempt.Quiz.Title,
                StudentId = attempt.StudentId,
                StudentName = $"{attempt.Student?.FirstName} {attempt.Student?.LastName}",
                StartTime = attempt.StartTime,
                EndTime = attempt.EndTime,
                Score = attempt.Score,
                TotalPointsPossible = totalPointsPossible,
                Status = attempt.Status,
                AnswerResults = attempt.Answers.Select(ans =>
                {
                    var questionEntity = ans.Question;
                    var correctOption = questionEntity.Options.FirstOrDefault(o => o.IsCorrect);
                    return new StudentAnswerResultDTO
                    {
                        QuestionId = ans.QuestionId,
                        QuestionText = questionEntity.Text,
                        SelectedOptionId = ans.SelectedOptionId,
                        SelectedOptionText = ans.SelectedOption?.Text,
                        CorrectOptionId = correctOption?.Id,
                        CorrectOptionText = correctOption?.Text,
                        IsCorrect = ans.IsCorrect ?? false,
                        PointsAwarded = ans.PointsAwarded ?? 0,
                        PointsPossibleForQuestion = questionEntity.Points
                    };
                }).ToList()
            };
        }

        public async Task<QuizAttemptResultDTO> GetStudentAttemptDetailsForInstructorAsync(string attemptId, string instructorId)
        {
            var attempt = await _db.StudentQuizAttempts
                .Include(a => a.Student)
                .Include(a => a.Quiz)
                .Include(a => a.QuizModel)
                    .ThenInclude(qm => qm.Questions)
                .Include(a => a.Answers)
                    .ThenInclude(ans => ans.Question)
                        .ThenInclude(q => q.Options)
                .Include(a => a.Answers)
                    .ThenInclude(ans => ans.SelectedOption)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == attemptId);

            if (attempt == null) throw new ArgumentException("Attempt not found.");
            if (attempt.Quiz.InstructorId != instructorId)
            {
                throw new ArgumentException($"Quiz for this attempt does not belong to instructor {instructorId}.");
            }
            return await GetStudentQuizAttemptResultAsync(attemptId, attempt.StudentId);
        }

        public async Task<IReadOnlyList<QuizAttemptResultDTO>> GetAllAttemptsForQuizAsync(string quizId, string instructorId)
        {
            var quiz = await _db.Quizzes.FindAsync(quizId);
            if (quiz == null) throw new ArgumentException("Quiz not found.");
            if (quiz.InstructorId != instructorId)
            {
                throw new ArgumentException($"Quiz does not belong to instructor {instructorId}.");
            }

            var attempts = await _db.StudentQuizAttempts
                .Where(a => a.QuizId == quizId)
                .Include(a => a.Student)
                .Include(a => a.Quiz)
                .Include(a => a.QuizModel).ThenInclude(qm => qm.Questions)
                .Include(a => a.Answers).ThenInclude(ans => ans.Question).ThenInclude(q => q.Options)
                .Include(a => a.Answers).ThenInclude(ans => ans.SelectedOption)
                .AsNoTracking()
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();

            var results = new List<QuizAttemptResultDTO>();
            foreach (var attempt in attempts)
            {
                int totalPointsPossible = attempt.QuizModel.Questions.Sum(q => q.Points);
                results.Add(new QuizAttemptResultDTO
                {
                    AttemptId = attempt.Id,
                    QuizId = attempt.QuizId,
                    QuizTitle = attempt.Quiz.Title,
                    StudentId = attempt.StudentId,
                    StudentName = $"{attempt.Student?.FirstName} {attempt.Student?.LastName}",
                    StartTime = attempt.StartTime,
                    EndTime = attempt.EndTime,
                    Score = attempt.Score,
                    TotalPointsPossible = totalPointsPossible,
                    Status = attempt.Status,
                    AnswerResults = attempt.Answers.Select(ans =>
                    {
                        var questionEntity = ans.Question;
                        var correctOption = questionEntity.Options.FirstOrDefault(o => o.IsCorrect);
                        return new StudentAnswerResultDTO
                        {
                            QuestionId = ans.QuestionId,
                            QuestionText = questionEntity.Text,
                            SelectedOptionId = ans.SelectedOptionId,
                            SelectedOptionText = ans.SelectedOption?.Text,
                            CorrectOptionId = correctOption?.Id,
                            CorrectOptionText = correctOption?.Text,
                            IsCorrect = ans.IsCorrect ?? false,
                            PointsAwarded = ans.PointsAwarded ?? 0,
                            PointsPossibleForQuestion = questionEntity.Points
                        };
                    }).ToList()
                });
            }
            return results;
        }
    }
}