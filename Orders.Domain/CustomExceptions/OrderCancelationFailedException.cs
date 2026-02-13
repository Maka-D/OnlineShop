using Shared.Exceptions;

namespace Orders.Domain.CustomExceptions;

public class OrderCancelationFailedException() : BaseCustomException("Only Pending orders can be cancelled.");
