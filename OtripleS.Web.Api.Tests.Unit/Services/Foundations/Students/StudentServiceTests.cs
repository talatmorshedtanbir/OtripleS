﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Services.Foundations.Students;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Students
{
    public partial class StudentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IStudentService studentService;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;

        public StudentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.studentService = new StudentService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static Student CreateRandomStudent() =>
            CreateStudentFiller(dates: DateTimeOffset.UtcNow).Create();

        private static Student CreateRandomStudent(DateTimeOffset dates) =>
            CreateStudentFiller(dates).Create();

        public static TheoryData InvalidMinuteCases()
        {
            int randomMoreThanMinuteFromNow = GetRandomNumber();
            int randomMoreThanMinuteBeforeNow = GetNegativeRandomNumber();

            return new TheoryData<int>
            {
                randomMoreThanMinuteFromNow ,
                randomMoreThanMinuteBeforeNow
            };
        }

        private static Expression<Func<Exception, bool>> SameValidationExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message
                && (actualException.InnerException as Xeption).DataEquals(expectedException.InnerException.Data);
        }

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                expectedException.Message == actualException.Message
                && expectedException.InnerException.Message == actualException.InnerException.Message;
        }

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static IQueryable<Student> CreateRandomStudents(DateTimeOffset dates) =>
            CreateStudentFiller(dates).Create(GetRandomNumber()).AsQueryable();

        private static Filler<Student> CreateStudentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Student>();
            Guid createdById = Guid.NewGuid();

            filler.Setup()
                .OnProperty(student => student.BirthDate).Use(GetRandomDateTime())
                .OnProperty(student => student.CreatedDate).Use(dates)
                .OnProperty(student => student.UpdatedDate).Use(dates)
                .OnProperty(student => student.CreatedBy).Use(createdById)
                .OnProperty(student => student.UpdatedBy).Use(createdById)
                .OnProperty(student => student.StudentSemesterCourses).IgnoreIt()
                .OnProperty(student => student.StudentGuardians).IgnoreIt()
                .OnProperty(student => student.StudentContacts).IgnoreIt()
                .OnProperty(student => student.StudentExams).IgnoreIt()
                .OnProperty(student => student.StudentAttachments).IgnoreIt()
                .OnProperty(student => student.StudentExamFees).IgnoreIt()
                .OnProperty(student => student.StudentRegistrations).IgnoreIt();

            return filler;
        }
    }
}
