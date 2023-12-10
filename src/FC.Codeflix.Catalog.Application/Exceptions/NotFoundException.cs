﻿namespace FC.Codeflix.Catalog.Application.Exceptions;
public class NotFoundException : ApplicationException
{
    public NotFoundException(string? message) : base(message)
    {}

    public static void ThrowIfNull(object? obj, string exceptionMessage)
    {
        if (obj == null) throw new NotFoundException(exceptionMessage);
    }
}
