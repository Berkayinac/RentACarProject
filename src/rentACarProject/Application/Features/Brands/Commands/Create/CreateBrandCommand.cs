using Application.Features.Brands.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Brands.Commands.Create;

// IRequest -> CQRS Request'i olduðunu belirtmek için implement ediyoruz. 

//                  ADD-Request                //Response 
public class CreateBrandCommand : IRequest<CreatedBrandResponse>
{
    public string Name { get; set; }

    //              Handler -> Command'i iþleten koddur.
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;
        private readonly BrandBusinessRules _brandBusinessRules;

        public CreateBrandCommandHandler(IMapper mapper, IBrandRepository brandRepository,
                                         BrandBusinessRules brandBusinessRules)
        {
            _mapper = mapper;
            _brandRepository = brandRepository;
            _brandBusinessRules = brandBusinessRules;
        }

        public async Task<CreatedBrandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            // Ýþ kuralý hatasý olduðunda nasýl handle edersin.

            await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenInserted(request.Name);
            // kaç tane iþ kuralýn varsa alt alta yapýþtýr.
            //await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenInserted(request.Name);
            //await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenInserted(request.Name);

            Brand brand = _mapper.Map<Brand>(request);

            await _brandRepository.AddAsync(brand);

            CreatedBrandResponse response = _mapper.Map<CreatedBrandResponse>(brand);
            return response;
        }
    }
}

// Tüm kodlarýn try catch içerisinde olmalýdýr.
// Ancak tüm kodlar try catch içerisinde olursa tüm proje spagetti koda döner
// bizde tüm projeyi arka planda tek bir try catch içerisinde alýyoruz.
// try { tümproje() } catch{hata durumunda ne olsun } --> Middleware
// program.cs -> app.ConfigureCustomExceptionMiddleware(); bu bizim middleware'imz.
// requestDelegate -> yapýlan tüm request'ler buraya gelerek try catch içerisinde çalýþtýrarak hem handle edilir hemde loglama iþlemi yapýlýr.
// Core.CrossCuttingConcern.Exception.WebApi.Middleware kýsmýnda detay metto var