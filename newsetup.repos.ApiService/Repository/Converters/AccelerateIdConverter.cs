using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using newsetup.repos.ApiService.Repository.ValueObjects;

namespace newsetup.repos.ApiService.Repository.Converters;

public class AccelerateIdConverter() : ValueConverter<AccelerateId, Guid>(v => v.Id,
    v => new AccelerateId(v));