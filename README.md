# 🚗 Taggy - Mobilidade Inteligente & Sustentável

Sistema de gerenciamento de pedágios, planejamento de viagens, controle financeiro, automação de recargas e análise de impacto ambiental para motoristas e frotas.

Desenvolvido como MVP acadêmico para a disciplina de Projetos da CESAR School.

---

# 📌 Objetivo

O Taggy foi criado para fornecer uma plataforma capaz de:

* Planejar viagens considerando custos reais.
* Calcular gastos com combustível ou energia elétrica.
* Calcular custos de pedágios.
* Gerenciar saldo de uma TAG virtual.
* Automatizar recargas de saldo.
* Registrar histórico de viagens.
* Gerenciar veículos e frotas.
* Medir economia de tempo em pedágios.
* Gerar indicadores ambientais.
* Fornecer métricas consolidadas de utilização da frota.

---

# 🚀 Funcionalidades

## 💳 Conta TAG Inteligente

Cada usuário possui automaticamente uma conta TAG.

Funcionalidades:

* Consulta de saldo.
* Recarga manual.
* Extrato financeiro.
* Integração com Auto Refill.
* Integração com Simulador de Pedágio.

Endpoints:

POST /api/v1/tag-account/recharge

GET /api/v1/tag-account/balance

GET /api/v1/tag-account/statement

---

## 🔄 Auto Refill

Monitoramento automático do saldo da TAG.

Quando o saldo atingir o limite mínimo configurado, o sistema realiza automaticamente uma recarga.

Endpoints:

GET /api/v1/auto-refill

POST /api/v1/auto-refill

Exemplo:

```json
{
  "enabled": true,
  "minimumBalance": 20,
  "rechargeAmount": 50
}
```

---

## 🛣 Simulador de Pedágio

Simula a passagem por uma praça de pedágio.

Funcionalidades:

* Débito automático.
* Registro financeiro.
* Integração com Auto Refill.

Endpoint:

POST /api/v1/toll/simulate

---

## 🚙 Gestão de Veículos

CRUD completo de veículos.

Tipos suportados:

* Combustion
* Electric
* Hybrid

Endpoints:

GET /api/v1/vehicles

GET /api/v1/vehicles/{id}

POST /api/v1/vehicles

PUT /api/v1/vehicles/{id}

DELETE /api/v1/vehicles/{id}

---

## 🚚 Importação em Massa de Veículos

Permite cadastrar múltiplos veículos simultaneamente.

Endpoint:

POST /api/v1/vehicles/bulk

Recursos:

* Cadastro em lote.
* Validação automática.
* Controle de duplicidade.
* Associação ao usuário autenticado.

Exemplo de retorno:

```json
{
  "created": 2,
  "duplicates": 0
}
```

---

## 🗺 Planejamento de Viagens

Calcula o custo total estimado de uma viagem.

Considera:

* Distância percorrida.
* Consumo do veículo.
* Preço do combustível.
* Preço da energia elétrica.
* Custos de pedágio.
* Emissão de CO₂.

Endpoint:

POST /api/v1/trips/calculate

---

## 📚 Histórico de Viagens

Permite registrar e consultar viagens realizadas.

Informações armazenadas:

* Veículo utilizado.
* Distância percorrida.
* Custos de combustível.
* Custos de energia.
* Custos de pedágio.
* Emissão de CO₂.
* Quantidade de passagens em pedágios.

Endpoints:

POST /api/v1/trips

GET /api/v1/trips

GET /api/v1/trips/{id}

---

## 🌱 Eco Simulator

Módulo responsável por indicadores ambientais.

Calcula:

* Emissões de CO₂.
* CO₂ evitado pelo uso da TAG.
* Impacto ambiental da frota.

---

# 📊 Fleet Analytics

Módulo de análise consolidada da frota.

## Dashboard

Endpoint:

GET /api/v1/fleet/dashboard

Indicadores:

* Total de veículos.
* Total de viagens.
* Distância total percorrida.
* Gasto total com combustível.
* Gasto total com pedágios.
* Emissão total de CO₂.
* Total gasto com TAG.

---

## 📅 Relatório Mensal

Endpoint:

GET /api/v1/fleet/monthly?year=2026&month=6

Indicadores:

* Veículos utilizados.
* Quantidade de viagens.
* Distância percorrida.
* Custos operacionais.
* Emissões de CO₂.

---

## 🌱 Impacto Ambiental

Endpoint:

GET /api/v1/fleet/environment

Retorna:

* Total de passagens em pedágios.
* CO₂ evitado.

---

## ⏱ Economia de Tempo

Endpoint:

GET /api/v1/fleet/time-savings

Retorna:

* Total de passagens.
* Minutos economizados.
* Horas economizadas.
* Dias economizados.

Premissa utilizada:

* 5 minutos economizados por passagem utilizando TAG.

---

# 🔐 Autenticação e Usuários

Autenticação baseada em JWT.

Endpoints:

POST /api/v1/auth/register

POST /api/v1/auth/login

GET /api/v1/users/me

PUT /api/v1/users/me

PATCH /api/v1/users/me/password

DELETE /api/v1/users/me

Recursos:

* Cadastro.
* Login.
* Alteração de senha.
* Atualização de perfil.
* Exclusão de conta.

---

# 🏗 Arquitetura

O projeto segue arquitetura em camadas:

API

* Controllers

Application

* DTOs
* Services

Domain

* Entities
* Interfaces
* Value Objects

Infrastructure

* Repositories
* Services
* Data
* Migrations

---

# 🛠 Tecnologias Utilizadas

* .NET 10
* ASP.NET Core
* Entity Framework Core
* SQLite
* JWT Authentication
* Swagger / OpenAPI
* Git
* GitHub

---

# 🗄 Banco de Dados

Banco utilizado:

SQLite

Arquivo:

taggy.db

Migrations:

* InitialCreate
* AddTagAccountModule
* AddAutoRefillAndTollSimulator
* AddTripHistory
* AddFleetAnalyticsAndVehicleOwnership
* AddTripTollPassageCount

---

# 🚀 Como Executar

## Restaurar Dependências

```bash
dotnet restore
```

## Aplicar Migrations

```bash
dotnet ef database update \
--project src/taggyManagement.Infrastructure/taggyManagement.Infrastructure.csproj \
--startup-project src/taggyManagement.API/taggyManagement.API.csproj
```

## Executar API

```bash
dotnet run --project src/taggyManagement.API
```

---

# 📖 Swagger

Após iniciar a aplicação:

http://localhost:5158/swagger

Permite:

* Testar endpoints.
* Realizar autenticação JWT.
* Validar regras de negócio.
* Simular fluxos completos do sistema.

---

# 🔒 Requisitos Não Funcionais

## Segurança

* JWT Authentication.
* Senhas com Hash + Salt.
* Endpoints protegidos.
* Validação por DTOs.
* Controle de acesso por usuário.
* Isolamento multiusuário.

## LGPD

* Coleta mínima de dados.
* Proteção de credenciais.
* Controle de acesso aos dados.
* Exclusão de conta.
* Não compartilhamento de informações pessoais.

## Disponibilidade

* API REST stateless.
* Persistência SQLite.
* Arquitetura desacoplada.
* Documentação via Swagger.

## Backup e Continuidade

* Backup periódico do arquivo SQLite.
* Recuperação por restauração do banco.

---

# ✅ Funcionalidades Implementadas

* Autenticação JWT
* Cadastro de usuários
* Gestão de senhas
* CRUD de veículos
* Importação em massa de veículos
* Planejamento de viagens
* Histórico de viagens
* Conta TAG
* Recarga manual
* Extrato financeiro
* Simulador de pedágio
* Auto Refill
* Fleet Analytics
* Dashboard da frota
* Relatórios mensais
* Métricas ambientais
* Economia de tempo
* Cálculo de CO₂
* Cálculo de CO₂ evitado
* Ownership de veículos
* Isolamento multiusuário
* SQLite Persistence
* EF Core Migrations
* Swagger/OpenAPI

---

# 👥 Equipe

Grupo B

* Ramon Leal Frazão
* Pedro Raimundo Sampaio
* Tiago Alcoforado Santos
* Iago Figueiroa Soares
* Rafael Morais de Azevedo
* Vinicius Beltrão de Melo Ferraz Lima

---

# 📄 Licença

Projeto desenvolvido exclusivamente para fins acadêmicos.
