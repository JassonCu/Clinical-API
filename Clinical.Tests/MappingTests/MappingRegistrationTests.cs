using AutoMapper;
using Clinical.Application.DTOS.Doctor.Response;
using Clinical.Domain.Entities;
using Clinical.UseCases.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Clinical.Test.MappingTests
{
    public class MappingRegistrationTests
    {
        // Guards the AutoMapper 15 registration (AddMaps): if profiles stop being discovered,
        // every mapping endpoint breaks at runtime — this fails at build/test time instead.
        [Fact]
        public void ApplicationRegistration_DiscoversMappingProfiles()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddInyectionApplication();
            using var provider = services.BuildServiceProvider();

            var mapper = provider.GetRequiredService<IMapper>();
            var dto = mapper.Map<GetAllDoctorResponseDto>(new Doctor { DoctorId = 5 });

            Assert.NotNull(dto);
            Assert.Equal(5, dto.DoctorId);
        }
    }
}
