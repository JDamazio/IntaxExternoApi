# IntaxExterno - API

API voltada para disponibilização aos clientes desenvolvida com .NET 10 & PostgreSQL.

---

## Tecnologias Utilizadas

### Backend & Core:
- **.NET 10.0** - Plataforma de desenvolvimento para construir APIs e aplicações web
- **PostgreSQL** - Sistema de gerenciamento de banco de dados relacional
- **Entity Framework Core** - ORM para .NET
- **JWT** - Autenticação baseada em tokens
- **Swagger** - Documentação interativa da API
- **AutoMapper** - Mapeamento de objetos
- **Identity** - Sistema de autenticação e autorização

### Infraestrutura:
- **Docker & Docker Compose** - Containerização completa da aplicação
- **RabbitMQ** - Sistema de mensageria assíncrona

---

## Pré-requisitos

- .NET 10.0 SDK ou superior
- PostgreSQL 12 ou superior
- Git

---

## Instalação e Execução

### 1️⃣ Clonar o repositório:
```sh
git clone [URL_DO_REPOSITORIO]
cd IntaxExternoApi
```

### 2️⃣ Instalar as dependências:
```sh
dotnet restore
```

### 3️⃣ Configurar variáveis de ambiente:
Configure a connection string no arquivo `appsettings.json` ou `appsettings.Development.json`.

### 4️⃣ Executar o projeto em desenvolvimento:
```sh
dotnet run --project src/IntaxExterno.Api
```

### 5️⃣ Gerar build para produção:
```sh
dotnet publish -c Release -o ./published
```

---

## Estrutura do Projeto

```
src/
├── IntaxExterno.Api/               # Controllers, Program.cs, Helpers
├── IntaxExterno.Application/       # Services, DTOs, Interfaces, Mappings
├── IntaxExterno.Domain/            # Entities, Interfaces, Responses
├── IntaxExterno.Infra.Data/        # Context, Repositories, Identity
├── IntaxExterno.Infra.IoC/         # Dependency Injection
└── IntaxExterno.Infra.Messaging/   # RabbitMQ
```

---

## Arquitetura

O projeto segue os princípios da **Clean Architecture** / **Onion Architecture**:

- **Domain**: Entidades de negócio e interfaces
- **Application**: Lógica de aplicação, DTOs e serviços
- **Infrastructure**: Implementações de infraestrutura (Data, IoC, Messaging)
- **API**: Controllers e configurações da API

---

## Padrões Utilizados

- Repository Pattern
- Service Layer Pattern
- Dependency Injection
- DTO Pattern
- Response Pattern
- AutoMapper
- Soft Delete
- Auditoria completa (Created/Updated/Deleted By/At)

---

## Versão

**Versão:** 1.0.0
**Última atualização:** Novembro 2025
