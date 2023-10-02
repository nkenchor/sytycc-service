using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sytycc_Service.Domain;
using Sytycc_Service.Infrastructure;

namespace Sytycc_Service.Api;

public class FacilitatorSeeder
{
    private readonly IFacilitatorRepository _facilitatorRepository;

    public FacilitatorSeeder(IFacilitatorRepository facilitatorRepository)
    {
        _facilitatorRepository = facilitatorRepository;
    }

    public async Task SeedFacilitatorAsync()
    {
        var facilitatorDtos = new List<CreateFacilitatorDto>
        {
            new CreateFacilitatorDto
            {
                LastName = "Nonso",
                FirstName = "Sapphire",
                Bio = "Trainer",
                Email = "admin@sytycc.com",
                Phone = "+447876042889",
            },
        };

        foreach (var facilitatorDto in facilitatorDtos)
        {
            var facilitator = new Facilitator(facilitatorDto)
            {
                Reference = "b1f394c7-717f-4b33-b1ef-f73e970005cd"
            };
            await _facilitatorRepository.CreateFacilitator(facilitator);
        }
    }
}
