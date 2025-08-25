<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Sistema de Cadastro de Cliente - Documentação Técnica</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #333;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
        }

        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
            background: rgba(255, 255, 255, 0.95);
            backdrop-filter: blur(10px);
            border-radius: 20px;
            box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
            margin-top: 20px;
            margin-bottom: 20px;
        }

        .header {
            text-align: center;
            margin-bottom: 40px;
            padding: 30px;
            background: linear-gradient(135deg, #667eea, #764ba2);
            color: white;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.2);
        }

        .header h1 {
            font-size: 2.5em;
            margin-bottom: 10px;
            text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.3);
        }

        .header p {
            font-size: 1.2em;
            opacity: 0.9;
        }

        .section {
            margin-bottom: 40px;
            padding: 30px;
            background: white;
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
            border-left: 5px solid #667eea;
        }

        .section h2 {
            color: #667eea;
            font-size: 2em;
            margin-bottom: 20px;
            border-bottom: 2px solid #667eea;
            padding-bottom: 10px;
        }

        .section h3 {
            color: #764ba2;
            font-size: 1.5em;
            margin-top: 25px;
            margin-bottom: 15px;
        }

        .flowchart {
            background: #f8f9ff;
            padding: 30px;
            border-radius: 15px;
            margin: 20px 0;
            border: 2px dashed #667eea;
            text-align: center;
            min-height: 600px;
            position: relative;
            overflow-x: auto;
        }

        .flow-step {
            background: linear-gradient(135deg, #667eea, #764ba2);
            color: white;
            padding: 15px 20px;
            border-radius: 10px;
            margin: 10px;
            display: inline-block;
            min-width: 200px;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
            transition: transform 0.3s ease;
        }

        .flow-step:hover {
            transform: translateY(-5px);
        }

        .flow-arrow {
            font-size: 2em;
            color: #667eea;
            margin: 10px;
            display: block;
        }

        .code-block {
            background: #2d3748;
            color: #e2e8f0;
            padding: 20px;
            border-radius: 10px;
            margin: 15px 0;
            font-family: 'Courier New', monospace;
            font-size: 0.9em;
            overflow-x: auto;
            border-left: 4px solid #667eea;
        }

        .component-list {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 20px;
            margin-top: 20px;
        }

        .component-card {
            background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
            color: white;
            padding: 20px;
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
            transition: transform 0.3s ease;
        }

        .component-card:hover {
            transform: translateY(-5px);
        }

        .component-card h4 {
            font-size: 1.3em;
            margin-bottom: 10px;
        }

        .tech-stack {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
            margin-top: 20px;
        }

        .tech-badge {
            background: linear-gradient(135deg, #667eea, #764ba2);
            color: white;
            padding: 8px 16px;
            border-radius: 20px;
            font-size: 0.9em;
            font-weight: bold;
        }

        .workflow-diagram {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 20px;
            background: linear-gradient(135deg, #f8f9ff, #e8efff);
            border-radius: 15px;
            margin: 20px 0;
        }

        .workflow-row {
            display: flex;
            align-items: center;
            margin: 10px 0;
            flex-wrap: wrap;
            justify-content: center;
        }

        .workflow-box {
            background: white;
            border: 2px solid #667eea;
            border-radius: 10px;
            padding: 15px;
            margin: 5px 10px;
            min-width: 150px;
            text-align: center;
            box-shadow: 0 3px 10px rgba(0, 0, 0, 0.1);
        }

        .workflow-box.api {
            background: linear-gradient(135deg, #4facfe, #00f2fe);
            color: white;
            border-color: #4facfe;
        }

        .workflow-box.command {
            background: linear-gradient(135deg, #43e97b, #38f9d7);
            color: white;
            border-color: #43e97b;
        }

        .workflow-box.handler {
            background: linear-gradient(135deg, #fa709a, #fee140);
            color: white;
            border-color: #fa709a;
        }

        .workflow-box.infra {
            background: linear-gradient(135deg, #a8edea, #fed6e3);
            color: #333;
            border-color: #a8edea;
        }

        .workflow-box.messaging {
            background: linear-gradient(135deg, #ff9a9e, #fecfef);
            color: #333;
            border-color: #ff9a9e;
        }

        .workflow-box.worker {
            background: linear-gradient(135deg, #667eea, #764ba2);
            color: white;
            border-color: #667eea;
        }

        @media print {
            body {
                background: white;
            }
            .container {
                background: white;
                box-shadow: none;
                margin: 0;
            }
            .section {
                page-break-inside: avoid;
            }
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1>Sistema de Cadastro de Cliente</h1>
            <p>Documentação Técnica e Fluxograma Arquitetural</p>
            <p>Versão 1.0 - .NET 8 com RabbitMQ</p>
        </div>

        <div class="section">
            <h2>📋 Visão Geral do Sistema</h2>
            <p>Este sistema implementa uma arquitetura baseada em CQRS (Command Query Responsibility Segregation) e Event-Driven Architecture, utilizando .NET 8, RabbitMQ para mensageria e padrões de Worker Services para processamento assíncrono.</p>
            
            <h3>🎯 Objetivos</h3>
            <ul>
                <li>Cadastro de clientes via API REST</li>
                <li>Processamento assíncrono de eventos de cartão</li>
                <li>Arquitetura escalável e desacoplada</li>
                <li>Tratamento de erros e resiliência</li>
            </ul>

            <div class="tech-stack">
                <span class="tech-badge">.NET 8</span>
                <span class="tech-badge">ASP.NET Core Web API</span>
                <span class="tech-badge">RabbitMQ</span>
                <span class="tech-badge">CQRS Pattern</span>
                <span class="tech-badge">Worker Services</span>
                <span class="tech-badge">MediatR</span>
                <span class="tech-badge">FluentValidation</span>
            </div>
        </div>

        <div class="section">
            <h2>🏗️ Arquitetura do Sistema</h2>
            
            <div class="workflow-diagram">
                <div class="workflow-row">
                    <div class="workflow-box api">
                        <strong>Cliente.API</strong><br>
                        <small>Controller REST</small>
                    </div>
                </div>
                
                <div class="flow-arrow">↓</div>
                
                <div class="workflow-row">
                    <div class="workflow-box command">
                        <strong>CadastroClienteController</strong><br>
                        <small>Recebe HTTP Request</small>
                    </div>
                </div>
                
                <div class="flow-arrow">↓</div>
                
                <div class="workflow-row">
                    <div class="workflow-box command">
                        <strong>CadastroClienteCommand</strong><br>
                        <small>Command Object</small>
                    </div>
                </div>
                
                <div class="flow-arrow">↓</div>
                
                <div class="workflow-row">
                    <div class="workflow-box handler">
                        <strong>CadastroClienteCommandHandler</strong><br>
                        <small>Command Handler</small>
                    </div>
                </div>
                
                <div class="flow-arrow">↓ ↓ ↓</div>
                
                <div class="workflow-row">
                    <div class="workflow-box infra">
                        <strong>Validator</strong><br>
                        <small>Validação</small>
                    </div>
                    <div class="workflow-box infra">
                        <strong>Repository</strong><br>
                        <small>Persistência</small>
                    </div>
                    <div class="workflow-box messaging">
                        <strong>Message Bus</strong><br>
                        <small>Publica Evento</small>
                    </div>
                </div>
                
                <div class="flow-arrow">↓</div>
                
                <div class="workflow-row">
                    <div class="workflow-box messaging">
                        <strong>RabbitMQ Exchange</strong><br>
                        <small>cartao.exchange</small>
                    </div>
                </div>
                
                <div class="flow-arrow">↓</div>
                
                <div class="workflow-row">
                    <div class="workflow-box worker">
                        <strong>ErroGeracaoCartaoWorker</strong><br>
                        <small>Consumer Service</small>
                    </div>
                    <div class="workflow-box worker">
                        <strong>Outros Consumers</strong><br>
                        <small>Processamento Adicional</small>
                    </div>
                </div>
            </div>
        </div>

        <div class="section">
            <h2>🔧 Componentes Detalhados</h2>
            
            <div class="component-list">
                <div class="component-card">
                    <h4>📡 Cliente.API</h4>
                    <p>API REST responsável por receber requisições HTTP de cadastro de clientes. Implementa controllers usando ASP.NET Core Web API com padrões RESTful.</p>
                </div>
                
                <div class="component-card">
                    <h4>🎮 CadastroClienteController</h4>
                    <p>Controller específico que gerencia as requisições de cadastro, validações básicas e orquestração do comando através do MediatR.</p>
                </div>
                
                <div class="component-card">
                    <h4>📝 CadastroClienteCommand</h4>
                    <p>Objeto Command que encapsula os dados necessários para o cadastro de cliente, seguindo o padrão CQRS para separação de responsabilidades.</p>
                </div>
                
                <div class="component-card">
                    <h4>⚙️ CommandHandler</h4>
                    <p>Implementa a lógica de negócio para processamento do comando, coordenando validação, persistência e publicação de eventos.</p>
                </div>
                
                <div class="component-card">
                    <h4>✅ Validation Layer</h4>
                    <p>Camada de validação usando FluentValidation para garantir integridade dos dados antes do processamento.</p>
                </div>
                
                <div class="component-card">
                    <h4>💾 Repository Pattern</h4>
                    <p>Implementação In-Memory para persistência de dados dos clientes, seguindo o padrão Repository para abstração da camada de dados.</p>
                </div>
                
                <div class="component-card">
                    <h4>🔄 Message Bus</h4>
                    <p>Camada de abstração para publicação de eventos no RabbitMQ, permitindo comunicação assíncrona entre componentes.</p>
                </div>
                
                <div class="component-card">
                    <h4>🐰 RabbitMQ Exchange</h4>
                    <p>Exchange configurado no RabbitMQ (cartao.exchange) para roteamento de mensagens para diferentes consumers.</p>
                </div>
                
                <div class="component-card">
                    <h4>🔄 ErroGeracaoCartaoWorker</h4>
                    <p>Worker Service que consome eventos relacionados a erros na geração de cartões, implementando AsyncEventingBasicConsumer.</p>
                </div>
            </div>
        </div>

        <div class="section">
            <h2>🚀 Fluxo de Execução Detalhado</h2>
            
            <h3>1️⃣ Requisição HTTP</h3>
            <div class="code-block">
POST /api/clientes
{
  "nome": "João Silva",
  "email": "joao@email.com",
  "cpf": "12345678901"
}
            </div>
            
            <h3>2️⃣ Processamento do Controller</h3>
            <div class="code-block">
[HttpPost]
public async Task&lt;IActionResult&gt; CadastrarCliente([FromBody] CadastroClienteCommand command)
{
    var resultado = await _mediator.Send(command);
    return Ok(resultado);
}
            </div>
            
            <h3>3️⃣ Execução do Command Handler</h3>
            <div class="code-block">
public async Task&lt;Unit&gt; Handle(CadastroClienteCommand request, CancellationToken cancellationToken)
{
    // 1. Validação
    await _validator.ValidateAndThrowAsync(request);
    
    // 2. Persistência
    await _repository.AdicionarAsync(cliente);
    
    // 3. Publicação do Evento
    await _messageBus.PublishAsync(new CartaoEvent { ClienteId = cliente.Id });
    
    return Unit.Value;
}
            </div>
            
            <h3>4️⃣ Processamento no Worker Service</h3>
            <div class="code-block">
public async Task ProcessarCartaoEvent(CartaoEvent evento)
{
    try
    {
        _logger.LogInformation($"Processando evento para cliente: {evento.ClienteId}");
        
        // Simula processamento
        await Task.Delay(1000);
        
        // Atualiza status do cartão
        _logger.LogInformation("Status do cartão atualizado com sucesso");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erro ao processar evento de cartão");
    }
}
            </div>
        </div>

        <div class="section">
            <h2>⚡ Configurações e Padrões</h2>
            
            <h3>🔧 Configuração do RabbitMQ</h3>
            <div class="code-block">
services.AddSingleton&lt;IRabbitMQConnection&gt;(provider =&gt;
{
    var factory = new ConnectionFactory()
    {
        HostName = configuration["RabbitMQ:HostName"],
        UserName = configuration["RabbitMQ:UserName"],
        Password = configuration["RabbitMQ:Password"],
        AutomaticRecoveryEnabled = true,
        NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
    };
    
    return new RabbitMQConnection(factory);
});
            </div>
            
            <h3>📦 Registro do Worker Service</h3>
            <div class="code-block">
services.AddHostedService&lt;ErroGeracaoCartaoWorker&gt;();
services.AddScoped&lt;IMessageBus, RabbitMQMessageBus&gt;();
services.AddScoped&lt;ICustomerRepository, InMemoryCustomerRepository&gt;();
            </div>
            
            <h3>✅ Configuração de Validação</h3>
            <div class="code-block">
public class CadastroClienteCommandValidator : AbstractValidator&lt;CadastroClienteCommand&gt;
{
    public CadastroClienteCommandValidator()
    {
        RuleFor(x =&gt; x.Nome)
            .NotEmpty()
            .MaximumLength(100);
            
        RuleFor(x =&gt; x.Email)
            .NotEmpty()
            .EmailAddress();
            
        RuleFor(x =&gt; x.CPF)
            .NotEmpty()
            .Length(11);
    }
}
            </div>
        </div>

        <div class="section">
            <h2>🛡️ Tratamento de Erros e Resiliência</h2>
            
            <h3>🔄 Retry Policy</h3>
            <p>Implementação de políticas de retry para cenários de falha temporária na comunicação com RabbitMQ.</p>
            
            <h3>🔍 Logging e Monitoramento</h3>
            <p>Sistema de logs estruturado para rastreabilidade de eventos e debugging de problemas.</p>
            
            <h3>💊 Circuit Breaker</h3>
            <p>Padrão implementado para evitar cascata de falhas em cenários de indisponibilidade de serviços externos.</p>
            
            <h3>🔒 Validation Guards</h3>
            <p>Múltiplas camadas de validação para garantir integridade dos dados em todos os pontos do fluxo.</p>
        </div>

        <div class="section">
            <h2>📈 Métricas e Performance</h2>
            
            <h3>🎯 KPIs Principais</h3>
            <ul>
                <li><strong>Latência da API:</strong> < 200ms para 95% das requisições</li>
                <li><strong>Throughput:</strong> Suporte a 1000+ requisições por minuto</li>
                <li><strong>Disponibilidade:</strong> 99.9% uptime</li>
                <li><strong>Taxa de Erro:</strong> < 0.1% de falhas</li>
            </ul>
            
            <h3>🔍 Monitoramento</h3>
            <ul>
                <li>Health Checks para todos os componentes</li>
                <li>Métricas de consumo de mensagens RabbitMQ</li>
                <li>Alertas automatizados para falhas críticas</li>
                <li>Dashboard de métricas em tempo real</li>
            </ul>
        </div>

        <div class="section">
            <h2>🚀 Deploy e Escalabilidade</h2>
            
            <h3>📦 Containerização</h3>
            <p>Sistema preparado para deploy em containers Docker com suporte a orquestração Kubernetes.</p>
            
            <h3>⚖️ Escalabilidade Horizontal</h3>
            <p>Arquitetura permite escalar Workers e APIs independentemente baseado na demanda.</p>
            
            <h3>🔄 CI/CD Pipeline</h3>
            <p>Pipeline automatizado para build, testes e deploy com zero downtime.</p>
        </div>

        <div class="section">
            <h2>🔮 Próximos Passos</h2>
            
            <h3>📋 Roadmap</h3>
            <ul>
                <li>Implementação de banco de dados persistente</li>
                <li>Autenticação e autorização JWT</li>
                <li>Cache distribuído com Redis</li>
                <li>Observabilidade com OpenTelemetry</li>
                <li>Testes de carga automatizados</li>
                <li>Documentação OpenAPI/Swagger</li>
            </ul>
            
            <h3>🛠️ Melhorias Técnicas</h3>
            <ul>
                <li>Dead Letter Queue para mensagens com falha</li>
                <li>Implementação de Saga Pattern</li>
                <li>Event Sourcing para auditoria</li>
                <li>Compressão de mensagens RabbitMQ</li>
            </ul>
        </div>

        <div class="section" style="text-align: center; background: linear-gradient(135deg, #667eea, #764ba2); color: white;">
            <h2>📞 Contato e Suporte</h2>
            <p>Sistema desenvolvido seguindo as melhores práticas de arquitetura de software</p>
            <p><strong>Versão:</strong> 1.0.0 | <strong>Data:</strong> Agosto 2025</p>
            <div style="margin-top: 20px; opacity: 0.8;">
                <small>Documentação técnica gerada automaticamente</small>
            </div>
        </div>
    </div>

    <script>
        // Função para gerar PDF
        function generatePDF() {
            window.print();
        }

        // Adicionar botão para gerar PDF
        document.addEventListener('DOMContentLoaded', function() {
            const header = document.querySelector('.header');
            const button = document.createElement('button');
            button.textContent = '📄 Gerar PDF';
            button.style.cssText = `
                margin-top: 20px;
                padding: 12px 24px;
                background: rgba(255, 255, 255, 0.2);
                border: 2px solid white;
                color: white;
                border-radius: 25px;
                cursor: pointer;
                font-size: 16px;
                font-weight: bold;
                transition: all 0.3s ease;
            `;
            
            button.onmouseover = () => {
                button.style.background = 'white';
                button.style.color = '#667eea';
            };
            
            button.onmouseout = () => {
                button.style.background = 'rgba(255, 255, 255, 0.2)';
                button.style.color = 'white';
            };
            
            button.onclick = generatePDF;
            header.appendChild(button);
        });

        // Animações de entrada
        const observerOptions = {
            threshold: 0.1,
            rootMargin: '0px 0px -50px 0px'
        };

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }
            });
        }, observerOptions);

        // Aplicar animações aos elementos
        document.querySelectorAll('.section').forEach(section => {
            section.style.opacity = '0';
            section.style.transform = 'translateY(30px)';
            section.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
            observer.observe(section);
        });
    </script>
</body>
</html>
