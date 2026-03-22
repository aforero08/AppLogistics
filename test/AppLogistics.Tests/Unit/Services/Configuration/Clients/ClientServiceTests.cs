using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace AppLogistics.Services.Tests;

public class ClientServiceTests
{
    private ClientService service;
    private DbContext context;
    private Client client;

    public ClientServiceTests()
    {
        context = TestingContext.Create();
        service = new ClientService(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

        context.Set<Client>().Add(client = ObjectsFactory.CreateClient());
        context.SaveChanges();
    }

    #region Get<TView>(String id)

    [Fact]
    public void Get_ReturnsViewById()
    {
        ClientView actual = service.Get<ClientView>(client.Id);
        ClientView expected = TestingContext.Mapper.Map<ClientView>(client);

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Address, actual.Address);
        Assert.Equal(expected.Contact, actual.Contact);
        Assert.Equal(expected.Phone, actual.Phone);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Nit, actual.Nit);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Get<TView>(String id)

    #region GetViews()

    [Fact]
    public void GetViews_ReturnsClientViews()
    {
        ClientView[] actual = service.GetViews().ToArray();
        ClientView[] expected = context
            .Set<Client>()
            .ProjectTo<ClientView>(TestingContext.Mapper.ConfigurationProvider)
            .OrderByDescending(view => view.CreationDate)
            .ToArray();

        for (int i = 0; i < expected.Length || i < actual.Length; i++)
        {
            Assert.Equal(expected[i].CreationDate, actual[i].CreationDate);
            Assert.Equal(expected[i].Address, actual[i].Address);
            Assert.Equal(expected[i].Contact, actual[i].Contact);
            Assert.Equal(expected[i].Phone, actual[i].Phone);
            Assert.Equal(expected[i].Name, actual[i].Name);
            Assert.Equal(expected[i].Nit, actual[i].Nit);
            Assert.Equal(expected[i].Id, actual[i].Id);
        }
    }

    #endregion GetViews()

    #region Create(ClientView view)

    [Fact]
    public void Create_Client()
    {
        ClientCreateEditView view = ObjectsFactory.CreateClientCreateEditView(1);
        view.Id = 0;

        service.Create(view);

        Client actual = context.Set<Client>().AsNoTracking().Single(model => model.Id != client.Id);
        ClientCreateEditView expected = view;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Address, actual.Address);
        Assert.Equal(expected.Contact, actual.Contact);
        Assert.Equal(expected.Phone, actual.Phone);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Nit, actual.Nit);
    }

    #endregion Create(ClientView view)

    #region Edit(ClientView view)

    [Fact(Skip = "Need to check execution order?")]
    public void Edit_Client()
    {
        ClientCreateEditView view = ObjectsFactory.CreateClientCreateEditView(client.Id);
        view.Name = "Name0";
        view.Nit = "Nit0";
        view.Address = "Address0";
        view.Phone = "Phone0";
        view.Contact = "Contact0";

        service.Edit(view);

        Client actual = context.Set<Client>().AsNoTracking().Single();
        Client expected = client;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Address, actual.Address);
        Assert.Equal(expected.Contact, actual.Contact);
        Assert.Equal(expected.Phone, actual.Phone);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Nit, actual.Nit);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Edit(ClientView view)

    #region Delete(String id)

    [Fact]
    public void Delete_Client()
    {
        service.Delete(client.Id);

        Assert.Empty(context.Set<Client>());
    }

    #endregion Delete(String id)
}
