﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Courses.Exceptions
{
    public class LockedCourseException : Exception
    {
        public LockedCourseException(Exception innerException)
            : base(message: "Locked course record exception, please try again later.", innerException) { }
    }
}
