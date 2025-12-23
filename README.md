# To Do List Poker

Proyecto **full-stack** de gestión de tareas.  
Consta de un **backend en ASP.NET Core** y un **frontend en Angular**, comunicados mediante una API REST.

El objetivo del proyecto es utilizar angular y practicar el lenguaje c# e ir adquiriendo conocimientos de **arquitectura**, **buenas prácticas**, **testing**, **frontend moderno** y **automatización de ejecución**.

---

## ¿De qué va el proyecto?

**To do List** es una aplicación que permite gestionar tareas de forma visual e intuitiva.

El usuario puede:

- Crear tareas
- Ver tareas en formato de cartas
- Editar tareas
- Eliminar tareas con confirmación visual personalizada
- Marcar tareas como completadas / no completadas
- Filtrar tareas
- Gestionar el flujo de tareas mediante una interfaz visual (deck / discard)

El proyecto está dividido claramente en **backend** y **frontend**, ambos desarrollados siguiendo buenas prácticas y una arquitectura limpia.

---

## Arquitectura del Backend

El backend está desarrollado en **ASP.NET Core** siguiendo **Arquitectura Hexagonal (Clean Architecture)**.

Estructura:

backend/

└─ src/

├─ TaskManager.Domain

├─ TaskManager.Application

├─ TaskManager.Infrastructure

├─ TaskManager.Api

└─ TaskManager.Tests


### 🔹 Domain
- Contiene las **entidades del dominio** (`TaskItem`)
- Reglas de negocio puras
- No depende de frameworks ni infraestructura

### 🔹 Application
- Casos de uso
- Servicios de aplicación
- Interfaces de repositorios
- Validaciones de negocio (por ejemplo, título obligatorio)

### 🔹 Infrastructure
- Implementación concreta de repositorios
- Acceso a datos con **Entity Framework Core**
- Base de datos **InMemory**

### 🔹 API
- Controladores REST
- Exposición de endpoints HTTP:
  - `GET /api/tasks`
  - `POST /api/tasks`
  - `PUT /api/tasks/{id}`
  - `DELETE /api/tasks/{id}`

---

## Frontend (Angular)

El frontend está desarrollado en **Angular** y consume la API REST del backend.

### Funcionalidades implementadas

- Listado de tareas en formato **cartas**
- Creación de nuevas tareas
- Edición de tareas existentes
- Eliminación de tareas con **vista de confirmación personalizada**
- Marcado de tareas como completadas / incompletas
- Filtrado de tareas
- Paginación
- Animaciones y efectos visuales (hover, transiciones)
- Comunicación con el backend mediante **servicios Angular**

### Tecnologías usadas

- Angular
- TypeScript
- HTML
- SCSS
- Arquitectura por componentes
- Servicios para comunicación con API

---

## Tests

Se han implementado **tests en el backend**.



### Qué se valida con los tests

- Creación de tareas
- Validaciones de dominio (el título no puede estar vacío)
- Comportamiento del servicio de tareas
- Funcionamiento de los casos de uso
- Integración con base de datos **InMemory**

### Tecnologías de testing

- **xUnit**
- **Entity Framework Core InMemory**

---

## ¿Cómo ejecutar la API?

### Requisitos

- **.NET SDK 10.0**
- Node.js
- Angular CLI

El proyecto ya tiene un **.bat** que inicia el proyecto directamente. Tras ejecutarlo, ya se puede ver el proyecto en la ruta http://localhost:4200/

