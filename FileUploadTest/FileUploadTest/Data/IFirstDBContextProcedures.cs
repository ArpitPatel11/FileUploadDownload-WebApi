﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using FileUploadTest.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadTest.Data
{
    public partial interface IFirstDBContextProcedures
    {
        Task<List<USP_Image_GetResult>> USP_Image_GetAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<USP_Image_InsertResult>> USP_Image_InsertAsync(string ImagePath, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}