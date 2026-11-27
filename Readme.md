# ArquiteturaBaseApi - Template para Web API .NET 10

Este repositório contém uma arquitetura base para criação de APIs RESTful em .NET 10, utilizando MongoDB, JWT, Rate Limiting, Health Checks e tratamento global de exceções. O projeto foi projetado para servir como template (`dotnet new baseapi`) para novas APIs.

---

## 📁 Estrutura de Pastas

```
.
├── Common/            # Tipos compartilhados e exceções personalizadas (AppException, NotFoundException, etc.)
├── Config/            # Métodos de extensão para configuração de serviços (DI, Mongo, JWT, Swagger, RateLimiter, Cors, HealthCheck)
├── Controllers/       # Controllers HTTP enxutos (delegam regras para os Services)
├── DTOs/              # Data Transfer Objects (Records para requests e responses)
├── Middlewares/       # Middlewares customizados (Tratamento global de exceções)
├── Models/            # Entidades de domínio persistidas no MongoDB
├── Repositories/      # Acesso aos dados no MongoDB (interfaces em Repositories/Interfaces)
├── Services/          # Regras de negócio da aplicação (interfaces em Services/Interfaces)
└── Settings/          # Classes de configuração tipadas (Options pattern - MongoDbSettings, JwtSettings)
```

---

## ⚙️ Configurações Necessárias (`appsettings.json`)

Para executar a aplicação, certifique-se de preencher as seções `MongoDbSettings` e `JwtSettings` no arquivo `appsettings.json` (ou via variáveis de ambiente):

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "ArquiteturaBaseDb"
  },
  "JwtSettings": {
    "SecretKey": "SUA_CHAVE_SECRETA_COM_PELO_MENOS_32_CARACTERES"
  }
}
```

---

## 🚀 Endpoints Disponíveis

| Método | Endpoint | Autenticação | Descrição |
|--------|----------|--------------|-----------|
| `GET` | `/health` | Pública | Health check da aplicação e do MongoDB |
| `GET` | `/api/HelloWorld/publico` | Pública | Retorna mensagem de teste pública |
| `GET` | `/api/HelloWorld/privado` | Bearer JWT | Retorna mensagem de teste e status do MongoDB |
| `POST` | `/api/Auth/login` | Pública (Rate Limit) | Autentica usuário e retorna o token JWT |
| `POST` | `/api/User` | Bearer JWT | Cadastra um novo usuário |
| `GET` | `/api/User` | Bearer JWT | Listagem paginada de usuários (`pageNumber`, `pageSize`) |

---

## 💻 Instruções de Execução

### Pré-requisitos
- .NET 10 SDK instalado (`dotnet --version`)
- Instância do MongoDB em execução

### Executar a API
1. Restaure as dependências:
   ```powershell
   dotnet restore
   ```
2. Executar a aplicação:
   ```powershell
   dotnet run
   ```
3. Acesse a documentação do Swagger no navegador em `http://localhost:5159` ou na raiz (`http://localhost:8080`).

---

## 📦 Como Usar como Template (`dotnet new`)

Para instalar o template localmente e criar novos projetos:

1. **Instalar o template a partir da pasta raiz:**
   ```powershell
   dotnet new install .
   ```

2. **Criar um novo projeto baseado no template:**
   ```powershell
   dotnet new baseapi -n MeuNovoProjeto
   ```
