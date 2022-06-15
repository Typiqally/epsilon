﻿using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions.Service;

public interface ISubmissionService
{
    public Task<IEnumerable<Submission>> GetAllFromStudent(int courseId, IEnumerable<string> include, int limit = 100);
}