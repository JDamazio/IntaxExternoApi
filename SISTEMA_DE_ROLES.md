# Sistema de Roles - Intax Externo

## Implementação Completa

Este documento descreve o sistema de roles e autenticação implementado no Intax Externo API e Frontend.

---

## Backend (.NET API)

### Roles Disponíveis

O sistema possui 2 roles definidas no enum `RoleType`:

1. **Admin** - Acesso completo ao sistema
2. **Cliente** - Acesso limitado (apenas Relatórios de Crédito e Compensação)

### Arquivos Criados/Modificados

#### 1. Domain Layer
- `src/IntaxExterno.Domain/Enums/RoleType.cs` - Enum com as roles do sistema
- `src/IntaxExterno.Domain/Enums/RoleTypeExtensions.cs` - Extensões para trabalhar com roles
- `src/IntaxExterno.Domain/Interfaces/ISeedUserAndRoleInitial.cs` - Interface para seed

#### 2. Infrastructure Layer
- `src/IntaxExterno.Infra.Data/Helpers/SeedUserAndRoleInitial.cs` - Implementação de seed
- `src/IntaxExterno.Infra.IoC/DependecyInjection.cs` - Configuração de policies e registro do seed

#### 3. API Layer
- `src/IntaxExterno.Api/Program.cs` - Chamada do seed na inicialização
- `src/IntaxExterno.Api/Controllers/AuthController.cs` - Uso do enum RoleType

### Configuração de Policies

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy(RoleType.Admin.ToString(),
        policy => policy.RequireRole(RoleType.Admin.ToString()));

    options.AddPolicy(RoleType.Cliente.ToString(),
        policy => policy.RequireRole(RoleType.Cliente.ToString()));
});
```

### Seed Automático

Ao iniciar a aplicação, as seguintes roles e usuários são criados automaticamente:

**Roles:**
- Admin
- Cliente

**Usuários de Teste:**
- **Admin**: `test.admin@intaxexterno.com` (senha: `Test@123`)
- **Cliente**: `test.cliente@intaxexterno.com` (senha: `Test@123`)

### JWT Token

O token JWT inclui as seguintes claims:
- `Name` - Nome do usuário
- `Email` - Email do usuário
- `Sub` - ID do usuário
- `Jti` - ID único do token
- `Roles` - Roles do usuário (uma claim por role)

**Expiração:** 10 horas

**Formato do claim de role:**
```json
{
  "Roles": "Admin"
}
```

### Como Usar Autorização nos Controllers

#### Exemplo 1: Autorização por Role
```csharp
[Authorize(Roles = nameof(RoleType.Admin))]
public async Task<ActionResult> AdminOnlyEndpoint()
{
    // Apenas Admin pode acessar
}
```

#### Exemplo 2: Múltiplas Roles
```csharp
[Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Cliente)}")]
public async Task<ActionResult> AdminOrClienteEndpoint()
{
    // Admin OU Cliente podem acessar
}
```

#### Exemplo 3: Apenas Autenticação (sem role específica)
```csharp
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public async Task<ActionResult> AuthenticatedEndpoint()
{
    // Qualquer usuário autenticado pode acessar
}
```

---

## Frontend (Vue.js)

### Implementação

#### 1. Tipos TypeScript
- `src/types/api.d.ts` - Tipo `UserRole = 'Admin' | 'Cliente'`

#### 2. Auth Store
- `src/stores/auth.ts` - Decodificação de JWT e gerenciamento de roles
- Getters: `isAdmin`, `isCliente`, `userRole`
- Função `decodeJWT()` para extrair roles do token

#### 3. Menu Filtrado
- `src/stores/menu.ts` - Filtragem automática de menus por role
- `src/main/side-menu.ts` - Menus com propriedade `roles`

#### 4. Proteção de Rotas
- `src/router/index.ts` - Guards de rota baseados em roles
- Redirecionamento automático baseado no role do usuário

### Distribuição de Acesso

**Admin tem acesso a:**
- Todos os Dashboards
- Gestão (Clientes, Parceiros, Teses, Propostas, Oportunidades, Usuários)
- Relatórios de Crédito
- Cálculos (PERSE, Exclusão ICMS)
- Compensação

**Cliente tem acesso a:**
- Relatórios de Crédito
- Compensação

### Configuração da API

Arquivo `.env`:
```
VITE_API_BASE_URL=https://localhost:63313
```

---

## Como Testar

### 1. Iniciar a API

```bash
cd C:\Projetos\Intax\Intax_Externo\IntaxExternoApi
dotnet run --project src/IntaxExterno.Api
```

A API iniciará em `https://localhost:63313`

### 2. Verificar Seed

Ao iniciar, a API automaticamente:
1. Aplica migrações pendentes
2. Cria as roles Admin e Cliente
3. Cria usuários de teste

Você verá no console que o seed foi executado.

### 3. Testar Login como Admin

**Endpoint:** `POST https://localhost:63313/api/Auth/Login`

**Request:**
```json
{
  "email": "test.admin@intaxexterno.com",
  "password": "Test@123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2025-12-15T10:00:00Z",
  "message": "Token created successfully"
}
```

**Decodificando o Token:**
O token JWT conterá:
```json
{
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": "Test Admin",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": "test.admin@intaxexterno.com",
  "sub": "user-id",
  "jti": "guid",
  "Roles": "Admin",
  "exp": 1234567890
}
```

### 4. Testar Login como Cliente

**Request:**
```json
{
  "email": "test.cliente@intaxexterno.com",
  "password": "Test@123"
}
```

O token conterá `"Roles": "Cliente"`

### 5. Testar Frontend

```bash
cd "C:\Users\João Damazio\Downloads\themeforest-vcvSUIJk-midone-vuejs-admin-dashboard-template\Midone Vue\Vue Version\Source"
npm run dev
```

**Login como Admin:**
1. Acesse `http://localhost:5173/login`
2. Use: `test.admin@intaxexterno.com` / `Test@123`
3. Você será redirecionado para `/` (Dashboard)
4. Verá todos os itens do menu

**Login como Cliente:**
1. Acesse `http://localhost:5173/login`
2. Use: `test.cliente@intaxexterno.com` / `Test@123`
3. Você será redirecionado para `/relatorios-credito-perse`
4. Verá apenas "Relatórios de Crédito" e "Compensação" no menu

### 6. Testar Proteção de Rotas

**Como Cliente:**
- Tente acessar `http://localhost:5173/clientes`
- Você será redirecionado automaticamente para `/relatorios-credito-perse`
- A API retornará 403 (Forbidden) se tentar acessar endpoints de Admin

**Como Admin:**
- Você pode acessar qualquer rota
- A API permitirá acesso a todos os endpoints

---

## Estrutura do JWT

### Claims Incluídas

```javascript
{
  // Identificação do usuário
  "sub": "user-id",                    // ID do usuário (Subject)
  "jti": "unique-token-id",            // ID único do token

  // Informações do usuário
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": "Nome do Usuário",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": "email@example.com",

  // Roles (pode ser múltiplas)
  "Roles": "Admin",                    // ou "Cliente"

  // Expiração
  "exp": 1234567890,                   // Unix timestamp (10 horas)
  "iss": "https://api.intaxexterno.com.br",
  "aud": "IntaxExternoClients"
}
```

### Como o Frontend Extrai as Roles

```typescript
function decodeJWT(token: string): any {
  const base64Url = token.split('.')[1];
  const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
  const jsonPayload = decodeURIComponent(
    atob(base64)
      .split('')
      .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
      .join('')
  );
  return JSON.parse(jsonPayload);
}

// Extrai roles (suporta múltiplos formatos de claims .NET)
let roles: string[] = [];
if (decodedToken.role) {
  roles = Array.isArray(decodedToken.role) ? decodedToken.role : [decodedToken.role];
} else if (decodedToken.roles) {
  roles = Array.isArray(decodedToken.roles) ? decodedToken.roles : [decodedToken.roles];
} else if (decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']) {
  const rolesClaim = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  roles = Array.isArray(rolesClaim) ? rolesClaim : [rolesClaim];
}
```

---

## Troubleshooting

### Problema: Roles não aparecem no token

**Solução:** Verifique se o usuário tem roles atribuídas:
```csharp
var userRoles = await _userManager.GetRolesAsync(user);
Console.WriteLine($"User {user.Email} has roles: {string.Join(", ", userRoles)}");
```

### Problema: Frontend não reconhece as roles

**Solução:** Verifique se o `RoleClaimType` está configurado:
```csharp
RoleClaimType = "Roles"  // No DependencyInjection.cs
```

### Problema: Menu não filtra corretamente

**Solução:** Verifique se o usuário está salvo corretamente no localStorage:
```javascript
console.log('User data:', localStorage.getItem('user_data'));
```

### Problema: Redirecionamento não funciona

**Solução:** Verifique os guards de rota no `router/index.ts` e o `userRole` no store.

---

## Próximos Passos

1. **Adicionar mais roles** se necessário (editar `RoleType` enum)
2. **Criar usuários reais** via endpoint de registro
3. **Implementar gestão de usuários** para admins atribuírem roles
4. **Adicionar auditoria** de ações baseadas em roles
5. **Implementar refresh tokens** para renovação automática

---

## Segurança

- Tokens expiram em 10 horas
- Senha mínima de 6 caracteres (configurável no IdentityOptions)
- HTTPS obrigatório em produção
- CORS configurado para permitir apenas origens confiáveis
- Roles validadas tanto no backend quanto no frontend
- Proteção contra acesso direto a rotas via URL
