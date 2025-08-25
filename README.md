🔄 Fluxo Detalhado:
1. 📱 Camada de API (Cliente.API)
Endpoint: CadastroClienteController

Função: Recebe requisições HTTP POST para cadastro de clientes

Entrada: Dados do cliente via JSON

Saída: Retorna status da operação

2. ⚙️ Processamento do Command (CQRS)
Command: CadastroClienteCommand - Objeto com dados do cliente

Handler: CadastroClienteCommandHandler - Orquestra o processamento

Validação: CadastroClienteCommandValidator - Regras de negócio

Repositório: InMemoryCustomerRepository - Persistência em memória

3. 📨 Publicação de Evento (Message Bus)
ServiceBus: Sistema de mensageria

Exchange: cartao.exchange no RabbitMQ

Routing: Mensagens roteadas para múltiplos consumidores

4. 🛠️ Consumidores (Workers)
ErroGeracaoCartaoWorker: Worker especializado em tratamento de erros

Outros consumidores: Possíveis outros serviços interessados no evento

5. 🔄 Processamento no Worker
Consumer: AsyncEventingBasicConsumer - Consumidor assíncrono

Processamento: Processa CartaoEvent - Lógica de negócio

Logs: Registro de atividades e simulação de atualização

Resultado: Status atualizado no sistema

📋 Tipos de Mensagens Processadas:
✅ Sucesso no Cadastro
json
{
  "clienteId": "123e4567-e89b-12d3-a456-426614174000",
  "nome": "João Silva",
  "email": "joao@email.com",
  "status": "cadastrado",
  "timestamp": "2024-01-15T10:30:00Z"
}
⚠️ Erro na Geração de Cartão
json
{
  "clienteId": "123e4567-e89b-12d3-a456-426614174000",
  "erro": "Falha na geração do cartão",
  "motivo": "Problema no sistema de terceiros",
  "timestamp": "2024-01-15T10:30:00Z"
}
🎯 Funcionalidades do ErroGeracaoCartaoWorker:
1. Tratamento de Erros
Captura exceções do processo de geração de cartão

Logs detalhados para debugging

Retry automático para erros temporários

2. Simulação de Atualização
Atualiza status do cliente no sistema

Simula integração com sistemas legados

Mantém consistência dos dados
