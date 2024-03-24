using Application.Features.Brands.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Brands.Commands.Create;

// IRequest -> CQRS Request'i oldu�unu belirtmek i�in implement ediyoruz. 

//                  ADD-Request                //Response 
public class CreateBrandCommand : IRequest<CreatedBrandResponse>
{
    public string Name { get; set; }

    //              Handler -> Command'i i�leten koddur.
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
            // �� kural� hatas� oldu�unda nas�l handle edersin.

            await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenInserted(request.Name);
            // ka� tane i� kural�n varsa alt alta yap��t�r.
            //await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenInserted(request.Name);
            //await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenInserted(request.Name);

            Brand brand = _mapper.Map<Brand>(request);

            await _brandRepository.AddAsync(brand);

            CreatedBrandResponse response = _mapper.Map<CreatedBrandResponse>(brand);
            return response;
        }
    }
}

// T�m kodlar�n try catch i�erisinde olmal�d�r.
// Ancak t�m kodlar try catch i�erisinde olursa t�m proje spagetti koda d�ner
// bizde t�m projeyi arka planda tek bir try catch i�erisinde al�yoruz.
// try { t�mproje() } catch{hata durumunda ne olsun } --> Middleware
// program.cs -> app.ConfigureCustomExceptionMiddleware(); bu bizim middleware'imz.
// requestDelegate -> yap�lan t�m request'ler buraya gelerek try catch i�erisinde �al��t�rarak hem handle edilir hemde loglama i�lemi yap�l�r.
// Core.CrossCuttingConcern.Exception.WebApi.Middleware k�sm�nda detay metto var