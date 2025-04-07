namespace IsitechEfCoreApp.Entities;

public class TodoItem
{
  public int Id { get; set; } // Clé primaire
  public string Title { get; set; } // Titre de la tâche
  public bool IsComplete { get; set; } // Indique si la tâche est terminée
}