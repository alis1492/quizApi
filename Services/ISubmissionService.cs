using quizApi.Models;
using quizApi.DTOs;

namespace quizApi.Services;

public interface ISubmissionService
{
    // public Task<List<SubmissionDTO>> GetSubmissionzes(int? limit = null, int? skip = null);
    // public Task<List<Submission>> GetFullSubmissionzes(int? limit = null, int? skip = null);

    // // Get a single quiz by id
    // public Task<SubmissionDTO> GetSubmissionById(ObjectId id);
    // public Task<Submission> GetFullSubmissionById(ObjectId id);
    /// <summary>
    /// Create a new submission with status InProgress
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="DocumentNotFoundException">when QuizId is invalid</exception>
    public Task<Submission> CreateSubmission(CreateSubmissionDTO dto);
    // public Task<Submission> UpdateSubmission(ObjectId Id, UpdateSubmissionDTO dto);
    // public Task DeleteSubmission(ObjectId Id);
}