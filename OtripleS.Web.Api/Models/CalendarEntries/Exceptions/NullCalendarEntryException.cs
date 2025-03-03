﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CalendarEntries.Exceptions
{
    public class NullCalendarEntryException : Exception
    {
        public NullCalendarEntryException() : base(message: "The calendar entry is null.") { }
    }
}
