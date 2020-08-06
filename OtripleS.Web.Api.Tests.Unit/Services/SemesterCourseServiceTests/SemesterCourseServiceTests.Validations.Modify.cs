﻿// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.SemesterCourseServiceTests
{
    public partial class SemesterCourseServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenSemesterCourseIsNullAndLogItAsync()
        {
            //given
            SemesterCourse invalidSemesterCourse = null;
            var nullSemesterCourseException = new NullSemesterCourseException();

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(nullSemesterCourseException);

            //when
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(invalidSemesterCourse);

            //then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
