using AutoMapper;
using ExpressWorld.Core.Entities;
using ExpressWorld.Shared.Factories;
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

        //public JSONSupplierAdapter(string relativeFilePath, string rootPropertyName, string dtoType, IMapper mapper)
        //{
        //    _filePath = Path.Combine(AppContext.BaseDirectory, "Data", relativeFilePath);
        //    _rootPropertyName = rootPropertyName;
        //    _dtoType = dtoType;
        //    _mapper = mapper;
        //}

        public JSONSupplierAdapter(SupplierConfig supplierConfig, IMapper mapper)
        {
            _filePath = Path.Combine(AppContext.BaseDirectory, "Data", supplierConfig.SourcePath);
            _rootPropertyName = supplierConfig.RootPropertyName;
            _dtoType = supplierConfig.DtoType;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Product>> LoadProductsAsyncold(CancellationToken cancellationToken)
        {
            var jsonData = await File.ReadAllTextAsync(_filePath, cancellationToken);
            List<TDto> supplierData = null;

            using (var document = JsonDocument.Parse(jsonData))
            {
                var root = document.RootElement;

                // Dynamically select the root property based on the configuration
                if (!root.TryGetProperty(_rootPropertyName, out JsonElement rootElement))
                {
                    throw new InvalidOperationException($"Root property '{_rootPropertyName}' not found in JSON.");
                }

                supplierData = JsonSerializer.Deserialize<List<TDto>>(rootElement.GetRawText());
            }

            var results = _mapper.Map<IEnumerable<Product>>(supplierData);
            return results;
        }

        public async Task<IEnumerable<Product>> LoadProductsAsyncv2(CancellationToken cancellationToken)
        {
            var jsonData = await File.ReadAllTextAsync(_filePath, cancellationToken);
            var supplierData = new List<object>();

            using (var document = JsonDocument.Parse(jsonData))
            {
                var root = document.RootElement;

                // Dynamically select the root property based on the configuration
                if (!root.TryGetProperty(_rootPropertyName, out JsonElement rootElement))
                {
                    throw new InvalidOperationException($"Root property '{_rootPropertyName}' not found in JSON.");
                }

                // Deserialize into a generic List based on the DTO type defined in the configuration
                Type dtoType = Type.GetType(_dtoType); // e.g., "ExpressWorld.Shared.DTOs.TourGuyProductDTO"
                var rawData = JsonSerializer.Deserialize(rootElement.GetRawText(), dtoType.MakeGenericType(typeof(List<>)));

                // Add logic to extract and convert items from rawData to the supplierData list.
                supplierData.AddRange((IEnumerable<object>)rawData);
            }

            // Map the DTOs to the common Product model
            var results = _mapper.Map<IEnumerable<Product>>(supplierData);
            return results;
        }

        public async Task<IEnumerable<Product>> LoadProductsAsyncv3(CancellationToken cancellationToken)
        {
            var jsonData = await File.ReadAllTextAsync(_filePath, cancellationToken);

            // Parse the JSON document
            using (var document = JsonDocument.Parse(jsonData))
            {
                var root = document.RootElement;

                // Dynamically select the root property based on the configuration
                if (!root.TryGetProperty(_rootPropertyName, out JsonElement rootElement))
                {
                    throw new InvalidOperationException($"Root property '{_rootPropertyName}' not found in JSON.");
                }

                // Dynamically determine the DTO type
                var dtoType = Type.GetType(_dtoType); // Assuming _dtoType is the full type name in the configuration
                if (dtoType == null)
                {
                    throw new InvalidOperationException($"Could not find type '{_dtoType}'.");
                }

                // Create a list of the appropriate type
                var listType = typeof(List<>).MakeGenericType(dtoType);
                var supplierData = (IList)Activator.CreateInstance(listType);

                // Deserialize the JSON into the DTO type using reflection
                foreach (var item in JsonSerializer.Deserialize<IEnumerable<JsonElement>>(rootElement.GetRawText()))
                {
                    var dtoInstance = JsonSerializer.Deserialize(item.GetRawText(), dtoType);
                    supplierData.Add(dtoInstance);
                }

                // Map the DTOs to the common Product model
                var results =  _mapper.Map<IEnumerable<Product>>(supplierData.Cast<object>());
                return results;
            }
        }

        public async Task<IEnumerable<Product>> LoadProductsAsyncWorking(CancellationToken cancellationToken)
        {
            var jsonData = await File.ReadAllTextAsync(_filePath, cancellationToken);

            using (var document = JsonDocument.Parse(jsonData))
            {
                var root = document.RootElement;

                // Dynamically select the root property based on the configuration
                if (!root.TryGetProperty(_rootPropertyName, out JsonElement rootElement))
                {
                    throw new InvalidOperationException($"Root property '{_rootPropertyName}' not found in JSON.");
                }

                var dtoType = Type.GetType(_dtoType);
                if (dtoType == null)
                {
                    throw new InvalidOperationException($"Could not find type '{_dtoType}'.");
                }

                var listType = typeof(List<>).MakeGenericType(dtoType);
                var supplierData = (IList)Activator.CreateInstance(listType);

                // Create options for JSON deserialization
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Enable case-insensitive property matching
                };

                // Deserialize the JSON into the DTO type using reflection
                foreach (var item in JsonSerializer.Deserialize<IEnumerable<JsonElement>>(rootElement.GetRawText(), options))
                {
                    var dtoInstance = JsonSerializer.Deserialize(item.GetRawText(), dtoType, options);
                    supplierData.Add(dtoInstance);
                }

                // Map the DTOs to the common Product model
                var results = _mapper.Map<IEnumerable<Product>>(supplierData.Cast<object>());
                return results;
            }
        }

        public async Task<IEnumerable<Product>> LoadProductsAsync(CancellationToken cancellationToken)
        {
            // Setup JSON serializer options with case-insensitive property matching
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Enables case-insensitive property matching
            };

            using (var stream = File.OpenRead(_filePath)) // Open the JSON file as a stream
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

                    // Map the DTOs to the common Product model
                    var results = _mapper.Map<IEnumerable<Product>>(supplierData);
                    return results;
                }
            }
        }
    }


    //public class JSONSupplierAdapter<TDto> : IProductAdapter where TDto : class
    //{
    //    private readonly string _filePath;
    //    private string _basePath = "";
    //    private readonly IMapper _mapper;

    //    public JSONSupplierAdapter(string relativeFilePath, IMapper mapper)
    //    {
    //        _filePath = Path.Combine(AppContext.BaseDirectory, "Data", relativeFilePath);
    //        _mapper = mapper;
    //    }

    //    public async Task<IEnumerable<Product>> LoadProductsAsync(CancellationToken cancellationToken)
    //    {
    //        var jsonData = await File.ReadAllTextAsync(_filePath, cancellationToken);
    //        var supplierData = JsonSerializer.Deserialize<List<TDto>>(jsonData);

    //        // Map the DTOs to the common Product model
    //        return _mapper.Map<IEnumerable<Product>>(supplierData);
    //    }
    //}

}
