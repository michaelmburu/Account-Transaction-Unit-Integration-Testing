using AccountTransactionLogic.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    [TestClass]
    [TestCategory("Integration")]
    public class AccountTransactionTests
    {
       public async Task AccountTransaction_PostNewTransaction_IsSuccessful()
        {
            //Arrange
            var transactionInfo = new TransactionInfo
            {
                AccountNumber = "0000000020",
                Amount = 100.2M,
                TransactionDate = new DateTime(2010, 1, 1)
            };

            var client = new HttpClient(

                new HttpClientHandler
                {
                    Credentials = CredentialCache.DefaultNetworkCredentials,
                    AllowAutoRedirect = true
                });

            var url = ConfigurationManager.AppSettings["AccountTransactionUrl"] + "Transaction";
            var content = new StringContent(
            JsonConvert.SerializeObject(transactionInfo), Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync(url, content);

            //Assert
            response.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}
