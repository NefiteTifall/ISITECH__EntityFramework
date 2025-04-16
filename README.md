# Système de Gestion d'Événements - API REST

Ce projet est une API REST pour une plateforme de gestion d'événements professionnels (conférences, salons, workshops, etc.). L'API permet la gestion complète des événements, des participants, des sessions, des intervenants et des lieux.

## Architecture du Projet

Le projet suit une architecture en couches (Clean Architecture) avec les composants suivants :

### Couches

- **API** : Contrôleurs REST, DTOs, Filtres, Middleware et Mapping
- **Application** : Services d'application qui implémentent la logique métier
- **Domain** : Entités, Interfaces et Services de domaine
- **Infrastructure** : Implémentation des repositories, configuration de la base de données et migrations

### Structure des Dossiers

```
ISITECH__EventsArea/
├── API/
│   ├── Controllers/       # Contrôleurs REST
│   ├── DTO/               # Objets de transfert de données
│   ├── Filters/           # Filtres ASP.NET Core
│   ├── Mapping/           # Configuration AutoMapper
│   └── Middleware/        # Middleware personnalisé
├── Application/
│   └── Services/          # Implémentation des services
├── Domain/
│   ├── Entities/          # Entités du domaine
│   ├── Interfaces/        # Interfaces des repositories et services
│   └── Services/          # Interfaces des services
├── Infrastructure/
│   ├── Data/              # Configuration du DbContext
│   ├── Migrations/        # Migrations EF Core
│   └── Repositories/      # Implémentation des repositories
└── Tests/                 # Tests unitaires
```

## Choix Techniques

- **Framework** : ASP.NET Core 8
- **ORM** : Entity Framework Core 8
- **SGBD** : PostgreSQL
- **Architecture** : Clean Architecture
- **Documentation API** : Swagger
- **Mapping** : AutoMapper
- **Tests** : xUnit, Moq

## Modèle de Données

Le système comprend les entités principales suivantes :

- **Event** : Représente un événement avec titre, description, dates, statut, etc.
- **EventCategory** : Catégorie d'événement (conférence, workshop, séminaire, etc.)
- **Participant** : Utilisateur qui peut s'inscrire à des événements
- **EventParticipant** : Relation Many-to-Many entre Event et Participant
- **Session** : Session spécifique au sein d'un événement
- **Speaker** : Intervenant qui peut animer des sessions
- **SessionSpeaker** : Relation Many-to-Many entre Session et Speaker
- **Location** : Lieu où se déroule un événement
- **Room** : Salle spécifique au sein d'un lieu
- **Rating** : Évaluation d'une session par un participant

## Fonctionnalités Implémentées

- Gestion complète des événements (CRUD)
- Gestion des catégories d'événements (CRUD)
- Filtrage d'événements par date, lieu, catégorie, statut
- Pagination des résultats
- Gestion des lieux et des salles
- Gestion des participants et des intervenants
- Gestion des sessions
- Système de notation des sessions

## Installation et Configuration

### Prérequis

- .NET 8 SDK
- PostgreSQL

### Configuration

1. Clonez le dépôt :
   ```
   git clone <url-du-repo>
   cd ISITECH__EventsArea
   ```

2. Configurez la chaîne de connexion à la base de données dans `appsettings.json` :
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=eventsarea;Username=postgres;Password=yourpassword"
     }
   }
   ```

3. Appliquez les migrations pour créer la base de données :
   ```
   dotnet ef database update
   ```

4. Exécutez l'application :
   ```
   dotnet run
   ```

5. Accédez à l'interface Swagger pour tester l'API :
   ```
   https://localhost:5001/swagger
   ```

## Exemples d'Utilisation de l'API

### Récupérer tous les événements
```
GET /api/events
```

### Récupérer un événement spécifique
```
GET /api/events/{id}
```

### Créer un nouvel événement
```
POST /api/events
Content-Type: application/json

{
  "title": "Conférence Tech 2025",
  "description": "Une conférence sur les technologies émergentes",
  "startDate": "2025-06-15T09:00:00",
  "endDate": "2025-06-17T18:00:00",
  "status": "Scheduled",
  "categoryId": 1,
  "locationId": 1
}
```

### Mettre à jour un événement
```
PATCH /api/events/{id}
Content-Type: application/json

{
  "title": "Conférence Tech 2025 - Édition Spéciale",
  "description": "Une conférence sur les technologies émergentes et l'IA"
}
```

### Supprimer un événement
```
DELETE /api/events/{id}
```

### Récupérer toutes les catégories d'événements
```
GET /api/eventcategories
```

### Récupérer une catégorie spécifique
```
GET /api/eventcategories/{id}
```

### Créer une nouvelle catégorie
```
POST /api/eventcategories
Content-Type: application/json

{
  "name": "Formation",
  "description": "Session éducative approfondie"
}
```

### Mettre à jour une catégorie
```
PATCH /api/eventcategories/{id}
Content-Type: application/json

{
  "name": "Formation Avancée",
  "description": "Session éducative approfondie pour experts"
}
```

### Supprimer une catégorie
```
DELETE /api/eventcategories/{id}
```

## Tests

Le projet inclut des tests unitaires pour les services principaux. Pour exécuter les tests :

```
dotnet test
```

## Bonnes Pratiques Implémentées

- Utilisation des principes SOLID
- Validation des données entrantes
- Gestion appropriée des exceptions
- Documentation de l'API avec Swagger
- Utilisation de DTOs pour les réponses et requêtes API
- Pagination pour les listes potentiellement longues
- Transactions pour les opérations complexes
- Optimisation des requêtes avec Include, AsNoTracking, etc.
