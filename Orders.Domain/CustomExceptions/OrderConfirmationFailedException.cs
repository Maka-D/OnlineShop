using Shared.Exceptions;

namespace Orders.Domain.CustomExceptions;

public class OrderConfirmationFailedException() : BaseCustomException("Only Pending orders can be Confirmed.");