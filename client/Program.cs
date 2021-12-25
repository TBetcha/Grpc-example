using System;
using GrpcServer;
using Grpc.Core;
using Grpc.Net.Client;
using System.Threading.Tasks;

namespace client
{
  class Program
  {
    static async Task Main(string[] args)
    {
      //   var input = new HelloRequest { Name = "Troy" };
      //   var channel = GrpcChannel.ForAddress("http://localhost:5000");
      //   var client = new Greeter.GreeterClient(channel);
      //   var reply = await client.SayHelloAsync(input);
      //   Console.WriteLine(reply.Message);
      var channel = GrpcChannel.ForAddress("http://localhost:5000");
      var customerClient = new Customer.CustomerClient(channel);
      var clientRequested = new CustomerLookupModel { UserId = 2 };
      var customer = await customerClient.GetCustomerInfoAsync(clientRequested);
      Console.WriteLine($"{customer.FirstName} {customer.LastName}");

      Console.WriteLine("New customer list");

      using (var call = customerClient.GetNewCustomers(new NewCustomerRequest()))
      {
        while (await call.ResponseStream.MoveNext())
        {
          var currentCustomer = call.ResponseStream.Current;
          Console.WriteLine($"{currentCustomer.FirstName} {currentCustomer.LastName} {currentCustomer.EmailAddress}");
        }
      }
      Console.ReadLine();
    }
  }
}
