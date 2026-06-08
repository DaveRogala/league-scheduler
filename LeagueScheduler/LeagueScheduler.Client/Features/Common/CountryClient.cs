using LeagueScheduler.Shared.Common;
using System.Net.Http.Json;

namespace LeagueScheduler.Client.Features.Common
{
    public class CountryClient(HttpClient http)
    {
        public async Task<List<CountryDto>?> GetAllAsync() =>
            await http.GetFromJsonAsync<List<CountryDto>>("/api/countries");

        public async Task<List<CountryRegionDto>?> GetRegionsAsync(Guid countryId) =>
            await http.GetFromJsonAsync<List<CountryRegionDto>>($"/api/countries/{countryId}/regions");

        public Task<HttpResponseMessage> AddCountryAsync(CountryDto dto) =>
            http.PostAsJsonAsync("/api/admin/countries", dto);

        public Task<HttpResponseMessage> UpdateCountryAsync(Guid id, CountryDto dto) =>
            http.PutAsJsonAsync($"/api/admin/countries/{id}", dto);

        public Task<HttpResponseMessage> DeleteCountryAsync(Guid id) =>
            http.DeleteAsync($"/api/admin/countries/{id}");

        public Task<HttpResponseMessage> AddRegionAsync(Guid countryId, CountryRegionDto dto) =>
            http.PostAsJsonAsync($"/api/admin/countries/{countryId}/regions", dto);

        public Task<HttpResponseMessage> DeleteRegionAsync(Guid countryId, Guid regionId) =>
            http.DeleteAsync($"/api/admin/countries/{countryId}/regions/{regionId}");
    }
}
