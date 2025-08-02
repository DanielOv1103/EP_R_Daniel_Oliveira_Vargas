// DTOs/RegisterDto.cs
using backend.EP_R_Daniel_Oliveira_Vargas.Models;


namespace backend.EP_R_Daniel_Oliveira_Vargas.DTOs
{
    public record RegisterDto(
        string Name,
        string Email,
        string Password,
        Role Role = Role.User
    );
}

// DTOs/LoginDto.cs
namespace backend.EP_R_Daniel_Oliveira_Vargas.DTOs
{
    public record LoginDto(string Email, string Password);
}

// DTOs/CreatePollDto.cs
namespace backend.EP_R_Daniel_Oliveira_Vargas.DTOs
{
    public record CreatePollDto(string Title, string Description);
}

// DTOs/UpdatePollDto.cs
namespace backend.EP_R_Daniel_Oliveira_Vargas.DTOs
{
    public record UpdatePollDto(string Title, string Description, bool IsActive);
}

// DTOs/CreateOptionDto.cs
namespace backend.EP_R_Daniel_Oliveira_Vargas.DTOs
{
    public record CreateOptionDto(
        int VoteId,
        string Text
    );
}

// DTOs/CreateVoteDto.cs
namespace backend.EP_R_Daniel_Oliveira_Vargas.DTOs
{
    public record CreateVoteDto(int PollId, int OptionId);
}

// DTOs/UpdateUserDto.cs
namespace backend.EP_R_Daniel_Oliveira_Vargas.DTOs
{
    public record UpdateUserDto(string Name, string Email, Role Role, Status Status, string? Password = null);
}
