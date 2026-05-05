# Taggy - Mobilidade Inteligente & Sustentável

Um ecossistema completo para gestão de pedágios, monitoramento financeiro e análise de impacto ambiental em tempo real. O sistema une a conveniência do pagamento automático com a inteligência de dados para uma condução mais eficiente e ecológica.

## 🚀 Funcionalidades

* **💳 Monitor de Recarga Inteligente (Auto-Refill):** Lógica em C# que monitora o saldo da Taggy após cada débito. Caso atinja o limite mínimo de segurança, dispara uma recarga automática (via PIX/Débito), garantindo que o motorista nunca fique parado na cancela.
* **🛣️ Calculadora de Rota com Custo Total:** Integração que soma o valor de todos os pedágios do trajeto à estimativa de consumo de combustível ou energia, entregando o custo real da viagem antes da partida.
* **🌱 Eco-Simulator (Carbono e Bateria):** Funcionalidade que calcula a emissão de CO2 evitada (Classe Carbono) ou a economia de bateria (Classe Bateria), comparando diferentes rotas e sugerindo o caminho mais sustentável.
* **🚗 Gestor de Frotas e Veículos (CRUD):** Painel administrativo para cadastrar veículos, definindo se são a combustão ou elétricos, permitindo que os cálculos de eficiência e sustentabilidade sejam precisos.
* **📊 Extrato de Impacto Ambiental:** Um relatório detalhado gerado a partir do banco SQL que mostra o histórico de passagens e o acumulado de "Carbono não emitido" ao longo do tempo.
* **🔍 Validador de Status em Tempo Real:** API de segurança que consulta a situação da tag e do veículo, bloqueando a criação de rotas ou sugerindo alternativas caso a tag esteja inativa ou o saldo seja insuficiente.

---

## 🛠️ Estrutura de Negócio e Lógica

Para garantir o funcionamento do ecossistema, o projeto implementa:

1.  **Simulador de Passagem:** Como o sistema é digital, implementamos um "Webhook Simulador" que emula a antena física do pedágio para disparar os gatilhos de débito e recarga automática.
2.  **Lógica de Propulsão:** Diferenciação algorítmica entre veículos a combustão (foco em emissão de CO2) e elétricos (foco em autonomia de bateria e eficiência energética).
3.  **Conversão Ambiental:** Algoritmo que transforma a distância percorrida e a eficiência do motor em dados reais de preservação ambiental (Carbono salvo).

---

## 📦 Dependências

* **ASP.NET Core** (C# 12)
* **SQL Server** (Transações, Tarifas e Parâmetros de Sustentabilidade)

---

## 🗄️ Diagrama do Banco de Dados

O banco de dados foi estruturado para suportar escalabilidade e histórico:
* **Tabela de Transações:** Histórico de passagens e débitos.
* **Tabela de Concessionárias:** Consulta de tarifas vigentes.
* **Tabela de Veículos:** Atributos de consumo (km/l) e índices de emissão.

<img width="1461" height="668" alt="image" src="https://github.com/user-attachments/assets/bf4a1031-3414-40a6-a9b1-e2a5c6f9352a" />

> 🔗 [Acesse o diagrama aqui]()

---

## 🗺️ Fluxograma do Usuário

O fluxo abrange desde o check de saldo inicial, a escolha da "Rota Verde", até o disparo do Auto-Refill após a validação da tag na praça de pedágio.

<img width="1600" height="872" alt="image" src="https://github.com/user-attachments/assets/cac5a680-cc59-4017-8215-7e4a0187dfac" />

> 🔗 [Acesse o Miro aqui]()

---

## 💡 Diferencial
Diferente de sistemas de monitoramento passivo, o **Taggy** atua na **operação direta**: ele gerencia o pagamento, garante o saldo via automação e valida a segurança da tag em tempo real, mantendo a pegada ecológica como o critério principal para a escolha de rotas.
