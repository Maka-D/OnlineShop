using Shared.Exceptions;

namespace Orders.Domain.CustomExceptions;

public class OrderRejectFailedException() : BaseCustomException("Only Pending orders can be Rejected.");