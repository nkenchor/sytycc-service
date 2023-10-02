using System.Data;
using Sytycc_Service.Domain;
using Serilog;

namespace Sytycc_Service.Api;

public class FacilitatorService : IFacilitatorService
{
    private readonly IFacilitatorRepository _facilitatorRepository;
    private readonly IFacilitatorValidationService _facilitatorValidationService;

    public FacilitatorService(IFacilitatorRepository facilitatorRepository, IFacilitatorValidationService facilitatorValidationService)
    {
        _facilitatorRepository = facilitatorRepository;
        _facilitatorValidationService = facilitatorValidationService;
    }

    public async Task<string> CreateFacilitator(CreateFacilitatorDto facilitatorDto)
    {
        try
        {
            var validationException = _facilitatorValidationService.ValidateCreateFacilitator(facilitatorDto);
            if (validationException != null) throw validationException;

            var availableFacilitator = await _facilitatorRepository.GetFacilitatorByFullName($"{facilitatorDto.LastName}, {facilitatorDto.FirstName}");
            if (availableFacilitator != null)
            {
                Log.Warning($"There is already a facilitator found with the given fullname: {facilitatorDto.LastName}, {facilitatorDto.FirstName}.");
                throw new ConflictException($"there is already a facilitator found with the given fullname: {facilitatorDto.LastName}, {facilitatorDto.FirstName}.");
            }

            var facilitator = new Facilitator(facilitatorDto);
            return await _facilitatorRepository.CreateFacilitator(facilitator);
        }
        catch (BadRequestException e)
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        catch (ConflictException e)
        {
            Log.Error($"Conflict Error: {e.Message}");
            throw;
        }
        catch (AppException e)
        {
            Log.Error($"Database Error: {e.Message}");
            throw;
        }

        // General error (this should be last in the catch sequence)
        catch (Exception e)
        {
            Log.Error($"Error Creating Facilitator: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<string> UpdateFacilitator(string reference, UpdateFacilitatorDto facilitatorDto)
    {
        try
        {
        var validationException = _facilitatorValidationService.ValidateUpdateFacilitator(facilitatorDto);
        if (validationException != null) throw validationException;

        await GetFacilitatorByReference(reference);

        var facilitator = new Facilitator(facilitatorDto)
        {
            Reference = reference
        };

        return await _facilitatorRepository.UpdateFacilitator(reference, facilitator);
        }
        catch (BadRequestException e)
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        catch (AppException e)
        {
            Log.Error($"Database Error: {e.Message}");
            throw;
        }

        // General error (this should be last in the catch sequence)
        catch (Exception e)
        {
            Log.Error($"Error Updating Facilitator: {e.Message}");
            throw new InternalServerException(e.Message);
        }

    }

    public async Task<string> DeleteFacilitator(string reference)
    {
        try
        {
        return await _facilitatorRepository.DeleteFacilitator(reference);
        }
        catch (BadRequestException e)
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        catch (AppException e)
        {
            Log.Error($"Database Error: {e.Message}");
            throw;
        }

        // General error (this should be last in the catch sequence)
        catch (Exception e)
        {
            Log.Error($"Error Deleting Facilitator: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<FacilitatorDto> GetFacilitatorByReference(string reference)
    {
        try
        {
            var facilitator = await _facilitatorRepository.GetFacilitatorByReference(reference) ?? throw new NotFoundException("Facilitator not found by the given reference.");
            return new FacilitatorDto
            {
                Reference = facilitator.Reference,
                FullName = facilitator.FullName,
                Bio = facilitator.Bio,
                Email = facilitator.Email,
                Phone = facilitator.Phone,
   
            };
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching facilitator by reference: {e.Message}");
        }
    }
  public async Task<FacilitatorDto> GetFacilitatorByFullName(string fullname)
    {
        try
        {
            var facilitator = await _facilitatorRepository.GetFacilitatorByFullName(fullname) ?? throw new NotFoundException("Facilitator not found by the given fullname.");
            return new FacilitatorDto
            {
                Reference = facilitator.Reference,
                FullName = facilitator.FullName,
                Bio = facilitator.Bio,
                Email = facilitator.Email,
                Phone = facilitator.Phone,
         
            };
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching facilitator by fullname: {e.Message}");
        }
    }

    public async Task<List<FacilitatorDto>> GetFacilitatorList(int page)
    {
        try
        {
            var facilitators = await _facilitatorRepository.GetFacilitatorList(page);
            
            if (facilitators == null || !facilitators.Any())
                throw new NotFoundException("No facilitators found for the given page.");

            return facilitators.Select(facilitator => new FacilitatorDto
            {
                Reference = facilitator.Reference,
                FullName = facilitator.FullName,
                Bio = facilitator.Bio,
                Email = facilitator.Email,
                Phone = facilitator.Phone,
                // LinkedIn = facilitator.LinkedIn,
                // Instagram = facilitator.Instagram,
                // Twitter = facilitator.Twitter
            }).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching facilitator list: {e.Message}");
        }
    }


    public async Task<List<FacilitatorDto>> SearchFacilitatorList(int page, string fullname)
    {
        try
        {
            var facilitators = await _facilitatorRepository.SearchFacilitatorList(page, fullname);
            
            if (facilitators == null || !facilitators.Any())
                throw new NotFoundException("No facilitators found with the given title.");

            return facilitators.Select(facilitator => new FacilitatorDto
            {
                Reference = facilitator.Reference,
                FullName = facilitator.FullName,
                Bio = facilitator.Bio,
                Email = facilitator.Email,
                Phone = facilitator.Phone,
                // LinkedIn = facilitator.LinkedIn,
                // Instagram = facilitator.Instagram,
                // Twitter = facilitator.Twitter
            }).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error searching facilitator list: {e.Message}");
        }
    }

}

