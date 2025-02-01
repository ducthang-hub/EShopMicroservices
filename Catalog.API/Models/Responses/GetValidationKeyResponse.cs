using System.Net;
using BuildingBlocks.Contracts;
using Catalog.API.DTOs;

namespace Catalog.API.Models.Responses;

public class GetValidationKeyResponse : ErrorResponse
{
    public AuthTokenDto Tokens { get; set; }
}