namespace Shared.DataTransferObjects
{
    [Serializable]
    public record EmployeeDto {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Age { get; init; }
        public string? Position { get; init; }
    }
}
