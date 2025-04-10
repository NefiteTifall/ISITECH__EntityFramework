using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Middleware pour intercepter et gérer les exceptions de manière centralisée dans l'application.
/// Convertit les exceptions en réponses HTTP structurées et cohérentes.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initialise une nouvelle instance du middleware de gestion d'exceptions.
    /// </summary>
    /// <param name="next">Le délégué représentant le prochain middleware dans le pipeline.</param>
    /// <param name="logger">Le logger pour enregistrer les détails des exceptions.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Traite la requête HTTP et capture toute exception non gérée.
    /// </summary>
    /// <param name="context">Le contexte HTTP de la requête en cours.</param>
    /// <returns>Une tâche représentant l'opération asynchrone.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Passe la requête au prochain middleware dans le pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Journalise l'exception avec tous ses détails
            _logger.LogError(ex, "Une exception non gérée s'est produite lors du traitement de la requête");
            
            // Transforme l'exception en réponse HTTP appropriée
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Convertit l'exception en réponse HTTP appropriée avec un format standardisé.
    /// </summary>
    /// <param name="context">Le contexte HTTP de la requête.</param>
    /// <param name="exception">L'exception à traiter.</param>
    /// <returns>Une tâche représentant l'opération asynchrone.</returns>
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Définit le type de contenu de la réponse comme JSON
        context.Response.ContentType = "application/json";
        
        // Détermine le code de statut HTTP et le message appropriés en fonction du type d'exception
        var (statusCode, message) = exception switch
        {
            // Ressource non trouvée
            KeyNotFoundException _ => (StatusCodes.Status404NotFound, 
                                      "La ressource demandée n'a pas été trouvée"),
            
            // Requête invalide
            ArgumentException _ => (StatusCodes.Status400BadRequest, 
                                   exception.Message),
            
            // Toute autre exception est considérée comme une erreur interne du serveur
            _ => (StatusCodes.Status500InternalServerError, 
                 "Une erreur interne s'est produite")
        };

        // Définit le code de statut HTTP dans la réponse
        context.Response.StatusCode = statusCode;

        // Crée un objet ProblemDetails conforme à la RFC 7807 pour les détails de l'erreur
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = message,            // Message d'erreur général
            Detail = exception.Message, // Détails spécifiques de l'exception
            Instance = context.Request.Path // URL qui a généré l'erreur
        };

        // Écrit la réponse JSON
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}