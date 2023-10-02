using Sytycc_Service.Domain;
using MongoDB.Driver;
using Serilog;

namespace Sytycc_Service.Api;

public class ParticipantService : IParticipantService
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IParticipantValidationService _participantValidationService;

    public ParticipantService(IParticipantRepository participantRepository, IParticipantValidationService participantValidationService)
    {
        _participantRepository = participantRepository;
        _participantValidationService = participantValidationService;
    }

    public async Task<string> CreateParticipant(CreateParticipantDto participantDto)
    {
        try
        {
            var validationResult = _participantValidationService.ValidateCreateParticipant(participantDto);
            if (validationResult != null)
                throw validationResult;

            var existingParticipant = await _participantRepository.GetParticipantByEmail(participantDto.Email);
            if(existingParticipant != null)
            {
                Log.Information($"Participant with email {existingParticipant.Email} already exists");
                return existingParticipant.Reference;
            }

            var participant = new Participant(participantDto);
            return await _participantRepository.CreateParticipant(participant);
        }
        catch (BadRequestException e) 
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        
        catch (AppException e)
        {
            Log.Error($"Server Error: {e.Message}");
            throw;
        }
        catch (Exception e) 
        {
            Log.Error($"Error Creating Participant: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<string> UpdateParticipant(string reference, UpdateParticipantDto participantDto)
    {
        try
        {
            var validationResult = _participantValidationService.ValidateUpdateParticipant(participantDto);
            if (validationResult != null)
                throw validationResult;

            await GetParticipantByReference(reference);
            var participant = await _participantRepository.GetParticipantByReference(reference);
            if (participant == null)
                throw new AppException(new[] { "Participant not found" }, "DATABASE", 404);

            var updatedParticipant = new Participant(participantDto) { Reference = reference };
            return await _participantRepository.UpdateParticipant(reference, updatedParticipant);
        }
            catch (BadRequestException e) 
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        catch (AppException e)
        {
            Log.Error($"Server Error: {e.Message}");
            throw;
        }
        catch (Exception e) 
        {
            Log.Error($"Error Updating Participant: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<string> DeleteParticipant(string reference)
    {
        try
        {
            await GetParticipantByReference(reference);
            return await _participantRepository.DeleteParticipant(reference);
        }
            catch (BadRequestException e) 
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        catch (AppException e)
        {
            Log.Error($"Server Error: {e.Message}");
            throw;
        }
        catch (Exception e) 
        {
            Log.Error($"Error Deleting Participant: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<ParticipantDto> GetParticipantByReference(string reference)
    {
        try
        {
            var participant = await _participantRepository.GetParticipantByReference(reference);
            if (participant == null)
                throw new AppException(new[] { "Participant not found" }, "DATABASE", 404);

            // TODO: Map Participant to ParticipantDto
            var participantDto = new ParticipantDto 
            {
                Reference = participant.Reference,
                FirstName = participant.FirstName,
                LastName = participant.LastName,
                Bio = participant.Bio,
                Email = participant.Email,
                Phone = participant.Phone,
        
            };

            return participantDto;
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching participant by reference: {e.Message}");
        }
    }
 public async Task<ParticipantDto> GetParticipantByEmail(string email)
    {
        try
        {
            var participant = await _participantRepository.GetParticipantByEmail(email) ?? throw new AppException(new[] { "Participant not found" }, "DATABASE", 404);

            // TODO: Map Participant to ParticipantDto
            var participantDto = new ParticipantDto 
            {
                Reference = participant.Reference,
                FirstName = participant.FirstName,
                LastName = participant.LastName,
                Bio = participant.Bio,
                Email = participant.Email,
                Phone = participant.Phone,
          
            };

            return participantDto;
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching participant by email: {e.Message}");
        }
    }
    public async Task<List<ParticipantDto>> GetParticipantList(int page)
    {
        try
        {
            var participants = await _participantRepository.GetParticipantList(page);
            if (participants == null || !participants.Any())
                throw new AppException(new[] { "No participants found" }, "DATABASE", 404);

            // TODO: Map List<Participant> to List<ParticipantDto>
            var participantDtos = participants.Select(p => new ParticipantDto
            {
                Reference = p.Reference,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Bio = p.Bio,
                Email = p.Email,
                Phone = p.Phone,
                // Instagram = p.Instagram,
                // LinkedIn = p.LinkedIn,
                // Twitter = p.Twitter

            }).ToList();

            return participantDtos;
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching participant: {e.Message}");
        }
    }

    public async Task<List<ParticipantDto>> SearchParticipantList(int page, string title)
    {
        try
        {
            var participants = await _participantRepository.SearchParticipantList(page, title);
            if (participants == null || !participants.Any())
                throw new AppException(new[] { "No participants found with the given title" }, "DATABASE", 404);

            // TODO: Map List<Participant> to List<ParticipantDto>
            var participantDtos = participants.Select(p => new ParticipantDto
            {
                Reference = p.Reference,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Bio = p.Bio,
                Email = p.Email,
                Phone = p.Phone,
                // Instagram = p.Instagram,
                // LinkedIn = p.LinkedIn,
                // Twitter = p.Twitter
            }).ToList();

            return participantDtos;
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            throw new InternalServerException($"Error fetching participant: {e.Message}");
        }
    }
}

