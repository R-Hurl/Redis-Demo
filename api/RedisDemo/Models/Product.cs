namespace RedisDemo.Models;

public record Product(short Id, short CategoryId, string Name, string Description, decimal Price);
