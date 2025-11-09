using Identity.Application.DTOs;
using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Identity.Application.Queries.GetUserById;

/// <summary>
/// Query to get user by ID
/// </summary>
public class GetUserByIdQuery : IRequest<Result<UserDto>>
{
    public Guid UserId { get; set; }

    public GetUserByIdQuery(Guid userId)
    {
        UserId = userId;
    }
}
