using Grpc.Core;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GrpcServer.Services
{
  public class CustomersService : Customer.CustomerBase
  {
    private readonly ILogger<CustomersService> _logger;
    public CustomersService(ILogger<CustomersService> logger)
    {
      _logger = logger;
    }

    public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
    {
      CustomerModel output = new CustomerModel();
      if (request.UserId == 1)
      {
        output.FirstName = "Jamie";
        output.LastName = "Smith";
      }

      else if (request.UserId == 2)
      {
        output.FirstName = "Jane";
        output.LastName = "Doe";
      }

      else
      {
        output.FirstName = "Greg";
        output.LastName = "Thomas";
      }

      return Task.FromResult(output);
    }


    public override async Task GetNewCustomers(
      NewCustomerRequest request,
      IServerStreamWriter<CustomerModel> responseStream,
      ServerCallContext context)
    {
      List<CustomerModel> customers = new List<CustomerModel>
    {
      new CustomerModel
      {
        FirstName = "Troy",
        LastName = "Boyer",
        EmailAddress = "troy@troy.com",
        Age = 41,
        IsAlive = true
      },

      new CustomerModel
      {
        FirstName = "Sue",
        LastName = "Storm",
        EmailAddress = "sue@stromboli.com",
        Age = 88,
        IsAlive = false
      },

      new CustomerModel
      {
        FirstName = "Roger",
        LastName = "Rosetta",
        EmailAddress = "RogHog@aol.com",
        Age = 101,
        IsAlive = false
      },

    };

      foreach (var cust in customers)
      {
        await Task.Delay(1000);
        await responseStream.WriteAsync(cust);
      }
    }
  }
}