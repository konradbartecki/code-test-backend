using System;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Moq;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;
using Xunit;

namespace SlothEnterprise.ProductApplication.Tests
{
    public class ProductApplicationTests
    {
        [Fact]
        public void CanSubmit_SelectiveInvoiceDiscount()
        {
            //Arrange
            const int returnedAppId = 885544;
            const decimal invoiceAmount = 1234.56m;
            const decimal advancePercentage = 0.23m;
            const int companyNumber = 5433;
            string companyNumberString = companyNumber.ToString();

            var mock = new Mock<ISelectInvoiceService>();
            var application = new SellerApplication()
            {
                Product = new SelectiveInvoiceDiscount()
                {
                    Id = 123,
                    AdvancePercentage = advancePercentage,
                    InvoiceAmount = invoiceAmount
                },
                CompanyData = new SellerCompanyData()
                {
                    Founded = new DateTime(2020, 05, 03),
                    Name = "Contoso Ltd.",
                    Number = companyNumber,
                    DirectorName = "John Smith"
                }
            };
            var service = new ProductApplicationService(mock.Object, null, null);

            mock.Setup(serviceMock => serviceMock.SubmitApplicationFor(
                    It.Is<string>(x => x == companyNumberString),
                    It.Is<decimal>(x => x == invoiceAmount),
                    It.Is<decimal>(x => x == advancePercentage)))
                .Returns(returnedAppId);
            
            //Act
            var result = service.SubmitApplicationFor(application);

            //Assert
            Assert.Equal(returnedAppId, result);
        }

        [Fact]
        public void CanSubmit_ConfidentialInvoiceDiscount()
        {
            //Arrange
            const int returnedAppId = 885544;
            const decimal invoiceLedgerTotalValue = 1234.56m;
            const decimal advancePercentage = 0.23m;
            const decimal vatRate = 0.19m;
            const int companyNumber = 5433;

            var mock = new Mock<IConfidentialInvoiceService>();
            var companyDataRequest = new SellerCompanyData()
            {
                Founded = new DateTime(2011, 04, 05, 04, 03, 02),
                Name = "Contoso Ltd.",
                Number = companyNumber,
                DirectorName = "John Smith"
            };

            var service = new ProductApplicationService(null, mock.Object, null);

            Expression<Func<CompanyDataRequest, bool>> companyMatcher = x =>
                x.CompanyFounded == companyDataRequest.Founded
                && x.CompanyName == companyDataRequest.Name
                && x.CompanyNumber == companyNumber
                && x.DirectorName == companyDataRequest.DirectorName;

            var resultMock = new Mock<IApplicationResult>();
            resultMock.SetupGet(x => x.ApplicationId).Returns(returnedAppId);
            resultMock.SetupGet(x => x.Success).Returns(true);

            mock.Setup(serviceMock => serviceMock.SubmitApplicationFor(
                It.Is<CompanyDataRequest>(companyMatcher),
                It.Is<decimal>(x => x == invoiceLedgerTotalValue),
                It.Is<decimal>(x => x == advancePercentage),
                It.Is<decimal>(x => x == vatRate)))
                .Returns(resultMock.Object);

            var fakeProduct = new ConfidentialInvoiceDiscount()
            {
                Id = 123,
                AdvancePercentage = advancePercentage,
                TotalLedgerNetworth = invoiceLedgerTotalValue,
                VatRate = vatRate,
            };

            //Act
            var result = service.SubmitApplicationFor(new SellerApplication()
            {
                CompanyData = companyDataRequest,
                Product = fakeProduct
            });

            //Assert
            Assert.Equal(returnedAppId, result);
        }

        [Fact]
        public void CanSubmit_BusinessLoan()
        {
            //Arrange
            const int companyNumber = 5433;
            var mock = new Mock<IBusinessLoansService>();
            var companyDataRequest = new SellerCompanyData()
            {
                Founded = new DateTime(2011, 04, 05, 04, 03, 02),
                Name = "Contoso Ltd.",
                Number = companyNumber,
                DirectorName = "John Smith"
            };
            var product = new BusinessLoans()
            {
                Id = 985,
                InterestRatePerAnnum = 21.37m,
                LoanAmount = 9500m
            };

            Expression<Func<CompanyDataRequest, bool>> companyMatcher = x =>
                x.CompanyFounded == companyDataRequest.Founded
                && x.CompanyName == companyDataRequest.Name
                && x.CompanyNumber == companyNumber
                && x.DirectorName == companyDataRequest.DirectorName;

            Expression<Func<LoansRequest, bool>> loansRequestMatcher = request =>
                request.InterestRatePerAnnum == product.InterestRatePerAnnum
                && request.LoanAmount == product.LoanAmount;

            var resultMock = new Mock<IApplicationResult>();
            resultMock.SetupGet(x => x.ApplicationId).Returns(5000);
            resultMock.SetupGet(x => x.Success).Returns(true);

            mock.Setup(x => x.SubmitApplicationFor(
                    It.Is(companyMatcher), It.Is(loansRequestMatcher)))
                .Returns(resultMock.Object);

            var service = new ProductApplicationService(null, null, mock.Object);

            //Act
            var result = service.SubmitApplicationFor(new SellerApplication()
            {
                CompanyData = companyDataRequest,
                Product = product
            });

            //Assert
            Assert.Equal(5000, result);

        }

        [Fact]
        public void Throws_InvalidOperation_OnProductIsNull()
        {
            //Arrange
            var service = new ProductApplicationService(null, null, null);

            //Act & Assert
            Assert.Throws<InvalidOperationException>(() => service.SubmitApplicationFor(new SellerApplication()));
        }
        
    }
}
