﻿using MediatR;
using SocialMedia.Application.Models;

namespace SocialMedia.Application.UserProfile.Commands;
public class UpdateUserCommand : IRequest<OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>>
{
    public Guid UserProfileId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CurrentCity { get; set; }
}
