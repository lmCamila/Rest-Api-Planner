using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanRestApi.Documentation
{
    public class TagDescriptionDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new[]
            {
                new Tag{Name = "Plan", Description = "Consulta e mantém os Planos."},
                new Tag { Name = "PlanHistory", Description = "Consulta o Histórico dos Planos." },
                new Tag { Name = "PlanStatus", Description = "Consulta e mantém ao Status dos Planos." },
                new Tag { Name = "TypePlan", Description = "Consulta e mantém os Tipos de Plano." },
                new Tag { Name = "User", Description = "Consulta e mantém os Usuários." },
                new Tag { Name = "UserHistory", Description = "Consulta o Histórico de Usuários." }
            };
        }
    }
}
