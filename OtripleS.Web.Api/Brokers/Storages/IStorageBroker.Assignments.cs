﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Assignments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Assignment> InsertAssignmentAsync(Assignment assignment);
        IQueryable<Assignment> SelectAllAssignments();
        ValueTask<Assignment> SelectAssignmentByIdAsync(Guid assignmentId);
        ValueTask<Assignment> UpdateAssignmentAsync(Assignment assignment);
        ValueTask<Assignment> DeleteAssignmentAsync(Assignment assignment);
    }
}
