﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CustomEncryptorSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var keysFolder = Path.Combine(Directory.GetCurrentDirectory(), "temp-keys");
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
                .UseXmlEncryptor(s => new CustomXmlEncryptor(s));

            var services = serviceCollection.BuildServiceProvider();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddConsole();

            var protector = services.GetDataProtector("SamplePurpose");
            
            // protect the payload
            var protectedPayload = protector.Protect("Hello World!");
            Console.WriteLine($"Protect returned: {protectedPayload}");

            // unprotect the payload
            var unprotectedPayload = protector.Unprotect(protectedPayload);
            Console.WriteLine($"Unprotect returned: {unprotectedPayload}");
        }
    }
}
