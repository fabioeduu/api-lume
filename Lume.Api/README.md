# Lume API - Saúde Emocional

API RESTful desenvolvida em ASP.NET Core para o aplicativo Lume, focado em saúde emocional com recursos de registro de emoções, histórico de check-ins, e chat de suporte emocional.

## 📋 Requisitos
- .NET 8.0 ou superior
- SQL Server (LocalDB ou instalado)
- Visual Studio 2022 ou Visual Studio Code

## 🚀 Configuração Inicial

## 1. Clonar ou abrir o projeto

```bash
cd Lume.Api
```

## 2. Configurar banco de dados

Abra o arquivo `appsettings.json` e ajuste a connection string para SQL Server:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=LumeDb;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

**Para usar LocalDB:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LumeDb;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

## 3. Configurar JWT Secret

No `appsettings.json`, altere o JWT Secret para uma chave mais segura em produção:

```json
"Jwt": {
  "Secret": "sua-chave-secreta-muito-segura-com-minimo-32-caracteres",
  "Issuer": "Lume.Api",
  "Audience": "Lume.Client",
  "ExpirationMinutes": 60
}
```

### 4. Restaurar dependências e executar a aplicação

```bash
dotnet restore
dotnet run
```

**A API rodará em:**
- 🌐 HTTP: `http://localhost:5007`

## 📚 Documentação da API




### Fluxo de Autenticação:

1. **Registrar novo usuário** → Receber token JWT
2. **Fazer login** → Receber token JWT
3. **Usar token** em requisições autenticadas no header `Authorization: Bearer {token}`

## 📡 Endpoints
```
AUTENTICAÇÃO
┌─────────────────────────────────────────────────────────────┐
│ POST   /api/auth/register              [201] - Novo usuário │
│ POST   /api/auth/login                 [200] - Login        │
└─────────────────────────────────────────────────────────────┘

USUÁRIOS
┌─────────────────────────────────────────────────────────────┐
│ GET    /api/users/{id}                 [200] - Obter perfil │
│ GET    /api/users/profile/me           [200] - Meu perfil   │
│ PUT    /api/users                      [200] - Atualizar    │
│ DELETE /api/users                      [200] - Deletar      │
└─────────────────────────────────────────────────────────────┘

CHECK-INS
┌─────────────────────────────────────────────────────────────┐
│ POST   /api/checkins                   [201] - Criar        │
│ GET    /api/checkins/{id}              [200] - Obter um     │
│ GET    /api/checkins/my-checkins       [200] - Meus         │
│ GET    /api/checkins/user/{id}         [200] - De usuário   │
│ PUT    /api/checkins/{id}              [200] - Atualizar    │
│ DELETE /api/checkins/{id}              [200] - Deletar      │
└─────────────────────────────────────────────────────────────┘

CHAT
┌─────────────────────────────────────────────────────────────┐
│ POST   /api/chat/message               [201] - Enviar msg   │
│ GET    /api/chat/history               [200] - Meu hist.    │
│ GET    /api/chat/history/{userId}      [200] - Hist. user   │
│ DELETE /api/chat/message/{id}          [200] - Deletar msg  │
└─────────────────────────────────────────────────────────────┘

[🔒] = Requer autenticação JWT
```

### 🔑 Autenticação

## Registrar novo usuário
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "usuario@email.com",
  "password": "senha123",
  "fullName": "Nome do Usuário"
}
```

**Resposta (200):**
```json
{
  "success": true,
  "message": "User registered successfully",
  "token": "eyJhbGc...",
  "user": {
    "id": 1,
    "email": "usuario@email.com",
    "fullName": "Nome do Usuário",
    "bio": null,
    "createdAt": "2025-11-22T10:30:00Z",
    "isActive": true
  }
}
```

## Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@email.com",
  "password": "senha123"
}
```

### 👤 Usuários

#### Obter perfil do usuário
```http
GET /api/users/{id}
Authorization: Bearer {token}
```

#### Obter meu perfil
```http
GET /api/users/profile/me
Authorization: Bearer {token}
```

#### Atualizar perfil
```http
PUT /api/users
Authorization: Bearer {token}
Content-Type: application/json

{
  "fullName": "Novo Nome",
  "bio": "Minha bio"
}
```

#### Deletar conta
```http
DELETE /api/users
Authorization: Bearer {token}
```

### 📊 Check-ins

#### Criar novo check-in
```http
POST /api/checkins
Authorization: Bearer {token}
Content-Type: application/json

{
  "emotion": "alegre",
  "emotionalLevel": 8,
  "notes": "Tive um ótimo dia!"
}
```

**Emoções suportadas:** alegre, triste, ansioso, calmo, frustrado, esperançoso

#### Obter um check-in específico
```http
GET /api/checkins/{id}
Authorization: Bearer {token}
```

#### Obter histórico de check-ins (por usuário)
```http
GET /api/checkins/user/{userId}
Authorization: Bearer {token}

# Com filtro de datas
GET /api/checkins/user/{userId}?fromDate=2025-11-01&toDate=2025-11-30
Authorization: Bearer {token}
```

#### Obter meus check-ins
```http
GET /api/checkins/my-checkins
Authorization: Bearer {token}
```

#### Atualizar check-in
```http
PUT /api/checkins/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "emotion": "calmo",
  "emotionalLevel": 7,
  "notes": "Notas atualizadas"
}
```

#### Deletar check-in
```http
DELETE /api/checkins/{id}
Authorization: Bearer {token}
```

### 💬 Chat

#### Enviar mensagem
```http
POST /api/chat/message
Authorization: Bearer {token}
Content-Type: application/json

{
  "message": "Estou me sentindo ansioso"
}
```

A API responderá automaticamente com mensagens de suporte emocional.

#### Obter histórico de chat (por usuário)
```http
GET /api/chat/history/{userId}
Authorization: Bearer {token}

# Com limite de mensagens
GET /api/chat/history/{userId}?limit=50
Authorization: Bearer {token}
```

#### Obter meu histórico de chat
```http
GET /api/chat/history
Authorization: Bearer {token}

# Com limite
GET /api/chat/history?limit=50
Authorization: Bearer {token}
```

#### Deletar mensagem
```http
DELETE /api/chat/message/{messageId}
Authorization: Bearer {token}
```

## 🏗️ Estrutura do Projeto

```
Lume.Api/
├── Models/                # Entidades de negócio
│   ├── User.cs
│   ├── Checkin.cs
│   └── ChatMessage.cs
├── Controllers/           # Controllers RESTful
│   ├── AuthController.cs
│   ├── UsersController.cs
│   ├── CheckinsController.cs
│   └── ChatController.cs
├── Services/              # Lógica de negócio
│   ├── TokenService.cs
│   ├── AuthService.cs
│   ├── UserService.cs
│   ├── CheckinService.cs
│   └── ChatService.cs
├── Repositories/          # Acesso aos dados
│   ├── Repository.cs (genérico)
│   ├── UserRepository.cs
│   ├── CheckinRepository.cs
│   └── ChatMessageRepository.cs
├── DTOs/                  # Data Transfer Objects
│   ├── UserDto.cs
│   ├── CheckinDto.cs
│   ├── ChatMessageDto.cs
│   └── AuthResponseDto.cs
├── Data/
│   ├── LumeContext.cs     # DbContext
│   └── Migrations/        # Migrações EF Core
├── appsettings.json       # Configurações
└── Program.cs             # Startup
```

## 🗄️ Banco de Dados

### Tabelas

- **Users**: Usuários registrados
- **Checkins**: Registros de check-ins emocionais
- **ChatMessages**: Histórico de mensagens de chat

### Relacionamentos

- Um usuário pode ter muitos check-ins (1:N)
- Um usuário pode ter muitas mensagens de chat (1:N)
- Relacionamentos com cascade delete

## 🔒 Segurança

- ✅ Autenticação JWT com token Bearer
- ✅ Hash de senha com BCrypt
- ✅ HTTPS obrigatório
- ✅ CORS configurável
- ✅ Validação de dados em DTOs
- ✅ Autorização por atributo `[Authorize]`

## 📝 Exemplo de Uso Completo

### 1. Registrar
```bash
curl -X POST https://localhost:5007/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "joao@email.com",
    "password": "senha123",
    "fullName": "João Silva"
  }'
```

### 2. Login
```bash
curl -X POST https://localhost:5007/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "joao@email.com",
    "password": "senha123"
  }'
```

Guarde o token retornado.

### 3. Criar check-in
```bash
curl -X POST https://localhost:5007/api/checkins \
  -H "Authorization: Bearer {TOKEN}" \
  -H "Content-Type: application/json" \
  -d '{
    "emotion": "alegre",
    "emotionalLevel": 8,
    "notes": "Dia fantástico!"
  }'
```

### 4. Enviar mensagem de chat
```bash
curl -X POST https://localhost:5007/api/chat/message \
  -H "Authorization: Bearer {TOKEN}" \
  -H "Content-Type: application/json" \
  -d '{
    "message": "Estou me sentindo bem hoje"
  }'
```

Fabio H S Eduardo - RM560416
Gabriel Wu Castro - RM560210
Renato Kenji Sugaki - RM559810

Projeto academico desenvolvido para a disciplina de Advanced Business Development with .NET

**Desenvolvido para apoiar a saúde emocional**
