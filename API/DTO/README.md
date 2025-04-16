# Guide d'utilisation des DTOs et des Migrations Entity Framework Core

Ce document explique comment utiliser les DTOs (Data Transfer Objects) et les migrations Entity Framework Core dans ce projet.

## DTOs (Data Transfer Objects)

Les DTOs sont utilisés pour transférer des données entre les couches de l'application, notamment entre l'API et les clients. Ils permettent de :
- Contrôler les données exposées par l'API
- Valider les données entrantes
- Adapter le format des données aux besoins spécifiques

### Types de DTOs

Pour chaque entité, trois types de DTOs ont été créés :

1. **EntityDto** - Pour retourner les données complètes d'une entité
2. **EntityCreateDto** - Pour créer une nouvelle entité
3. **EntityPatchDto** - Pour mettre à jour partiellement une entité

### Liste des DTOs disponibles

- **Event** : EventDto, EventCreateDto, EventPatchDto
- **Participant** : ParticipantDto, ParticipantCreateDto, ParticipantPatchDto
- **Session** : SessionDto, SessionCreateDto, SessionPatchDto
- **Speaker** : SpeakerDto, SpeakerCreateDto, SpeakerPatchDto
- **Location** : LocationDto, LocationCreateDto, LocationPatchDto
- **Room** : RoomDto, RoomCreateDto, RoomPatchDto
- **Rating** : RatingDto, RatingCreateDto, RatingPatchDto

## Migrations Entity Framework Core

Les migrations permettent de gérer les changements de schéma de base de données de manière contrôlée et reproductible.

### Installation des outils dotnet-ef

Avant de pouvoir utiliser les commandes dotnet-ef, vous devez installer l'outil global :

```bash
dotnet tool install --global dotnet-ef
```

Pour mettre à jour l'outil à la dernière version :

```bash
dotnet tool update --global dotnet-ef
```

### Commandes principales

#### Créer une nouvelle migration

```bash
dotnet ef migrations add NomDeLaMigration --project Infrastructure --startup-project API
```

#### Appliquer les migrations à la base de données

```bash
dotnet ef database update --project Infrastructure --startup-project API
```

#### Lister les migrations existantes

```bash
dotnet ef migrations list --project Infrastructure --startup-project API
```

#### Supprimer la dernière migration (si elle n'a pas été appliquée)

```bash
dotnet ef migrations remove --project Infrastructure --startup-project API
```

#### Générer un script SQL pour une migration

```bash
dotnet ef migrations script MigrationPrécédente MigrationCible --project Infrastructure --startup-project API --output script.sql
```

Pour générer un script depuis le début jusqu'à la dernière migration :

```bash
dotnet ef migrations script 0 --project Infrastructure --startup-project API --output script.sql
```

### Bonnes pratiques pour les migrations

1. **Créez des migrations pour chaque changement significatif** du modèle de données
2. **Testez les migrations** dans un environnement de développement avant de les appliquer en production
3. **Vérifiez les scripts générés** pour comprendre les changements qui seront appliqués
4. **Utilisez des noms descriptifs** pour les migrations (ex: "AjoutTableUtilisateurs", "ModificationChampEmail")
5. **Conservez toutes les migrations** dans le contrôle de source
6. **Ne modifiez jamais une migration existante** qui a déjà été appliquée en production

## Exemple d'utilisation

### Scénario : Ajout d'un nouveau champ à une entité

1. Modifiez la classe d'entité dans le projet Domain
2. Mettez à jour les DTOs correspondants dans le projet API
3. Créez une nouvelle migration :
   ```bash
   dotnet ef migrations add AjoutChampXYZ --project Infrastructure --startup-project API
   ```
4. Vérifiez le code de la migration générée
5. Appliquez la migration à la base de données :
   ```bash
   dotnet ef database update --project Infrastructure --startup-project API
   ```
6. Mettez à jour les services et contrôleurs pour utiliser le nouveau champ

## Résolution des problèmes courants

### La commande dotnet-ef n'est pas reconnue

Assurez-vous que l'outil dotnet-ef est installé globalement :
```bash
dotnet tool install --global dotnet-ef
```

### Erreur lors de la création d'une migration

Vérifiez que :
- Le DbContext est correctement configuré
- Les entités sont correctement définies
- Les références entre les projets sont correctes

### Erreur lors de l'application d'une migration

Vérifiez que :
- La chaîne de connexion à la base de données est correcte
- Vous avez les droits suffisants sur la base de données
- La base de données est accessible