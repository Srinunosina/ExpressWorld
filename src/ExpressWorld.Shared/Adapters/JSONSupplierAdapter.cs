using AutoMapper;
using ExpressWorld.Core.Entities;
using ExpressWorld.Shared.Configurations;
using System.Collections;
using System.Text.Json;

namespace ExpressWorld.Shared.Adapters
{
    public class JSONSupplierAdapter<TDto> : IProductAdapter where TDto : class
    {
        private readonly string _filePath;
        private readonly string _rootPropertyName;
        private readonly string _dtoType;
        private readonly IMapper _mapper;

        public JSONSupplierAdapter(SupplierConfig supplierConfig, IMapper mapper)
        {
            _filePath = Path.Combine(AppContext.BaseDirectory, "Data", supplierConfig.SourcePath);
            _rootPropertyName = supplierConfig.RootPropertyName;
            _dtoType = supplierConfig.DtoType;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Product>> LoadProductsAsync(CancellationToken cancellationToken)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Enables case-insensitive property matching
            };

            using (var stream = File.OpenRead(_filePath))
            {
                using (var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken))
                {
                    var root = document.RootElement;

                    // Dynamically select the root property based on the configuration
                    if (!root.TryGetProperty(_rootPropertyName, out JsonElement rootElement))
                    {
                        throw new InvalidOperationException($"Root property '{_rootPropertyName}' not found in JSON.");
                    }

                    // Deserialize the data from the specified root element into a list of TDto
                    var supplierData = JsonSerializer.Deserialize<List<TDto>>(rootElement.GetRawText(), options);

                    // Extract file name without extension as SupplierName
                    var supplierName = Path.GetFileNameWithoutExtension(_filePath);
                    return _mapper.Map<IEnumerable<Product>>(supplierData, opt => opt.Items["SupplierName"] = supplierName);
                }
            }
        }
    }
}
