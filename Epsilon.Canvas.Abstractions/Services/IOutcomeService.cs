﻿using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions.Services;

public interface IOutcomeService
{
    Task<Outcome?> Find(int id);
    Task<IEnumerable<OutcomeResult>?> AllResults(int courseId, int count = 1000);
}