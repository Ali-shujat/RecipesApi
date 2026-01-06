# Contributing — DTO organization and project conventions

## Purpose
This document defines the project's conventions for Data Transfer Objects (DTOs), their location, naming, validation and mapping practices. It helps keep the codebase consistent and reduces file clutter by grouping related DTOs.

## Directory structure
- Place all DTO types under `Models/Dtos/`.
- Group DTOs by aggregate (e.g., Recipes, Comments/Ratings) into a small number of files. Example:
  - `Models/Dtos/RecipeDtos.cs` — `RecipeDto`, `RecipeCreateDto`, `RecipeUpdateDto`, etc.
  - `Models/Dtos/InteractionDtos.cs` — `CommentDto`, `CommentCreateDto`, `RatingDto`, `RatingCreateDto`.

Rationale: grouping related DTOs reduces the number of files while keeping types discoverable.

## Naming conventions
- Suffix DTO types with `Dto`, `CreateDto`, or `UpdateDto` depending on purpose.
  - `RecipeDto` — response DTO for recipe details.
  - `RecipeCreateDto` — fields required for creation.
  - `RecipeUpdateDto` — fields allowed for update.
- Keep property names PascalCase and use data-annotation attributes for basic validation.

## Validation
- Use data-annotations on Create/Update DTOs (e.g., `[Required]`, `[Range]`) and validate `ModelState` in controllers.
- Avoid placing validation attributes on response DTOs unless necessary for model binding.

## Mapping
- Prefer projection in EF Core queries where possible to avoid materializing full entities.
- For non-trivial mapping, use a mapper (AutoMapper) or small manual mapping helpers.

## Controller patterns
- Controllers should accept Create/Update DTOs, map to entities server-side, and return response DTOs.
- Avoid returning EF entities from controller actions directly.

## Serialization and cycles
- DTO projection prevents serialization cycles; if entities must be returned, configure JSON to ignore cycles in `Program.cs` or use `[JsonIgnore]` on back-references.

## File maintenance
- When adding new DTOs for an existing aggregate, add them to the existing aggregate DTO file rather than creating a new single-type file.
- If a DTO file grows extremely large (>300 lines), consider splitting it by responsibility (e.g., `RecipeDtos.Paged.cs`).

## Examples
- `Models/Dtos/RecipeDtos.cs` contains `RecipeDto`, `RecipeCreateDto`, `RecipeUpdateDto`.
- `Models/Dtos/InteractionDtos.cs` contains comment and rating DTOs.

## Tests and reviews
- Add unit tests for mapping projections where business logic exists in mapping.
- Pull requests that add DTOs must follow these conventions; reviewers should request consolidation if the repository accumulates single-type DTO files.