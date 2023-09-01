namespace WebAppForGame.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }


public static class ErrorViewModelEndpoints
{
	public static void MapErrorViewModelEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/ErrorViewModel", () =>
        {
            return new [] { new ErrorViewModel() };
        })
        .WithName("GetAllErrorViewModels")
        .Produces<ErrorViewModel[]>(StatusCodes.Status200OK);

        routes.MapGet("/api/ErrorViewModel/{id}", (int id) =>
        {
            //return new ErrorViewModel { ID = id };
        })
        .WithName("GetErrorViewModelById")
        .Produces<ErrorViewModel>(StatusCodes.Status200OK);

        routes.MapPut("/api/ErrorViewModel/{id}", (int id, ErrorViewModel input) =>
        {
            return Results.NoContent();
        })
        .WithName("UpdateErrorViewModel")
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/ErrorViewModel/", (ErrorViewModel model) =>
        {
            //return Results.Created($"//api/ErrorViewModels/{model.ID}", model);
        })
        .WithName("CreateErrorViewModel")
        .Produces<ErrorViewModel>(StatusCodes.Status201Created);

        routes.MapDelete("/api/ErrorViewModel/{id}", (int id) =>
        {
            //return Results.Ok(new ErrorViewModel { ID = id });
        })
        .WithName("DeleteErrorViewModel")
        .Produces<ErrorViewModel>(StatusCodes.Status200OK);
    }
}}