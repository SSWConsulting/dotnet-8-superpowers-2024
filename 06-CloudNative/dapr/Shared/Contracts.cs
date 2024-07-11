namespace Shared;

public record ProduceItemRequest(string Name, int Quantity);

public record ProduceItemMessage(string Name, DateTime RequestedAt);