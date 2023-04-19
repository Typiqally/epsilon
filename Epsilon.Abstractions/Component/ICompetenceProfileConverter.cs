﻿using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQl;
using Epsilon.Canvas.Abstractions.QueryResponse;

namespace Epsilon.Abstractions.Component;

public interface ICompetenceProfileConverter
{
    public CompetenceProfile ConvertFrom(GetAllUserCoursesSubmissionOutcomes getAllUserCoursesSubmissionOutcomes,IHboIDomain domain, IEnumerable<EnrollmentTerm> terms);
}