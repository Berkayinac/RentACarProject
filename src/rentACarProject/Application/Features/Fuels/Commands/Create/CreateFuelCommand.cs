using Application.Features.Fuels.Constants;
using Application.Features.Fuels.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.Fuels.Constants.FuelsOperationClaims;

namespace Application.Features.Fuels.Commands.Create;

// rol bazl� implement i�in -> ISecuredRequest implement edilir.
public class CreateFuelCommand : IRequest<CreatedFuelResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public string Name { get; set; }

    // features i�erisindeki operation claimler veya admin veya write yetkilerine sahip olan ki�iler bu i�lemi yapabilmesini sa�lar.
    // giri� yap�ld���nda yetki varsa bu i�lemi kullanabiliriz.
    public string[] Roles => [Admin, Write, FuelsOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetFuels"];

    public class CreateFuelCommandHandler : IRequestHandler<CreateFuelCommand, CreatedFuelResponse>
    {
        private readonly IMapper _mapper;
        private readonly IFuelRepository _fuelRepository;
        private readonly FuelBusinessRules _fuelBusinessRules;

        public CreateFuelCommandHandler(IMapper mapper, IFuelRepository fuelRepository,
                                         FuelBusinessRules fuelBusinessRules)
        {
            _mapper = mapper;
            _fuelRepository = fuelRepository;
            _fuelBusinessRules = fuelBusinessRules;
        }

        public async Task<CreatedFuelResponse> Handle(CreateFuelCommand request, CancellationToken cancellationToken)
        {
            Fuel fuel = _mapper.Map<Fuel>(request);

            await _fuelRepository.AddAsync(fuel);

            CreatedFuelResponse response = _mapper.Map<CreatedFuelResponse>(fuel);
            return response;
        }
    }
}