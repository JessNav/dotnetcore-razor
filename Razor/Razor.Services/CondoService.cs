using Razor.Services.Mapping;

namespace Razor.Services
{
    public class CondoService
    {
        private readonly IMapper _mapper;

        public CondoService(IMapper mapper)
        {
            _mapper = mapper;
        }

        // Testing Unit Test
        public bool IsCondo(int id)
        {
            if (id < 1) {
                return false;
            }

            return true;
        }

    }
}
