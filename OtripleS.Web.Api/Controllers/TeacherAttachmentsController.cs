﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.TeacherAttachments;
using OtripleS.Web.Api.Models.TeacherAttachments.Exceptions;
using OtripleS.Web.Api.Services.TeacherAttachments;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAttachmentsController : RESTFulController
    {
        private readonly ITeacherAttachmentService teacherAttachmentService;

        public TeacherAttachmentsController(ITeacherAttachmentService teacherAttachmentService) =>
            this.teacherAttachmentService = teacherAttachmentService;

        [HttpPost]
        public async ValueTask<ActionResult<TeacherAttachment>> PostTeacherAttachmentAsync(
            TeacherAttachment teacherAttachment)
        {
            try
            {
                TeacherAttachment persistedTeacherAttachment =
                    await this.teacherAttachmentService.AddTeacherAttachmentAsync(teacherAttachment);

                return Ok(persistedTeacherAttachment);
            }
            catch (TeacherAttachmentValidationException teacherAttachmentValidationException)
                when (teacherAttachmentValidationException.InnerException is AlreadyExistsTeacherAttachmentException)
            {
                string innerMessage = GetInnerMessage(teacherAttachmentValidationException);

                return Conflict(innerMessage);
            }
            catch (TeacherAttachmentValidationException teacherAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(teacherAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (TeacherAttachmentDependencyException teacherAttachmentDependencyException)
            {
                return Problem(teacherAttachmentDependencyException.Message);
            }
            catch (TeacherAttachmentServiceException teacherAttachmentServiceException)
            {
                return Problem(teacherAttachmentServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<TeacherAttachment>> GetAllTeacherAttachments()
        {
            try
            {
                IQueryable storageTeacherAttachments =
                    this.teacherAttachmentService.RetrieveAllTeacherAttachments();

                return Ok(storageTeacherAttachments);
            }
            catch (TeacherAttachmentDependencyException teacherAttachmentDependencyException)
            {
                return Problem(teacherAttachmentDependencyException.Message);
            }
            catch (TeacherAttachmentServiceException teacherAttachmentServiceException)
            {
                return Problem(teacherAttachmentServiceException.Message);
            }
        }

        [HttpGet("teachers/{teacherId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<TeacherAttachment>> GetTeacherAttachmentAsync(
            Guid teacherId,
            Guid attachmentId)
        {
            try
            {
                TeacherAttachment storageTeacherAttachment =
                    await this.teacherAttachmentService.RetrieveTeacherAttachmentByIdAsync(teacherId, attachmentId);

                return Ok(storageTeacherAttachment);
            }
            catch (TeacherAttachmentValidationException semesterTeacherAttachmentValidationException)
                when (semesterTeacherAttachmentValidationException.InnerException is NotFoundTeacherAttachmentException)
            {
                string innerMessage = GetInnerMessage(semesterTeacherAttachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (TeacherAttachmentValidationException semesterTeacherAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(semesterTeacherAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (TeacherAttachmentDependencyException semesterTeacherAttachmentDependencyException)
            {
                return Problem(semesterTeacherAttachmentDependencyException.Message);
            }
            catch (TeacherAttachmentServiceException semesterTeacherAttachmentServiceException)
            {
                return Problem(semesterTeacherAttachmentServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
