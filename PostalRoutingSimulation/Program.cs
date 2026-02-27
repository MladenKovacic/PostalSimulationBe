// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PostalRoutingSimulation.ContactInformation;
using PostalRoutingSimulation.MailItem;
using PostalRoutingSimulation.PostalRouting;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var regionalOffice = new RegionalCenter();

var farsta = new PostalOffice(new Address("farstavägen", "farsta", "12640"), regionalOffice);
var solna = new PostalOffice(new Address("solnavägen", "solna", "80800"), regionalOffice);
var nacka = new PostalOffice(new Address("skogalundsklippan", "nacka", "13139"), regionalOffice);

regionalOffice.RegisterOffice(farsta);
regionalOffice.RegisterOffice(solna);
regionalOffice.RegisterOffice(nacka);

var person1 = new Person("Mladen",
    new Address("Stövelvägen 18", "Stockholm", "12640"));

var person2 = new Person("Milo",
    new Address("Solnavägen 10", "Solna", "80800"));


farsta.RegisterResident(person1);
solna.RegisterResident(person2);


app.MapPost("/sendpackage", (CreateMailItemReq req) =>
{
    if (!regionalOffice.DoesZipCodeExist(req.SenderZipCode))
        throw new("ZipCode not found");

    var office = regionalOffice.GetOffice(req.SenderZipCode);

    var mailItem = MailItem.CreateFrom(req);
    office.CreateMailItemAtOffice(mailItem);

    return mailItem;
});


app.MapPost("/sendmail/{zipcode}", (string zipcode, CreateMailItemReq req) =>
{
    if (!regionalOffice.DoesZipCodeExist(zipcode))
        throw new("ZipCode not found");

    var office = regionalOffice.GetOffice(zipcode);

    var mailItem = MailItem.CreateFrom(req);
    office.CreateMailItemAtOffice(mailItem);

    //---------------------------------
    office.ProcessMailCycle();
    //---------------------------------------
    regionalOffice.RouteMailCycle();

    var destinationOffice = regionalOffice.GetOffice(mailItem.Recipient.Address.ZipCode);

    destinationOffice.ProcessMailCycle();

    return mailItem;
});


app.MapGet("/getall/package/list/{zipcode}", (string zipcode) =>
{
    if (!regionalOffice.DoesZipCodeExist(zipcode))
        throw new("ZipCode not found");

    var office = regionalOffice.GetOffice(zipcode);


    var mappedOutgoing = office.Outgoing.Select(FilteredMailItem.toDTO).ToList();

    var mappedIncoming = office.Incoming.Select(FilteredMailItem.toDTO).ToList();

    return new ZipcodePackageListResponse(mappedOutgoing, mappedIncoming);
});

app.MapGet("/getall/postaloffices", () => regionalOffice.GetAllOffices());

app.Run();