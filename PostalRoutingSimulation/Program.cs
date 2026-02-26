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

var farsta = new PostalOffice("12640", regionalOffice);
var solna = new PostalOffice("80800", regionalOffice);


regionalOffice.RegisterOffice(farsta);
regionalOffice.RegisterOffice(solna);

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

        var mappedIncoming =  office.Incoming.Select(FilteredMailItem.toDTO).ToList();
    
    return new ZipcodePackageListResponse(mappedOutgoing, mappedIncoming);
});


// app.MapGet("/getall/packageInfo/{zipcode}", (string zipcode) =>
// {
//  
// });
app.Run();


// app.MapGet("/sendmail", ()  =>
// {
//  
// });
// app.MapPost("/namelist", (PersonDto per) =>
// {
//     var personEntity = new Person();
//
//     personEntity.Name = per.Name;
//     
//     persons.Add(personEntity);
//     return ("/namelist", personEntity);
// } );
//
// app.MapGet("namelist", () =>
// {
//     return persons;
// });
// app.MapGet("/zipcode/{id}", (string id) => { return regionalOffice.DoesZipCodeExist(id); });

// app.MapPost($"/address", ([FromBody] Address address) =>
//     {
//         return PostOfficeFarsta.DoesAddressCodeExist(address);
//     }
// );
// //

// app.MapPost("/sendmail", ([FromBody] MailItem item) =>
//     { return regionalOffice.CreateMailItem(item); });

// app.MapPost("/address/", ([FromBody] Address address) =>
// {
//      return regionalOffice.DoesAddressExist(address) ;
//     
// });
// // app.MapGet("/zipcode/{id}", getId);
// // app.MapGet("/zipcode/{id}", getId);
// // app.MapGet("/zipcode/{id}", (string id) => getId(id));
//
//
// string getId(string id)
// {
//     return $"Hej {id}";
// }


//basically du ska kunna skicka in en zipkod och endpointens job
//är att kolla om den zipkoden existerar i regional


// Console.WriteLine(mailItem.ShowHistory());
// Console.WriteLine(person1.GetHashCode());
// var mailItem = office.CreateOutgoingMail(
//     item.Sender,
//     item.Recipient,
//     item.WeightGrams,
//     item.MailType
// );

// app.MapPost("/sendmail/{zipcode}", (string zipcode, MailItem item) =>
// {
//     if (!regionalOffice.DoesZipCodeExist(zipcode))
//       throw new ("ZipCode not found");
//
//     var office = regionalOffice.GetOffice(zipcode);
//
//     office.CreateMailItemAtOffice(item);
//
//     office.ProcessMailCycle();
//     regionalOffice.RouteMailCycle();
//
//     var destinationOffice = regionalOffice.GetOffice(item.Recipient.Address.ZipCode);
//     
//     destinationOffice.ProcessMailCycle();
//     
//     return item;
// });