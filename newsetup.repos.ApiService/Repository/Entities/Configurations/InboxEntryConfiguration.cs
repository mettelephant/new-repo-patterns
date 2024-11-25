using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using newsetup.repos.ApiService.Uitlities;

namespace newsetup.repos.ApiService.Repository.Entities.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

public class InboxEntryConfiguration<TMessage> :
    IEntityTypeConfiguration<InboxEntry<TMessage>> where TMessage : class
{
    public void Configure(EntityTypeBuilder<InboxEntry<TMessage>> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.MessageType)
            .IsRequired();

        builder.Property(e => e.ReceivedAt)
            .IsRequired();
        builder.Property(e => e.ProcessedAt);
        builder.Property(e => e.NotBefore);
        builder.Property(e => e.ErrorMessages);
        builder.Property(e => e.AttemptCount)
            .IsRequired();
        builder.Property(e => e.Status)
            .HasConversion<EnumToStringConverter<EntryStatus>>()
            .IsRequired();

        // Configure Message as a JSON column
        builder.Property(e => e.Message)
            .HasConversion(
                v => JsonSerializer.Serialize(v, DatabaseJsonOptions.SerializerOptions),
                v => JsonSerializer.Deserialize<TMessage>(v, DatabaseJsonOptions.SerializerOptions)!);
    }
}